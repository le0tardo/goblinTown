using UnityEngine;

public class DomicileBehaviour : MonoBehaviour
{
    public int goblinCapIncrease = 1;
    public int level = 1;

    private void Start()
    {
        UnitManager.inst.maxUnits += goblinCapIncrease;
    }
    private void OnDestroy()
    {
        UnitManager.inst.maxUnits -= goblinCapIncrease;
    }


}
