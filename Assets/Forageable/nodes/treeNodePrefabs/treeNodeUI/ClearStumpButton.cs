using UnityEngine;

public class ClearStumpButton : MonoBehaviour
{
    Vector3 startPos;

    private void Start()
    {
        startPos = transform.position;
    }
    public void ClearStump(GameObject clear)
    {
        UnitManager.inst.ClearSelection();
        Destroy(clear);
        transform.position = startPos;
    }
}
