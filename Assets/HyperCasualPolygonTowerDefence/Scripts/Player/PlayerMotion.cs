using System;
using UnityEngine;

[Serializable]
public class PlayerMotion : ControllerMotion
{
    [SerializeField] private Joystick joystick;
    private PlayerInputHandler playerInputHandler;

    public PlayerMotion()
    {
        playerInputHandler = new PlayerInputHandler();
    }

    public override void Move()
    {
        Vector3 direction = ComputeDirection();
        if (playerInputHandler.ComputeCurrentPlayerBehavior(direction) is PlayerBehaviorMove move)
        {
            var translation = new Vector3(move.rotationMultiplier * moveSpeed * Time.deltaTime,
                moveSpeed * Time.deltaTime * move.directionMultiplier);
            transform.position += translation;
        }
    }

    public override void Rotate()
    {
        var direction = ComputeDirection();
        if (playerInputHandler.ComputeCurrentPlayerBehavior(direction) is PlayerBehaviorMove)
            transform.rotation = ComputeCurrentRotation(joystick.Direction, transform.eulerAngles);
    }

    protected override Vector2 ComputeDirection()
    {
        return joystick.Direction;
    }
}