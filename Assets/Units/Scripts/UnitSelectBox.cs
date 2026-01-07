using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitSelectBox : MonoBehaviour
{
    [Header("Solo Box")]
    [SerializeField] TextMeshProUGUI unitName;
    [SerializeField] TextMeshProUGUI healthAmount;
    [SerializeField] TextMeshProUGUI carryAmount;
    [SerializeField] Image carryIcon;
    UnitStatus cachedStatus;
    Unit cachedUnit;

    private void Update()
    {
        if(UnitManager.inst.selectedUnits.Count<=0) return;
        var unit = UnitManager.inst.selectedUnits[0];
        if (unit == null)
        {
            cachedUnit = null; 
            return;
        }

        if (unit != cachedUnit)
        {
            cachedUnit = unit;
            cachedStatus = unit.GetComponent<UnitStatus>();
        }

        if (cachedUnit != null && cachedStatus != null)
        {
            unitName.text = cachedStatus.unitName;
            healthAmount.text = cachedStatus.hp.ToString() + "/" + cachedStatus.maxHp.ToString();
            if (unit.carriedAmount <= 0){carryAmount.text = "";}
            else{carryAmount.text = unit.carriedAmount.ToString();}

            if (unit.carriedResource == null) { carryIcon.sprite = null;carryIcon.enabled = false; }
            else { carryIcon.enabled = true; carryIcon.sprite = unit.carriedResource.resourceSprite; }
        }

    }
}
