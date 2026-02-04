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
    [SerializeField] public int clothesLevel;

    [Header("Tools")]
    [SerializeField] GameObject fishingRod;
    [SerializeField] GameObject thrownSpear;
    Unit unit;

    [SerializeField] GameObject[] axes;
    [SerializeField] GameObject[] pickAxes;
    [SerializeField] GameObject[] spears;
    [SerializeField] GameObject[] thrownSpears;

    [Header("Clothes")]
    [SerializeField] GameObject[] clothes;


    private void Start()
    {
        unit = GetComponent<Unit>();

        toolLevel=EquipmentManager.inst.toolLevel;
        clothesLevel=EquipmentManager.inst.clothesLevel;

        UneqipTools();

    }

    public void UneqipTools()
    {
        foreach (GameObject axe in axes)
        {
            axe.SetActive(false);
        }

        foreach(GameObject paxe in pickAxes)
        {
            paxe.SetActive(false);
        }

        if(fishingRod.activeSelf)fishingRod.SetActive(false);
       if(thrownSpear.activeSelf)thrownSpear.SetActive(false);
        foreach(GameObject spear in spears)
        {
            spear.SetActive(false);
        }
    }
    public void EquipAxe()
    {
        int toolTier = toolLevel - 1;
        if (!axes[toolTier].activeSelf) axes[toolTier].SetActive(true);
    }
    public void EquipPickAxe()
    {
        int toolTier = toolLevel - 1;
        if (!pickAxes[toolTier].activeSelf) pickAxes[toolTier].SetActive(true);
    }
    public void EquipFishingRod()
    {
        if (!fishingRod.activeSelf) fishingRod.SetActive(true);
    }
    public void EquipSpear()
    {
        int toolTier = toolLevel - 1;
        if (!spears[toolTier].activeSelf) spears[toolTier].SetActive(true);
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
        if (spears[toolLevel - 1].activeInHierarchy) spears[toolLevel-1].SetActive(false);

        thrownSpear.SetActive(true);
        StartCoroutine(AnimateSpear(targetPosition));
    }

    private IEnumerator AnimateSpear(Vector3 targetPosition)
    {
        if(!thrownSpear.activeInHierarchy)thrownSpear.SetActive(true);

        if (!thrownSpears[toolLevel-1].activeInHierarchy)thrownSpears[toolLevel-1].SetActive(true);

        Vector3 start = thrownSpear.transform.position; // make empty transform to move!
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

    public void EquipClothes()
    {

        for (int i = 0; i < clothes.Length; i++)
        {
            clothes[i].gameObject.SetActive(false);
        }
        if (clothesLevel > 0)
        {
            clothes[clothesLevel - 1].SetActive(true);
        }
    }
}
