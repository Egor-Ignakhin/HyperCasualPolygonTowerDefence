using System.Collections.Generic;
using UnityEngine;

namespace HyperCasualPolygonTowerDefence.Scripts.Bot
{
    public class BotPathFactory : IPathFactory
    {
        private readonly List<Vector3> path = new();

        public List<Vector3> Generate(Vector3 towerPos)
        {
            path.Clear();

            float vecLenght = 2;
            var degreeOfSmoothing = 1;

            CreateCircle(towerPos, vecLenght, degreeOfSmoothing);

            return path;
        }

        private void CreateCircle(Vector3 towerPos, float vecLenght, int degreeOfSmoothing)
        {
            path.Add(towerPos + new Vector3(1,0,0) * vecLenght);
            path.Add(towerPos + new Vector3(0,-1,0) * vecLenght);
            path.Add(towerPos + new Vector3(-1,0,0) * vecLenght);
            path.Add(towerPos + new Vector3(0,1,0) * vecLenght);
            path.Add(towerPos + new Vector3(1,0,0) * vecLenght);

       /*     for (var i = 0; i < degreeOfSmoothing; i++)
            {
                if(i == 0)
                    continue;
                
                path.Add(path.Add());
            }*/
        }
    }
}