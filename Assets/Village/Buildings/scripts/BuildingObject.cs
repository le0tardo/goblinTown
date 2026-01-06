using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct BuildingCost
{
    public VillageResource resource;
    public int amount;
}
public enum BuildingType
{
    Deposit,
    Domicile,
    Production
}

[CreateAssetMenu(
    fileName = "BuildingDefinition",
    menuName = "Buildings/Building Definition"
)]
public class BuildingObject : ScriptableObject
{
    public string buildingName;
    public Sprite buildingIcon;
    public BuildingType buildingType;
    public BuildingCost[] costs;
    public Vector2 footprintSize;
    public Mesh previewMesh;
    public GameObject buildingPrefab;

    void OnValidate() //max 4 different resources
    {
        if (costs != null && costs.Length > 4)
        {
            Debug.LogWarning($"{name} has more than 4 costs. Trimming.");
            System.Array.Resize(ref costs, 4);
        }
    }
}
