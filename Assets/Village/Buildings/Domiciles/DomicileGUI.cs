using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;
using TMPro;


public class DomicileGUI : MonoBehaviour
{
    public VillageResource resource;
    [SerializeField] GameObject unit;
    public float required;
    public float available;
    [SerializeField] float spawnTime;
    [SerializeField] float maxSpawnTime;
    public bool spawning = false;
    Vector3 spawnPosition;

    DomicileBehaviour dom;

    [Header("UI")]
    [SerializeField] TextMeshProUGUI unitCount;
    [SerializeField] TextMeshProUGUI unitCost;
    [SerializeField] Image costResource;
    [SerializeField] Image spawnProgression;
    [SerializeField] Button spawnButton;

    private void Start()
    {
        unitCost.text = required.ToString();
        costResource.sprite=resource.resourceIcon;
        spawnPosition = Vector3.zero;
    }

    private void Update()
    {
        dom=BuildingManager.inst.selectedBuilding.GetComponent<DomicileBehaviour>();
        if (dom == null){return;}

        spawnProgression.fillAmount = spawnTime / maxSpawnTime;
        unitCount.text=UnitManager.inst.units.Count.ToString()+"/"+UnitManager.inst.maxUnits.ToString();
        SetColors();

        if(VillageResourceManager.inst.villageResources.TryGetValue(resource,out int a))
        {
            available=a;
        }
        else
        {
            available= 0;   
        }

        if (UnitManager.inst.units.Count < UnitManager.inst.maxUnits && required<=available)
        {
            spawnButton.interactable = true;
        }
        else
        {
            spawnButton.interactable= false;
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
        spawnPosition = GetNavMeshPointRadial(spawnNear,2f,3f);
    }
    Vector3 GetNavMeshPointRadial(Vector3 center,float minRadius,float maxRadius)
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

        Instantiate(unit,spawnPosition,Quaternion.identity);
        
        spawnTime = maxSpawnTime;
        spawning = false;
    }

    void SetColors()
    {
        if (required > available)
        {
            unitCost.color = Color.red;
        }
        else
        {
            unitCost.color = Color.white;
        }
        if (UnitManager.inst.units.Count < UnitManager.inst.maxUnits)
        {
            unitCount.color = Color.white;
        }
        else
        {
            unitCount.color = Color.red;
        }
    }
}
