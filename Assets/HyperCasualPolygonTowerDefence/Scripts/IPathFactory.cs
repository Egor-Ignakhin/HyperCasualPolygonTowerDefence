using System.Collections.Generic;
using UnityEngine;

namespace HyperCasualPolygonTowerDefence.Scripts
{
    internal interface IPathFactory
    {
        List<Vector3> Generate(Vector3 towerPos);
    }
}