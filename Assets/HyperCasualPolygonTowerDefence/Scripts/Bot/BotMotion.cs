using System;
using UnityEngine;

namespace HyperCasualPolygonTowerDefence.Scripts.Bot
{
    [Serializable]
    internal class BotMotion : PersonMotion
    {
        private Vector2 destination;
        public event Action TargetIsReached;

        public void SetDestination(Vector2 v)
        {
            destination = v;
        }

        public override void Move()
        {
            var direction = ComputeDirection();
            var translation = direction * (moveSpeed * Time.deltaTime);
            transform.position += new Vector3(translation.x, translation.y);

            if (Vector2.Distance(transform.position, destination) < 0.1f) TargetIsReached?.Invoke();
        }

        protected override Vector2 ComputeDirection()
        {
            return (destination - (Vector2)transform.position).normalized;
        }

        public override void Rotate()
        {
            var direction = ComputeDirection();
            transform.rotation = ComputeCurrentRotation(direction, transform.eulerAngles);
        }
    }
}