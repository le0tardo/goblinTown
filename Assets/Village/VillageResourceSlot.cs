using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VillageResourceSlot : MonoBehaviour
{
    [SerializeField]Image img;
    [SerializeField]TextMeshProUGUI txt;
    public void Set(VillageResource data, int amount)
    {
        img.sprite = data.resourceIcon;
        txt.text = amount.ToString();
    }

}
