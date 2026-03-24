using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class Unit : MonoBehaviour, ISelectable, IMovable
{
    public NavMeshAgent agent;
    UnitAnimation anim;
    UnitEquipment equip;
    UnitStatus status;
    [SerializeField] GameObject selectionMarker;
    public enum UnitState
    {
        Idle,
        Moving,
        Foraging,
        Hunting
    }
    public UnitState state;
    public enum EndAction
    {
        None,
        Forage,
        Deposit,
        Pickup,
        Attack,
        Hunt,
        Work,
        Sit
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
    public IHuntable huntTarget;
    public IWorkable workTarget;
    public IFireplace fireplaceTarget;

    [Header("Slot")]
    public ISlotProvider currentSlotProvider;
    public string slotProviderName = " "; //just for debug
    private void Start()
    {
        //UnitManager.inst.units.Add(this);
        UnitManager.inst.AddUnit(this);
        agent = GetComponent<NavMeshAgent>();
        anim=GetComponent<UnitAnimation>();
        equip=GetComponent<UnitEquipment>();
        status=GetComponent<UnitStatus>();

        state=UnitState.Idle;
    }
    void Update()
    {
        if (agent == null) return;

        if (state==UnitState.Moving && HasReachedDestination())
        {
            DoEndAction();
        }

        if (state == UnitState.Foraging && forageTarget!=null && forageTarget.IsDepleted) //not needed any more??
        {
            state = UnitState.Idle;
            //find closest dropOff-point?
        }

        if(state==UnitState.Hunting)
        {
            if (huntTarget.IsDead)
            {
                huntTarget = null;
                state = UnitState.Idle;
                ClearEndAction();
                return;
            }
            else
            {
                float dist = Vector3.Distance(transform.position, huntTarget.Position);
                if (dist > 5)
                {
                    huntTarget = null;
                    state = UnitState.Idle;
                    ClearEndAction();
                }
                if(huntTarget!=null)FacePosition(huntTarget.Position);
            }
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
        agent.avoidancePriority = 20;

        if (carriedAmount <= 0) carriedResource = null;
        if (forageRoutine != null) forageRoutine = null;
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
        agent.avoidancePriority = 60;
        switch (endAction)
        {
            case EndAction.None:
                state = UnitState.Idle;    
            break;

            case EndAction.Forage:
                if (
                    forageTarget != null &&
                    !forageTarget.IsDepleted &&
                    carriedAmount < carryCapacity &&
                    (carriedResource == null || carriedResource == forageTarget.Resource) &&
                    (!forageTarget.NeedsTool || equip.toolLevel > 0)
                )
                {
                    state = UnitState.Foraging;
                    FacePosition(forageTarget.Position);

                    if (carriedResource == null)
                        carriedResource = forageTarget.Resource;

                    StartForaging();
                }
                else
                {
                    if (equip.toolLevel < 1) {
                        EventLogManager.inst.AddToLog(
                        status.unitName + " needs better tools!"
                        );
                    }
                    state = UnitState.Idle;
                    ClearEndAction();
                }
                break;


            case EndAction.Deposit:
                if (depositTarget != null && carriedResource != null && carriedAmount > 0)
                {
                    DepositAtStorage(depositTarget.Storage);
                }
                else{ ClearEndAction();}
            break;  
            case EndAction.Pickup:
                if(carriedResource!=null && carriedResource != pickupTarget.Resource)
                {
                    //Debug.Log("trying to pickup anohter kind of resource, not allowed");
                    ClearEndAction();
                    break;
                }
                if (carriedAmount >= carryCapacity)
                {
                    //Debug.Log("unit already full");
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
            case EndAction.Hunt:
                if (huntTarget == null)
                {
                    ClearEndAction();
                    break;
                }
                state = UnitState.Hunting;
                HuntAnimal(huntTarget);
                FacePosition(huntTarget.Position);

                ClearEndAction();
            break;
            case EndAction.Work:
                if (workTarget == null) { print("no work target"); return; }
                if (workTarget.NeedsWorker)
                {
                    workTarget.AssignWorker(this);
                }
                else
                {
                    workTarget = null;
                    state=UnitState.Idle;
                    ClearEndAction();
                }
                break;
            case EndAction.Sit:
                anim.SitTrigger();
                state=UnitState.Idle;
                if(fireplaceTarget!=null)FacePosition(fireplaceTarget.SitPosition);
                ClearEndAction();
            break;
        }
    }
    public void ClearEndAction()
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

    public void TryFaceTarget()
    {
        if (forageTarget != null) FacePosition(forageTarget.Position);
        //if (huntTarget != null) FacePosition(huntTarget.Position);
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
        if (carriedResource != null)
        {
            if (carriedResource != forageTarget.Resource)
            {
                StopForaging();
                Debug.Log("trying to farm different resource than you carry, not allowed");
                return;
            }
            else
            {
                //Debug.Log("farmingng the same resource you are carrying, allowed.");
            }
        }

        if (forageRoutine != null)
        {
            //Debug.Log("double routine");
            return;
        }

        state = UnitState.Foraging;
        forageRoutine = StartCoroutine(ForageLoop());
    }

    void StopForaging()
    {
        if (forageRoutine != null)
        {
            StopCoroutine(forageRoutine);
            forageRoutine = null;
        }
        ClearEndAction();
        forageTarget = null;
        state = UnitState.Idle;
    }

    IEnumerator ForageLoop()
    {
        while (state == UnitState.Foraging && forageTarget != null)
        {
            // Wait for harvest time BEFORE gaining resource
            float waitTime = (forageTarget.NodeData.harvestDuration-equip.toolLevel);
            if (waitTime <1){waitTime = 1f;}
            yield return new WaitForSeconds(waitTime);

            // Re-check after waiting (node might be gone)
            if (state != UnitState.Foraging || forageTarget == null)
                yield break;

            forageTarget.Forage(this);

            // Stop if full
            if (carriedAmount >= carryCapacity)
            {
                forageRoutine = null;
                StopForaging();
                yield break;
            }
        }
        forageRoutine = null;
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
            EventLogManager.inst.AddToLog("Storages are full of "+ carriedResource.villageResource.name+"!");
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
    void HuntAnimal(IHuntable animalTarget)
    {
        animalTarget.OnHit(1+equip.toolLevel,this); //wood spear = 1+0=1; etc.
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
