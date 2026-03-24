using UnityEngine;

public class WorkerBehaviour : MonoBehaviour
{
    WorkHouseBehaviour myWorkHouse;
    Unit myUnit;
    UnitStatus myStatus;

    private void OnEnable()
    {
        myWorkHouse= GetComponentInParent<WorkHouseBehaviour>();
        myUnit=myWorkHouse.currentWorker;

        if (myWorkHouse != null)
        {
            print("my workhouse is " + myWorkHouse.name);
        }
        else
        {
            print("no workhouse...");
        }

        if (myUnit != null)
        {
            print("my worker is" + myUnit.name);
            myStatus=myUnit.GetComponent<UnitStatus>();
        }
        else
        {
            print("no worker...");
        }
    }

    private void Update()
    {
        if (myStatus.hp <= 0)
        {
            myWorkHouse.FireWorker();
        }
    }
}
