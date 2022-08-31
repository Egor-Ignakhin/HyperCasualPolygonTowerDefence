using System;
using System.Collections.Generic;
using HyperCasualPolygonTowerDefence.Scripts.Environment;
using HyperCasualPolygonTowerDefence.Scripts.Extensions;
using UnityEngine;

namespace HyperCasualPolygonTowerDefence.Scripts
{
    [Serializable]
    internal class TrailCutter
    {
        [SerializeField] private TrailRenderer trailRenderer;
        [SerializeField] private Transform transform;
        [SerializeField] private TowerInvader mInvader;

        public void Update()
        {
            if (trailRenderer.positionCount == 0)
                return;

            TryCutInvaderTrails();
        }

        private void TryCutInvaderTrails()
        {
            var invaders = new List<IInvader>(InvadersCounter.GetInvaders());
            
            var advancedLine = new Vector3Line
            {
                From = trailRenderer.GetPosition(trailRenderer.positionCount - 1),
                To = transform.position
            };
            
            foreach (var invader in invaders)
            {
                if((TowerInvader)invader == mInvader)
                    continue;
                
                TryCutInvaderTrail(invader,advancedLine);
            }
        }

        private void TryCutInvaderTrail(IInvader invader, Vector3Line advancedLine)
        {
            var enemyVertices = new Vector3[invader.GetPositionsCount()];
            invader.GetTrailPositions(enemyVertices);

            if (MathExtensions.LineIsIntersectedCurve(advancedLine, enemyVertices))
                invader.Die();
        }
    }
}