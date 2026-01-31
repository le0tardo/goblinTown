using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.AI;

public class WorkHouseBehaviour : MonoBehaviour, IWorkable
{
    public bool needsWorker=true;
    public bool NeedsWorker
    {
        get => needsWorker;
        set => needsWorker = value;
    }

    Vector3 position;
    public Vector3 Position => position;

    Vector3 spawnPosition;

    [SerializeField] GameObject workerGfx;
    public int toolLevel;
    public int weaponLevel;
    public int clothesLevel;

    public Unit currentWorker;

    private void Start()
    {
        if(workerGfx!=null && needsWorker)workerGfx.SetActive(false);
        position= transform.position;
        SetEquipmentLevels();

    }
    public void AssignWorker(Unit unitWorker)
    {

        if(!needsWorker)return;

        if (workerGfx != null) workerGfx.SetActive(true);

        currentWorker = unitWorker;

        UnitManager.inst.DeselectUnit(unitWorker);

        currentWorker.state = Unit.UnitState.Idle;
        currentWorker.ClearEndAction();

        currentWorker.gameObject.SetActive(false);
        needsWorker = false;

        //toolManager.inst.toolLevel=toolLevel;
        //toolManager.inst.weaponLevel=weaponLevel; if > etc...
        //equipmentManager, set clothes from this obj too...
    }

    public void FireWorker()
    {
        if (currentWorker == null) return;
        currentWorker.gameObject.SetActive(true);

        Vector3 spawnNear = BuildingManager.inst.selectedBuilding.transform.position;
        spawnPosition = GetNavMeshPointRadial(spawnNear, 2f, 3f);

        Unit unitWorker = currentWorker.GetComponent<Unit>();
        
        if (unitWorker != null)
        {
            //set to random empty position on the navmesh
            unitWorker.transform.position = spawnPosition;
            unitWorker.ClearEndAction();
            unitWorker.state=Unit.UnitState.Idle;
            
            UnitAnimation ua=currentWorker.GetComponent<UnitAnimation>();
            ua.ApplyState(unitWorker.state);
        }


        if (workerGfx != null) workerGfx.SetActive(false);
        currentWorker =null;
        needsWorker = true;
    }

    Vector3 GetNavMeshPointRadial(Vector3 center, float minRadius, float maxRadius)
    {
        for (int i = 0; i < 10; i++)
        {
            // Random direction on XZ plane
            Vector2 dir2D = Random.insideUnitCircle.normalized;
            Vector3 dir = new Vector3(dir2D.x, 0f, dir2D.y);

            // Pick distance away from building
            float distance = Random.Range(minRadius, maxRadius);

            Vector3 candidate = center + dir * distance;

            if (NavMesh.SamplePosition(candidate, out NavMeshHit hit, 2f, NavMesh.AllAreas))
                return hit.position;
        }

        Debug.LogWarning("No valid NavMesh point found near building");
        return center;
    }

    void SetEquipmentLevels()
    {
        if (toolLevel > 0)
        {
            if (EquipmentManager.inst.toolLevel < toolLevel)
            {
                EquipmentManager.inst.toolLevel = toolLevel;

                foreach(Unit unit in UnitManager.inst.units)
                {
                    UnitEquipment eq=unit.GetComponent<UnitEquipment>();
                    eq.toolLevel = EquipmentManager.inst.toolLevel;
                }
            }
        }
        if (weaponLevel > 0)
        {
            if (EquipmentManager.inst.weaponLevel < weaponLevel)
            {
                EquipmentManager.inst.weaponLevel = weaponLevel;
            }
        }

        if (clothesLevel > 0)
        {
            if (EquipmentManager.inst.clothesLevel < clothesLevel)
            {
                EquipmentManager.inst.clothesLevel = clothesLevel;
            }
        }
    }
}
