using System;

namespace HyperCasualPolygonTowerDefence.Scripts.PlayerLoop.Scripts
{
    public interface ILoopSystem
    {
        IDisposable Start(Action updatable);
    }
}