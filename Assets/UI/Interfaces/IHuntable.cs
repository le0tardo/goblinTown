using UnityEngine;

public interface IHuntable
{
    Vector3 Position { get; }
    void OnHit(int damage, Unit attacker);
}
