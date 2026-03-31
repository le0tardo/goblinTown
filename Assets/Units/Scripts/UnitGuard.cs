using System.Collections.Generic;
using UnityEngine;

public class UnitGuard : MonoBehaviour
{

    [Header("combat stats")]
    [SerializeField] int damage;
    [SerializeField] float range;

    [SerializeField] List<HumanBehaviour> targets = new List<HumanBehaviour>();
    [SerializeField] HumanBehaviour target;

    [SerializeField]Animator anim;

    SphereCollider trigger;
    enum GuardState
    {
        Idle,
        Attack
    }
    [SerializeField]GuardState guardState;

    private void Start()
    {
        guardState = GuardState.Idle;
        targets.Clear();
        target = null;

        trigger=GetComponent<SphereCollider>();
        if(range>0)trigger.radius = range;

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
            }
        }
    }

    private void Update()
    {
        //check if HumanBehaviour.dead
        if (targets[0] != null)
        {
            target = targets[0];
        }
        else
        {
            target=null;
        }

        if (target != null)
        {
            guardState=GuardState.Attack;

        }
        else
        {
            guardState = GuardState.Idle;
        }
    }

    void FaceTarget(Vector3 direction)
    {

    }
}
