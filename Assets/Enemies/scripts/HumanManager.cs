using UnityEngine;

public class HumanManager : MonoBehaviour
{
    [SerializeField] GameObject[] humans;
    [SerializeField] float raidCoolDown = 10f;
    [SerializeField] bool raid=false;
    [SerializeField] int activeHumans;
    private void Update()
    {
        CountHumans();

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

    void StartRaid()
    {
        int r = Random.Range(3, humans.Length+1);
        for (int i = 0; i < r; i++)
        {
            humans[i].SetActive(true);
        }
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
