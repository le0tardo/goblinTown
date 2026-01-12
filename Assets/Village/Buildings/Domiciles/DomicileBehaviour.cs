using UnityEngine;
using UnityEngine.AI;

public class DomicileBehaviour : MonoBehaviour
{
    public int goblinCapIncrease = 1;
    public int level = 1;

    public VillageResource resource;
    [SerializeField] GameObject unit;
    public float required;
    public float available;
    [SerializeField] public float spawnTime;
    [SerializeField] public float maxSpawnTime;
    public bool spawning = false;
    public bool canSpawn=false;
    Vector3 spawnPosition;

    private void Start()
    {
        UnitManager.inst.maxUnits += goblinCapIncrease;
        spawnPosition = Vector3.zero;
    }
    private void Update()
    {
        if (VillageResourceManager.inst.villageResources.TryGetValue(resource, out int a))
        {
            available = a;
        }
        else
        {
            available = 0;
        }

        if (UnitManager.inst.units.Count < UnitManager.inst.maxUnits && required <= available &&!spawning)
        {
            canSpawn = true;
        }
        else
        {
            canSpawn = false;
        }

        if (spawning)
        {
            if (spawnTime > 0)
            {
                spawnTime -= Time.deltaTime;
            }
            else
            {
                SpawnUnit();
            }
        }
    }

    public void StartSpawn() //call this from button
    {
        spawning = true;
        Vector3 spawnNear = BuildingManager.inst.selectedBuilding.transform.position;
        spawnPosition = GetNavMeshPointRadial(spawnNear, 2f, 3f);
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
    public void SpawnUnit()
    {
        int cost = Mathf.RoundToInt(required);
        VillageResourceManager.inst.RemoveResource(resource, cost);

        Instantiate(unit, spawnPosition, Quaternion.identity);

        spawnTime = maxSpawnTime;
        spawning = false;
    }

    private void OnDestroy()
    {
        UnitManager.inst.maxUnits -= goblinCapIncrease;
    }


}
