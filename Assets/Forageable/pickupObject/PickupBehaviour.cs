using System.Collections;
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

    [SerializeField] int waitSeconds;
    SphereCollider coll;

    [SerializeField] bool respaw;

    private void Start()
    {
        displayName =resource.resourceName;
        rend = GetComponentInChildren<MeshRenderer>();
        filt = GetComponentInChildren<MeshFilter>();
        respaw = true;
        filt.mesh=resource.mesh;
        rend.material=resource.material;

        float randomY = Random.Range(0f, 360f);
        transform.rotation = Quaternion.Euler(0f, randomY, 0f);
        coll=GetComponent<SphereCollider>();
    }

    public bool TryPickup(Unit unit)
    {
        if (claimed)
            return false;

        claimed = true;

        if(unit.carriedResource==null)unit.carriedResource = resource;
        unit.carriedAmount++;

        if (respaw)
        {
            StartCooldown();
        }
        else
        {
           Destroy(gameObject);
        }
        return true;
    }

    public void StartCooldown()
    {
        StartCoroutine(DisableTemporarily());
    }

    IEnumerator DisableTemporarily()
    {
        rend.enabled = false;
        coll.enabled = false;

        yield return new WaitForSeconds(waitSeconds);

        CheckOverlappingColliders();

        rend.enabled = true;
        coll.enabled = true;
        claimed=false;
    }

    void CheckOverlappingColliders()
    {
        float radius = coll.radius * transform.lossyScale.x;

        Collider[] overlaps = Physics.OverlapSphere(
            transform.position,
            radius
        );

        if (overlaps.Length > 1)
        {
            Destroy(this.gameObject);
        }
    }

}
