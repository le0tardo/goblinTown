using UnityEngine;

public class SeasonFoliage : MonoBehaviour
{
    static readonly int SeasonID = Shader.PropertyToID("_seasonFloat");
    static readonly int WinterID = Shader.PropertyToID("_winterFloat");

    [SerializeField] bool everGreen;

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
        season = SeasonManager.inst.seasonT;
        winter= SeasonManager.inst.winterT;

        mpb.SetFloat(SeasonID, season);
       if(!everGreen)mpb.SetFloat(WinterID, winter);
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
