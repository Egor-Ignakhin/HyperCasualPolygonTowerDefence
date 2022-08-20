using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BotController : MonoBehaviour
{
    [SerializeField] private Transform towerTr;

    [SerializeField] private List<Vector3> path = new();
    [SerializeField] private int vertexCount = 5;

    private void OnDrawGizmos()
    {
        for (var i = 0; i < path.Count; i++)
        {
            Handles.color = Color.red;
            if (i < path.Count - 1)
                Handles.DrawLine(path[i], path[i + 1]);
            Gizmos.color = new Color(0f, 0f, 0f, i / (float)path.Count + 0.1f);
            Gizmos.DrawSphere(path[i], 0.1f);
        }
    }

    private void Start()
    {
        GeneratePath();
    }

    private void GeneratePath()
    {
        path.Clear();
        var towerPos = towerTr.position;

        var radDifference = vertexCount / 360f;

        float vecLenght = 2;
        path.Add(towerPos + Vector3.right * vecLenght);
        path.Add(towerPos + Vector3.down * vecLenght);
        path.Add(towerPos + Vector3.left * vecLenght);
        path.Add(towerPos + Vector3.up * vecLenght);

        for (var i = 0; i < vertexCount; i++)
        {
            /*var pointPos = towerPos;
            var vectorToPos = new Vector3(1*(i%2==0?1:-1),1*(i%2==1?1:-1));
            
            vectorToPos = vectorToPos.Rotate(radDifference * i*2);
            pointPos += vectorToPos;
            path.Add(pointPos);*/
        }
    }
}