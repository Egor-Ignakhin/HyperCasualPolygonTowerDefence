using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public void Move()
    {
        SetupPositionAsMousePosition();
    }

    private void SetupPositionAsMousePosition()
    {
        var screenPoint = Input.mousePosition;
        screenPoint.z = 100.0f; //distance of the plane from the camera
        transform.position = Camera.main.ScreenToWorldPoint(screenPoint);
    }
}