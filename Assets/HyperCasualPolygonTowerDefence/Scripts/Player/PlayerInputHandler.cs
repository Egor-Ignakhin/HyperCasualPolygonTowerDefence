using HyperCasualPolygonTowerDefence.Scripts.Player.Behavior;
using UnityEngine;

namespace HyperCasualPolygonTowerDefence.Scripts.Player
{
    public class PlayerInputHandler
    {
        public IPlayerBehavior ComputeCurrentPlayerBehavior(Vector2 joystickDirection)
        {
            if (joystickDirection.y == 0) return new PlayerIdleBehavior();
            if (joystickDirection.x != 0)
                return new PlayerMoveBehavior
                {
                    DirectionMultiplier = joystickDirection.y,
                    RotationMultiplier = joystickDirection.x
                };
            return new PlayerIdleBehavior();
        }
    }
}