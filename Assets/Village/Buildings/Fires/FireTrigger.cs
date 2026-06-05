using UnityEditor;
using UnityEngine;
using System.Collections.Generic;

public class FireTrigger : MonoBehaviour
{
    [SerializeField] List<UnitStatus> units = new List<UnitStatus>();

    private void Start()
    {
        InvokeRepeating("Heal", 60, 60);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Unit"))
        {
            UnitStatus us =  other.GetComponent<UnitStatus>();
            if (us != null)
            {
                us.warm = true;
                if (!units.Contains(us)){units.Add(us);}
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Unit"))
        {
            UnitStatus us = other.GetComponent<UnitStatus>();
            if (us != null)
            {
                us.warm = false;
                if (units.Contains(us)) { units.Remove(us);}
            }
        }
    }

    void Heal()
    {
        if (units.Count <= 0) { return;}
        foreach (var unit in units)
        {
            if (unit.hp < unit.maxHp)
            {
                unit.hp++;
                return;
            }
        }
    }
}
