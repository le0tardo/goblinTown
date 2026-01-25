using System.Collections.Generic;
using UnityEngine;

public class PathPlacer : MonoBehaviour
{
    public bool placePath=false;
    public bool erasePath=false;
    [SerializeField] GameObject pathPrefab;
    [SerializeField] float minSpacing=0.5f;
    [SerializeField] LayerMask groundLayer;
    Vector3 lastPlaced;

    Camera cam;

    List<GameObject> paths = new List<GameObject>();

    private void Start()
    {
        cam = Camera.main;
    }
    private void Update()
    {
        ClickManager.inst.enabled = !placePath;

        if (placePath)
        {
            if (!erasePath)
            {
                if (Input.GetMouseButton(0))
                {
                    StartDraw();
                }

                if (Input.GetMouseButtonUp(0))
                {
                    placePath = false;
                }
            }
            else
            {
                if (Input.GetMouseButtonDown(0))
                {
                    StartErase();
                }
                if (Input.GetMouseButton(0))
                {
                    Erase();
                }
                if (Input.GetMouseButtonUp(0))
                {
                    EndErase();
                    erasePath = false;
                    placePath = false;
                }
            }
        }
    }
    void StartDraw()
    {
        if (!RayToGround(out Vector3 hitPos))
            return;

        if (Vector3.Distance(hitPos, lastPlaced) < minSpacing)
            return;

        var currentPath= Instantiate(pathPrefab, hitPos, Quaternion.identity,this.transform);

        float r = Random.Range(0.8f,1.2f);
        currentPath.transform.localScale = new Vector3(
            currentPath.transform.localScale.x*r,
            1,
            currentPath.transform.localScale.z*r);
        
        float rr = Random.Range(0f, 360f);
        currentPath.transform.rotation = Quaternion.Euler(0f, rr, 0f);
        paths.Add(currentPath);

        lastPlaced = hitPos;
    }

    void StartErase()
    {
        foreach (var path in paths)
        {
            BoxCollider box=path.GetComponent<BoxCollider>();
            box.enabled = true;
        }
    }
    void Erase()
    {
        if (!RayToGround(out Vector3 hitPos))
            return;

        if (Vector3.Distance(hitPos, lastPlaced) < minSpacing)
            return;

        // Raycast against the Path layer to see what to erase
        int pathLayer = LayerMask.NameToLayer("Path");
        int mask = 1 << pathLayer; // only Path

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 1000f, mask))
        {
            GameObject pathObj = hit.collider.gameObject;
            Debug.Log("erasing path: " + pathObj.name);

            // remove from your list if you keep track
            paths.Remove(pathObj);

            // destroy the object
            Destroy(pathObj);
        }

        lastPlaced = hitPos;
    }

    void EndErase()
    {
        foreach (var path in paths)
        {
            BoxCollider box = path.GetComponent<BoxCollider>();
            box.enabled = false;
        }
    }
    bool RayToGround(out Vector3 pos)
    {
        Ray ray = cam.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 200f,groundLayer))
        {
            pos = hit.point;
            pos = new Vector3(hit.point.x,0,hit.point.z);
            return true;
        }
        pos = default;
        return false;
    }
}
