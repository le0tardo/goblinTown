using UnityEngine;

public class UnitAnimation : MonoBehaviour
{
    Animator anim;
    Unit unit;
    Unit.UnitState lastState;

    [SerializeField] GameObject carryObject;
    int randomIdle;

    UnitEquipment equip;
    private void Start()
    {
        anim = GetComponent<Animator>();
        unit = GetComponent<Unit>();
        equip = GetComponent<UnitEquipment>();

        lastState = unit.state;
        if(carryObject.activeInHierarchy)carryObject.SetActive(false);

        //randomize wobble animation
        float r = Random.value;
        anim.Play("wobble",1,r);
        anim.Play("unit_idle1", 0, r);

        RandomIdle();
    }

    void Update()
    {
        if (unit.state == lastState) { return; }
        else
        {
            lastState = unit.state;
            ApplyState(lastState);
        }
    }

    void RandomIdle()
    {
        randomIdle=Random.Range(0,4);
        anim.SetInteger("randomIdle", randomIdle);
    }
    public void ApplyState(Unit.UnitState state)
    {
       if(state==Unit.UnitState.Idle) anim.SetTrigger("idle"); anim.SetBool("carrying", carrying);carryObject.SetActive(carrying);equip.UneqipTools();
       if(state==Unit.UnitState.Moving) anim.SetTrigger("moving"); carryObject.SetActive(carrying); equip.UneqipTools();
        if (state == Unit.UnitState.Foraging)
        {
            if (unit.carriedResource.villageResource.resource == VillageResource.Resource.Food) //do elif here, else defaults ot foraging
            {
               anim.SetTrigger("foraging");
            }
            if (unit.carriedResource.villageResource.resource == VillageResource.Resource.Wood)
            {
                anim.SetTrigger("forageTree");
                equip.EquipAxe();
            }
            if (unit.carriedResource.villageResource.resource == VillageResource.Resource.Stone
                ||unit.carriedResource.villageResource.resource==VillageResource.Resource.Clay)
            {
                anim.SetTrigger("forageRock");
                equip.EquipPickAxe();
            }

            carryObject.SetActive(false);
        }
    }
    bool carrying
    {
        get { return unit.carriedAmount > 0; }
    }
}
