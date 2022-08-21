using System.Collections.Generic;
using UnityEngine;

public class BotPathGenerator : IPathGenerator
{
    private readonly List<Vector3> path = new();

    public void Generate(Vector3 towerPos)
    {
        path.Clear();

        float vecLenght = 2;
        path.Add(towerPos + Vector3.right * vecLenght);
        path.Add(towerPos + Vector3.down * vecLenght);
        path.Add(towerPos + Vector3.left * vecLenght);
        path.Add(towerPos + Vector3.up * vecLenght);
        path.Add(towerPos + Vector3.right * vecLenght);
    }

    public List<Vector3> GetPath()
    {
        return path;
    }
}