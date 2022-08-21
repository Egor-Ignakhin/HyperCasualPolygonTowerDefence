using System;
using System.Collections;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private float spawnTime;
    private float maxSpawnTime;
    private Vector3 spawnPosition;

    private void Start()
    {
        maxSpawnTime = spawnTime;
        spawnPosition = transform.position;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 0.1f);
    }

    private bool CanSpawn()
    {
        return spawnTime < 0;
    }
    
    public void Spawn(Transform transform)
    {
       // StartCoroutine(Spawning(transform));
        transform.position = spawnPosition;
    }

    private IEnumerator Spawning(Transform transform)
    {
        while (!CanSpawn())
        {
            yield return null;
            spawnTime -= Time.deltaTime;
        }

        transform.position = spawnPosition;

        spawnTime = maxSpawnTime;
    }
}