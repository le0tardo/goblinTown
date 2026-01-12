using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;


public class DomicileGUI : MonoBehaviour
{
    DomicileBehaviour dom;

    [Header("UI")]
    [SerializeField] TextMeshProUGUI unitCount;
    [SerializeField] TextMeshProUGUI unitCost;
    [SerializeField] Image costResource;
    [SerializeField] Image spawnProgression;
    [SerializeField] Button spawnButton;

    private void Start()
    {
        if (BuildingManager.inst.selectedBuilding == null) return;
        dom = BuildingManager.inst.selectedBuilding.GetComponent<DomicileBehaviour>();
        if (dom != null)
        {
            unitCost.text = dom.required.ToString();
            costResource.sprite = dom.resource.resourceIcon;
        }
    }

    private void Update()
    {
        if(BuildingManager.inst.selectedBuilding==null)return;
        dom=BuildingManager.inst.selectedBuilding.GetComponent<DomicileBehaviour>();
        if (dom != null)
        {
            spawnProgression.fillAmount = dom.spawnTime / dom.maxSpawnTime;
            unitCount.text = UnitManager.inst.units.Count.ToString() + "/" + UnitManager.inst.maxUnits.ToString();
            SetColors();
            spawnButton.interactable = dom.canSpawn;
        }
    }
   
    public void ClickDomicile()
    {
        dom = BuildingManager.inst.selectedBuilding.GetComponent<DomicileBehaviour>();
        if (dom == null) { return; }
        dom.StartSpawn();
    }

    void SetColors()
    {
        if (dom.required > dom.available)
        {
            unitCost.color = Color.red;
        }
        else
        {
            unitCost.color = Color.white;
        }
        if (UnitManager.inst.units.Count < UnitManager.inst.maxUnits)
        {
            unitCount.color = Color.white;
        }
        else
        {
            unitCount.color = Color.red;
        }
    }
}
