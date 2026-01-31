using System.Collections;
using UnityEngine;

public enum ToolTier //set this in on spawn depending on unitManager
{
    None,
    Stone,
    Bronze,
    Iron,
    Steel
}
public enum ClothesTier
{
    None,
    Skin,
    Hide,
    Cloth
}
public class UnitEquipment : MonoBehaviour
{
    public bool hasTools;
    public ToolTier toolTier=ToolTier.Stone;

    [SerializeField] public int toolLevel;
    [SerializeField] public int weaponLevel;
    [SerializeField] public int clothesLevel;

    [SerializeField] GameObject axe; //[] axes [0=woodAxe, [1]=stone axe etc.]
    [SerializeField] GameObject pickAxe;
    [SerializeField] GameObject fishingRod;
    [SerializeField] GameObject spear;
    [SerializeField] GameObject thrownSpear;
    Unit unit;

    [SerializeField] GameObject[] axes;
    [SerializeField] GameObject[] pickAxes;
    [SerializeField] GameObject[] spears;


    private void Start()
    {
        unit = GetComponent<Unit>();

        toolLevel=EquipmentManager.inst.toolLevel;
        weaponLevel=EquipmentManager.inst.weaponLevel;
        clothesLevel=EquipmentManager.inst.clothesLevel;

        UneqipTools();

    }

    public void UneqipTools()
    {
        if (axe.activeSelf) axe.SetActive(false);
        if (pickAxe.activeSelf) pickAxe.SetActive(false);
        if(fishingRod.activeSelf)fishingRod.SetActive(false);
        if(spear.activeSelf)spear.SetActive(false);
    }
    public void EquipAxe()
    {
        if(!axe.activeSelf)axe.SetActive(true);
    }
    public void EquipPickAxe()
    {
        if (!pickAxe.activeSelf) pickAxe.SetActive(true);
    }
    public void EquipFishingRod()
    {
        if (!fishingRod.activeSelf) fishingRod.SetActive(true);
    }
    public void EquipSpear()
    {
        if(!spear.activeSelf)spear.SetActive(true);
    }

    public void TryThrowSpear() //call from animation event
    {
        if (unit.huntTarget != null)
        {
            ThrowSpear(unit.huntTarget.Position);
        }
        else
        {
            Vector3 targetPos = unit.transform.position + unit.transform.forward * 5f;
            ThrowSpear(targetPos);
        }
    }
    void ThrowSpear(Vector3 targetPosition)
    {
        if(spear.activeInHierarchy)spear.SetActive(false);
        thrownSpear.SetActive(true);
        StartCoroutine(AnimateSpear(targetPosition));
    }

    private IEnumerator AnimateSpear(Vector3 targetPosition)
    {
        if(!thrownSpear.activeInHierarchy)thrownSpear.SetActive(true);
        Vector3 start = thrownSpear.transform.position;
        float duration = 0.25f;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime / duration;
            thrownSpear.transform.position = Vector3.Lerp(start, targetPosition, t);
            yield return null;
        }

        thrownSpear.transform.position=start;
        thrownSpear.SetActive(false);
    }
}
