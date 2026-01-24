using UnityEngine;
using UnityEngine.AI;

public enum AnimalState
{
    Idle,
    Walk,
    Run,
    Dead
}
public class AnimalBehaviour : MonoBehaviour, IHuntable
{
    [SerializeField] public AnimalObject animalObject;
    NavMeshAgent agent;
    public AnimalState animalState;

    Vector3 position;
    public Vector3 Position =>position;

    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        position= transform.position;

        switch (animalState)
        {
            case AnimalState.Idle:
                //wait for random time
                break;
                case AnimalState.Walk:
                    //get new random position on navmesh, move there * walkSpeed
                break;
                case AnimalState.Run:
                    //get new random position on navmesh, move there * runSpeed
                break;
                case AnimalState.Dead:
                    //drop animalObject.drop
                    //no wait! become foragable node!
                break;
        }
    }

   public void OnHit(int damage, Unit attacker)
    {
        Debug.Log(animalObject.title+ ": "+ attacker.name +" hit me!");
    }
}
