using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct VillageResourceAmount
{
    public VillageResource resource;
    public int amount;
}
public class VillageResourceManager : MonoBehaviour
{
    public List<VillageResourceAmount> StartingResources;

    public static VillageResourceManager inst { get; private set; }
    public Dictionary<VillageResource, int> villageResources = new Dictionary<VillageResource, int>();
    public Dictionary<VillageResource, int> villageCaps = new Dictionary<VillageResource, int>();

    [SerializeField] VillageResourceGUI gui;
    [SerializeField] VillageResource[] villageResourceList;

    void Awake()
    {
        if (inst != null && inst != this) { Destroy(gameObject); return; }inst = this;

        villageResources.Clear();

        foreach (var villageResource in villageResourceList)
        {
            villageResources.Add(villageResource, 0);
        }
        foreach (var villageResource in villageResourceList) //resourceCap
        {
            villageCaps.Add(villageResource, 50);
        }

        foreach (var entry in StartingResources)
        {
            if (entry.resource == null)
                continue;

            villageResources[entry.resource] = entry.amount;
        }
    }

    private void Start()
    {
        gui.Initialize(villageResources);
    }

    public void AddResource(VillageResource data, int amount)
    {
        villageResources[data] += amount;

        gui.UpdateResource(data, villageResources[data]);
    }
    public void RemoveResource(VillageResource data, int amount)
    {
        villageResources[data] -= amount;

        gui.UpdateResource(data, villageResources[data]);
    }
    public void ChangeCap(int changeAmount) // chatGpt wrote this
    {
        if (changeAmount == 0)
            return;

        // Copy keys so we can safely modify values
        var keys = new List<VillageResource>(villageCaps.Keys);

        foreach (var resource in keys)
        {
            villageCaps[resource] += changeAmount;

            // Optional safety clamp
            if (villageCaps[resource] < 0)
                villageCaps[resource] = 0;

            // Optional: clamp current amount if cap shrinks
            if (villageResources.TryGetValue(resource, out int current))
            {
                if (current > villageCaps[resource])
                    villageResources[resource] = villageCaps[resource];
            }
        }

        gui.UpdateAllResources(villageResources);
    }


    public void SpendBuildingCost(BuildingObject building)
    {
        for (int i = 0; i < building.costs.Length; i++)
        {
            var cost = building.costs[i];

            if (villageResources.TryGetValue(cost.resource, out int currentAmount))
            {
                villageResources[cost.resource] = currentAmount - cost.amount;
            }
            else
            {
                // This should never happen if CanAffordBuilding() was called first
                Debug.LogError($"Trying to spend missing resource: {cost.resource.name}");
            }
        }
         gui.UpdateAllResources(villageResources);
    }

}
