using UnityEngine;

public interface IAttackable
{
    bool IsAlive { get; }
    Vector3 Position { get; }

    void TakeDamage(int amount);
}
