using UnityEngine;
using UnityEngine.Rendering.Universal;

public class SeasonLight : MonoBehaviour
{
    [SerializeField] Light sun;

    [SerializeField] float springTemp = 6000f;
    [SerializeField] float summerTemp = 5500f;
    [SerializeField] float fallTemp = 3000f;
    [SerializeField] float winterTemp = 8000f;

    float targetTemp;
    float lerpSpeed = 1f;

    UniversalAdditionalLightData cookie;
    Vector2 cookieSpeed = new Vector2(1f,1f);
    float cloudSpeed = 1.5f;
    float cloudMax = 64f;
    Vector2 maxOff = new Vector2(128f, 128f);
    private void Start()
    {
        sun.useColorTemperature = true;
        cookie=GetComponent<UniversalAdditionalLightData>();
        cookie.lightCookieOffset =Vector2.zero;
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

        MoveCloudsX();
        //MoveCloudsXY();
    }

    void MoveCloudsX()
    {
        Vector2 offset = cookie.lightCookieOffset;

        offset.x += Time.deltaTime * cloudSpeed;

        if (offset.x >= cloudMax)
            offset.x = 0f;

        cookie.lightCookieOffset = offset;
    }

    void MoveCloudsXY()
    {
        cookie.lightCookieOffset += Time.deltaTime * cookieSpeed;

        if (cookie.lightCookieOffset.x >= maxOff.x &&
            cookie.lightCookieOffset.y >= maxOff.y)
        {
            cookie.lightCookieOffset = Vector2.zero;
        }
    }

}
