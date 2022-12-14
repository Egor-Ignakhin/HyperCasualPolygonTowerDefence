using System;
using HyperCasualPolygonTowerDefence.Joystick_Pack.Scripts.Base;
using HyperCasualPolygonTowerDefence.Scripts.Player.Behavior;
using UnityEngine;

namespace HyperCasualPolygonTowerDefence.Scripts.Player
{
    [Serializable]
    public class PlayerMotion : PersonMotion
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
            if (playerInputHandler.ComputeCurrentPlayerBehavior(direction) is not PlayerMoveBehavior move) return;
            var translation = new Vector3(move.RotationMultiplier * moveSpeed * Time.deltaTime,
                moveSpeed * Time.deltaTime * move.DirectionMultiplier);
            transform.position += translation;
        }

        protected override Vector2 ComputeDirection()
        {
            return joystick.Direction;
        }
    }
}