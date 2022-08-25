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
        [SerializeField] private Transform transform;
        [SerializeField] private TrailCutter trailCutter;
        private List<Vector3> carvedPositions;
        private Vector3 intersectionPoint;
        private int lastPositionsCount;
        private TowerInvader mInvader;
        public event Action TrailIsIntersecting;

        public void Initialize(TowerInvader invader)
        {
            lastPositionsCount = trailRenderer.positionCount;
            trailCutter.Initialize();
            mInvader = invader;
        }

        public void Update()
        {
            trailCutter.TryCutTrail();

            if (lastPositionsCount != trailRenderer.positionCount)
                OnChangedPositionsCount();
        }

        private void OnChangedPositionsCount()
        {
            var positions = new Vector3[trailRenderer.positionCount];
            trailRenderer.GetPositions(positions);

            lastPositionsCount = positions.Length;

            if (lastPositionsCount < 3)
                return;


            var curveIsIntersectItself = MathExtensions.CurveIsIntersectItself(
                positions, out var outerVertices, out intersectionPoint);
            if (curveIsIntersectItself)
            {
            }
            else
            {
                if (CurveIsIntersectMesh(positions))
                {
                }
                else
                {
                    return;
                }
            }
            
            carvedPositions = new List<Vector3>(positions);
            foreach (var t in outerVertices) carvedPositions.Remove(t);

            TrailIsIntersecting?.Invoke();
        }

        private bool CurveIsIntersectMesh(Vector3[] positions)
        {
            var mMeshFilter = MeshCreator.GetMesh(mInvader);
            if (!mMeshFilter)
                return false;

            var mMeshVertices = mMeshFilter.mesh.vertices;

            var curveIsIntersectMesh = MathExtensions.CurveIsIntersectMesh(
                positions, mMeshVertices, out intersectionPoint);

            return curveIsIntersectMesh;
        }

        public void Clear()
        {
            trailRenderer.Clear();
        }

        public void ResetTrailPosition(List<Vector3> vertices)
        {
            trailRenderer.Clear();
            trailRenderer.AddPosition(vertices.Last());
            trailRenderer.AddPosition(trailRenderer.transform.position);
            lastPositionsCount = trailRenderer.positionCount;
        }

        public Vector3[] GetCarvedPositions()
        {
            return carvedPositions.ToArray();
        }

        public Vector3 GetIntersectionPoint()
        {
            return intersectionPoint;
        }
    }
}