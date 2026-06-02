using UnityEngine;
using UnityEngine.InputSystem;

public class StumpClick : MonoBehaviour
{
    [SerializeField] GameObject destroyButton;
    [SerializeField] GameObject objectToDestroy;
    ClearStumpButton clearStump;

    private void Awake()
    {
        if (destroyButton == null)
        {
             destroyButton= GameObject.Find("KillStumpButton");
        }
    }

    private void OnMouseEnter()
    {
        destroyButton.transform.position = Input.mousePosition;
    }

    private void OnMouseDown()
    {
        //destroyButton.transform.position = Input.mousePosition;
        clearStump=destroyButton.GetComponent<ClearStumpButton>();
        clearStump.ClearStump(objectToDestroy);
    }

    private void OnMouseExit()
    {
        destroyButton.transform.position=Vector3.zero;
    }
}
