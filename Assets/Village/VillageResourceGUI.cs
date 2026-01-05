using System.Collections.Generic;
using UnityEngine;

public class VillageResourceGUI : MonoBehaviour
{
    [SerializeField] VillageResourceSlot slotPrefab;
    Dictionary<MacroResourceCategory, VillageResourceSlot> slots = new Dictionary<MacroResourceCategory, VillageResourceSlot>();

    public void Initialize(Dictionary<MacroResourceCategory, int> resources)
    {
        foreach (var kvp in resources)
        {
            VillageResourceSlot slot = Instantiate(slotPrefab, this.transform);
            slot.Set(kvp.Key, kvp.Value);
            slots.Add(kvp.Key, slot);
        }
    }
    public void UpdateResource(MacroResourceCategory data, int amount)
    {
        if (slots.TryGetValue(data, out var slot))
        {
            slot.Set(data, amount);
        }
    }
}
