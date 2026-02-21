using UnityEngine;

public class FireTrigger : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Unit"))
        {
            UnitStatus us =  other.GetComponent<UnitStatus>();
            if(us!=null) us.warm=true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Unit"))
        {
            UnitStatus us = other.GetComponent<UnitStatus>();
            if (us != null) us.warm = false;
        }
    }
}
