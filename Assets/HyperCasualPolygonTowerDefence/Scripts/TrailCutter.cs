using HyperCasualPolygonTowerDefence.Scripts.Extensions;
using UnityEngine;

namespace HyperCasualPolygonTowerDefence.Scripts
{
    [System.Serializable]
    internal class TrailCutter
    {
        [SerializeField] private GameObject invaderGm;
        private IInvader invader;
    
        [SerializeField] private TrailRenderer trailRenderer;
        [SerializeField] private TrailRenderer enemyTrail;
        [SerializeField] private Transform transform;

        public void Initialize()
        {
            invader = invaderGm.GetComponent<IInvader>();
        }

        public void Update()
        {
            if (trailRenderer.positionCount == 0)
                return;

            TryCutTrail();
        }

        private void TryCutTrail()
        {
            var enemyVertices = new Vector3[enemyTrail.positionCount];
            enemyTrail.GetPositions(enemyVertices);

            var mAdvancedLine = new Vector3Line
            {
                From = trailRenderer.GetPosition(trailRenderer.positionCount - 1),
                To = transform.position
            };
            if (MathExtensions.LineIsIntersectedCurve(mAdvancedLine, enemyVertices))
                invader.Die();
        }
    }
}