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

}
