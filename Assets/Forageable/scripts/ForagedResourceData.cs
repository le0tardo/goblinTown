using UnityEngine;

[CreateAssetMenu(
    fileName = "ForageableResource",
    menuName = "Forageables/Forageable Resource"
)]
public class ForagedResourceData : ScriptableObject
{
    /*public enum ForageResource
    {
        //woods
        Oak,
        Birch,
        Pine,
        //foods
        Berry,
        Fish,
        //stones
        Stone,
        //clay
        Clay
         
    }
    */
    //public ForageResource resourceType;
    public MacroResourceCategory category;
    public string resourceName;
    //public GameObject resourceModel;
    public Mesh mesh;
    public Material material;
    public Sprite resourceSprite;
}
