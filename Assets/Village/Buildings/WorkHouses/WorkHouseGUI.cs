using UnityEngine;
using UnityEngine.UI;

public class WorkHouseGUI : MonoBehaviour
{
    WorkHouseBehaviour whb;

    [SerializeField] Button fireWorkerButton;
    private void Update()
    {
        whb = null;
        if (BuildingManager.inst.selectedBuilding != null)
        {
            whb = BuildingManager.inst.selectedBuilding.GetComponent<WorkHouseBehaviour>();
            if (whb != null)
            {
                fireWorkerButton.interactable=!whb.needsWorker;
            }
        }
    }

    public void FireWorker()
    {
        if (whb != null)
        {
            whb.FireWorker();
        }
    }
}
