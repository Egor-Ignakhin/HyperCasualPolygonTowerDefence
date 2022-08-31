using System;
using System.Collections.Generic;
using System.Linq;
using HyperCasualPolygonTowerDefence.Scripts.Environment;
using HyperCasualPolygonTowerDefence.Scripts.Extensions;
using HyperCasualPolygonTowerDefence.Scripts.Instruments;
using UnityEngine;

namespace HyperCasualPolygonTowerDefence.Scripts.Player
{
    [Serializable]
    internal class TrailController
    {
        [SerializeField] private TrailRenderer trailRenderer;
        [SerializeField] private TrailCutter trailCutter;
        private List<Vector3> carvedPositions;
        private Vector3 intersectionPoint;
        private int lastPositionsCount;
        private TowerInvader mInvader;
        private Transform trailRendererTr;

        public void Initialize(TowerInvader invader)
        {
            lastPositionsCount = trailRenderer.positionCount;
            trailCutter.Initialize();
            mInvader = invader;
            mInvader.Died += ClearTrail;
            trailRendererTr = trailRenderer.transform;
        }

        public void Update()
        {
            trailCutter.Update();

            var positionsCountChanged = lastPositionsCount != trailRenderer.positionCount;
            if (positionsCountChanged)
                OnPositionsCountChanged();
        }

        private void OnPositionsCountChanged()
        {
            var positions = new Vector3[trailRenderer.positionCount];
            trailRenderer.GetPositions(positions);

            lastPositionsCount = positions.Length;

            if (lastPositionsCount < 3)
                return;

            Vector3[] outerVertices;
            
            if (CurveIsIntersectMesh(positions))
            {
                outerVertices = Array.Empty<Vector3>();
            }
            else
            {
                var curveIsIntersectItself = MathExtensions.CurveIsIntersectItself(
                    positions, out outerVertices, out intersectionPoint);
                
                if (!curveIsIntersectItself)
                    return;
            }

            carvedPositions = new List<Vector3>(positions);
            foreach (var t in outerVertices) carvedPositions.Remove(t);

            OnTrailIntersecting();
        }

        private bool CurveIsIntersectMesh(Vector3[] positions)
        {
            var mMeshFilter = MeshCreator.GetMesh(mInvader);
            return mMeshFilter && MathExtensions.CurveIsIntersectMesh(positions, mMeshFilter.mesh, out intersectionPoint);
        }

        private void OnTrailIntersecting()
        {
            var vertices = mInvader.CloseAFigure(carvedPositions.ToArray(), intersectionPoint);
            ResetTrailFromPoint(vertices.Last());
        }

     

        public void ClearTrail()
        {
            trailRenderer.Clear();
        }

        public void ResetTrailFromPoint(Vector3 point)
        {
            ClearTrail();
            trailRenderer.AddPosition(point);
            trailRenderer.AddPosition(trailRendererTr.position);
            lastPositionsCount = trailRenderer.positionCount;
        }
    }
}