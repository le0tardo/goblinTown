using UnityEngine;

public interface IWorkable
{
    bool NeedsWorker { get; set; }

    Vector3 Position { get; }
    void AssignWorker(Unit unit);
}
