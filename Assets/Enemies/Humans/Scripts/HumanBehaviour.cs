using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HumanBehaviour : MonoBehaviour
{
    [Header("Combat Stats")]
    [SerializeField] int hp = 10;
    [SerializeField] int maxHp = 10;
    [SerializeField] int damage = 3;
    [SerializeField] float despawnTime = 10f;
    [SerializeField] float despawnTimer = 10f;
    [SerializeField] bool doneFighting = false;

    [SerializeField] float distanceToTarget;

    public bool dead=false;

    enum HumanState
    {
        Idle,
        Moving,
        Attacking,
        Dead
    }

    [SerializeField] HumanState state;
    UnitStatus[] targets;
    [SerializeField] UnitStatus target;

    [Header("Movement")]
    NavMeshAgent agent;
    Vector3 startPos;

    [Header("Graphics")]
    Animator anim;

    Coroutine currentRoutine;

    private void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        startPos = transform.position;
        anim = GetComponentInChildren<Animator>();
    }

    private void OnEnable()
    {
        target = null;
        despawnTimer = despawnTime;
        state = HumanState.Moving;
        doneFighting = false;
        hp = maxHp;
        dead = false;

        if (anim != null)
        {
            Animate();
        }

        FindTargets();
    }

    void FindTargets()
    {
        UnitStatus[] allUnits = FindObjectsByType<UnitStatus>(FindObjectsSortMode.None);

        // --- NO GOBLINS ---
        if (allUnits.Length == 0)
        {
            //print("no goblins found - fallback state");
            targets = new UnitStatus[0];
            target = null;

            doneFighting = true;
            return;
        }

        // --- SORT BY DISTANCE ---
        System.Array.Sort(allUnits, (a, b) =>
            Vector3.Distance(transform.position, a.transform.position)
            .CompareTo(Vector3.Distance(transform.position, b.transform.position)));

        UnitStatus closest = allUnits[0];

        List<UnitStatus> validTargets = new List<UnitStatus>();
        float maxDistanceFromClosest = 10f;

        // --- BUILD CLUSTER (up to 5, but works with fewer) ---
        for (int i = 0; i < allUnits.Length && validTargets.Count < 5; i++)
        {
            if (Vector3.Distance(closest.transform.position, allUnits[i].transform.position) <= maxDistanceFromClosest)
            {
                validTargets.Add(allUnits[i]);
            }
        }

        // --- RANDOMLY REMOVE UP TO 2 (BUT NEVER BREAK LIST) ---
        int removeCount = Mathf.Min(2, validTargets.Count - 1);
        // ensures at least 1 remains if possible

        for (int i = 0; i < removeCount; i++)
        {
            int index = Random.Range(0, validTargets.Count);
            validTargets.RemoveAt(index);
        }

        targets = validTargets.ToArray();

        // --- PICK TARGET SAFELY ---
        if (targets.Length > 0)
        {
            target = targets[Random.Range(0, targets.Length)];
            //print("picked target from " + targets.Length + " candidates");

            StartNewRoutine(MoveAndAttack(target));
        }
        else
        {
            // fallback if something weird happened
            target = closest;

            StartNewRoutine(MoveAndAttack(target));
        }
    }
    private void Update()
    {
        // despawn countdown ONLY when done fighting
        if (doneFighting)
        {
            despawnTimer -= Time.deltaTime;

            if (despawnTimer <= 0)
            {
                StartNewRoutine(Despawn());
            }
        }

        if (target == null && !doneFighting)
        {
            FindNewTarget();

            if (target != null)
            {
                StartNewRoutine(MoveAndAttack(target));
            }
            else
            {
                // no targets left, exit combat
                doneFighting = true;
                state = HumanState.Idle;
                Animate();

                StartNewRoutine(Despawn());
            }
        }
        if (hp <= 0 && !dead)
        {
            StopAllCoroutines();
            agent.isStopped= true;
            state = HumanState.Dead;
            //Animate();
            anim.SetTrigger("dead");
            Invoke("Kill",1f);
            dead = true;
        }
    }
    void Kill()
    {
        Destroy(this.gameObject);
    }
    void FindNewTarget()
    {
        for (int i = 0; i < targets.Length; i++)
        {
            if (targets[i] != null)
            {
                target = targets[i];
                return;
            }
        }

        target = null;
    }

    void StartNewRoutine(IEnumerator routine)
    {
        if (currentRoutine != null)
        {
            StopCoroutine(currentRoutine);
        }

        currentRoutine = StartCoroutine(routine);
    }

    IEnumerator MoveAndAttack(UnitStatus targ)
    {
        if (targ == null) yield break;

        agent.isStopped = false;
        agent.SetDestination(targ.transform.position);
        state = HumanState.Moving;
        Animate();

        // wait until close enough
        while (targ != null && Vector3.Distance(transform.position, targ.transform.position) > agent.stoppingDistance)
        {
            agent.SetDestination(targ.transform.position);
            yield return null;
        }

        if (targ == null) yield break;

        agent.isStopped = true;
        state = HumanState.Attacking;
        Animate();

        // attack loop
        float attackRange = agent.stoppingDistance + 0.5f;

        while (targ != null)
        {
            float dist = Vector3.Distance(transform.position, targ.transform.position);

            //Target fled → go back to chasing
            if (dist > attackRange)
            {
                agent.isStopped = false;
                state = HumanState.Moving;
                Animate();

                // restart movement toward target
                while (targ != null && Vector3.Distance(transform.position, targ.transform.position) > attackRange)
                {
                    agent.SetDestination(targ.transform.position);
                    yield return null;
                }

                if (targ == null) break;

                agent.isStopped = true;
                state = HumanState.Attacking;
                Animate();
            }

            //Attack
            DealDamage();
            yield return new WaitForSeconds(1f);
        }

        target = null;
    }

    IEnumerator Despawn()
    {
        agent.isStopped = false;
        agent.SetDestination(startPos);
        state = HumanState.Moving;
        Animate();

        while (Vector3.Distance(transform.position, startPos) > agent.stoppingDistance)
        {
            yield return null;
        }

        state = HumanState.Idle;
        Animate();

        gameObject.SetActive(false);
    }

    void TakeDamage()
    {
        hp -= 1;

        if (hp <= 0)
        {
            state = HumanState.Dead;
            Animate();
            agent.isStopped = true;
            return;
        }

        anim.SetTrigger("hurt");
    }

    void DealDamage()
    {
        if (target != null)
        {
            target.TakeDamageFromSource(damage, UnitStatus.CauseOfDeath.Enemy);
        }
    }

    void Animate()
    {
        if (anim == null) return;

        switch (state)
        {
            case HumanState.Idle:
                anim.SetTrigger("idle");
                break;
            case HumanState.Attacking:
                anim.SetTrigger("attacking");
                break;
            case HumanState.Moving:
                anim.SetTrigger("moving");
                break;
            case HumanState.Dead:
                anim.SetTrigger("dead");
                break;
        }
    }
}