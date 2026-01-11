using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class FactoryGUI : MonoBehaviour
{
    [SerializeField] Image productionProgress;
    [SerializeField] TextMeshProUGUI produceText;


    private void Update()
    {
        if (BuildingManager.inst.selectedBuilding.building.buildingType == BuildingType.Factory)
        {
            FactoryBehaviour factory = BuildingManager.inst.selectedBuilding.gameObject.GetComponent<FactoryBehaviour>();
            if (factory != null)
            {
                productionProgress.fillAmount = factory.productionTime/factory.maxProductionTime;

                if (factory.working)
                {
                    produceText.text = "Producing...";
                }
                else
                {
                    produceText.text = "Produce";
                }
            }
        }
    }
}
