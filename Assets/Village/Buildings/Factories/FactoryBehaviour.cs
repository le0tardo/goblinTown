using UnityEngine;


public class FactoryBehaviour : MonoBehaviour, IProducer
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

    //sync to interface
    public bool IsProducing => working;
    public float Progress01 => working && maxProductionTime > 0 ? 1f - (productionTime / maxProductionTime) : 0f;
    public Transform WorldTransform => transform;

    private void Update()
    {
        if (!working)
            return;

        if (!AffordProduction())
        {
            StopWorking();
            return;
        }

        productionTime -= Time.deltaTime;

        if (productionTime <= 0f)
        {
            Produce();
        }

        //GUI sync
        if (!working)
        {
            MiniProductionBarManager.inst.Hide(this);
            return;
        }
        else
        {
            MiniProductionBarManager.inst.Show(this);
        }
    }

    public void ToggleWork()
    {
        working = !working;
        workingVisuals.SetActive(working);
    }

    void Produce()
    {
        for (int i = 0; i < input.Length; i++)
        {
            VillageResourceManager.inst.RemoveResource(
                input[i].resource,
                input[i].amount
            );
        }

        VillageResourceManager.inst.AddResource(
            output.resource,
            output.amount
        );

        productionTime = maxProductionTime;
    }


    void StopWorking()
    {
        working = false;
        productionTime = maxProductionTime;
        workingVisuals.SetActive(false);
    }

    public bool AffordProduction()
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
