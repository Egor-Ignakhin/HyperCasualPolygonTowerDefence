using System;
using System.Collections.Generic;
using HyperCasualPolygonTowerDefence.Scripts;
using UnityEngine;

public class TowerInvader : MonoBehaviour, IInvader
{
    [SerializeField] private Inventory inventory;
    [SerializeField] private Color color = Color.black;
    [SerializeField] private Material material;

    public Vector2 GetPosition()
    {
        return transform.position;
    }

    public void Die()
    {
        Died?.Invoke();
    }

    public event Action Died;

    public void TryInvadeTowers(List<Vector3> path)
    {
        for (var i = 0; i < path.Count; i++)
        {
            var vertices = new Vector2[3];

            //Setting central point
            vertices[0] = path[0];

            //Setting a current point
            vertices[1] = path[i];

            //Setting a next point
            var iIsLastIndex = i == path.Count - 1;
            if (iIsLastIndex)
                vertices[2] = path[1];
            else
                vertices[2] = path[i + 1];

            var towers = Tower.GetTowers();
            foreach (var t in towers)
            {
                var towerIsInsideTriangle =
                    MathExtensions.PointIsInsideTriangle(t.transform.position, vertices);

                if (!towerIsInsideTriangle)
                    continue;

                t.SetInvaderInventory(inventory);
                t.SetInvaderColor(color);
                t.SetInvader(this);
                break;
            }
        }
    }

    public void TryInvadeInvaders(List<Vector3> path)
    {
        for (var i = 0; i < path.Count; i++)
        {
            var vertices = new Vector2[3];

            //Setting central point
            vertices[0] = path[0];

            //Setting a current point
            vertices[1] = path[i];

            //Setting a next point
            var iIsLastIndex = i == path.Count - 1;
            if (iIsLastIndex)
                vertices[2] = path[1];
            else
                vertices[2] = path[i + 1];

            var invaders = InvadersCounter.GetInvaders();
            foreach (var someInvader in invaders)
            {
                var towerIsInsideTriangle =
                    MathExtensions.PointIsInsideTriangle(someInvader.GetPosition(), vertices);

                if (!towerIsInsideTriangle)
                    continue;

                if (someInvader == this)
                    continue;

                someInvader.Die();
            }
        }
    }

    public List<Vector3> CloseAFigure(Vector3[] positions, Vector3 intersectionPoint)
    {
        var vertices = new List<Vector3>();

        var centralPoint = MathExtensions.FindCentroid3D(positions);

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
        newGm.GetComponent<MeshRenderer>().sharedMaterial = material;

        TryInvadeTowers(vertices);
        TryInvadeInvaders(vertices);

        return vertices;
    }
}