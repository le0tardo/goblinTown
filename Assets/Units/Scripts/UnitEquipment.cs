using UnityEngine;

public class UnitEquipment : MonoBehaviour
{
    [SerializeField] GameObject axe;
    [SerializeField] GameObject pickAxe;
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
    }
    public void EquipAxe()
    {
        if(!axe.activeSelf)axe.SetActive(true);
    }
    public void EquipPickAxe()
    {
        if (!pickAxe.activeSelf) pickAxe.SetActive(true);
    }

}
