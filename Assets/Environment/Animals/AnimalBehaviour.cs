using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public enum AnimalState
{
    Idle,
    Walking,
    Fleeing,
    Dead
}
public class AnimalBehaviour : MonoBehaviour, IHuntable
{
    [SerializeField] public AnimalObject animalObject;
    NavMeshAgent agent;
    public AnimalState animalState;

    Vector3 position;
    Vector3 targetPosition;
    public Vector3 Position =>position;

    [SerializeField] float hp;
    [SerializeField] float maxHp;
    [SerializeField] float walkSpeed;
    [SerializeField] float runSpeed;

    bool isDead = false;

    Coroutine fleeRoutine;
    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animalState=AnimalState.Idle;
    }

    private void Update()
    {
        position= transform.position;


        switch (animalState)
        {
            case AnimalState.Idle:
                //wait for random time
                break;
                case AnimalState.Walking:
                    //get new random position on navmesh, move there * walkSpeed
                break;
                case AnimalState.Fleeing:
                    //get new random position on navmesh, move there * runSpeed
                break;
                case AnimalState.Dead:
                    //drop animalObject.drop
                    //no wait! become foragable node!
                break;
        }

        if (agent == null) return;

        if (animalState == AnimalState.Fleeing)
        {
            if (!agent.pathPending &&
                agent.remainingDistance <= agent.stoppingDistance)
            {
                agent.ResetPath();
                animalState = AnimalState.Idle;
                Debug.Log("I have stopped running");
            }
        }
    }

   public void OnHit(int damage, Unit attacker)
    {
        Debug.Log(animalObject.title+ ": "+ attacker.name +" hit me!");
        hp-=damage;

        if (hp <= 0)
        {
            if (!isDead)
            {
                Invoke("Die",1f);
                isDead = true;
            }
            //Die(); //ivoke 1 sec here too?? crazy if it works..
        }
        else
        {
            //Flee(attacker.transform);
            if (fleeRoutine != null)
            {
                StopCoroutine(fleeRoutine);
            }
            fleeRoutine = StartCoroutine(FleeRoutine(attacker.transform, 1f));
        }
    }

    private IEnumerator FleeRoutine(Transform attacker, float delay)
    {
        yield return new WaitForSeconds(delay);
        Flee(attacker);
        fleeRoutine = null; // clear the reference
    }

    public void Flee(Transform attacker)
    {
        if (agent == null) return;
        if (animalState == AnimalState.Fleeing) return;
        if (attacker == null) return;

        animalState = AnimalState.Fleeing;
        agent.speed = runSpeed;

        const float fleeDistance = 10f;
        const int maxAttempts = 6;

        // Base direction: directly away from attacker
        Vector3 baseDir = transform.position - attacker.position;
        baseDir.y = 0f;

        if (baseDir.sqrMagnitude < 0.01f)
            baseDir = transform.forward;

        baseDir.Normalize();

        for (int i = 0; i < maxAttempts; i++)
        {
            // Add some randomness (cone)
            Vector3 dir = Quaternion.Euler(
                0f,
                Random.Range(-30f, 30f),
                0f
            ) * baseDir;

            Vector3 rawTarget = transform.position + dir * fleeDistance;

            if (NavMesh.SamplePosition(rawTarget, out NavMeshHit hit, 2f, NavMesh.AllAreas))
            {
                float dist = Vector3.Distance(transform.position, hit.position);

                if (dist >= fleeDistance * 0.8f)
                {
                    agent.SetDestination(hit.position);
                    return;
                }
            }
        }

        agent.SetDestination(transform.position + baseDir * (fleeDistance * 0.5f));
    }
    void Die()
    {
        //dead
        //mainly for animation, invoke destroyandcreate
        Destroy(gameObject);
    }

    void DestroyAndCreate()
    {
        //instantiate node prefab
        //destroy game object
    }

}
