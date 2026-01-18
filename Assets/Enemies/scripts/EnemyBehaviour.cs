using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class EnemyBehaviour : MonoBehaviour, IAttackable
{
    [Header("Combat")]
    public int hp;
    public int maxhp;
    public UnitStatus combatTarget = null;
    public bool inCombat;
    public bool isAlive = true;
    public int damage;
    public float aggroRange = 10f;
    public float attackRange = 2f;
    public float attackSpeed = 2f;
    Coroutine attackRoutine;

    [Header("Movement")]
    NavMeshAgent agent;
    Vector3 position;
    public float moveSpeed;

    EnemyAnimation anim;

    //interface sync
    public bool IsAlive => isAlive;
    public Vector3 Position => position;
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        anim=GetComponent<EnemyAnimation>();
    }

    private void Update()
    {
        if (combatTarget == null)
        {
            FindTarget();
            return;
        }
        if (combatTarget != null)
        {
            if (!inCombat) //only move when not in combat.
            {
                agent.SetDestination(combatTarget.gameObject.transform.position);
                anim.MoveAnimation();
            }

            float dist = Vector3.Distance(transform.position, combatTarget.gameObject.transform.position);

            if (dist <= attackRange)
            {
                if (!inCombat)
                {
                    StartAttack();
                    anim.AttackAnimation();
                    inCombat = true;
                }

            }
            else
            {
                inCombat= false;
            }
        }
 

        position = transform.position;
    }
    public void TakeDamage(int amount)
    {
        hp -= amount;
    }
    void DealDamage()
    {
        if (combatTarget != null)
        {
            combatTarget.TakeDamage(damage);
        }
    }

    void FindTarget()
    {
        UnitStatus[] units = Object.FindObjectsByType<UnitStatus>(FindObjectsSortMode.None);

        float bestDist = float.MaxValue;
        UnitStatus best = null;

        foreach (var unit in units)
        {
            if (unit.isDead) continue;

            float dist = Vector3.Distance(transform.position, unit.transform.position);

            if (dist < bestDist && dist <= aggroRange)
            {
                bestDist = dist;
                best = unit;
                combatTarget = unit;
            }
        }
    }

    void StartAttack()
    {
        Debug.Log("started attack loop");
        if (attackRoutine != null)
            return;

        attackRoutine = StartCoroutine(AttackLoop());
    }

    void StopAttack()
    {
        Debug.Log("stopped attack loop");
        if (attackRoutine != null)
        {
            StopCoroutine(attackRoutine);
            attackRoutine = null;
        }
        anim.IdleAnimation();
    }

    IEnumerator AttackLoop()
    {
        Debug.Log("in attack loop");
        while (combatTarget != null && !combatTarget.isDead)
        {
            // Face target
            Vector3 dir = combatTarget.transform.position - transform.position;
            dir.y = 0f;
            if (dir != Vector3.zero)
                transform.rotation = Quaternion.LookRotation(dir);

            DealDamage();

            yield return new WaitForSeconds(attackSpeed);
        }
        StopAttack();
        combatTarget = null;
        attackRoutine = null;
    }

}
