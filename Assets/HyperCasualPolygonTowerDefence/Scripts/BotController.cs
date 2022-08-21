using System.Collections.Generic;
using System.Linq;
using HyperCasualPolygonTowerDefence.Scripts;
using UnityEngine;

public class BotController : MonoBehaviour
{
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private BotMotion botMotion;
    [SerializeField] private BotTowerInvader towerInvader;
    [SerializeField] private SpawnPoint spawnPoint;
    [SerializeField] private TrailCutter trailCutter;
    private readonly IPathGenerator pathGenerator = new BotPathGenerator();
    private int currentPathId;
    private Vector2 target;

    private void Start()
    {
        trailCutter.OnInit();
        InvadersCounter.invaders.Add(towerInvader);
        towerInvader.Died += TowerInvaderOnDied;

        ReInit();
        botMotion.TargetIsReached += ChangeTarget;
    }

    private void Update()
    {
        trailCutter.TryCutPlayerTrail();

        botMotion.Move();
        botMotion.Rotate();
    }

    private void OnDrawGizmos()
    {
        for (var i = 0; i < trailRenderer.positionCount; i++) Gizmos.DrawSphere(trailRenderer.GetPosition(i), 0.1f);
    }

    private void TowerInvaderOnDied()
    {
        currentPathId = 0;
        ReInit();
        spawnPoint.Spawn(transform);
        trailRenderer.Clear();
    }

    private void ReInit()
    {
        var towerPos = towerInvader.FindNearestTowerPosition(transform.position);
        pathGenerator.Generate(towerPos);
        ChangeTarget();
    }

    private void ChangeTarget()
    {
        var path = pathGenerator.GetPath();
        if (currentPathId == path.Count)
        {
            currentPathId = 0;
            towerInvader.TryInvadeTowers(path);
            towerInvader.TryInvadeInvaders(path);

            var positions = new Vector3[trailRenderer.positionCount];
            trailRenderer.GetPositions(positions);

            var intersectionPoint = path[0];
            var outerVertices = MathExtensions.ComputeOuterVerticesFromClosedFigure(positions, intersectionPoint);
            var carvedVertices = new List<Vector3>(positions);
            for (var i = 0; i < outerVertices.Count; i++)
                carvedVertices.Remove(outerVertices[i]);

            var vertices = towerInvader.CloseAFigure(carvedVertices.ToArray(), intersectionPoint);
            ResetTrailPosition(vertices);

            var towerPos = towerInvader.FindNearestTowerPosition(transform.position);
            pathGenerator.Generate(towerPos);
        }

        if (currentPathId > 0) trailRenderer.AddPosition(transform.position);

        target = path[currentPathId];
        currentPathId++;

        botMotion.SetDestination(target);
    }

    private void ResetTrailPosition(List<Vector3> vertices)
    {
        trailRenderer.Clear();
        trailRenderer.AddPosition(vertices.Last());
        trailRenderer.AddPosition(trailRenderer.transform.position);
    }
}