using UnityEngine;

public class WorkHouseBehaviour : MonoBehaviour
{
    public bool needsWorker=true;
    [SerializeField] GameObject workerGfx;
    public int toolLevel;
    public int weaponLevel;

    private void Start()
    {
        if(workerGfx!=null)workerGfx.SetActive(false);
    }
    public void AssignWorker(Unit unitWorker)
    {
        needsWorker = false;
        if (workerGfx != null) workerGfx.SetActive(true);
        //toolManager.inst.toolLevel=toolLevel;
        //toolManager.inst.weaponLevel=weaponLevel; if > etc...
        //equipmentManager, set clothes from this obj too...
    }
}
