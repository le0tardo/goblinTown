using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(
    fileName = "Database",
    menuName = "Forageables/Forageable Database"
)]
public class ResourceDatabaseObject : ScriptableObject
{
    public List<ForagedResourceData> resources;
}
