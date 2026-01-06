using System.Collections.Generic;
using UnityEngine;

public class VillageResourceGUI : MonoBehaviour
{
    [SerializeField] VillageResourceSlot slotPrefab;
    Dictionary<VillageResource, VillageResourceSlot> slots = new Dictionary<VillageResource, VillageResourceSlot>();

    public void Initialize(Dictionary<VillageResource, int> resources)
    {
        foreach (var kvp in resources)
        {
            VillageResourceSlot slot = Instantiate(slotPrefab, this.transform);
            slot.Set(kvp.Key, kvp.Value);
            slots.Add(kvp.Key, slot);
        }
    }
    public void UpdateResource(VillageResource data, int amount)
    {
        if (slots.TryGetValue(data, out var slot))
        {
            slot.Set(data, amount);
        }
    }

    public void UpdateAllResources(Dictionary<VillageResource, int> resources)
    {
        foreach (var kvp in resources)
        {
            UpdateResource(kvp.Key, kvp.Value);
        }
    }
}
