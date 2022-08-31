using System.Collections.Generic;

namespace HyperCasualPolygonTowerDefence.Scripts
{
    public static class InvadersCounter
    {
        private static readonly List<IInvader> invaders = new();

        public static void AddInvader(IInvader invader)
        {
            invaders.Add(invader);
        }
        
        public static List<IInvader> GetInvaders()
        {
            return invaders;
        }
    }
}