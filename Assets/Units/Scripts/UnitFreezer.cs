using UnityEngine;

public class UnitFreezer : MonoBehaviour
{
    [SerializeField] float winterDuration;
    private void Start()
    {
        if (winterDuration == 0) winterDuration = 10;

        InvokeRepeating("TryFreeze",1,winterDuration);
    }

    void TryFreeze()
    {
        if (SeasonManager.inst.currentSeason == Season.Winter)
        {
            //FreezeUnitsByDamage();
            FreezeUnitsByChance();
        }
    }
    void FreezeUnitsByDamage()
    {
        foreach(Unit unit in UnitManager.inst.units)
        {
            UnitEquipment eq=unit.GetComponent<UnitEquipment>();
            int clothesLvl=eq.clothesLevel; //between 0-3;

            int damageToDeal=3;
            damageToDeal-=clothesLvl;

            if (damageToDeal > 0)
            {
                UnitStatus status = unit.GetComponent<UnitStatus>();
                status.TakeDamageFromSource(damageToDeal, UnitStatus.CauseOfDeath.Cold);
            }
        }
    }
    void FreezeUnitsByChance()
    {
        foreach (Unit unit in UnitManager.inst.units)
        {
            UnitEquipment eq = unit.GetComponent<UnitEquipment>();
            int clothesLvl = eq.clothesLevel; // 0–3

            float freezeChance = 1f - (clothesLvl / 3f);

            if (Random.value < freezeChance)
            {
                UnitStatus status = unit.GetComponent<UnitStatus>();
                status.TakeDamageFromSource(1,UnitStatus.CauseOfDeath.Cold);
            }
        }
    }

}
