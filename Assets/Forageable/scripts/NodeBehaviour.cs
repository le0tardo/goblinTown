using UnityEngine;

public class NodeBehaviour : MonoBehaviour,IForageable
{
    [SerializeField] public ForageableNodeData node; //what node this is
    [SerializeField] public ForagedResourceData resource; //what resource it drops
    public bool isDepleted=false;
    public bool IsDepleted => isDepleted;
   
    public ForageableNodeData NodeData => node;
    public ForagedResourceData Resource => resource;

    public int resourceAmount = 10;
    public int resourceCount = 10;
    public Vector3 Position => transform.position;

    [SerializeField] float interactionRange = 1f; //dont think this is used
    public float InteractionRange => interactionRange; //or this... but need for interface atm...

    [SerializeField] Animator anim;
    public bool renewableResource = true;
    [SerializeField] float respawnTime=10f; //set from scriptable object class?

    [SerializeField] bool randomRotationSpawn = true;

    private void Start()
    {
        if (anim != null) anim.enabled = false;
        float r = Random.Range(0,360);
        if(randomRotationSpawn)transform.rotation = new Quaternion(0,r,0,0);
    }
    public void Forage(Unit unit)
    {
        if (this == null)
            return;
        if (isDepleted)
            return;
        // If carrying something else, abort
        if (unit.carriedResource != null && unit.carriedResource != resource)
            return;

        // If full, abort
        if (unit.carriedAmount >= unit.carryCapacity)
        {
            return;
        }

        // Assign resource type if empty
        if (unit.carriedResource == null)
            unit.carriedResource = resource;

        // Harvest
        unit.carriedAmount++;
        resourceAmount--;

        // Deplete node
        if (resourceAmount <= 0)
        {
            if (anim != null&&!IsDepleted)
            {
                anim.enabled = true;
                anim.SetTrigger("depleat");
                Invoke("DisableAnim", 1f);
                if (renewableResource)
                {
                    Invoke("Respawn", respawnTime);
                }
                else
                {
                    Invoke("DestroyNode",1f);
                }

            }
            isDepleted = true;
        }

        if (unit.carriedAmount >= unit.carryCapacity)
        {
            unit.state=Unit.UnitState.Idle;
        }
    }

    void Respawn()
    {
        resourceAmount = resourceCount;
        isDepleted=false;
        if (anim != null)
        {
            anim.enabled = true;
            anim.SetTrigger("grow");
            Invoke("DisableAnim", 1f);
        }
    }
    void DisableAnim() 
    {
        if (anim.enabled)
        {
            anim.enabled=false;
        }
    }

    void DestroyNode()
    {
        Destroy(gameObject);
    }
}
