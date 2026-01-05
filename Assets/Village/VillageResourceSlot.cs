using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class VillageResourceSlot : MonoBehaviour
{
    [SerializeField]Image img;
    [SerializeField]TextMeshProUGUI txt;
    public void Set(MacroResourceCategory data, int amount)
    {
        img.sprite = data.macroIcon;
        txt.text = amount.ToString();
    }

}
