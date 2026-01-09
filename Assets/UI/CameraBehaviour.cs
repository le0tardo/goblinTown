using UnityEngine;

public class CameraBehaviour : MonoBehaviour
{
    [Header("Zoom")]
    [SerializeField] float zoomSpeed = 20f;
    [SerializeField] float zoomSmooth = 10f;

    [Tooltip("How far forward the camera may move from its start position")]
    [SerializeField] float maxZoomDistance = 40f;

    [SerializeField] float currentZoom; // visible in Inspector

    Vector3 startPosition;
    float targetZoom;

    [Header("Mouse Edge Pan")]
    [SerializeField] float edgeSize = 25f;
    [SerializeField] float worldHalfSize = 10f;
    [SerializeField] float minPanSpeed = 5f;
    [SerializeField] float maxPanSpeed = 25f;
    Vector3 panOffset;
    [Header("Keyboard Pan")]
    [SerializeField] bool keyboardPanEnabled = true;
    [SerializeField] float keyboardPanMultiplier = 2f;


    Camera cam;
    public bool edgePanEnabled;

    void Start()
    {
        cam = Camera.main;
        startPosition = transform.position;
        currentZoom = 0f;     // fully zoomed OUT
        targetZoom = 0f;
    }


    void Update()
    {
        HandleZoom();
        HandleEdgePan();
        HandleKeyboardPan();

        currentZoom = Mathf.Lerp(
            currentZoom,
            targetZoom,
            Time.deltaTime * zoomSmooth
        );

        transform.position =
            startPosition +
            panOffset +
            transform.forward * currentZoom;

    }


    void HandleZoom()
    {
        float scroll = Input.mouseScrollDelta.y;
        if (Mathf.Abs(scroll) < 0.01f)
            return;

        // Scroll up = zoom in
        targetZoom += scroll * zoomSpeed;

        targetZoom = Mathf.Clamp(targetZoom, 0f, maxZoomDistance);
    }
    void HandleEdgePan()
    {
        if (!edgePanEnabled || !MouseInsideScreen()) return;
        Vector3 dir = Vector3.zero;

        Vector3 mouse = Input.mousePosition;

        if (mouse.x <= edgeSize) dir.x -= 1f;
        if (mouse.x >= Screen.width - edgeSize) dir.x += 1f;
        if (mouse.y <= edgeSize) dir.z -= 1f;
        if (mouse.y >= Screen.height - edgeSize) dir.z += 1f;

        if (dir == Vector3.zero)
            return;

        float zoomT = 1f - (currentZoom / maxZoomDistance);
        float panSpeed = Mathf.Lerp(minPanSpeed, maxPanSpeed, zoomT);

        Vector3 move =
            transform.right * dir.x +
            Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized * dir.z;

        panOffset += move * panSpeed * Time.deltaTime;
        ClampPanOffset();

    }
    void HandleKeyboardPan()
    {
        if (!keyboardPanEnabled) return;

        float h = Input.GetAxisRaw("Horizontal"); // A / D
        float v = Input.GetAxisRaw("Vertical");   // W / S

        if (Mathf.Abs(h) < 0.01f && Mathf.Abs(v) < 0.01f)
            return;

        float zoomT = 1f - (currentZoom / maxZoomDistance);
        float panSpeed = Mathf.Lerp(minPanSpeed, maxPanSpeed, zoomT);

        Vector3 move =
            transform.right * h +
            Vector3.ProjectOnPlane(transform.forward, Vector3.up).normalized * v;

        if (move.sqrMagnitude > 1f)
            move.Normalize();

        panOffset += move * panSpeed * keyboardPanMultiplier * Time.deltaTime;
        ClampPanOffset();
    }



    bool MouseInsideScreen()
    {
        Vector3 m = Input.mousePosition;
        return m.x >= 0 && m.x <= Screen.width &&
               m.y >= 0 && m.y <= Screen.height;
    }
    void ClampCamera()
    {
        Vector3 pos = transform.position;

        pos.x = Mathf.Clamp(pos.x, -worldHalfSize, worldHalfSize);
        pos.z = Mathf.Clamp(pos.z, -worldHalfSize, worldHalfSize);

        transform.position = pos;
    }
    void ClampPanOffset()
    {
        panOffset.x = Mathf.Clamp(panOffset.x, -worldHalfSize, worldHalfSize);
        panOffset.z = Mathf.Clamp(panOffset.z, -worldHalfSize, worldHalfSize);
    }

}
