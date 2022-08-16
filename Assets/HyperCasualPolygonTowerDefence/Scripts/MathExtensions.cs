using UnityEngine;

namespace HyperCasualPolygonTowerDefence.Scripts
{
    public static class MathExtensions
    {
        public static bool PointIsInsideTriangle(Vector2 point, Vector2[] vertices)
        {
            var v1y = vertices[1].y;
            var v2x = vertices[2].x;
            var v0y = vertices[0].y;
            var v1x = vertices[1].x;
            var v2y = vertices[2].y;
            var v0x = vertices[0].x;

            var Area = 0.5 * (-v1y * v2x + v0y * (-v1x + v2x) +
                              v0x * (v1y - v2y) + v1x * v2y);
            var s = 1 / (2 * Area) * (v0y * v2x - v0x * v2y +
                                      (v2y - v0y) * point.x +
                                      (v0x - v2x) * point.y);
            var t = 1 / (2 * Area) * (v0x * v1y - v0y * v1x +
                                      (v0y - v1y) * point.x +
                                      (v1x - v0x) * point.y);
            var p = vertices[0] + (vertices[1] - vertices[0]) * (float)s + (vertices[2] - vertices[0]) * (float)t;


            return s is >= 0 and <= 1 &&
                   t is >= 0 and <= 1 &&
                   s + t <= 1;
        }
    }
}