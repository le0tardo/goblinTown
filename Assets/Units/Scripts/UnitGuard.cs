using System.Collections.Generic;
using UnityEngine;

public class UnitGuard : MonoBehaviour
{
    enum GuardState
    {
        Idle,
        Attack
    }

    [Header("combat stats")]
    [SerializeField] int damage;
    [SerializeField] float range;
    [SerializeField] GuardState guardState;

    [SerializeField] List<HumanBehaviour> targets = new List<HumanBehaviour>();
    [SerializeField] HumanBehaviour target;

    [SerializeField]Animator anim;
    SphereCollider trigger;

    [SerializeField] GameObject spearIdle;
    [SerializeField] GameObject spearAttack;
    [SerializeField] GameObject spearFly;

    [SerializeField] GuardSpearScript spearScript;
    private void Start()
    {
        guardState = GuardState.Idle;
        targets.Clear();
        target = null;

        trigger=GetComponent<SphereCollider>();
        if(range>0)trigger.radius = range;

        spearIdle.SetActive(true);
        spearAttack.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            HumanBehaviour hb=other.GetComponent<HumanBehaviour>();
            if (hb != null)
            {
                targets.Add(hb);
            }

            CheckTarget();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            HumanBehaviour hb = other.GetComponent<HumanBehaviour>();
            if (hb != null)
            {
                if (targets.Contains(hb))
                {
                    targets.Remove(hb);
                }

                CheckTarget();
            }
        }
    }

    private void Update()
    {
        if (target!=null && target.dead == true)
        {
            if (targets.Contains(target))
            {
                targets.Remove(target);
                target = null;
            }
            CheckTarget();
        }

        if (target != null)
        {
            guardState=GuardState.Attack;
            FaceTarget(target.gameObject.transform.position);

        }
        else
        {
            guardState = GuardState.Idle;
        }

    }

    void FaceTarget(Vector3 targetPosition)
    {
        Vector3 dir = targetPosition - transform.position;
        dir.y = 0f;

        if (dir == Vector3.zero) return;

        transform.rotation = Quaternion.LookRotation(dir);
    }

    void CheckTarget()
    {
        if (targets.Count > 0)
        {
            target = targets[0];
        }
        else
        {
            target = null;
        }

        UpdateAnimator();
    }
    void UpdateAnimator()
    {
        if (target != null)
        {
            anim.SetBool("target",true);
            spearIdle.SetActive(false);
            spearAttack.SetActive(true);
        }
        else
        {
            anim.SetBool("target", false);
            spearIdle.SetActive(true);
            spearAttack.SetActive(false);
        }
    }

    public void ThrowSpear()
    {
        spearScript.gameObject.SetActive(true);
        if (target != null)
        {
            print("aiming spear at target: "+target.name);
            spearScript.ThrowSpear(target.transform.position);
        }
    }
    public void GuardAttack()
    {
        target.TakeDamage(damage);
    }
}
