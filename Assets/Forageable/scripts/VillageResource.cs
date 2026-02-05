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
        Bricks,
        Coal,
        Skin,
        Leather
    }
    public Resource resource;
    public Sprite resourceIcon;
}
