using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FactoryCostSlot : MonoBehaviour
{
    [SerializeField] public Image icon;
    [SerializeField] public TMP_Text text;
    public float resourceRequired=0;
    public float resourceAvailable=0;

    private void Update()
    {
        if (resourceAvailable >= resourceRequired)
        {
            text.color = Color.white;
        }
        else
        {
            text.color= Color.red;
        }
    }


}
