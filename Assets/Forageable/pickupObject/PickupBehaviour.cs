using UnityEngine;

public class PickupBehaviour : MonoBehaviour, IPickupable, IHoverable
{
    [SerializeField] public ForagedResourceData resource;
    public ForagedResourceData Resource=>resource;

    string displayName;
    public string DisplayName => displayName;

    MeshRenderer rend;
    MeshFilter filt;

    bool claimed=false;

    private void Start()
    {
        displayName =resource.resourceName;
        rend = GetComponentInChildren<MeshRenderer>();
        filt = GetComponentInChildren<MeshFilter>();

        filt.mesh=resource.mesh;
        rend.material=resource.material;

        float randomY = Random.Range(0f, 360f);
        transform.rotation = Quaternion.Euler(0f, randomY, 0f);
    }

    public bool TryPickup(Unit unit)
    {
        if (claimed)
            return false;

        claimed = true;

        if(unit.carriedResource==null)unit.carriedResource = resource;
        unit.carriedAmount++;

        Destroy(gameObject);
        return true;
    }
}
