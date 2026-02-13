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
    [SerializeField] TextMeshProUGUI toolLevel;
    [SerializeField] TextMeshProUGUI clothesLevel;
    UnitStatus cachedStatus;
    Unit cachedUnit;
    UnitEquipment cachedEquip;

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
            cachedEquip=unit.GetComponent<UnitEquipment>();
        }

        if (cachedUnit != null && cachedStatus != null &&cachedEquip!=null)
        {
            unitName.text = cachedStatus.unitName;
            healthAmount.text = cachedStatus.hp.ToString() + "/" + cachedStatus.maxHp.ToString();
            if (unit.carriedAmount <= 0){carryAmount.text = "0/"+unit.carryCapacity.ToString(); }
            else { carryAmount.text = unit.carriedAmount.ToString() +"/" + unit.carryCapacity.ToString(); }

            if (unit.carriedResource == null) { carryIcon.sprite = null;carryIcon.enabled = false; }
            else { carryIcon.enabled = true; carryIcon.sprite = unit.carriedResource.resourceSprite; }

            switch (cachedEquip.toolLevel)
            {
                case 0:
                    toolLevel.text = "Wood";
                    break;
                case 1:
                    toolLevel.text = "Stone";
                    break;
                 case 2:
                    toolLevel.text = "Iron";
                    break;
                 case 3:
                    toolLevel.text = "Steel";
                    break;
                 default:
                    toolLevel.text = "ERROR";
                    break;
            }

            switch (cachedEquip.clothesLevel)
            {
                case 0:
                    clothesLevel.text = "None";
                    break;
                 case 1:
                    clothesLevel.text = "Skin";
                    break;
                case 2:
                    clothesLevel.text = "Leather";
                    break;
                case 3:
                    clothesLevel.text = "Cloth";
                    break;
                default:
                    clothesLevel.text = "ERROR";
                    break;
            }
        }

    }
}
