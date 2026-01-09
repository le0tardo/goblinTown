using UnityEngine;

public class HoverManager : MonoBehaviour
{
    IHoverable currentHover;
    public string hoverString;

    void Update()
    {
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
    }

    void ClearHover()
    {
        if (currentHover != null)
        {
            hoverString = "";
            currentHover = null;
        }
    }
}
