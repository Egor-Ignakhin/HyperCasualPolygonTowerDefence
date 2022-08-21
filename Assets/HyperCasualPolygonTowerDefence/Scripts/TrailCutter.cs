using HyperCasualPolygonTowerDefence.Scripts;
using UnityEngine;

[System.Serializable]
internal class TrailCutter
{
    [SerializeField] private GameObject invaderGm;
    private IInvader invader;
    
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private TrailRenderer enemyTrail;
    [SerializeField] private Transform transform;

    public void OnInit()
    {
        invader = invaderGm.GetComponent<IInvader>();
    }

    public void TryCutPlayerTrail()
    {
        if (trailRenderer.positionCount == 0)
            return;
        
        var playerVertices = new Vector3[enemyTrail.positionCount];
        enemyTrail.GetPositions(playerVertices);

        var mAdvancedLine = new Vector3Line
        {
            From = trailRenderer.GetPosition(trailRenderer.positionCount - 1),
            To = transform.position
        };
        if (MathExtensions.LineIsIntersectedCurve(mAdvancedLine, playerVertices))
            invader.Die();
    }
}