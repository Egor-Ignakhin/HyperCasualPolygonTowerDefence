using System;
using System.Collections.Generic;
using HyperCasualPolygonTowerDefence.Scripts.Environment;
using UnityEngine;

namespace HyperCasualPolygonTowerDefence.Scripts.Instruments
{
    public static class MeshCreator
    {
        private static readonly Dictionary<TowerInvader, MeshFilter> invaderMeshes = new();

        public static void CreateMesh(Vector3[] vertices, int[] triangles, TowerInvader key, Material material)
        {
            Mesh mesh;
            var meshExisted = invaderMeshes.ContainsKey(key);
            if (meshExisted)
            {
                var meshFilter = invaderMeshes[key];
                mesh = meshFilter.mesh;
                var existedVertices = mesh.vertices;
                var existedTriangles = mesh.triangles;

                var newVertices = new List<Vector3>(existedVertices);
                var newTriangles = new List<int>(existedTriangles);

                newVertices.AddRange(vertices);

                for (var i = 0; i < triangles.Length; i++)
                {
                    triangles[i] += existedVertices.Length;
                    
                    newTriangles.Add(triangles[i]);
                }

                mesh.Clear();
                mesh.vertices = newVertices.ToArray();
                mesh.triangles = newTriangles.ToArray();
            }
            else
            {
                var newGm = new GameObject(key + " Mesh");
                newGm.AddComponent<MeshRenderer>().sharedMaterial = material;
                mesh = newGm.AddComponent<MeshFilter>().mesh;
                invaderMeshes.Add(key, newGm.GetComponent<MeshFilter>());

                mesh.Clear();
                mesh.vertices = vertices;
                mesh.triangles = triangles;
            }

            mesh.uv = Array.Empty<Vector2>();
            mesh.Optimize();
            mesh.RecalculateNormals();
        }

        public static MeshFilter GetMesh(TowerInvader key)
        {
            return invaderMeshes.ContainsKey(key) ? invaderMeshes[key] : null;
        }
    }
}