using UnityEngine;

public class SnowFlakeFX : MonoBehaviour
{
    ParticleSystem snow;
    [SerializeField] bool isWinter;
    [SerializeField] bool followCamera;

    [SerializeField] Transform cam;
    Vector3 offset;

    private void Awake()
    {
        if (!cam) cam = Camera.main.transform;
        offset = transform.position - cam.position;
    }

    private void Start()
    {
        snow = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if(followCamera)SnowToCamera();

        if (SeasonManager.inst.seasonT > 0.75f && SeasonManager.inst.seasonT<0.9f)
        {
            isWinter = true;
        }
        else
        {
            isWinter = false;
        }

        if (isWinter)
        {
            if (!snow.isPlaying)
                snow.Play();
        }
        else
        {
            if (snow.isPlaying)
                snow.Stop();
        }
    }

    void SnowToCamera()
    {
        Vector3 camPos = cam.position;

        Vector3 targetPos = new Vector3(
            camPos.x + offset.x,
            transform.position.y,
            camPos.z + offset.z
        );

        transform.position = Vector3.Lerp(
            transform.position,
            targetPos,
            2.5f * Time.deltaTime
        );
    }
}
