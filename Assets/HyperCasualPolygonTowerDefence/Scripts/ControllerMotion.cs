using System;
using UnityEngine;

[Serializable]
public abstract class ControllerMotion
{
    [SerializeField] protected Transform transform;

    [SerializeField] protected float moveSpeed = 2;
    protected readonly float rotationSmoothTime = 0.12f;
    protected float rotationVelocity;
    protected float targetRotation;

    public abstract void Move();
    public abstract void Rotate();

    protected abstract Vector2 ComputeDirection();
    
    protected Quaternion ComputeCurrentRotation(Vector3 direction, Vector3 currentGlobalEulers)
    {
        targetRotation = Mathf.Atan2(-direction.x, direction.y) * Mathf.Rad2Deg;
        var rotation = Mathf.SmoothDampAngle(currentGlobalEulers.z,
            targetRotation,
            ref rotationVelocity,
            rotationSmoothTime);

        return Quaternion.Euler(0.0f, 0.0f, rotation);
    }
}