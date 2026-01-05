using System.Collections.Generic;
using UnityEngine;

public class UnitManager : MonoBehaviour
{
    public static UnitManager inst { get; private set; }

    public List<Unit> units = new List<Unit>();
    public List<Unit> selectedUnits = new List<Unit>();

    private void Awake()
    {
        if (inst != null && inst != this){ Destroy(gameObject); return; }
        inst = this;
    }

    public void SelectUnit(Unit unit)
    {
        if (!selectedUnits.Contains(unit))
        {
            selectedUnits.Add(unit);
            unit.SetSelected(true);
        }
    }
    public bool IsSelected(Unit unit)
    {
        return selectedUnits.Contains(unit);
    }
    public void DeselectUnit(Unit unit)
    {
        if (selectedUnits.Remove(unit))
        {
            unit.SetSelected(false);
        }
    }

    public void ClearSelection()
    {
        foreach (Unit unit in selectedUnits)
        {
            unit.SetSelected(false);
        }
        selectedUnits.Clear();
    }
}
