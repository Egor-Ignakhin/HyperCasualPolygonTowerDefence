using System;
using System.Collections.Generic;
using HyperCasualPolygonTowerDefence.Scripts.PlayerLoop.Scripts;
using UnityEngine;

namespace HyperCasualPolygonTowerDefence.Scripts.Bot
{
    public class BotBoot : MonoBehaviour
    {
        [SerializeField] private BotMotion botMotion;
        [SerializeField] private BotRotation botRotation;

        [SerializeField] private BotTowerInvader towerInvader;
        [SerializeField] private TrailCutter trailCutter;

        [SerializeField] private BotTargetDesignator botTargetDesignator;

        private List<IDisposable> subscriptions;

        private void Start()
        {
            InitializeLoopSubs();

            botTargetDesignator.TargetSwitched += () => { botMotion.SetDestination(botTargetDesignator.GetTarget()); };
            botTargetDesignator.TargetSwitched += () =>
            {
                botRotation.SetDestination(botTargetDesignator.GetTarget());
            };

            botTargetDesignator.ReInitialize();
            botMotion.TargetIsReached += botTargetDesignator.SwitchTarget;

            towerInvader.Died += botTargetDesignator.ReInitialize;
        }

        private void OnDestroy()
        {
            foreach (var subscription in subscriptions)
                subscription.Dispose();
        }

        private void InitializeLoopSubs()
        {
            subscriptions = new List<IDisposable>
            {
                Loops.Update.Start(trailCutter.Update),
                Loops.Update.Start(botMotion.Move),
                Loops.Update.Start(botRotation.Rotate)
            };
        }
    }
}