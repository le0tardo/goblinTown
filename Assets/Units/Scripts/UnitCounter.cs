using UnityEngine;
using TMPro;

public class UnitCounter : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI unitCount;

    private void Start()
    {
        unitCount.text = UnitManager.inst.units.Count.ToString()+"/"+UnitManager.inst.maxUnits.ToString();
    }

    private void Update() //sloppy or lazy?
    {
        unitCount.text = UnitManager.inst.units.Count.ToString() + "/" + UnitManager.inst.maxUnits.ToString();
    }
    public void UpdateUnitCount()
    {
        unitCount.text = UnitManager.inst.units.Count.ToString() + "/" + UnitManager.inst.maxUnits.ToString();
    }
}
