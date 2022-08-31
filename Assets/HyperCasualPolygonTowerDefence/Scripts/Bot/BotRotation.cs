using System;
using HyperCasualPolygonTowerDefence.Scripts.Player;
using UnityEngine;

namespace HyperCasualPolygonTowerDefence.Scripts.Bot
{
    [Serializable]
    public class BotRotation : PersonRotation
    {
        private Vector2 destination;

        public void SetDestination(Vector2 v)
        {
            destination = v;
        }

        public override void Rotate()
        {
            var direction = ComputeDirection();
            transform.rotation = ComputeCurrentRotation(direction, transform.eulerAngles);
        }

        protected override Vector2 ComputeDirection()
        {
            return (destination - (Vector2)transform.position).normalized;
        }
    }
}