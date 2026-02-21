using UnityEngine;

public class FireplaceBehaviour : MonoBehaviour, IFireplace
{
    Vector3 pos;
    public Vector3 SitPosition => pos;

    private void Start()
    {
        pos = transform.position;
    }
}
