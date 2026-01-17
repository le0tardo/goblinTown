using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour, ISelectable, IMovable
{
    NavMeshAgent agent;
    NavMeshObstacle obs;
    UnitAnimation anim;
    [SerializeField] GameObject selectionMarker;
    public enum UnitState
    {
        Idle,
        Moving,
        Foraging,
    }
    public UnitState state;
    public enum EndAction
    {
        None,
        Forage,
        Deposit,
        Pickup,
        Attack
    }
    public EndAction endAction;

    [Header("Foraging")]
    public IForageable forageTarget;
    public ForagedResourceData carriedResource;
    public int carriedAmount;
    public int carryCapacity = 5;
    public float forageSpeed = 2f;
    Coroutine forageRoutine;

    public IDepositable depositTarget;
    public IPickupable pickupTarget;

    [Header("Slot")]
    public ISlotProvider currentSlotProvider;
    public string slotProviderName = " "; //just for debug
    private void Start()
    {
        //UnitManager.inst.units.Add(this);
        UnitManager.inst.AddUnit(this);
        agent = GetComponent<NavMeshAgent>();
        obs = GetComponent<NavMeshObstacle>();
        anim=GetComponent<UnitAnimation>();
        state=UnitState.Idle;
    }
    void Update()
    {
        if (state==UnitState.Moving && HasReachedDestination())
        {
            DoEndAction();
        }

        if (state == UnitState.Foraging && forageTarget!=null && forageTarget.IsDepleted) //not needed any more??
        {
            state = UnitState.Idle;
            //find closest dropOff-point?
        }
    }
    public void SetSelected(bool selected)
    {
        selectionMarker.SetActive(selected);
    }

    public void MoveTo(Vector3 destination)
    {
        agent.SetDestination(destination);
        state=UnitState.Moving;
    }
    public void ReleaseSlot()
    {
        if (currentSlotProvider != null)
        {
            currentSlotProvider.ReleaseSlot(this);
            currentSlotProvider = null;
        }
    }
    void DoEndAction()
    {
        switch (endAction)
        {
            case EndAction.None:
                state = UnitState.Idle;    
            break;

            case EndAction.Forage:
                if (forageTarget != null && !forageTarget.IsDepleted && carriedAmount<carryCapacity)
                {
                    state = UnitState.Foraging;
                    FacePosition(forageTarget.Position);
                    StartForaging();
                }
                else { ClearEndAction();}
                    break;

            case EndAction.Deposit:
                if (depositTarget != null && carriedResource != null && carriedAmount > 0)
                {
                    //OmniStorage storage = depositTarget.GetComponent<>(OmniStorage);
                    DepositAtStorage(depositTarget.Storage);
                }
                else{ ClearEndAction();}
            break;  
            case EndAction.Pickup:
                if(carriedResource!=null && carriedResource != pickupTarget.Resource)
                {
                    Debug.Log("trying to pickup anohter kind of resource, not allowed");
                    ClearEndAction();
                    break;
                }
                if (carriedAmount >= carryCapacity)
                {
                    Debug.Log("unit already full");
                    ClearEndAction();
                    break;
                }

                if (pickupTarget == null)
                {
                    ClearEndAction();
                    break;
                }

                if (pickupTarget.TryPickup(this))
                {
                    pickupTarget = null;
                    ClearEndAction();
                }
                else //some other unit got to it firts
                {
                    pickupTarget = null;
                    ClearEndAction();
                }
                break;

            //case EndAction.Build: break;
        }
    }
    void ClearEndAction()
    {
        endAction = EndAction.None;
        forageTarget = null;
        depositTarget = null;
        pickupTarget = null;
        //state = UnitState.Idle; //this is ugly sometimes? looks better without...
        anim.ApplyState(state);
    }
    bool HasReachedDestination()
    {
        if (agent.pathPending)
            return false;

        if (agent.remainingDistance > agent.stoppingDistance)
            return false;

        if (agent.hasPath && agent.velocity.sqrMagnitude > 0.01f)
            return false;

        return true;
    }
    public void FacePosition(Vector3 targetPosition)
    {
        Vector3 direction = targetPosition - transform.position;
        direction.y = 0f; // keep upright

        if (direction.sqrMagnitude < 0.0001f)
            return;

        Quaternion lookRotation = Quaternion.LookRotation(direction);
        transform.rotation = lookRotation;
    }
    void OnDestroy()
    {
        if (UnitManager.inst != null)
        {
            //UnitManager.inst.units.Remove(this);
            UnitManager.inst.RemoveUnit(this);
            UnitManager.inst.selectedUnits.Remove(this);
        }
    }

    //forage loop routine
    void StartForaging()
    {
        if (carriedResource != null && carriedResource != forageTarget.Resource)
        {
            forageTarget = null;
            state = UnitState.Idle; 
            return; 
        }
        if (forageRoutine != null)
        {
            //Debug.Log("duplicate routine!");
            return;
        }

        StopForaging();
        forageRoutine = StartCoroutine(ForageLoop());
    }
    void StopForaging()
    {
        if (forageRoutine != null)
        {
            StopCoroutine(forageRoutine);
            forageRoutine = null;
        }
    }

    IEnumerator ForageLoop()
    {
        while (
            state == UnitState.Foraging &&
            forageTarget != null
        )
        {
            forageTarget.Forage(this);

            if (carriedAmount >= carryCapacity)
            {
                StopForaging();
                //GoToNearestDropOff();
                yield break;
            }

            yield return new WaitForSeconds(1f);
        }

        StopForaging();
    }

    public void DepositAtStorage(OmniStorage storage)
    {
        if (carriedResource == null || carriedAmount <= 0)
            return;

        var village = VillageResourceManager.inst;
        VillageResource vr = carriedResource.villageResource;

        if (!village.villageResources.TryGetValue(vr, out int currentAmount))
            return;
        if (!village.villageCaps.TryGetValue(vr, out int cap))
            return;

        if (currentAmount >= cap)
        {
            ClearEndAction();
            return;
        }

        // Clamp
        int spaceLeft = cap - currentAmount;
        int depositAmount = Mathf.Min(carriedAmount, spaceLeft);

        storage.Deposit(carriedResource, depositAmount);

        carriedAmount -= depositAmount;

        if (carriedAmount <= 0)
        {
            carriedResource = null;
            carriedAmount = 0;
            ClearEndAction();
        }
    }

    public void Die()
    {
        UnitManager.inst.DeselectUnit(this);
        agent.isStopped = true;
        agent.enabled = false;

        Invoke("Kill", 1.0f);
    }

    void Kill()
    {
        Destroy(this.gameObject);
    }
}
