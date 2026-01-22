using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class SeasonBox : MonoBehaviour
{
    [SerializeField]TextMeshProUGUI season;
    [SerializeField]TextMeshProUGUI year;

    private void Update()
    {
        season.text=SeasonManager.inst.currentSeason.ToString();
        year.text="Year "+SeasonManager.inst.elapsedYears.ToString();
    }
}
