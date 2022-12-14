using System;
using System.Collections.Generic;
using UnityEngine;

namespace HyperCasualPolygonTowerDefence.Scripts.Extensions
{
    public static class MathExtensions
    {
        [Serializable]
        public enum LinesRelationship
        {
            Intersect,
            Parallel,
            Superposition,
            Equal
        }

        public static bool PointIsInsideTriangle(Vector2 point, Vector2[] vertices)
        {
            var v1Y = vertices[1].y;
            var v2X = vertices[2].x;
            var v0Y = vertices[0].y;
            var v1X = vertices[1].x;
            var v2Y = vertices[2].y;
            var v0X = vertices[0].x;

            var area = 0.5f * (-v1Y * v2X + v0Y * (-v1X + v2X) +
                               v0X * (v1Y - v2Y) + v1X * v2Y);
            var s = 1f / (2f * area) * (v0Y * v2X - v0X * v2Y +
                                        (v2Y - v0Y) * point.x +
                                        (v0X - v2X) * point.y);
            var t = 1f / (2f * area) * (v0X * v1Y - v0Y * v1X +
                                        (v0Y - v1Y) * point.x +
                                        (v1X - v0X) * point.y);
            var p = vertices[0] + (vertices[1] - vertices[0]) * s + (vertices[2] - vertices[0]) * t;


            return s is >= 0 and <= 1 &&
                   t is >= 0 and <= 1 &&
                   s + t <= 1;
        }

        private static List<LinesRelationship> GetLinesRelationship(Vector3Line lineA, Vector3Line lineB,
            out Vector3 intersectionPoint, bool a0IsEnd = true, bool a1IsEnd = true, bool b0IsEnd = true,
            bool b1IsEnd = true, bool x = true, bool y = true, bool z = true)
        {
            var relationships = GetLinesRelationship(lineA.From, lineA.To, lineB.From, lineB.To, out intersectionPoint,
                out var u, out var t, x, y, z);
            if (!relationships.Contains(LinesRelationship.Intersect)) return relationships;

            if (!(!a0IsEnd || t >= 0) || !(!a1IsEnd || t <= 1) || !(!b0IsEnd || u >= 0) ||
                !(!b1IsEnd || u <= 1))
                relationships.Remove(LinesRelationship.Intersect);
            return relationships;
        }

        private static List<LinesRelationship> GetLinesRelationship(Vector3 a0, Vector3 a1, Vector3 b0, Vector3 b1,
            out Vector3 intersectionPoint, out float u, out float t, bool x = true, bool y = true, bool z = true)
        {
            if (!x)
                a0.x = a1.x = b0.x = b1.x = 0;
            if (!y)
                a0.y = a1.y = b0.y = b1.y = 0;
            if (!z)
                a0.z = a1.z = b0.z = b1.z = 0;

            var uxy = (b0.x - a0.x - (b0.y - a0.y) / (a1.y - a0.y) * (a1.x - a0.x)) /
                      ((b1.y - b0.y) / (a1.y - a0.y) * (a1.x - a0.x) - (b1.x - b0.x));
            var uxz = (b0.x - a0.x - (b0.z - a0.z) / (a1.z - a0.z) * (a1.x - a0.x)) /
                      ((b1.z - b0.z) / (a1.z - a0.z) * (a1.x - a0.x) - (b1.x - b0.x));
            var uyz = (b0.y - a0.y - (b0.z - a0.z) / (a1.z - a0.z) * (a1.y - a0.y)) /
                      ((b1.z - b0.z) / (a1.z - a0.z) * (a1.y - a0.y) - (b1.y - b0.y));

            u = !float.IsNaN(uxy) ? uxy : !float.IsNaN(uxz) ? uxz : uyz;


            var tx = (b0.x + u * (b1.x - b0.x) - a0.x) / (a1.x - a0.x);
            var ty = (b0.y + u * (b1.y - b0.y) - a0.y) / (a1.y - a0.y);
            var tz = (b0.z + u * (b1.z - b0.z) - a0.z) / (a1.z - a0.z);

            t = !float.IsNaN(tx) ? tx : !float.IsNaN(ty) ? ty : tz;

            intersectionPoint = (a1 - a0) * t + a0;

            var list = new List<LinesRelationship>();

            if (a0 == b0 && a1 == b1)
                list.Add(LinesRelationship.Equal);
            if ((a0 == b1 && a1 == b0) || (a0 == b0 && a1 == b1))
                list.Add(LinesRelationship.Superposition);
            if ((a0 - a1).normalized == (b0 - b1).normalized || (a0 - a1).normalized == (b1 - b0).normalized)
                list.Add(LinesRelationship.Parallel);
            if (!((float.IsNaN(intersectionPoint.x) && a0.x - a1.x != 0 && b0.x - b1.x != 0) ||
                  (float.IsNaN(intersectionPoint.y) && a0.y - a1.y != 0 && b0.y - b1.y != 0) ||
                  (float.IsNaN(intersectionPoint.z) && a0.z - a1.z != 0 && b0.z - b1.z != 0)))
                list.Add(LinesRelationship.Intersect);
            return list;
        }

        public static Vector3 FindCentroid3D(Vector3[] vertices)
        {
            float verticesXSum = 0;
            float verticesYSum = 0;
            float verticesZSum = 0;
            for (var i = 0; i < vertices.Length; i++)
            {
                verticesXSum += vertices[i].x;
                verticesYSum += vertices[i].y;
                verticesZSum += vertices[i].z;
            }

            var centroid = new Vector3(verticesXSum, verticesYSum, verticesZSum) * (1f / vertices.Length);

            return centroid;
        }

        public static bool LineIsIntersectedCurve(Vector3Line advancedLine, Vector3[] vertices)
        {
            for (var i = 0; i < vertices.Length - 1; i++)
            {
                var lineB = new Vector3Line
                {
                    From = vertices[i],
                    To = vertices[i + 1]
                };

                var linesRelationships =
                    GetLinesRelationship(advancedLine, lineB, out _);

                if (linesRelationships.Count != 1)
                    continue;

                if (linesRelationships[0] == LinesRelationship.Intersect)
                    return true;
            }

            return false;
        }

        private static bool LineIsIntersectedCurve(Vector3Line advancedLine, Vector3[] vertices,
            out Vector3 intersectionPoint)
        {
            for (var i = 0; i < vertices.Length - 1; i++)
            {
                var lineB = new Vector3Line
                {
                    From = vertices[i],
                    To = vertices[i + 1]
                };

                var linesRelationships =
                    GetLinesRelationship(advancedLine, lineB, out intersectionPoint);

                if (linesRelationships.Count != 1)
                    continue;

                if (linesRelationships[0] == LinesRelationship.Intersect)
                    return true;
            }

            intersectionPoint = Vector3.zero;
            return false;
        }

        public static bool CurveIsIntersectItself(Vector3[] vertices, out Vector3[] outerVertices,
            out Vector3 interSectionPoint)
        {
            for (var i = 0; i < vertices.Length - 1; i++)
            {
                var lineA = new Vector3Line
                {
                    From = vertices[i],
                    To = vertices[i + 1]
                };
                for (var j = 0; j < vertices.Length - 1; j++)
                {
                    var lineB = new Vector3Line
                    {
                        From = vertices[j],
                        To = vertices[j + 1]
                    };

                    if (lineA == lineB)
                        continue;

                    if (lineA.To == lineB.From)
                        continue;

                    if (lineA.From == lineB.To)
                        continue;

                    var linesRelationships =
                        GetLinesRelationship(lineA, lineB, out interSectionPoint);

                    if (linesRelationships.Count != 1)
                        continue;

                    if (linesRelationships[0] != LinesRelationship.Intersect)
                        continue;

                    outerVertices = new Vector3[i];
                    for (var k = 0; k < i; k++) outerVertices[k] = vertices[k];

                    return true;
                }
            }

            outerVertices = Array.Empty<Vector3>();
            interSectionPoint = Vector3.zero;
            return false;
        }

        public static List<Vector3> ComputeOuterVerticesFromClosedFigure(Vector3[] vertices, Vector3 intersectionPoint)
        {
            var outerVertices = new List<Vector3>();

            for (var i = 0; i < vertices.Length; i++)
            {
                if (Vector3.Distance(vertices[i], intersectionPoint) > 0.1f) continue;
                for (var j = 0; j < i; j++) outerVertices.Add(vertices[j]);

                break;
            }

            return outerVertices;
        }

        public static bool CurveIsIntersectMesh(IReadOnlyList<Vector3> positions, Mesh mesh,
            out Vector3 intersectionPoint)
        {
            var mMeshVertices = mesh.vertices;

            var lineEnd = new Vector3Line
            {
                From = positions[^2],
                To = positions[^1]
            };
            var curveIsIntersectMesh = LineIsIntersectMesh(
                lineEnd, mMeshVertices, out intersectionPoint);

            return curveIsIntersectMesh;
        }

        private static bool LineIsIntersectMesh(in Vector3Line lineIn, IReadOnlyList<Vector3> meshVertices,
            out Vector3 intersectionPoint)
        {
            for (var i = 0; i < meshVertices.Count - 1; i++)
            {
                var line = new Vector3Line
                {
                    From = meshVertices[i],
                    To = meshVertices[i + 1]
                };

                var linesRelationships =
                    GetLinesRelationship(lineIn, line, out intersectionPoint);

                if (linesRelationships.Count != 1)
                    continue;

                if (linesRelationships[0] == LinesRelationship.Intersect) return true;
            }

            intersectionPoint = Vector3.zero;
            return false;
        }
    }
}