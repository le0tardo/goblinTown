using UnityEngine;
using UnityEngine.UI;

public class HoverManager : MonoBehaviour
{
    IHoverable currentHover;
    public string hoverString;

    [SerializeField] Image cursor;
    [SerializeField] Sprite cursorDefault;
    [SerializeField] Sprite cursorWalk;

    void Start()
    {
       if(cursor!=null) Cursor.visible = false;
    }
    void Update()
    {
        cursor.transform.position=Input.mousePosition;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 500f))
        {
            IHoverable hoverable = hit.collider.GetComponentInParent<IHoverable>();

            if (hoverable != null)
            {
                if (hoverable != currentHover)
                {
                    currentHover = hoverable;
                    hoverString=hoverable.DisplayName;
                }
                return;
            }
        }

        ClearHover();
        ChangeCursor();
    }
    void ClearHover()
    {
        if (currentHover != null)
        {
            hoverString = "";
            currentHover = null;
        }
    }

    void ChangeCursor()
    {
        //uh...
    }
}
