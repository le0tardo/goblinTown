using UnityEngine;

[CreateAssetMenu(
    fileName = "VillageResource",
    menuName = "Forageables/Village Resource"
)]
public class VillageResource : ScriptableObject
{
    public enum Resource
    {
        Wood,
        Food,
        Stone,
        Clay,
        Coal
    }
    public Resource resource;
    public Sprite resourceIcon;
}
