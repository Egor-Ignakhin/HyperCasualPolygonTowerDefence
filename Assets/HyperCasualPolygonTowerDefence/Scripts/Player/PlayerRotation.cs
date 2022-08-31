using System;
using HyperCasualPolygonTowerDefence.Joystick_Pack.Scripts.Base;
using HyperCasualPolygonTowerDefence.Scripts.Player.Behavior;
using UnityEngine;

namespace HyperCasualPolygonTowerDefence.Scripts.Player
{
    [Serializable]
    internal class PlayerRotation : PersonRotation
    {
        [SerializeField] private Joystick joystick;
        private PlayerInputHandler playerInputHandler;

        public PlayerRotation()
        {
            playerInputHandler = new PlayerInputHandler();
        }
        
        public override void Rotate()
        {
            var direction = ComputeDirection();
            if (playerInputHandler.ComputeCurrentPlayerBehavior(direction) is PlayerMoveBehavior)
                transform.rotation = ComputeCurrentRotation(joystick.Direction, transform.eulerAngles);
        }

        protected override Vector2 ComputeDirection()
        {
            return joystick.Direction;
        }
    }
}