using Unity.VisualScripting;
using UnityEngine;

public enum Season
{
    Spring,
    Summer,
    Fall,
    Winter
}
public class SeasonManager : MonoBehaviour
{
    [SerializeField] public float seasonT;
    public Season currentSeason;
    public int elapsedYears = 0;
    [SerializeField] float yearDuration = 60f; //seconds for a full yeas, idk 600? 5 min?
    [SerializeField] float winterStart = 0.85f;
    [SerializeField] float winterEnd   = 0.15f;
    [SerializeField] public float winterT;

    public static SeasonManager inst;
    private void Awake()
    {
        inst = this;
    }

    private void Start()
    {
        currentSeason=Season.Spring;
        elapsedYears = 0;
        seasonT = 0;
        winterT = 0;
    }

    private void Update()
    {
        AdvanceSeason();
        SetCurrentSeason();
        SetWinter();


    }
    void AdvanceSeason()
    {
        seasonT += Time.deltaTime / yearDuration;

        if (seasonT >= 1f)
        {
            seasonT -= 1f;
            elapsedYears += 1;
        }
    }

    public void SetCurrentSeason()
    {
            if      (seasonT < 0.25f) currentSeason=Season.Spring;
            else if (seasonT < 0.75f) currentSeason=Season.Summer;
            else if (seasonT < 1f) currentSeason=Season.Fall;
            else    currentSeason=Season.Winter;
    }

    void SetWinter()
    {
        if (seasonT >= winterStart)
        {
            if(winterT<1)winterT += (Time.deltaTime);
        }
        else if (seasonT < winterEnd)
        {
            if(winterT>0)winterT -= (Time.deltaTime);
        }
        else
        {
            winterT = 0f;
        }

        winterT = Mathf.Clamp(winterT,0f,1f);
    }

}
