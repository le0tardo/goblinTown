using UnityEngine;

public class ProductionCostGUI : MonoBehaviour
{
    [SerializeField] GameObject[] productionCostSlots;
    FactoryBehaviour currentFactory;

    private void Awake()
    {
        HideSlots();
    }

    void Update()
    {
        if (currentFactory != null)
            ShowCost(currentFactory);
    }

    public void SetFactory(FactoryBehaviour factory)
    {
        currentFactory = factory;
        ShowCost(factory);
    }

    public void ShowCost(FactoryBehaviour factory)
    {


        for (int i = 0; i < factory.input.Length; i++)
        {
            productionCostSlots[i].SetActive(true);
            FactoryCostSlot slot =productionCostSlots[i].GetComponent<FactoryCostSlot>();
            if (slot != null)
            {
                slot.icon.sprite=factory.input[i].resource.resourceIcon;
                slot.text.text = factory.input[i].amount.ToString();

                if (VillageResourceManager.inst.villageResources.TryGetValue(factory.input[i].resource, out int available))
                {
                    slot.resourceAvailable = available;
                }
                else
                {
                    slot.resourceAvailable = 0;
                }

                slot.resourceRequired = factory.input[i].amount;
            }
        }
    }
    void HideSlots()
    {
        for (int i = 0; i < productionCostSlots.Length; i++)
        {
            productionCostSlots[i].gameObject.SetActive(false);
        }
    }

    bool ResourcesChanged()
    {
        for (int i = 0; i < currentFactory.input.Length; i++)
        {
            var io = currentFactory.input[i];

            if (!VillageResourceManager.inst.villageResources
                .TryGetValue(io.resource, out int available))
                available = 0;

            if (productionCostSlots[i]
                .GetComponent<FactoryCostSlot>()
                .resourceAvailable != available)
                return true;
        }

        return false;
    }


}
