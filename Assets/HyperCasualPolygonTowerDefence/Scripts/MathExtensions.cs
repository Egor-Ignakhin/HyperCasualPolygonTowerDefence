using System;
using System.Collections.Generic;
using UnityEngine;

namespace HyperCasualPolygonTowerDefence.Scripts
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

        public static List<LinesRelationship> GetLinesRelationship(Vector3Line lineA, Vector3Line lineB,
            out Vector3 intersectionPoint, bool a0IsEnd = true, bool a1IsEnd = true, bool b0IsEnd = true,
            bool b1IsEnd = true, bool x = true, bool y = true, bool z = true)
        {
            float uxy, uxz, uyz, u, tx, ty, tz, t;
            var relationships = GetLinesRelationship(lineA.From, lineA.To, lineB.From, lineB.To, out intersectionPoint,
                out uxy,
                out uxz, out uyz, out u, out tx, out ty, out tz, out t, x, y, z);
            if (!relationships.Contains(LinesRelationship.Intersect)) return relationships;

            if (!(!a0IsEnd || t >= 0) || !(!a1IsEnd || t <= 1) || !(!b0IsEnd || u >= 0) ||
                !(!b1IsEnd || u <= 1))
                relationships.Remove(LinesRelationship.Intersect);
            return relationships;
        }

        private static List<LinesRelationship> GetLinesRelationship(Vector3 a0, Vector3 a1, Vector3 b0, Vector3 b1,
            out Vector3 intersectionPoint, out float uxy, out float uxz, out float uyz, out float u, out float tx,
            out float ty, out float tz, out float t, bool x = true, bool y = true, bool z = true)
        {
            if (!x)
                a0.x = a1.x = b0.x = b1.x = 0;
            if (!y)
                a0.y = a1.y = b0.y = b1.y = 0;
            if (!z)
                a0.z = a1.z = b0.z = b1.z = 0;

            uxy = (b0.x - a0.x - (b0.y - a0.y) / (a1.y - a0.y) * (a1.x - a0.x)) /
                  ((b1.y - b0.y) / (a1.y - a0.y) * (a1.x - a0.x) - (b1.x - b0.x));
            uxz = (b0.x - a0.x - (b0.z - a0.z) / (a1.z - a0.z) * (a1.x - a0.x)) /
                  ((b1.z - b0.z) / (a1.z - a0.z) * (a1.x - a0.x) - (b1.x - b0.x));
            uyz = (b0.y - a0.y - (b0.z - a0.z) / (a1.z - a0.z) * (a1.y - a0.y)) /
                  ((b1.z - b0.z) / (a1.z - a0.z) * (a1.y - a0.y) - (b1.y - b0.y));

            u = !float.IsNaN(uxy) ? uxy : !float.IsNaN(uxz) ? uxz : uyz;


            tx = (b0.x + u * (b1.x - b0.x) - a0.x) / (a1.x - a0.x);
            ty = (b0.y + u * (b1.y - b0.y) - a0.y) / (a1.y - a0.y);
            tz = (b0.z + u * (b1.z - b0.z) - a0.z) / (a1.z - a0.z);

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
    }

    public struct Vector3Line
    {
        public Vector3 From;
        public Vector3 To;
    }
}