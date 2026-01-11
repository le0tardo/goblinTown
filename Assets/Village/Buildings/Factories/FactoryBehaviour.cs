using UnityEngine;


public class FactoryBehaviour : MonoBehaviour
{
    [System.Serializable]
    public struct FactoryIO
    {
        public VillageResource resource;
        public int amount;
    }

    public FactoryIO[] input;
    public FactoryIO output;

    public float productionTime;
    public float maxProductionTime;

    public bool working;
    [SerializeField] GameObject workingVisuals;

    private void Update()
    {
        if (!working) return;

        if (productionTime > 0)
        {
            productionTime-=Time.deltaTime;
        }
        if (productionTime <= 0)
        {
            Produce();
        }
    }

    public void ToggleWork()
    {
        working = !working;
        workingVisuals.SetActive(working);
    }

    void Produce()
    {
        if (!AffordProduction())
        {
            working = false;
            productionTime = maxProductionTime;
            ToggleWork();
            return;
        }

        for (int i = 0; i < input.Length; i++)
        {
            VillageResourceManager.inst.RemoveResource(input[i].resource, input[i].amount);
        }

        VillageResourceManager.inst.AddResource(output.resource, output.amount);

        productionTime = maxProductionTime;
    }

    bool AffordProduction()
    {
        var village = VillageResourceManager.inst;

        for (int i = 0; i < input.Length; i++)
        {
            var io = input[i];

            if (!village.villageResources.TryGetValue(io.resource, out int available))
                return false;

            if (available < io.amount)
                return false;
        }
        return true;
    }

}
