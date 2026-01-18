using UnityEngine;

public class FishLineFX : MonoBehaviour
{
    public Transform pointA;
    public Transform pointB;

    LineRenderer line;

    void Awake()
    {
        line = GetComponent<LineRenderer>();
        line.positionCount = 2;
        line.useWorldSpace = true;
    }

    void Update()
    {
        if (!pointA || !pointB) return;

        line.SetPosition(0, pointA.position);
        line.SetPosition(1, pointB.position);
    }
}
