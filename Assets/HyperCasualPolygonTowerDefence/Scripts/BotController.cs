using System.Collections.Generic;
using System.Linq;
using HyperCasualPolygonTowerDefence.Scripts;
using UnityEditor;
using UnityEngine;

public class BotController : MonoBehaviour
{
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private List<Vector3> path = new();
    [SerializeField] private int vertexCount = 5;
    [SerializeField] private BotMotion botMotion;
    [SerializeField] private BotTowerInvader towerInvader;
    private int currentPathId;
    private Vector2 target;
    [SerializeField] private TrailRenderer playerTrail;
    [SerializeField] private SpawnPoint spawnPoint;

    private void Start()
    {
        InvadersCounter.invaders.Add(towerInvader);
        towerInvader.Died += TowerInvaderOnDied;
        
        ReInit();
        transform.position = path[0];
        trailRenderer.Clear();
        botMotion.TargetIsReached += ChangeTarget;
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
        GeneratePath();
        ChangeTarget();
    }

    private void Update()
    {
        var mVertices = new Vector3[trailRenderer.positionCount];
        trailRenderer.GetPositions(mVertices);

        var playerVertices = new Vector3[playerTrail.positionCount];
        playerTrail.GetPositions(playerVertices);
        if (MathExtensions.CurvesAreIntersected(mVertices, playerVertices))
        {
            TowerInvaderOnDied();
            return;
        }
        
        botMotion.Move();
        botMotion.Rotate();
    }

    private void OnDrawGizmos()
    {
        for (var i = 0; i < path.Count; i++)
        {
            Handles.color = Color.red;
            if (i < path.Count - 1)
                Handles.DrawLine(path[i], path[i + 1]);
            Gizmos.color = new Color(0f, 0f, 0f, i / (float)path.Count + 0.1f);
            Gizmos.DrawSphere(path[i], 0.1f);
        }
    }

    private void ChangeTarget()
    {
        if (currentPathId == path.Count)
        {
            currentPathId = 0;
            towerInvader.TryInvadeTowers(path);
            towerInvader.TryInvadeInvaders(path);
            
            var positions = new Vector3[trailRenderer.positionCount];
            trailRenderer.GetPositions(positions);
            
            var intersectionPoint = new Vector3(0, 0, 0);
            if (true/*towerInvader.BotCanCloseFigure(positions, transform.position)*/)
            {
                var vertices = towerInvader.CloseAFigure(positions, intersectionPoint);
                ResetTrailPosition(vertices);
            }

            GeneratePath();
        }

        if(currentPathId > 0)
        {
            trailRenderer.AddPosition(transform.position);
        }

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

    private void GeneratePath()
    {
        path.Clear();
        var towerPos = towerInvader.FindTargetTowerPosition(transform.position);

        var radDifference = vertexCount / 360f;

        float vecLenght = 2;
        path.Add(towerPos + Vector3.right * vecLenght);
        path.Add(towerPos + Vector3.down * vecLenght);
        path.Add(towerPos + Vector3.left * vecLenght);
        path.Add(towerPos + Vector3.up * vecLenght);
        path.Add(towerPos + Vector3.right * vecLenght);

        for (var i = 0; i < vertexCount; i++)
        {
            /*var pointPos = towerPos;
            var vectorToPos = new Vector3(1*(i%2==0?1:-1),1*(i%2==1?1:-1));
            
            vectorToPos = vectorToPos.Rotate(radDifference * i*2);
            pointPos += vectorToPos;
            path.Add(pointPos);*/
        }
    }
}