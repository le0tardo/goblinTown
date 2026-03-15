using System.Collections;
using Unity.VisualScripting;
using UnityEditor.EditorTools;
using UnityEngine;
using UnityEngine.AI;

public class WorkHouseBehaviour : MonoBehaviour, IWorkable, IProducer
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

    public float workTime = 0;
    public float workProgress = 0;

    //sync to interface
    public bool IsProducing => !needsWorker;
    public float Progress01 =>
        !needsWorker && workTime > 0f
            ? Mathf.Clamp01(workProgress / workTime)
            : 0f;
    public Transform WorldTransform => transform;

    private void Start()
    {
        if(workerGfx!=null && needsWorker)workerGfx.SetActive(false);
        position= transform.position;

    }

    private void Update()
    {
        //GUI sync
        if (workProgress==0)
        {
            MiniProductionBarManager.inst.Hide(this);
            return;
        }
        else
        {
            MiniProductionBarManager.inst.Show(this);
        }
    }
    public void AssignWorker(Unit unitWorker)
    {

        if(!needsWorker)return;

        if (workerGfx != null) workerGfx.SetActive(true);

        currentWorker = unitWorker;

        UnitManager.inst.DeselectUnit(unitWorker);

        currentWorker.state = Unit.UnitState.Idle;
        currentWorker.ClearEndAction();
        currentWorker.ReleaseSlot();

        currentWorker.gameObject.SetActive(false);
        needsWorker = false;

        if(toolLevel>0)CountToolsNeeded();
        if(clothesLevel>0)CountClohtesNeeded();
        //SetEquipmentLevels(); <-delayed this
    }

    void CountToolsNeeded()
    {
        if (toolLevel > 0)
        {
            float n = 0;
            var units = UnitManager.inst.units;

            foreach(Unit unit in units)
            {
                UnitEquipment eq=unit.GetComponent<UnitEquipment>();
                if (eq.toolLevel < toolLevel)
                {
                    n++;
                }
            }

            if (n > 0)
            {
                workTime = n; workProgress = 0;
                //multiply n?
                StartCoroutine(CraftRoutine());
            }
        }
    }

    void CountClohtesNeeded()
    {
        if (clothesLevel > 0)
        {
            float n = 0;
            var units = UnitManager.inst.units;

            foreach (Unit unit in units)
            {
                UnitEquipment eq = unit.GetComponent<UnitEquipment>();
                if (eq.clothesLevel < clothesLevel)
                {
                    n++;
                }
            }

            if (n > 0)
            {
                workTime = n; workProgress = 0;
                //multiply n?
                StartCoroutine(CraftRoutine());
            }
        }
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

        StopAllCoroutines();
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

        if (clothesLevel > 0)
        {
            if (EquipmentManager.inst.clothesLevel < clothesLevel)
            {
                EquipmentManager.inst.clothesLevel = clothesLevel;
               
                foreach (Unit unit in UnitManager.inst.units)
                {
                    UnitEquipment eq = unit.GetComponent<UnitEquipment>();
                    eq.clothesLevel = EquipmentManager.inst.clothesLevel;
                    eq.EquipClothes();
                }
            }
        }
    }

    IEnumerator CraftRoutine()
    {
        // Reset progress
        workProgress = 0f;

        // Count up until we reach workTime
        while (workProgress < workTime)
        {
            workProgress += Time.deltaTime;

            // Optional: clamp so it never overshoots
            if (workProgress > workTime)
                workProgress = workTime;

            yield return null; // wait one frame
        }

        // Finished crafting
        workProgress = 0f;
        SetEquipmentLevels();
    }


}
