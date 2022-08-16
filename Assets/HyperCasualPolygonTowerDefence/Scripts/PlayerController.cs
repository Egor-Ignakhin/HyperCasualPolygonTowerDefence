using UnityEditor;
using UnityEngine;

namespace HyperCasualPolygonTowerDefence.Scripts
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private TrailRenderer trailRenderer;
        [SerializeField] private Transform towerTr;

        private void Update()
        {
            if (trailRenderer.positionCount == 1 && !Input.GetMouseButton(0))
                trailRenderer.SetPosition(0, transform.position);
            
            if(Input.GetMouseButton(1))
                trailRenderer.Clear();
            
            SetupPointOnMousePosition();

            if (!Input.GetMouseButton(0))
            {
                trailRenderer.minVertexDistance = 100f;
                return;
            }

            trailRenderer.minVertexDistance = 1f;
        }

        private void OnDrawGizmos()
        {
            if (!Application.isPlaying)
                return;

            var positionCount = trailRenderer.positionCount;
            for (var i = 0; i < positionCount; i++)
            {
                Gizmos.color = Color.yellow;
                var position = trailRenderer.GetPosition(i);
                Gizmos.DrawSphere(position, 0.1f);

                if (i == 0 || i == positionCount - 1)
                    continue;

                var vertices3D = new Vector3[3]
                {
                    trailRenderer.GetPosition(i - 1),
                    trailRenderer.GetPosition(i),
                    trailRenderer.GetPosition(i + 1)
                };
                var vertices2D = new Vector2[]
                {
                    vertices3D[0],
                    vertices3D[1],
                    vertices3D[2]
                };

                Handles.color = MathExtensions.PointIsInsideTriangle(towerTr.position, vertices2D)
                    ? Color.yellow
                    : Color.white;

                Handles.DrawAAConvexPolygon(vertices3D);
            }
        }

        private void SetupPointOnMousePosition()
        {
            var screenPoint = Input.mousePosition;
            screenPoint.z = 100.0f; //distance of the plane from the camera
            transform.position = Camera.main.ScreenToWorldPoint(screenPoint);
        }
    }
}