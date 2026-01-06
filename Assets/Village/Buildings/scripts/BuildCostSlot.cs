using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BuildCostSlot : MonoBehaviour
{
    public Image slotImage;
    public TextMeshProUGUI slotText;
    public VillageResource resource;
    public int requiredAmount;
    private void Awake()
    {
        slotImage = GetComponentInChildren<Image>();
        slotText = GetComponentInChildren<TextMeshProUGUI>();
    }

    public void SetValue(VillageResource res,Sprite spr, int val)
    {
        slotImage.sprite = spr;
        slotText.text=val.ToString();
        resource = res;
        requiredAmount = val;
    }

    public void UpdateColor(int available)
    {
        slotText.color = available >= requiredAmount
            ? Color.white
            : Color.red;
    }
    public void SetColor(bool canAfford)
    {
        slotText.color = canAfford ? Color.white : Color.red;
    }

}
