using UnityEngine;

namespace HyperCasualPolygonTowerDefence.Scripts
{
    public interface IInvader
    {
        Vector2 GetPosition();
        void Die();
    }
}