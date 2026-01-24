using UnityEngine;

public class AnimalProximityTrigger : MonoBehaviour
{
    AnimalBehaviour animal;

    private void Start()
    {
        animal=GetComponentInParent<AnimalBehaviour>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Unit"))
        {
            Debug.Log("a unit has spooked me!");
            animal.Flee(other.transform);
        }
    }
}
