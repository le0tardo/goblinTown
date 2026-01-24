using System.Collections;
using UnityEngine;

public enum ToolTier //set this in on spawn depending on unitManager
{
    None,
    Stone,
    Iron,
    Steel
}
public class UnitEquipment : MonoBehaviour
{
    public bool hasTools;
    public ToolTier toolTier=ToolTier.Stone;

    public int clothesTier=0; //0 = naked, 1= loin cloth, etc. 

    [SerializeField] GameObject axe; //[] axes [0=woodAxe, [1]=stone axe etc.]
    [SerializeField] GameObject pickAxe;
    [SerializeField] GameObject fishingRod;
    [SerializeField] GameObject spear;
    [SerializeField] GameObject thrownSpear;
    Unit unit;


    private void Start()
    {
        unit = GetComponent<Unit>();
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

    }
    void ThrowSpear(Vector3 targetPosition)
    {
        if(spear.activeInHierarchy)spear.SetActive(false);
        thrownSpear.SetActive(true);
        StartCoroutine(AnimateSpear(targetPosition));
    }

    private IEnumerator AnimateSpear(Vector3 targetPosition)
    {
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
