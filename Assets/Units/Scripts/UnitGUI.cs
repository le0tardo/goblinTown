using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class UnitGUI : MonoBehaviour
{
    [Header("Unit selection boxes")]
    [SerializeField] GameObject soloUnitBox;

    [SerializeField] GameObject multiUnitBox;
    [SerializeField] TextMeshProUGUI unitCount;

    [SerializeField] GameObject deselectButton;

    private void Update()
    {
        var selected = UnitManager.inst.selectedUnits.Count;

        if (selected == 1)
        {
            if (!soloUnitBox.activeInHierarchy)
                soloUnitBox.SetActive(true);
            if (!deselectButton.activeInHierarchy)
                deselectButton.SetActive(true);
            if (multiUnitBox.activeInHierarchy)
                multiUnitBox.SetActive(false);

        }
        else if (selected>1)
        {
            if(soloUnitBox.activeInHierarchy)
                soloUnitBox.SetActive(false);
            if(!multiUnitBox.activeInHierarchy)
                multiUnitBox.SetActive(true);
            if (!deselectButton.activeInHierarchy)
                deselectButton.SetActive(true);

            unitCount.text=selected.ToString()+" Goblins selected.";
        }
        else
        {
            if (soloUnitBox.activeInHierarchy)
                soloUnitBox.SetActive(false);
            if(multiUnitBox.activeInHierarchy)
                multiUnitBox.SetActive(false);
            if(deselectButton.activeInHierarchy)
                deselectButton.SetActive(false);
        }
    }

}
