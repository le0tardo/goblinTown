using UnityEngine;
using UnityEngine.UI;

public class UnitFeeder : MonoBehaviour
{
    [SerializeField] float feedingTime;
    private float feedingTimer;

    [SerializeField] Image feedFill;

    [SerializeField] VillageResource food;

    private void Start()
    {
        if(feedingTime==0)feedingTime=30f;
        feedingTimer = feedingTime;
    }
    private void Update()
    {
        if (feedingTimer <= 0)
        {
            FeedUnits();
            feedingTimer = feedingTime;
        }
        else
        {
            feedingTimer-= Time.deltaTime;
        }

        feedFill.fillAmount = 1- (feedingTimer/feedingTime);

    }
    public void FeedUnits()
    {
        int n=UnitManager.inst.units.Count;

        int remainingFood; 
        bool hasFood=VillageResourceManager.inst.villageResources.TryGetValue(food, out remainingFood);

        if (n > remainingFood)
        {
            /*int hungyUnits=n-remainingFood;
            
            for(int i = 0; i < hungyUnits; i++)
            {
                //TODO: deal hunger damage to ONLY untis that dont get food this rounf
            }*/

            foreach (Unit unit in UnitManager.inst.units)
            {
                UnitStatus status = unit.GetComponent<UnitStatus>();
                status.TakeDamageFromSource(1, UnitStatus.CauseOfDeath.Hunger);
            }

            EventLogManager.inst.AddToLog("Your village is out of food!");
        }

        VillageResourceManager.inst.RemoveResource(food,n); //auto caps

    }
}
