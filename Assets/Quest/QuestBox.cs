using UnityEngine;

public class QuestBox : MonoBehaviour
{
    float startX;
    float endX;

    [SerializeField] bool show=true;
    private void Start()
    {
        startX = transform.position.x;
        endX = transform.position.x+393;
    }

    public void ToggleBox()
    {
        show= !show;
    }

    private void Update()
    {
        if (show)
        {
            transform.position =new Vector3(startX,transform.position.y,transform.position.z);
        }
        else
        {
            transform.position = new Vector3(endX, transform.position.y, transform.position.z);
        }
    }
}
