using System;
using UnityEngine;

namespace HyperCasualPolygonTowerDefence.Scripts
{
    [Serializable]
    public class PersonRotation
    {
        protected readonly float rotationSmoothTime = 0.12f;
        protected float rotationVelocity;
        protected float targetRotation;
        [SerializeField] protected Transform transform;

        public virtual void Rotate()
        {
            throw new NotImplementedException();
        }

        protected virtual Vector2 ComputeDirection()
        {
            throw new NotImplementedException();
        }

        protected Quaternion ComputeCurrentRotation(Vector3 direction, Vector3 currentGlobalAngles)
        {
            targetRotation = Mathf.Atan2(-direction.x, direction.y) * Mathf.Rad2Deg;
            var rotation = Mathf.SmoothDampAngle(currentGlobalAngles.z,
                targetRotation,
                ref rotationVelocity,
                rotationSmoothTime);

            return Quaternion.Euler(0.0f, 0.0f, rotation);
        }
    }
}