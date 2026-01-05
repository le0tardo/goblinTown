using UnityEngine;
public interface ISlotProvider
{
    Vector3 RequestSlot(Unit unit);
    void ReleaseSlot(Unit unit);
}