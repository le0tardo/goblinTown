using UnityEngine;

[CreateAssetMenu(
    fileName = "ForageableNode",
    menuName = "Forageables/Forageable Node"
)]


public class ForageableNodeData : ScriptableObject
{
    public enum ForageNodeType
    {
        Tree,
        Bush,
        Rock
    }
    public ForageNodeType nodeType;

    [Header("Interaction")]
    public float interactionRange = 1.5f;

    [Header("Harvesting")]
    public float harvestDuration = 2f;
}
