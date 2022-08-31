using System;
using UnityEngine;

namespace HyperCasualPolygonTowerDefence.Scripts
{
    [Serializable]
    public class PersonMotion
    {
        [SerializeField] protected Transform transform;

        [SerializeField] protected float moveSpeed = 2;
        

        public virtual void Move()
        {
            throw new NotImplementedException();
        }

        protected virtual Vector2 ComputeDirection()
        {
            throw new NotImplementedException();
        }

       
    }
}