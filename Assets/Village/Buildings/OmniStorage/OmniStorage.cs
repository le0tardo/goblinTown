using UnityEngine;

public class OmniStorage : MonoBehaviour, IBuilding, IDepositable
{

    public Vector3 Position => transform.position;
    public OmniStorage Storage => this;
    public void Deposit(ForagedResourceData resource, int amount)
    {
        if (resource == null || amount <= 0)
            return;

        VillageResourceManager.inst.AddResource(resource.category, amount);
    }

    private void OnTriggerEnter(Collider other)
    {
        Unit unit = other.GetComponent<Unit>();
        if (unit == null)
            return;
        if (unit.carriedResource != null)
        {
            unit.DepositAtStorage(this);
        }
    }
}
