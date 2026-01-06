using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class BuildButton : MonoBehaviour
{
    [SerializeField] BuildingObject building;
    BuildingCost[] cost;

    [Header("UI")]
    [SerializeField] Image image;
    [SerializeField] TextMeshProUGUI title;
    [SerializeField] TextMeshProUGUI type;
    [SerializeField] BuildCostSlot[] costSlots;
    Button button;

    public bool canAfford =false;

    private void Start()
    {
        button = GetComponent<Button>();
        if (building != null)
        {
            title.text = building.buildingName;
            type.text = building.buildingType.ToString();
            image.sprite = building.buildingIcon;
        }

        for (int i = 0; i < costSlots.Length; i++)
        {
            costSlots[i].gameObject.SetActive(false);
        }

        for (int i = 0; i < building.costs.Length; i++)
        {
            costSlots[i].gameObject.SetActive(true);
            costSlots[i].SetValue(building.costs[i].resource.resourceIcon,building.costs[i].amount);
        }
    }

    void Update()
    {
        button.interactable = CanAffordBuilding(building);

        for(int i=0; i < costSlots.Length; i++)
        {
            if (!costSlots[i].gameObject.activeInHierarchy)
            {
                return;
            }
            costSlots[i].SetColor(CanAffordBuilding(building));
        }
        
    }

    bool CanAffordBuilding(BuildingObject building)
    {
        var village = VillageResourceManager.inst;

        foreach (var cost in building.costs)
        {
            if (!village.villageResources.TryGetValue(cost.resource, out int available))
                return false;

            if (available < cost.amount)
                return false;
        }

        return true;
    }



    public void ClickBuild()
    {
        UnitManager.inst.ClearSelection();
        BuildingManager.inst.SetBuilding(building);
    }

}
