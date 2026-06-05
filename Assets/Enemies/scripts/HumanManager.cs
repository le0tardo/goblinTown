using UnityEngine;

public class HumanManager : MonoBehaviour
{
    [SerializeField] GameObject[] humans;
    [SerializeField] float raidCoolDown = 10f;
    [SerializeField] bool raid=false;
    [SerializeField] int activeHumans;

    [SerializeField] int yearDelay;

    private void Start()
    {
        for (int i = 0; i < humans.Length; i++)
        {
            humans[i].SetActive(false);
        }
    }
    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.H))
        {
            DebugTestRaid();
        }

        CountHumans(); //if raid true??

        if (!raid && raidCoolDown>0)
        {
            raidCoolDown-= Time.deltaTime;
        }
        
        if (raidCoolDown <= 0)
        {
            if (!raid)
            {
                StartRaid();
                raid = true;
            }
        }

        if (raid&&activeHumans<=0)
        {
            raid = false;
            raidCoolDown = 10f;
        }
    }

    public void DebugTestRaid()
    {
        yearDelay = 0;
        print("debugging raid...");
        StartRaid();
    }

    void StartRaid()
    {
        print("raid called");
        if (SeasonManager.inst.elapsedYears < yearDelay) return;

        int r = Random.Range(2, humans.Length+1);
        for (int i = 0; i < r; i++)
        {
            humans[i].SetActive(true);
        }

        EventLogManager.inst.AddToLog("Humans are attacking the village!");
    }

    void CountHumans()
    {
        activeHumans = 0;

        foreach (GameObject human in humans)
        {
            if (human != null && human.activeInHierarchy)
            {
                activeHumans++;
            }
        }
    }
}
