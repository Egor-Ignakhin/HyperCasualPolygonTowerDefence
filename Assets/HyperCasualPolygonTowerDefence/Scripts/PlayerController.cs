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
        [SerializeField] private Transform towerTr;
        [SerializeField] private Material playerMaterial;
        [SerializeField] private PlayerMove playerMove;

        private int lastPositionsCount;


        private void Start()
        {
            lastPositionsCount = trailRenderer.positionCount;
        }

        private void Update()
        {
            if (trailRenderer.positionCount == 1 && !Input.GetMouseButton(0))
                trailRenderer.SetPosition(0, transform.position);

            if (Input.GetMouseButton(1))
                trailRenderer.Clear();

            if (lastPositionsCount != trailRenderer.positionCount)
                OnChangedPositionsCount();

            playerMove.Move();

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

            if (true)
                return;
            for (var i = 0; i < positionCount; i++)
            {
                if (i == 0 || i == positionCount - 1)
                    continue;


                var vertices3D = new Vector3[3]
                {
                    trailRenderer.GetPosition(i - 1),
                    trailRenderer.GetPosition(i),
                    trailRenderer.GetPosition(i + 1)
                };
                var vertices2D = new Vector2[]
                {
                    vertices3D[0],
                    vertices3D[1],
                    vertices3D[2]
                };

                Handles.color = MathExtensions.PointIsInsideTriangle(towerTr.position, vertices2D)
                    ? Color.yellow
                    : Color.green;

                Handles.DrawAAConvexPolygon(vertices3D);
            }
        }

        private void OnChangedPositionsCount()
        {
            var positions = new Vector3[trailRenderer.positionCount];
            trailRenderer.GetPositions(positions);
            
            ClearLog();

            lastPositionsCount = positions.Length;

            if (lastPositionsCount < 3)
                return;

            var lineA = new Vector3Line
            {
                From = positions[0],
                To = positions[1]
            };

            var lineB = new Vector3Line
            {
                From = positions[^2],
                To = positions[^1]
            };
            
            if(lineB.From == lineA.To)
                return;

            var linesRelationships =
                MathExtensions.GetLinesRelationship(lineA, lineB,
                    out var intersectionPoint);

            if (linesRelationships.Count != 1)
                return;

            if (linesRelationships[0] != MathExtensions.LinesRelationship.Intersect)
            {
                return;
            }

            var vertices = new List<Vector3>();

            var positionsWithoutStartPoint = new List<Vector3>(positions);
            positionsWithoutStartPoint.RemoveAt(0);
            var centralPoint = MathExtensions.FindCentroid3D(positionsWithoutStartPoint.ToArray());

            vertices.Add(centralPoint);
            for (var i = 1; i < positions.Length - 1; i++) vertices.Add(positions[i]);

            vertices.Add(intersectionPoint);

            var triangles = new List<int>();
            for (var i = 1; i < vertices.Count; i++)
            {
                //Setting central point
                triangles.Add(0);

                //Setting a current point
                triangles.Add(i);

                //Setting a next point
                var iIsLastIndex = i == vertices.Count - 1;
                if (iIsLastIndex)
                    triangles.Add(1);
                else
                    triangles.Add(i + 1);
            }

            var newGm = MeshCreator.CreateGameObject(vertices.ToArray(), triangles.ToArray());
            newGm.GetComponent<MeshRenderer>().sharedMaterial = playerMaterial;

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