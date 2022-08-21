using System.Collections.Generic;
using UnityEngine;

internal interface IPathGenerator
{
    void Generate(Vector3 towerPos);
    List<Vector3> GetPath();
}