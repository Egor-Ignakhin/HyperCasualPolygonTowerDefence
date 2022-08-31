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
    internal class TrailIntersectionChecker
    {
        [SerializeField] private TrailRenderer trailRenderer;
        private List<Vector3> carvedPositions;
        private Vector3 intersectionPoint;
        private int lastPositionsCount;
        private TowerInvader mInvader;

        private Transform trailRendererTr;

        public void Initialize(TowerInvader invader)
        {
            lastPositionsCount = trailRenderer.positionCount;
            mInvader = invader;
            trailRendererTr = trailRenderer.transform;
        }

        public void Update()
        {
            var positionsCountChanged = lastPositionsCount != trailRenderer.positionCount;
            if (positionsCountChanged)
                OnPositionsCountChanged();
        }

        private void OnPositionsCountChanged()
        {
            var positions = new Vector3[trailRenderer.positionCount];
            trailRenderer.GetPositions(positions);

            lastPositionsCount = positions.Length;

            IntersectingCheck(positions);
        }

        private void IntersectingCheck(Vector3[] positions)
        {
            if (lastPositionsCount < 3)
                return;

            var outerVertices = Array.Empty<Vector3>();

            var isIntersecting = CurveIsIntersectMesh(positions) || MathExtensions.CurveIsIntersectItself(
                positions, out outerVertices, out intersectionPoint);

            if (!isIntersecting)
                return;

            carvedPositions = new List<Vector3>(positions);
            foreach (var t in outerVertices) carvedPositions.Remove(t);

            OnTrailIntersecting();
        }

        private bool CurveIsIntersectMesh(Vector3[] positions)
        {
            var mMeshFilter = MeshCreator.GetMesh(mInvader);
            return mMeshFilter &&
                   MathExtensions.CurveIsIntersectMesh(positions, mMeshFilter.mesh, out intersectionPoint);
        }

        private void OnTrailIntersecting()
        {
            var vertices = mInvader.CloseAFigure(carvedPositions.ToArray(), intersectionPoint);
            ResetTrailFromPoint(vertices.Last());
        }

        public void ResetTrailFromPoint(Vector3 point)
        {
            trailRenderer.Clear();
            trailRenderer.AddPosition(point);
            trailRenderer.AddPosition(trailRendererTr.position);
            lastPositionsCount = trailRenderer.positionCount;
        }
    }
}