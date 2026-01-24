using UnityEngine;

[CreateAssetMenu(
    fileName = "Animal",
    menuName = "Forageables/Animal"
)]
public class AnimalObject : ScriptableObject
{
    public string title;
    public ForagedResourceData drop;
}
