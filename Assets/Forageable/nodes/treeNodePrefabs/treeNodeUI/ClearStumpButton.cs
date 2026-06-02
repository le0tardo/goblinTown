using UnityEngine;

public class ClearStumpButton : MonoBehaviour
{
    public void ClearStump(GameObject clear)
    {
        UnitManager.inst.ClearSelection();
        Destroy(clear);
        transform.position = Vector3.zero;
    }
}
