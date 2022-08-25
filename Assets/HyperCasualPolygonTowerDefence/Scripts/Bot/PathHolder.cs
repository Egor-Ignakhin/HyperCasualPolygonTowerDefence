using System.Collections.Generic;

namespace HyperCasualPolygonTowerDefence.Scripts.Bot
{
    internal struct PathHolder<T>
    {
        public List<T> Path;
        public int Iterator;
    }
}