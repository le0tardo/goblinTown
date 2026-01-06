using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildCostSlot : MonoBehaviour
{
    public Image slotImage;
    public TextMeshProUGUI slotText;

    private void Awake()
    {
        slotImage = GetComponentInChildren<Image>();
        slotText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetValue(Sprite spr, int val)
    {
        slotImage.sprite = spr;
        slotText.text=val.ToString();
    }
    public void SetColor(bool canAfford)
    {
        slotText.color = canAfford ? Color.white : Color.red;
    }

}
