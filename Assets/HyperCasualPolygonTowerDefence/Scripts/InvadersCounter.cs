using System.Collections.Generic;

namespace HyperCasualPolygonTowerDefence.Scripts
{
    public static class InvadersCounter
    {
        public static List<IInvader> invaders = new();

        public static List<IInvader> GetInvaders()
        {
            return invaders;
        }
    }
}