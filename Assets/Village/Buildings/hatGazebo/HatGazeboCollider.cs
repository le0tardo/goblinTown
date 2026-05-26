using UnityEngine;

public class HatGazeboCollider : MonoBehaviour
{
    SphereCollider col;

    private void Start()
    {
        col=GetComponent<SphereCollider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Unit"))
        {
            HatScript hatScript=other.GetComponentInChildren<HatScript>();
            if (hatScript != null)
            {
                hatScript.GetHat();
                hatScript.ColorHat();
            }
        }
    }
}
