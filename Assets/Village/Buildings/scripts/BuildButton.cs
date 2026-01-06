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
    Button button;

    public bool canAfford =false;

    private void Awake()
    {
        button = GetComponent<Button>();
        if (building != null)
        {
            title.text = building.buildingName;
            type.text = building.buildingType.ToString();
            image.sprite = building.buildingIcon;
        }
    }

    private void OnEnable()
    {
        if (CanAffordBuilding(building))
        {
            button.enabled = true;
        }
        else
        {
            button.enabled = false;
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
        BuildingManager.inst.SetBuilding(building);
    }

}
