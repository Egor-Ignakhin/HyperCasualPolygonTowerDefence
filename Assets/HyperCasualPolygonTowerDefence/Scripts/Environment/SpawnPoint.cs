using System.Collections;
using UnityEngine;

namespace HyperCasualPolygonTowerDefence.Scripts.Environment
{
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
}