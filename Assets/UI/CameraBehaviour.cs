using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    [SerializeField] float zoomSpeed = 5f;
    [SerializeField] float minFOV = 25f;
    [SerializeField] float maxFOV = 45f;

    [SerializeField] float panSpeed = 10f;
    [SerializeField] float edgeSize = 10f;
    [SerializeField] float worldHalfSize = 10f;


    Camera cam;
    float targetFOV;

    void Start()
    {
        cam = Camera.main;
        targetFOV = cam.fieldOfView;
    }

    void Zoom(float delta)
    {
        targetFOV -= delta * zoomSpeed;
        targetFOV = Mathf.Clamp(targetFOV, minFOV, maxFOV);
    }

    void Update()
    {
        HandleMouseZoom();
        //HandlePinchZoom(); //for mobile
        HandleEdgePan();

        cam.fieldOfView = Mathf.Lerp(
            cam.fieldOfView,
            targetFOV,
            Time.deltaTime * 8f
        );
    }

    void HandleMouseZoom()
    {
        float scroll = Input.mouseScrollDelta.y;
        if (Mathf.Abs(scroll) > 0.01f)
        {
            Zoom(scroll);
        }
    }
    void HandleEdgePan()
    {
        Vector3 move = Vector3.zero;
        Vector3 mouse = Input.mousePosition;

        if (mouse.x < edgeSize) move.x -= 1;
        if (mouse.x > Screen.width - edgeSize) move.x += 1;
        if (mouse.y < edgeSize) move.z -= 1;
        if (mouse.y > Screen.height - edgeSize) move.z += 1;

        transform.Translate(move * panSpeed * Time.deltaTime, Space.World);
        ClampCamera();
    }

    void ClampCamera()
    {
        Vector3 pos = transform.position;

        pos.x = Mathf.Clamp(pos.x, -worldHalfSize, worldHalfSize);
        pos.z = Mathf.Clamp(pos.z, -worldHalfSize, worldHalfSize);

        transform.position = pos;
    }
}
