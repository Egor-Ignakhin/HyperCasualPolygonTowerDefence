using UnityEngine;

public class PlayerInputHandler
{
   

    public IPlayerBehavior ComputeCurrentPlayerBehavior(Vector2 joystickDirection)
    {
        if (joystickDirection.y != 0)
            if (joystickDirection.x != 0)
                return new PlayerBehaviorMove
                {
                    directionMultiplier = joystickDirection.y,
                    rotationMultiplier = joystickDirection.x
                };
        return new PlayerBehaviorIdle();
    }

   
}