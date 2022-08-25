using UnityEngine;

namespace HyperCasualPolygonTowerDefence.Scripts.Extensions
{
    public struct Vector3Line
    {
        public Vector3 From;
        public Vector3 To;

        public static bool operator ==(Vector3Line line1, Vector3Line line2)
        {
            return line1.From == line2.From &&
                   line1.To == line2.To;
        }

        public static bool operator !=(Vector3Line line1, Vector3Line line2)
        {
            return !(line1.From == line2.From &&
                   line1.To == line2.To);
        }
    }
}