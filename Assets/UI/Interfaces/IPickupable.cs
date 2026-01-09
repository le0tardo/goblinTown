using UnityEngine;

public interface IPickupable
{
    ForagedResourceData Resource{  get; }
    bool TryPickup(Unit unit);
}
