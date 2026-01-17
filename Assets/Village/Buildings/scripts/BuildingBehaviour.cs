using UnityEngine;

public class BuildingBehaviour : MonoBehaviour, IBuilding
{
    public BuildingObject building;
    public BuildingObject Building => building;
    public BuildingBehaviour Bbh=>this;

    [SerializeField] SpriteRenderer selectionMarker;
    [SerializeField] GameObject destroyFX;
    Animator anim;

    private void Start()
    {
        BuildingManager.inst.buildings.Add(this);
        anim = GetComponentInChildren<Animator>();
        Invoke("DisableAnimator", 1f);

    }
    public void SelectBuilding()
    {
        selectionMarker.enabled = true;
    }
    public void DeselectBuilding()
    {
        selectionMarker.enabled = false;
    }

    void DisableAnimator()
    {
        if(anim!=null)anim.enabled = false;
    }

    public void DestroyFX()
    {
        Instantiate(destroyFX, transform.position, Quaternion.identity);
    }
    private void OnDestroy()
    {
        //Instantiate(destroyFX,transform.position,Quaternion.identity);
        BuildingManager.inst.buildings.Remove(this);
    }
}
