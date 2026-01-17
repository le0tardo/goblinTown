using UnityEngine;

public interface IProducer
{
    bool IsProducing { get; }
    float Progress01 { get; }
    Transform WorldTransform { get; }
    //Sprite ProductionIcon { get; }
}
