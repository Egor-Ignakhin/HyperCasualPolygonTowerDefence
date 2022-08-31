using System;
using System.Collections.Generic;
using System.Linq;
using HyperCasualPolygonTowerDefence.Scripts.Extensions;
using UnityEngine;

namespace HyperCasualPolygonTowerDefence.Scripts.Bot
{
    [Serializable]
    internal class BotTargetDesignator
    {
        public event Action TargetSwitched;
        
        [SerializeField] private BotTowerInvader towerInvader;
        private PathHolder<Vector3> pathHolder;
        private readonly IPathFactory pathFactory = new BotPathFactory();
        [SerializeField] private Transform transform;
        [SerializeField] private TrailRenderer trailRenderer;
        private Vector3 target;

        public void ReInitialize()
        {
            var towerPos = towerInvader.FindNearestTowerPosition(transform.position);
            pathHolder = new PathHolder<Vector3>
            {
                Path = pathFactory.Generate(towerPos)
            };
            SwitchTarget();
        }

        public void SwitchTarget()
        {
            if (pathHolder.GetIterator() == pathHolder.Path.Count)
            {
                CloseAFigure();
                RegeneratePath();
            }

            if (pathHolder.GetIterator() > 0)
                trailRenderer.AddPosition(transform.position);

            target = pathHolder.Path[pathHolder.GetIterator()];

            pathHolder.Next();
            
            TargetSwitched?.Invoke();
        }

        private void RegeneratePath()
        {
            var towerPos = towerInvader.FindNearestTowerPosition(transform.position);
            pathHolder.Path = pathFactory.Generate(towerPos);
        }

        private void CloseAFigure()
        {
            pathHolder.Reset();
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
            ResetTrailFromPoint(vertices.Last());
        }

        private void ResetTrailFromPoint(Vector3 point)
        {
            trailRenderer.Clear();
            trailRenderer.AddPosition(point);
            trailRenderer.AddPosition(trailRenderer.transform.position);
        }

        public Vector3 GetTarget()
        {
            return target;
        }
    }
}