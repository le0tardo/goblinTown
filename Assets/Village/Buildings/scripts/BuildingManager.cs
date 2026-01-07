using UnityEngine;

public class BuildingManager : MonoBehaviour
{
    public static BuildingManager inst;
    [SerializeField] public BuildingObject currentBuilding; //building to place!
    [SerializeField] public BuildingBehaviour selectedBuilding;

    [SerializeField] LayerMask groundLayer;
    [SerializeField] LayerMask nonBlockingLayers;
    [SerializeField] Material validMat;
    [SerializeField] Material invalidMat;

    GameObject previewInstance;
    MeshFilter previewMeshFilter;
    MeshRenderer previewRenderer;

    float gridSize = 0.5f;

    public bool isPlacingBuilding=false;
    private void Awake()
    {
        if (inst != null && inst != this){Destroy(gameObject);return;}inst = this;
    }

    #region //selection
    public void SelectBuilding(BuildingBehaviour bbh)
    {
        selectedBuilding= bbh;
        bbh.SelectBuilding();
    }
    public void DeselectBuilding()
    {
        if (selectedBuilding == null) {  return; }
        BuildingBehaviour bbh= selectedBuilding;
        bbh.DeselectBuilding();
        selectedBuilding= null;
    }
    #endregion
    public void SetBuilding(BuildingObject building)
    {
        currentBuilding = building;
        if (building != null) { CreatePreviewObject();}
    }

    void Update()
    {
        if (currentBuilding == null)
            return;

        isPlacingBuilding = true;
        UpdatePreview();

        if (Input.GetMouseButtonDown(0) && IsPlacementValid(previewInstance.transform.position)&&CanAffordBuilding(currentBuilding)) //can afford, add in updatePreview!!
        {
            VillageResourceManager.inst.SpendBuildingCost(currentBuilding);
            PlaceBuilding(previewInstance.transform.position);
            Invoke("DelayBool", 0.25f);
        }
    }

    void DelayBool()
    {
        isPlacingBuilding = false;
    }
    void CreatePreviewObject()
    {
        previewInstance = new GameObject("Building Preview");

        previewMeshFilter = previewInstance.AddComponent<MeshFilter>();
        previewRenderer = previewInstance.AddComponent<MeshRenderer>();

        previewMeshFilter.mesh = currentBuilding.previewMesh;
        previewRenderer.material = invalidMat;
    }
    void UpdatePreview()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(ray, out RaycastHit hit, 100f, groundLayer))
        {
            Vector3 pos = SnapToGrid(hit.point);

            //maybe snap to integers? so the pos can be 12,-43,2, but never 12.442,-43.33, etc..

            // snap to grid later if you want
            previewInstance.transform.position = pos;

            bool valid = IsPlacementValid(pos);
            previewRenderer.material = valid ? validMat : invalidMat;
        }
    }
    Vector3 SnapToGrid(Vector3 pos)
    {
        pos.x = Mathf.Round(pos.x / gridSize) * gridSize;
        pos.z = Mathf.Round(pos.z / gridSize) * gridSize;
        return pos;
    }
    void ClearPreview()
    {
        if (previewInstance != null)
        {
            Destroy(previewInstance);
            previewInstance = null;
        }
        currentBuilding = null;
    }
    void PlaceBuilding(Vector3 position)
    {
        Instantiate(
            currentBuilding.buildingPrefab,
            position,
            Quaternion.identity
        );

        ClearPreview();
        DeselectBuilding();
    }

    public void DestroyBuilding()
    {
        if (selectedBuilding != null)
        {
            //instantiate some destory vfx here
            Destroy(selectedBuilding.gameObject);
        }
    }
    bool CanAffordBuilding(BuildingObject building)
    {
        var village = VillageResourceManager.inst;

        foreach (var cost in building.costs)
        {
            if (!village.villageResources.TryGetValue(cost.resource, out int available))
                return false;

            if (available < cost.amount)
                return false;
        }

        return true;
    }

    bool IsPlacementValid(Vector3 position)
    {
        //sq buildings do overlap box instead of sphere

        float radius = currentBuilding.footprintSize.x * 2;

        LayerMask blockingMask = ~nonBlockingLayers;

        Collider[] hits = Physics.OverlapSphere(
            position,
            radius,
            blockingMask
        );

        foreach (var hit in hits)
        {
            //Debug.Log($"Blocked by: {hit.name} ({LayerMask.LayerToName(hit.gameObject.layer)})");
            return false;
        }

        return true;
    }

}
