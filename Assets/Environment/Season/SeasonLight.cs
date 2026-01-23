using UnityEngine;

public class SeasonLight : MonoBehaviour
{
    [SerializeField] Light sun;

    [SerializeField] float springTemp = 6000f;
    [SerializeField] float summerTemp = 5500f;
    [SerializeField] float fallTemp = 3000f;
    [SerializeField] float winterTemp = 8000f;

    float targetTemp;
    float lerpSpeed = 1f;

    private void Start()
    {
        sun.useColorTemperature = true;
    }

    private void Update()
    {
        switch (SeasonManager.inst.currentSeason)
        {
            case Season.Spring:
                targetTemp = springTemp;
                break;
            case Season.Summer:
                targetTemp = summerTemp;
                break;
            case Season.Fall:
                targetTemp=fallTemp;
                break;
            case Season.Winter:
                targetTemp=winterTemp;
                break;
        }

        sun.colorTemperature = Mathf.Lerp(
            sun.colorTemperature,
            targetTemp,
            lerpSpeed * Time.deltaTime
        );
    }
}
