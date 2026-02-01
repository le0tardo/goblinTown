using UnityEngine;

public class UnitFeeder : MonoBehaviour
{
    [SerializeField] float feedingTime;
    private float feedingTimer;

    [SerializeField] VillageResource food;

    private void Start()
    {
        if(feedingTime==0)feedingTime=30f;
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
    }
    public void FeedUnits()
    {
        int n=UnitManager.inst.units.Count;

        Debug.Log("Units consumed "+n+" food!");

        VillageResourceManager.inst.RemoveResource(food,n);
    }
}
