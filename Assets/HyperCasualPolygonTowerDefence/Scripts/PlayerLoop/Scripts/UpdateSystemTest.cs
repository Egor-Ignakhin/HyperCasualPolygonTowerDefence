using System;
using UnityEngine;

namespace HyperCasualPolygonTowerDefence.Scripts.PlayerLoop.Scripts
{
    public class UpdateSystemTest : MonoBehaviour
    {
        private IDisposable subscription;
        private void Start()
        {
            subscription = Loops.Update.Start(OnUpdate);
        }

        private void OnUpdate()
        {
            print("OnUpdate");
        }

        private void OnDestroy()
        {
            subscription.Dispose();
        }
    }
}
