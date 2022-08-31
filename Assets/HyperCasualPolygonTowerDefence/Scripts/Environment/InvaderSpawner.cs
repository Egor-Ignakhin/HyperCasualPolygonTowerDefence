using UnityEngine;

namespace HyperCasualPolygonTowerDefence.Scripts.Environment
{
    public class InvaderSpawner : MonoBehaviour
    {
        [SerializeField] private TowerInvader invader;
        [SerializeField] private SpawnPoint spawnPoint;

        private void Start()
        {
            invader.Died += ReSpawnInvader;
        }

        private void OnDestroy()
        {
            invader.Died -= ReSpawnInvader;
        }

        private void ReSpawnInvader()
        {
            spawnPoint.Spawn(transform);
        }
    }
}