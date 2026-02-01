using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SeasonBox : MonoBehaviour
{
    [SerializeField]TextMeshProUGUI season;
    [SerializeField]TextMeshProUGUI year;
    [SerializeField] Image seasonIco;
    [SerializeField] Sprite[] seasonSprites;

    private void Update()
    {
        season.text=SeasonManager.inst.currentSeason.ToString();
        year.text="Year "+SeasonManager.inst.elapsedYears.ToString();

        switch (SeasonManager.inst.currentSeason)
        {
            case Season.Spring:
                seasonIco.sprite = seasonSprites[0];
            break;
            case Season.Summer:
                seasonIco.sprite=seasonSprites[1];
            break;
            case Season.Fall:
                seasonIco.sprite = seasonSprites[2];
            break;
            case Season.Winter:
                seasonIco.sprite = seasonSprites[3];
            break;
        }
    }
}
