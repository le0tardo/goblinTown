using UnityEngine;

public class SeasonTerrainListener : MonoBehaviour
{
    [Header("Season Colors")]
    [SerializeField] Color spring;
    [SerializeField] Color summer;
    [SerializeField] Color fall;
    [SerializeField] Color winter;
    [SerializeField] Color currentColor;

    MeshRenderer rend;

    private void Start()
    {
        rend = GetComponent<MeshRenderer>();
    }
    private void Update()
    {
        currentColor=Evaluate(SeasonManager.inst.seasonT);
        rend.material.color = currentColor;

    }
    public Color Evaluate(float seasonT)
    {
        seasonT = Mathf.Repeat(seasonT, 1f);

        if (seasonT < 0.25f)
        {
            // Winter → Spring
            if (SeasonManager.inst.elapsedYears > 0) //skip first winterLerp
            {
                float t = Mathf.InverseLerp(0f, 0.25f, seasonT);
                t = Mathf.Pow(t, 0.15f);
                return Color.Lerp(winter, spring, t);
            }
            else
            {
                return spring;
            }

        }
        else if (seasonT < 0.5f)
        {
            // Spring → Summer
            float t = Mathf.InverseLerp(0.25f, 0.5f, seasonT);
            return Color.Lerp(spring, summer, t);
        }
        else if (seasonT < 0.8f) //was 75
        {
            // Summer → Fall
            float t = Mathf.InverseLerp(0.5f, 0.75f, seasonT);
            return Color.Lerp(summer, fall, t);
        }
        else
        {
            // Fall → Winter
            float t = Mathf.InverseLerp(0.75f, 1f, seasonT);
            return Color.Lerp(fall, winter, t);
        }
    }

}

