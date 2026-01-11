using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class BuildingGUI : MonoBehaviour
{
    [SerializeField] GameObject selectedBuildingPanel;
    [SerializeField] TextMeshProUGUI buildingName;
    [SerializeField] Vector3 screenOffset = new Vector3(0, 30, 0);

    Camera cam;
    BuildingBehaviour lastBuilding;

    [SerializeField] GameObject storageUI;
    [SerializeField] GameObject domicileUI;
    [SerializeField] GameObject factoryUI;

    void Awake()
    {
        cam = Camera.main;
        selectedBuildingPanel.SetActive(false);
    }
    private void Start()
    {
        HideTypeUI();
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

        // New selection
        if (building != lastBuilding)
        {
            NewBuilding(building);
        }

        if (building != null)
        {
            Transform t = ((MonoBehaviour)building).transform;
            Vector3 screenPos = cam.WorldToScreenPoint(t.position);
            transform.position = screenPos + screenOffset;
        }
    }

    void NewBuilding(BuildingBehaviour building)
    {
        selectedBuildingPanel.SetActive(true);

        buildingName.text = building.building.buildingName;

        switch (building.building.buildingType)
        {
            case BuildingType.Deposit:
                HideTypeUI();
                storageUI.SetActive(true);
                break;
            case BuildingType.Domicile:
                HideTypeUI();
                domicileUI.SetActive(true);
                break;
            case BuildingType.Factory:
                HideTypeUI();
                factoryUI.SetActive(true);
                break;
        }

        lastBuilding = building;
        // Follow building in screen space
        Transform t = ((MonoBehaviour)building).transform;
        Vector3 screenPos = cam.WorldToScreenPoint(t.position);
        transform.position = screenPos + screenOffset;
    }

    void HideTypeUI()
    {
        storageUI.SetActive(false);
        domicileUI.SetActive(false);
        factoryUI.SetActive(false);
    }

    public void ClickFactory()
    {
        FactoryBehaviour factory=BuildingManager.inst.selectedBuilding.gameObject.GetComponent<FactoryBehaviour>();
        if (factory == null)
        {
            return;
        }
        factory.ToggleWork();
    }
}
