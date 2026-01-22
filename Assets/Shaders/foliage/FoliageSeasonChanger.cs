using UnityEngine;

public class FoliageSeasonChanger : MonoBehaviour
{
    static readonly int SeasonID = Shader.PropertyToID("_seasonFloat");
    static readonly int WinterID = Shader.PropertyToID("_winterFloat");

    Renderer rend;
    MaterialPropertyBlock mpb;

    public float season;
    public float winter;

    void Awake()
    {
        rend = GetComponent<Renderer>();
        mpb = new MaterialPropertyBlock();
    }

    private void Update()
    {
        season = Mathf.PingPong(Time.time * 0.1f, 1f);
        winter = Mathf.Clamp01(season * 1.2f - 0.8f);

        mpb.SetFloat(SeasonID, season);
        mpb.SetFloat(WinterID, winter);
        rend.SetPropertyBlock(mpb);
    }
    public void SetSeason(float season)
    {
        mpb.SetFloat(SeasonID, season);
        rend.SetPropertyBlock(mpb);
    }

    public void SetWinter(float winter)
    {
        mpb.SetFloat(WinterID, winter);
        rend.SetPropertyBlock(mpb);
    }
}
