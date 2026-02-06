using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class ClickManager : MonoBehaviour
{
    [SerializeField] LayerMask unitLayer;
    [SerializeField] LayerMask groundLayer;
    Vector2 dragStartPos;
    Vector2 dragCurrentPos;
    bool isDragging;
    float dragThreshold = 10f;

    public static ClickManager inst;
    private void Awake()
    {
        if (inst != null && inst != this)
        {
            Destroy(gameObject);
            return;
        }
        inst = this;
    }
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            dragStartPos = Input.mousePosition;
            dragCurrentPos = dragStartPos;
            isDragging = true;
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            dragCurrentPos = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0))
        {
            isDragging = false;

            if (Vector2.Distance(dragStartPos, dragCurrentPos) < dragThreshold)
            {
                HandleClick();
            }
            else
            {
                HandleBoxSelection(dragStartPos, dragCurrentPos);
            }
        }
        if (Input.GetMouseButtonUp(1))
        {
            UnitManager.inst.ClearSelection();
            BuildingManager.inst.DeselectBuilding();
        }
    }

    #region RectTransforms
    Rect GetScreenRect(Vector2 start, Vector2 end)
    {
        Vector2 bottomLeft = Vector2.Min(start, end);
        Vector2 topRight = Vector2.Max(start, end);
        return new Rect(bottomLeft, topRight - bottomLeft);
    }
    Rect GetGuiRect(Vector2 start, Vector2 end)
    {
        Vector2 bottomLeft = Vector2.Min(start, end);
        Vector2 topRight = Vector2.Max(start, end);

        return new Rect(
            bottomLeft.x,
            Screen.height - topRight.y,
            topRight.x - bottomLeft.x,
            topRight.y - bottomLeft.y
        );
    }
    #endregion
    void HandleClick()
    {
        // Ignore while placing buildings
        if (BuildingManager.inst != null && BuildingManager.inst.isPlacingBuilding)
            return;

        // Ignore UI clicks
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (!Physics.Raycast(ray, out RaycastHit hit))
            return;

        // Slot management before anything else!
        ISlotProvider clickedSlotProvider =hit.collider.GetComponentInParent<ISlotProvider>();
        if (clickedSlotProvider == null)
        {
            foreach (Unit unit in UnitManager.inst.selectedUnits)
            {
                unit.ReleaseSlot();
            }
        }

            // Local helpers (only inside this method)

            void CommandUnit(Unit unit, Vector3 destination, Unit.EndAction action)
            {
                //if (UnitManager.inst.selectedUnits.Count < 2) { destination=hit.point; }
                
                unit.endAction = action;
                unit.MoveTo(destination);
            }

        void ClearTargets(Unit unit)
        {
            unit.forageTarget = null;
            unit.depositTarget = null;
            unit.huntTarget = null;
        }

        Vector3 GetSlotOrPosition(Unit unit, ISlotProvider provider, Vector3 fallback)
        {
            if (provider == null)
                return fallback;

            if (unit.currentSlotProvider != provider)
            {
                unit.ReleaseSlot();
                unit.currentSlotProvider = provider;
            }

            return provider.RequestSlot(unit);
        }



        List<Vector3> GetGroupPositions(Vector3 center, float radius = 0.25f)
        {
            int count = UnitManager.inst.selectedUnits.Count;
            List<Vector3> positions = new();

            radius = radius + (0.125f * count);

            for (int i = 0; i < count; i++)
            {
                float angle = (2 * Mathf.PI / count) * i;

                Vector3 offset = new Vector3(
                    Mathf.Cos(angle),
                    0,
                    Mathf.Sin(angle)
                ) * radius;

                positions.Add(center + offset);
            }

            return positions;
        }

        // 1. Unit selection

        ISelectable selectable = hit.collider.GetComponentInParent<ISelectable>();
        if (selectable != null)
        {
            Unit unit = selectable as Unit;

            if (unit != null)
            {
                if (UnitManager.inst.IsSelected(unit))
                {
                    UnitManager.inst.DeselectUnit(unit);
                    UnitManager.inst.ClearSelection();
                }
                else
                {
                    UnitManager.inst.ClearSelection();
                    UnitManager.inst.SelectUnit(unit);
                }

                BuildingManager.inst.DeselectBuilding();
            }

            return;
        }

        // 2. Buildings

        IBuilding building = hit.collider.GetComponentInParent<IBuilding>();
        if (building != null)
        {
            IWorkable workTarget = building.Bbh.GetComponent<IWorkable>();
            IDepositable depositTarget = building.Bbh.GetComponent<IDepositable>();
            ISlotProvider slotProvider = building.Bbh.GetComponent<ISlotProvider>();

            // No units selected = select building
            if (UnitManager.inst.selectedUnits.Count == 0)
            {
                if (BuildingManager.inst.selectedBuilding == building.Bbh)
                    BuildingManager.inst.DeselectBuilding();
                else
                {
                    BuildingManager.inst.DeselectBuilding();
                    BuildingManager.inst.SelectBuilding(building.Bbh);
                }

                return;
            }
            // Units selected = command
            BuildingManager.inst.DeselectBuilding();

            var units = UnitManager.inst.selectedUnits;
            var positions = GetGroupPositions(hit.point);

            for (int i = 0; i < units.Count; i++)
            {
                Unit unit = units[i];

                ClearTargets(unit);

                // Default movement
                Vector3 dest = positions[i];

                // If building has slots, use slot instead
                if (slotProvider != null)
                    dest = GetSlotOrPosition(unit, slotProvider, dest);

                CommandUnit(unit, dest, Unit.EndAction.None);

                // Assign Work
                if (workTarget != null)
                {
                    unit.workTarget = workTarget;
                    unit.endAction = Unit.EndAction.Work;
                }
                // Assign Deposit
                if (depositTarget != null)
                {
                    unit.depositTarget = depositTarget;
                    unit.endAction = Unit.EndAction.Deposit;
                }
            }

            return;
        }

        // 3. Forageable nodes

        IForageable forageable = hit.collider.GetComponentInParent<IForageable>();
        if (forageable != null)
        {
            ISlotProvider slotProvider = hit.collider.GetComponentInParent<ISlotProvider>();

            foreach (Unit unit in UnitManager.inst.selectedUnits)
            {
                ClearTargets(unit);

                unit.forageTarget = forageable;

                Vector3 dest = GetSlotOrPosition(unit, slotProvider, forageable.Position);
                CommandUnit(unit, dest, Unit.EndAction.Forage);
            }

            return;
        }

        // 4. Deposits

        IDepositable deposit = hit.collider.GetComponentInParent<IDepositable>();
        if (deposit != null)
        {
            ISlotProvider slotProvider = hit.collider.GetComponentInParent<ISlotProvider>();

            foreach (Unit unit in UnitManager.inst.selectedUnits)
            {
                ClearTargets(unit);

                unit.depositTarget = deposit;

                Vector3 dest = GetSlotOrPosition(unit, slotProvider, deposit.Position);
                CommandUnit(unit, dest, Unit.EndAction.Deposit);
            }

            return;
        }

        // 5. Pickups

        IPickupable pickup = hit.collider.GetComponentInParent<IPickupable>();
        if (pickup != null)
        {
            var units = UnitManager.inst.selectedUnits;
            var positions = GetGroupPositions(hit.point);

            for (int i = 0; i < units.Count; i++)
            {
                Unit unit = units[i];

                ClearTargets(unit);

                unit.pickupTarget = pickup;
                CommandUnit(unit, positions[i], Unit.EndAction.Pickup);
            }

            return;
        }

        // 6. animals (hunt)
        IHuntable animal = hit.collider.GetComponentInParent<IHuntable>();
        if (animal != null && UnitManager.inst.selectedUnits.Count > 0)
        {
            foreach (Unit unit in UnitManager.inst.selectedUnits)
            {
                //Spears and Fishing rod can be wood!?
               /* UnitEquipment eq = unit.GetComponent<UnitEquipment>();
                if (eq.toolLevel < 1)
                {
                    int r=Random.Range(0, UnitManager.inst.selectedUnits.Count);
                    var positions = GetGroupPositions(hit.point);
                    CommandUnit(unit, positions[r],Unit.EndAction.None);
                    continue;
                }*/

                ClearTargets(unit);

                Vector3 dir = (animal.Position - unit.transform.position).normalized;
                float spearRange = 4f;

                Vector3 rawDest = animal.Position - dir * spearRange;

                if (NavMesh.SamplePosition(rawDest, out NavMeshHit navHit, 1.5f, NavMesh.AllAreas))
                    rawDest = navHit.position;

                unit.huntTarget = animal;
                CommandUnit(unit, rawDest, Unit.EndAction.Hunt);
            }

            return;
        }

        // 7. Nothing (ground click)

        if (((1 << hit.collider.gameObject.layer) & groundLayer) != 0)
        {
            var units = UnitManager.inst.selectedUnits;
            if (units.Count == 0) return;

            var positions = GetGroupPositions(hit.point);

            for (int i = 0; i < units.Count; i++)
            {
                Unit unit = units[i];

                ClearTargets(unit);
                CommandUnit(unit, positions[i], Unit.EndAction.None);
            }

            return;
        }
    }

    void HandleBoxSelection(Vector2 start, Vector2 end)
    {
        Rect selectionRect = GetScreenRect(start, end);

        UnitManager.inst.ClearSelection();
        BuildingManager.inst.DeselectBuilding();

        foreach (Unit unit in UnitManager.inst.units)
        {
            Vector3 screenPos = Camera.main.WorldToScreenPoint(unit.transform.position);

            if (screenPos.z < 0)
                continue; // behind camera

            if (selectionRect.Contains(screenPos))
            {
                UnitManager.inst.SelectUnit(unit);
            }
        }
    }
    void OnGUI()
    {
        if (!isDragging)
            return;

        Rect rect = GetGuiRect(dragStartPos, dragCurrentPos);

        // Filled box
        GUI.color = new Color(0, 1, 0, 0.15f);
        GUI.DrawTexture(rect, Texture2D.whiteTexture);

        // Border
        GUI.color = Color.green;
        GUI.DrawTexture(new Rect(rect.xMin, rect.yMin, rect.width, 1), Texture2D.whiteTexture);
        GUI.DrawTexture(new Rect(rect.xMin, rect.yMax, rect.width, 1), Texture2D.whiteTexture);
        GUI.DrawTexture(new Rect(rect.xMin, rect.yMin, 1, rect.height), Texture2D.whiteTexture);
        GUI.DrawTexture(new Rect(rect.xMax, rect.yMin, 1, rect.height), Texture2D.whiteTexture);

        GUI.color = Color.white;
    }
}

