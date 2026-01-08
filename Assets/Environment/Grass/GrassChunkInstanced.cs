using System.Collections.Generic;
using UnityEngine;

public class GrassChunkInstanced : MonoBehaviour
{
    [Header("Grass Setup")]
    public Mesh[] grassMeshes;
    public Material grassMaterial;

    [Header("Chunk Settings")]
    public int instancesPerChunk = 300;
    public Vector2 chunkSize = new Vector2(10f, 10f);

    [Header("Variation")]
    public Vector2 scaleRange = new Vector2(0.8f, 1.2f);

    Dictionary<Mesh, List<Matrix4x4>> instances;
    Bounds chunkBounds;
    Camera mainCam;

    void Awake()
    {
        mainCam = Camera.main;
        Generate();
    }

    void Generate()
    {
        instances = new Dictionary<Mesh, List<Matrix4x4>>();

        foreach (var mesh in grassMeshes)
            instances[mesh] = new List<Matrix4x4>();

        Vector3 center = transform.position + new Vector3(chunkSize.x * 0.5f, 0, chunkSize.y * 0.5f);
        chunkBounds = new Bounds(center, new Vector3(chunkSize.x, 5f, chunkSize.y));

        for (int i = 0; i < instancesPerChunk; i++)
        {
            Vector3 pos = new Vector3(
                Random.Range(0, chunkSize.x),
                0f,
                Random.Range(0, chunkSize.y)
            ) + transform.position;

            Quaternion rot = Quaternion.Euler(0f, Random.Range(0f, 360f), 0f);
            float scale = Random.Range(scaleRange.x, scaleRange.y);

            Mesh mesh = grassMeshes[Random.Range(0, grassMeshes.Length)];
            instances[mesh].Add(Matrix4x4.TRS(pos, rot, Vector3.one * scale));
        }
    }

    void Update()
    {
        if (!IsVisible())
            return;

        foreach (var pair in instances)
        {
            var list = pair.Value;
            int count = list.Count;

            for (int i = 0; i < count; i += 1023)
            {
                int batchSize = Mathf.Min(1023, count - i);
                Graphics.DrawMeshInstanced(
                    pair.Key,
                    0,
                    grassMaterial,
                    list.GetRange(i, batchSize)
                );
            }
        }
    }

    bool IsVisible()
    {
        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(mainCam);
        return GeometryUtility.TestPlanesAABB(planes, chunkBounds);
    }
}
