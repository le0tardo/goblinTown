using UnityEngine;

public class WorkHouseEscape : MonoBehaviour
{

    WorkHouseBehaviour whb;

    private void Start()
    {
        whb=GetComponentInParent<WorkHouseBehaviour>();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            whb.FleeWorker();
        }
    }
}
