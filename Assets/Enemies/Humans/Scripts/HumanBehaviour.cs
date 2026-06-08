using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class HumanBehaviour : MonoBehaviour
{
    [Header("Combat Stats")]
    [SerializeField] int hp = 3;
    [SerializeField] int maxHp = 3;
    [SerializeField] int damage = 3;
    [SerializeField] float despawnTime = 10f;
    [SerializeField] float despawnTimer = 10f;
    [SerializeField] bool doneFighting = false;
    [SerializeField] float distanceToTarget;

    public bool dead = false;

    [SerializeField] GameObject hat;
    enum HumanState
    {
        Idle,
        Moving,
        Attacking,
        Dead
    }

    [SerializeField] HumanState state;
    private HumanState lastState; // Tracked to prevent animation trigger spamming
    UnitStatus[] targets;
    [SerializeField] UnitStatus target;

    [Header("Movement")]
    NavMeshAgent agent;
    Vector3 startPos;

    [Header("Graphics")]
    Animator anim;

    Coroutine currentRoutine;
    private float targetSearchCooldown = 0f; // Optimizes Update loop performance

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
        state = HumanState.Idle;
        lastState = HumanState.Dead; // Force animation update on spawn
        doneFighting = false;
        hp = maxHp;
        dead = false;

        if (hat != null)
        {
            hat.SetActive(Random.value <= 0.5f);
        }

        Animate();
        FindTargets();
    }

    void FindTargets()
    {
        // Performance Warning: FindObjectsByType is heavy. Limit usage via timers.
        UnitStatus[] allUnits = FindObjectsByType<UnitStatus>(FindObjectsSortMode.None);
        allUnits = System.Array.FindAll(allUnits, unit => unit != null && unit.transform.position.y >= -1f);

        if (allUnits.Length == 0)
        {
            targets = new UnitStatus[0];
            target = null;
            ClearCurrentRoutine();
            StartNewRoutine(ReturnToSpawnAndDespawn());
            return;
        }

        // Sort by distance
        System.Array.Sort(allUnits, (a, b) =>
            Vector3.Distance(transform.position, a.transform.position)
            .CompareTo(Vector3.Distance(transform.position, b.transform.position)));

        UnitStatus closest = allUnits[0];
        List<UnitStatus> validTargets = new List<UnitStatus>();
        float maxDistanceFromClosest = 10f;

        // Build cluster
        for (int i = 0; i < allUnits.Length && validTargets.Count < 5; i++)
        {
            if (Vector3.Distance(closest.transform.position, allUnits[i].transform.position) <= maxDistanceFromClosest)
            {
                validTargets.Add(allUnits[i]);
            }
        }

        // Randomly remove up to 2
        int removeCount = Mathf.Min(2, validTargets.Count - 1);
        for (int i = 0; i < removeCount; i++)
        {
            int index = Random.Range(0, validTargets.Count);
            validTargets.RemoveAt(index);
        }

        targets = validTargets.ToArray();

        if (targets.Length > 0)
        {
            target = targets[Random.Range(0, targets.Length)];
            StartNewRoutine(MoveAndAttack(target));
        }
        else
        {
            target = closest;
            StartNewRoutine(MoveAndAttack(target));
        }
    }

    private void Update()
    {
        if (dead) return;

        // Handle Health Check
        if (hp <= 0)
        {
            HandleDeath();
            return;
        }

        // Handle Despawn Counter ONLY when fully done fighting
        if (doneFighting)
        {
            despawnTimer -= Time.deltaTime;
            if (despawnTimer <= 0)
            {
                gameObject.SetActive(false);
            }
            return;
        }

        // Optimized Search Engine: Checks for targets safely over frames instead of spiking CPU
        if (target == null)
        {
            targetSearchCooldown -= Time.deltaTime;
            if (targetSearchCooldown <= 0f)
            {
                targetSearchCooldown = 0.5f; // Scan every half second max
                FindTargets();
            }
        }
        else if (target.transform.position.y < -1f)
        {
            // Target fell below map out of bounds
            target = null;
            ClearCurrentRoutine();
        }
    }

    void HandleDeath()
    {
        dead = true;
        ClearCurrentRoutine();
        agent.isStopped = true;
        state = HumanState.Dead;
        Animate();
        Invoke(nameof(Kill), 1f);
    }

    void Kill()
    {
        gameObject.SetActive(false);
    }

    void StartNewRoutine(IEnumerator routine)
    {
        ClearCurrentRoutine();
        currentRoutine = StartCoroutine(routine);
    }

    void ClearCurrentRoutine()
    {
        if (currentRoutine != null)
        {
            StopCoroutine(currentRoutine);
            currentRoutine = null;
        }
    }

    IEnumerator MoveAndAttack(UnitStatus targ)
    {
        while (targ != null && !dead)
        {
            float dist = Vector3.Distance(transform.position, targ.transform.position);
            distanceToTarget = dist;

            // State Handling: Chasing
            if (dist > agent.stoppingDistance)
            {
                if (agent.isStopped) agent.isStopped = false;
                agent.SetDestination(targ.transform.position);
                state = HumanState.Moving;
                Animate();
            }
            // State Handling: Attacking Range
            else
            {
                if (!agent.isStopped) agent.isStopped = true;
                state = HumanState.Attacking;
                Animate();

                DealDamage();
                yield return new WaitForSeconds(1f); // Attack speed delay
                continue;
            }

            yield return null;
        }

        // If target was destroyed or lost, clear reference to trigger FindTargets next cycle
        target = null;
    }

    IEnumerator ReturnToSpawnAndDespawn()
    {
        doneFighting = true; // Prevents re-entering search updates
        agent.isStopped = false;
        agent.SetDestination(startPos);
        state = HumanState.Moving;
        Animate();

        // Pathing back to initial spawn vector
        while (agent.pathPending || agent.remainingDistance > agent.stoppingDistance)
        {
            if (dead) yield break;
            yield return null;
        }

        // Reached Home safely
        agent.isStopped = true;
        state = HumanState.Idle;
        Animate();
    }

    public void TakeDamage(int damage)
    {
        if (dead) return;

        hp -= damage;
        if (hp <= 0)
        {
            HandleDeath();
            return;
        }

        if (anim != null) anim.SetTrigger("hurt");
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
        if (anim == null || state == lastState) return;

        // Reset any standard movement parameters if necessary, then set updates
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
                float rt = Random.value;
                anim.Play("walk", 0, rt);
                break;
            case HumanState.Dead:
                anim.SetTrigger("dead");
                break;
        }

        lastState = state; // Track current to block spam triggers on following frames
    }
}