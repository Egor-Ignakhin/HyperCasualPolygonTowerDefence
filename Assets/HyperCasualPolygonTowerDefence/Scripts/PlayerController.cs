using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEngine;

namespace HyperCasualPolygonTowerDefence.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private TrailRenderer trailRenderer;
        [SerializeField] private PlayerMotion playerMotion;
        [SerializeField] private SpawnPoint spawnPoint;
        [SerializeField] private TowerInvader towerInvader;
        [SerializeField] private TrailCutter trailCutter;
        private int lastPositionsCount;

        private void Start()
        {
            trailCutter.OnInit();

            InvadersCounter.invaders.Add(towerInvader);
            towerInvader.Died += TowerInvaderOnDied;

            lastPositionsCount = trailRenderer.positionCount;
        }

        private void Update()
        {
            trailCutter.TryCutPlayerTrail();

            if (trailRenderer.positionCount == 1 && !Input.GetMouseButton(0))
                trailRenderer.SetPosition(0, transform.position);

            if (Input.GetMouseButton(1))
                trailRenderer.Clear();

            if (lastPositionsCount != trailRenderer.positionCount)
                OnChangedPositionsCount();

            playerMotion.Move();
            playerMotion.Rotate();

            if (!Input.GetMouseButton(0))
            {
                trailRenderer.minVertexDistance = 100f;
                return;
            }

            trailRenderer.minVertexDistance = 1f;
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
                return;

            var positionCount = trailRenderer.positionCount;
            for (var i = 0; i < positionCount; i++)
            {
                Gizmos.color = Color.green;
                var position = trailRenderer.GetPosition(i);
                Gizmos.DrawSphere(position, 0.1f);
            }
        }

        private void TowerInvaderOnDied()
        {
            spawnPoint.Spawn(transform);
            trailRenderer.Clear();
        }

        private void OnChangedPositionsCount()
        {
#if UNITY_EDITOR
            ClearLog();
#endif

            var positions = new Vector3[trailRenderer.positionCount];
            trailRenderer.GetPositions(positions);

            lastPositionsCount = positions.Length;

            if (lastPositionsCount < 3)
                return;

            Vector3[] outerVertices;
            Vector3 intersectionPoint;
            if (!MathExtensions.CurveIsIntersectItself(positions, out outerVertices, out intersectionPoint))
                return;

            var carvedPositions = new List<Vector3>(positions);
            foreach (var t in outerVertices) carvedPositions.Remove(t);
            var vertices = towerInvader.CloseAFigure(carvedPositions.ToArray(), intersectionPoint);
            ResetTrailPosition(vertices);
        }

        private void ResetTrailPosition(List<Vector3> vertices)
        {
            trailRenderer.Clear();
            trailRenderer.AddPosition(vertices.Last());
            trailRenderer.AddPosition(trailRenderer.transform.position);
            lastPositionsCount = trailRenderer.positionCount;
        }

        private void ClearLog()
        {
            var assembly = Assembly.GetAssembly(typeof(Editor));
            var type = assembly.GetType("UnityEditor.LogEntries");
            var method = type.GetMethod("Clear");
            method.Invoke(new object(), null);
        }
    }
}