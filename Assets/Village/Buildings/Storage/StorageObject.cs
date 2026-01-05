using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Buildings/Storage")]
public class StorageObject : ScriptableObject
{

    public string buildingName;
    public List<ForagedResourceData> acceptedResources;
    public int capacity;
}
