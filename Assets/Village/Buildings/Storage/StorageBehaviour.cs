using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class StorageBehaviour : MonoBehaviour
{
    [SerializeField] StorageObject storage;

    private Dictionary<ForagedResourceData, int> stored =
    new Dictionary<ForagedResourceData, int>();

    public  int totalStored;

    void Awake()
    {
        foreach (var resource in storage.acceptedResources)
        {
            stored.Add(resource, 0);
        }

        totalStored = 0;
    }
    public int Deposit(ForagedResourceData resource, int amount)
    {
        if (!CanAccept(resource))
            return amount;

        int spaceLeft = storage.capacity - totalStored;
        if (spaceLeft <= 0)
            return amount;

        int deposited = Mathf.Min(spaceLeft, amount);

        stored[resource] += deposited;
        totalStored += deposited;

        return amount - deposited; // leftover
    }

    public void TransferToVillage(VillageResourceManager village)
    {
        foreach (var kvp in stored)
        {
            //village.AddResource(kvp.Key, kvp.Value);
        }

        foreach (var key in stored.Keys.ToList())
            stored[key] = 0;
    }

    public bool CanAccept(ForagedResourceData resource)
    {
        return stored.ContainsKey(resource);
    }
    public bool HasSpace()
    {
        return totalStored < storage.capacity;
    }

}
