using UnityEngine;

public class BuildingBehaviour : MonoBehaviour, IBuilding
{
    public BuildingObject building;
    public BuildingObject Building => building;
    public BuildingBehaviour Bbh=>this;

    [SerializeField] SpriteRenderer selectionMarker;
    public void SelectBuilding()
    {
        selectionMarker.enabled = true;
    }
    public void DeselectBuilding()
    {
        selectionMarker.enabled = false;
    }
}
