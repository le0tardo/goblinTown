using UnityEngine;

public class EquipmentManager : MonoBehaviour
{
    public static EquipmentManager inst;

    [SerializeField] public int toolLevel=0;
    [SerializeField] public int clothesLevel=0;

    private void Awake()
    {
        inst = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            print("pressed c");
            SetUnitClothes();
        }
    }

    public void SetUnitClothes()
    {
        print("setting clothes..");
        foreach(Unit unit in UnitManager.inst.units)
        {
            UnitEquipment eq=unit.GetComponent<UnitEquipment>();
            eq.EquipClothes();
        }
    }
}
