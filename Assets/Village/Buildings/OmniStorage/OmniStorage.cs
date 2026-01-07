using UnityEngine;

public class OmniStorage : MonoBehaviour, IDepositable
{

    public Vector3 Position => transform.position;
    public OmniStorage Storage => this;

    public int level = 1;
    public int resourceCapBoost = 10;

    private void Start()
    {
        VillageResourceManager.inst.ChangeCap(resourceCapBoost);
    }
    public void Deposit(ForagedResourceData resource, int amount)
    {
        if (resource == null || amount <= 0)
            return;

        VillageResourceManager.inst.AddResource(resource.villageResource, amount);
    }

    private void OnTriggerEnter(Collider other) //meh, works...
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
