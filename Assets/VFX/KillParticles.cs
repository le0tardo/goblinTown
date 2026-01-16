using UnityEngine;

public class KillParticles : MonoBehaviour
{
    ParticleSystem ps;
    void Start()
    {
        ps=GetComponentInChildren<ParticleSystem>();
    }

    void Update()
    {
        if (ps.isStopped)
        {
            Destroy(this.gameObject);
            Debug.Log("destroyed: "+this.gameObject.name);
        }
    }
}
