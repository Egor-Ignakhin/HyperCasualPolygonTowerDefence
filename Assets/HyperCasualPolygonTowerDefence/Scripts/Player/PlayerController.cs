using HyperCasualPolygonTowerDefence.Scripts.Environment;
using UnityEngine;

namespace HyperCasualPolygonTowerDefence.Scripts.Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private PlayerMotion motion;
        [SerializeField] private SpawnPoint spawnPoint;
        [SerializeField] private TowerInvader towerInvader;
        [SerializeField] private TrailController trailController;

        private void Start()
        {
            InvadersCounter.invaders.Add(towerInvader);
            towerInvader.Died += TowerInvaderOnDied;

            trailController.Initialize(towerInvader);
            trailController.TrailIsIntersecting += TrailController_OnTrailIntersecting;
        }

        private void Update()
        {
            trailController.Update();

            motion.Move();
            motion.Rotate();
        }

        private void OnDestroy()
        {
            trailController.TrailIsIntersecting -= TrailController_OnTrailIntersecting;
        }

        private void TrailController_OnTrailIntersecting()
        {
            var carvedPositions = trailController.GetCarvedPositions();
            var intersectionPoint = trailController.GetIntersectionPoint();

            var vertices = towerInvader.CloseAFigure(carvedPositions, intersectionPoint);
            trailController.ResetTrailPosition(vertices);
        }

        private void TowerInvaderOnDied()
        {
            spawnPoint.Spawn(transform);
            trailController.Clear();
        }
    }
}