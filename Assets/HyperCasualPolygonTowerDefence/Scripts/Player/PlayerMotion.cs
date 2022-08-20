using UnityEngine;

public class PlayerMotion : MonoBehaviour
{
    [SerializeField] private Joystick joystick;
    [SerializeField] private float speed = 2;
    private readonly float rotationSmoothTime = 0.12f;
    private PlayerInputHandler playerInputHandler;
    private float rotationVelocity;
    private float targetRotation;


    private void Start()
    {
        playerInputHandler = new PlayerInputHandler();
    }

    public void Move()
    {
        if (playerInputHandler.ComputeCurrentPlayerBehavior(joystick.Direction) is PlayerBehaviorMove move)
        {
            var translation = new Vector3(move.rotationMultiplier * speed * Time.deltaTime,
                speed * Time.deltaTime * move.directionMultiplier);
            transform.position += translation;
        }
    }

    public void Rotate()
    {
        if (playerInputHandler.ComputeCurrentPlayerBehavior(joystick.Direction) is PlayerBehaviorMove)
        {
            float speed = 1;
            transform.rotation = ComputeCurrentRotation(joystick.Direction, transform.eulerAngles);
        }
    }

    private Quaternion ComputeCurrentRotation(Vector3 direction, Vector3 currentGlobalEulers)
    {
        targetRotation = Mathf.Atan2(-direction.x, direction.y) * Mathf.Rad2Deg;
        var rotation = Mathf.SmoothDampAngle(currentGlobalEulers.z,
            targetRotation,
            ref rotationVelocity,
            rotationSmoothTime);

        return Quaternion.Euler(0.0f, 0.0f, rotation);
    }
}