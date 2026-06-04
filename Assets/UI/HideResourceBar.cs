using UnityEngine;
using UnityEngine.UI;

public class HideResourceBar : MonoBehaviour
{
    RectMask2D mask;
    [SerializeField] float left = -33;
    [SerializeField] float showMask = 0;
    [SerializeField] float hideMask = 1450;
    bool show = true;
    private void Start()
    {
        mask=GetComponent<RectMask2D>();
    }

    public void ToggleMask()
    {
        if (show)
        {
            //hide
            mask.padding = new Vector4(left, 0, hideMask, 0);
            show=false;
        }
        else
        {
            //show
            mask.padding = new Vector4(left, 0, showMask, 0);
            show=true;
        }
    }
}
