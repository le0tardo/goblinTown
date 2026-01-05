using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class SlotProvider : MonoBehaviour, ISlotProvider
{
    [SerializeField] float radius = 1.5f;
    [SerializeField] int maxSlots = 6;
    [SerializeField] float slotRadius = 0.3f;

    Dictionary<Unit, Vector3> occupiedSlots = new();

    [SerializeField] int freeSlots;

    SphereCollider trigger;

    private void Start()
    {
        trigger = GetComponent<SphereCollider>();
        freeSlots= Mathf.RoundToInt(maxSlots);
    }
    private void Update() //only debug!
    {
        freeSlots= Mathf.RoundToInt(maxSlots-occupiedSlots.Count);
    }
    public Vector3 RequestSlot(Unit unit)
    {
        if (occupiedSlots.TryGetValue(unit, out var pos))
            return pos;

        for (int i = 0; i < maxSlots; i++)
        {
            Vector3 candidate = GetSlotWorldPosition(i);

            if (IsSlotFree(candidate))
            {
                if (NavMesh.SamplePosition(candidate, out var hit, 1f, NavMesh.AllAreas))
                {
                    occupiedSlots[unit] = hit.position;
                    return hit.position;
                }
            }
        }

        return transform.position;
    }

    public void ReleaseSlot(Unit unit)
    {
        occupiedSlots.Remove(unit);
        freeSlots++;
    }

    Vector3 GetSlotWorldPosition(int index)
    {
        float angle = (2 * Mathf.PI / maxSlots) * index;
        Vector3 offset = new Vector3(
            Mathf.Cos(angle),
            0,
            Mathf.Sin(angle)
        ) * radius;

        return transform.position + offset;
    }

    bool IsSlotFree(Vector3 pos)
    {
        foreach (var p in occupiedSlots.Values)
        {
            if (Vector3.Distance(p, pos) < slotRadius * 2f)
                return false;
        }
        return true;
    }

    // ==========================
    // GIZMOS
    // ==========================
    void OnDrawGizmos()
    {
        if (!Application.isPlaying)
            return;
        

        for (int i = 0; i < maxSlots; i++)
        {
            Vector3 slotPos = GetSlotWorldPosition(i);

            bool occupied = false;
            Unit owner = null;

            foreach (var kvp in occupiedSlots)
            {
                if (Vector3.Distance(kvp.Value, slotPos) < slotRadius * 2f)
                {
                    occupied = true;
                    owner = kvp.Key;
                    break;
                }
            }

            Gizmos.color = occupied ? Color.red : Color.green;
            Gizmos.DrawSphere(slotPos, slotRadius);

            // Optional: draw line to owning unit
            if (occupied && owner != null)
            {
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(slotPos, owner.transform.position);
            }
        }
    }
}
