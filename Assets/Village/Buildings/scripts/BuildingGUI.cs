using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildingGUI : MonoBehaviour
{
    [SerializeField] GameObject selectedBuildingPanel;
    [SerializeField] TextMeshProUGUI buildingName;
    [SerializeField] Vector3 screenOffset = new Vector3(0, 30, 0);

    Camera cam;
    BuildingBehaviour lastBuilding;

    void Awake()
    {
        cam = Camera.main;
        selectedBuildingPanel.SetActive(false);
    }

    void LateUpdate()
    {
        var building = BuildingManager.inst.selectedBuilding;

        if (building == null)
        {
            if (selectedBuildingPanel.activeSelf)
                selectedBuildingPanel.SetActive(false);

            lastBuilding = null;
            return;
        }

        // New selection?
        if (building != lastBuilding)
        {
            selectedBuildingPanel.SetActive(true);

            buildingName.text =building.building.buildingName;

            lastBuilding = building;
        }

        // Follow building in screen space
        Transform t = ((MonoBehaviour)building).transform;
        Vector3 screenPos = cam.WorldToScreenPoint(t.position);
        transform.position = screenPos + screenOffset;
    }
}
