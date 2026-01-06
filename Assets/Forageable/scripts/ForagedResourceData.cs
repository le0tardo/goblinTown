using UnityEngine;

[CreateAssetMenu(
    fileName = "ForageableResource",
    menuName = "Forageables/Forageable Resource"
)]
public class ForagedResourceData : ScriptableObject
{

    public VillageResource category;
    public string resourceName;
    //public GameObject resourceModel;
    public Mesh mesh;
    public Material material;
    public Sprite resourceSprite;
}
