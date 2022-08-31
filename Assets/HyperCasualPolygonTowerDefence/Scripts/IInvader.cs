using UnityEngine;

namespace HyperCasualPolygonTowerDefence.Scripts
{
    public interface IInvader
    {
        Vector2 GetPosition();
        void Die();
        void GetTrailPositions(Vector3[] positions);
        int GetPositionsCount();
    }
}