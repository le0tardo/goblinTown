using UnityEngine;
public interface IForageable
{
    Vector3 Position { get; }
    ForageableNodeData NodeData { get; }
    public ForagedResourceData Resource {  get; }
    void Forage(Unit unit);

    bool IsDepleted { get; }
}