using System;
using UnityEngine;
using UnityEngine.LowLevel;
using UnityEngine.PlayerLoop;

namespace HyperCasualPolygonTowerDefence.Scripts.PlayerLoop.Scripts
{
    public class DisablePhysics : MonoBehaviour
    {
        private IDisposable subscription;
        private void Start()
        {
            PlayerLoopExtensions.ModifyCurrentPlayerLoop(((ref PlayerLoopSystem system) =>
            {
                system.RemoveSystem<FixedUpdate.PhysicsFixedUpdate>();
            }));
        }
    }
}
