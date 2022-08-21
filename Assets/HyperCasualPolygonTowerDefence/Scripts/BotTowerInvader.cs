using System.Linq;
using UnityEngine;

[System.Serializable]
public class BotTowerInvader : TowerInvader
{
    public Vector3 FindTargetTowerPosition(Vector3 currentPosition)
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

    public bool BotCanCloseFigure(Vector3[] positions, Vector3 position)
    {
        return Vector3.Distance(position, positions.Last()) <= 0.25f;
    }
}