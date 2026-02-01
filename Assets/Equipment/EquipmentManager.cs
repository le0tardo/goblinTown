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
}
