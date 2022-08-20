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

            var intersectionPoint = new Vector3(0, 0, 0);
            if (!CanClosingAFigure(positions, ref intersectionPoint))
                return;

            CloseAFigure(positions, intersectionPoint, out var triangles);
        }

        private void CloseAFigure(Vector3[] positions, Vector3 intersectionPoint, out List<int> triangles)
        {
            var vertices = new List<Vector3>();

            var positionsWithoutStartPoint = new List<Vector3>(positions);
            positionsWithoutStartPoint.RemoveAt(0);
            var centralPoint = MathExtensions.FindCentroid3D(positionsWithoutStartPoint.ToArray());

            vertices.Add(centralPoint);
            for (var i = 1; i < positions.Length - 1; i++) vertices.Add(positions[i]);

            vertices.Add(intersectionPoint);

            triangles = new List<int>();
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

            TryInvadeTower(vertices);
            ResetTrailPosition(vertices);
        }

        private static bool CanClosingAFigure(Vector3[] positions, ref Vector3 intersectionPoint)
        {
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

            if (lineB.From == lineA.To)
                return false;

            var linesRelationships =
                MathExtensions.GetLinesRelationship(lineA, lineB, out intersectionPoint);

            if (linesRelationships.Count != 1)
                return false;

            return linesRelationships[0] == MathExtensions.LinesRelationship.Intersect;
        }

        private void TryInvadeTower(List<Vector3> vertices)
        {
            for (var i = 0; i < vertices.Count; i++)
            {
                var vertices3D = new Vector3[3];

                //Setting central point
                vertices3D[0] = vertices[0];

                //Setting a current point
                vertices3D[1] = vertices[i];

                //Setting a next point
                var iIsLastIndex = i == vertices.Count - 1;
                if (iIsLastIndex)
                    vertices3D[2] = vertices[1];
                else
                    vertices3D[2] = vertices[i + 1];

                var vertices2D = new Vector2[]
                {
                    vertices3D[0],
                    vertices3D[1],
                    vertices3D[2]
                };

                var towerIsInsideTriangle = MathExtensions.PointIsInsideTriangle(towerTr.position, vertices2D);

                if (!towerIsInsideTriangle)
                    continue;

                towerTr.GetComponent<Tower>().SetInvaderInventory(GetComponent<Inventory>());
                break;
            }
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