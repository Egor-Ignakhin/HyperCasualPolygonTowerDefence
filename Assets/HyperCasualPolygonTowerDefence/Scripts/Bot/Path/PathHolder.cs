using System;
using System.Collections.Generic;

namespace HyperCasualPolygonTowerDefence.Scripts.Bot
{
    internal class PathHolder<T>
    {
        public List<T> Path;
        private int iterator;

        public void Reset()
        {
            iterator = 0;
        }

        public void Next()
        {
            iterator++;
        }

        public int GetIterator()
        {
            return iterator;
        }
    }
}