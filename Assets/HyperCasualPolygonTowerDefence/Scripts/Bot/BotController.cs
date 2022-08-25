using System.Collections.Generic;
using System.Linq;
using HyperCasualPolygonTowerDefence.Scripts.Environment;
using HyperCasualPolygonTowerDefence.Scripts.Extensions;
using UnityEngine;

namespace HyperCasualPolygonTowerDefence.Scripts.Bot
{
    public class BotController : MonoBehaviour
    {
        [SerializeField] private TrailRenderer trailRenderer;
        [SerializeField] private BotMotion botMotion;
        [SerializeField] private BotTowerInvader towerInvader;
        [SerializeField] private SpawnPoint spawnPoint;
        [SerializeField] private TrailCutter trailCutter;

        private readonly IPathFactory pathFactory = new BotPathFactory();
        private PathHolder<Vector3> pathHolder;

        private void Start()
        {
            trailCutter.Initialize();
            InvadersCounter.invaders.Add(towerInvader);
            towerInvader.Died += TowerInvaderOnDied;

            ReInit();
            botMotion.TargetIsReached += ChangeTarget;
        }

        private void Update()
        {
            trailCutter.TryCutTrail();

            botMotion.Move();
            botMotion.Rotate();
        }

        private void TowerInvaderOnDied()
        {
            pathHolder.Iterator = 0;
            ReInit();
            spawnPoint.Spawn(transform);
            trailRenderer.Clear();
        }

        private void ReInit()
        {
            var towerPos = towerInvader.FindNearestTowerPosition(transform.position);
            pathHolder = new PathHolder<Vector3>
            {
                Path = pathFactory.Generate(towerPos)
            };
            ChangeTarget();
        }

        private void ChangeTarget() //// 
        {
            if (pathHolder.Iterator == pathHolder.Path.Count)
            {
                pathHolder.Iterator = 0;
                towerInvader.TryInvadeTowers(pathHolder.Path);
                towerInvader.TryInvadeInvaders(pathHolder.Path);

                var positions = new Vector3[trailRenderer.positionCount];
                trailRenderer.GetPositions(positions);

                var intersectionPoint = pathHolder.Path[0];
                var outerVertices = MathExtensions.ComputeOuterVerticesFromClosedFigure(positions, intersectionPoint);
                var carvedVertices = new List<Vector3>(positions);
                foreach (var t in outerVertices)
                    carvedVertices.Remove(t);

                var vertices = towerInvader.CloseAFigure(carvedVertices.ToArray(), intersectionPoint);
                ResetTrailPosition(vertices);

                var towerPos = towerInvader.FindNearestTowerPosition(transform.position);
                pathHolder.Path = pathFactory.Generate(towerPos);
            }

            if (pathHolder.Iterator > 0) trailRenderer.AddPosition(transform.position);

            var target = pathHolder.Path[pathHolder.Iterator];
            botMotion.SetDestination(target);
            pathHolder.Iterator++;
        }

        private void ResetTrailPosition(List<Vector3> vertices)
        {
            trailRenderer.Clear();
            trailRenderer.AddPosition(vertices.Last());
            trailRenderer.AddPosition(trailRenderer.transform.position);
        }
    }
}