using UnityEngine;

public class CarryItem : MonoBehaviour
{
    [SerializeField]Unit unit;
    [SerializeField]MeshFilter filter;
    [SerializeField]MeshRenderer mesh;

    private void OnEnable()
    {
        if (unit.carriedResource != null)
        {
            filter.mesh = unit.carriedResource.mesh;
            mesh.material = unit.carriedResource.material;
        }
    }
}
