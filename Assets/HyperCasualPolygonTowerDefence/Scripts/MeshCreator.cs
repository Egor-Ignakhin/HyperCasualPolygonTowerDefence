using System;
using UnityEngine;

namespace HyperCasualPolygonTowerDefence.Scripts
{
    public static class MeshCreator
    {
        public static GameObject CreateGameObject(Vector3[] vertices, int[] triangles)
        {
            var newGm = new GameObject("Mesh", typeof(MeshRenderer), typeof(MeshFilter));
            var mesh = newGm.GetComponent<MeshFilter>().mesh;

            mesh.Clear();
            mesh.vertices = vertices;
            mesh.triangles = triangles;
            mesh.uv = Array.Empty<Vector2>();
            mesh.Optimize();
            mesh.RecalculateNormals();

            return newGm;
        }
    }
}