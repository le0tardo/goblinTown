using UnityEngine;

public class BoarSpawner : MonoBehaviour
{
    [SerializeField] GameObject boarPrefab;
    [SerializeField] GameObject currentBoar;
    [SerializeField] float spawnTime;
    bool spawning=false;

    private void Update()
    {
        if (currentBoar == null && !spawning)
        {
            Invoke("SpawnBoar", spawnTime);
            spawning = true;
        }
    }

    void SpawnBoar()
    {
        GameObject tempBoar= Instantiate(boarPrefab,transform.position,Quaternion.identity);
        currentBoar = tempBoar;
        spawning=false;
    }
}
