using System;
using System.Collections.Generic;
using HyperCasualPolygonTowerDefence.Scripts.Environment;
using HyperCasualPolygonTowerDefence.Scripts.PlayerLoop.Scripts;
using UnityEngine;

namespace HyperCasualPolygonTowerDefence.Scripts.Player
{
    public class PlayerBoot : MonoBehaviour
    {
        [SerializeField] private PlayerMotion motion;
        [SerializeField] private PlayerRotation rotation;
        [SerializeField] private TowerInvader towerInvader;
        [SerializeField] private TrailIntersectionChecker trailIntersectionChecker;
        [SerializeField] private TrailCutter trailCutter;

        private List<IDisposable> subscriptions;

        private void Start()
        {
            trailIntersectionChecker.Initialize(towerInvader);

            InitializeLoopSubs();
        }

        private void OnDestroy()
        {
            DisposeLoopSubs();
        }

        private void InitializeLoopSubs()
        {
            subscriptions = new List<IDisposable>
            {
                Loops.Update.Start(motion.Move),
                Loops.Update.Start(rotation.Rotate),
                Loops.Update.Start(trailIntersectionChecker.Update),
                Loops.Update.Start(trailCutter.Update)
            };
        }

        private void DisposeLoopSubs()
        {
            foreach (var subscription in subscriptions) subscription.Dispose();
        }
    }
}