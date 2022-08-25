using System.Linq;
using HyperCasualPolygonTowerDefence.Scripts.Environment;
using UnityEngine;

namespace HyperCasualPolygonTowerDefence.Scripts.Bot
{
    public class BotTowerInvader : TowerInvader
    {
        public Vector3 FindNearestTowerPosition(Vector3 currentPosition)
        {
            var towers = Tower.GetTowers();
            towers = towers.OrderBy(t => Vector3.Distance(currentPosition, t.transform.position)).Reverse().ToList();
            var targetTowerPosition = Vector3.zero;

            foreach (var t in towers)
            {
                if (t.GetInvader() == this)
                    continue;

                targetTowerPosition = t.transform.position;
            }

            return targetTowerPosition;
        }
    }
}