using UnityEngine;

public class ScenaryController : MonoBehaviour
{
    [SerializeField] private Inventory playerInv;
    [SerializeField] private Inventory enemyInv;
    [SerializeField] private ScoresSlider playerScoresSlider;
    [SerializeField] private ScoresSlider enemyScoresSlider;
    [SerializeField] private FinishScreen finishScreen;
    [SerializeField] private float scoresToWin = 10f;
    private bool canFinishGame;

    private void Update()
    {
        UpdatePlayersAchievements();

        if (canFinishGame)
            FinishGame();
    }

    private void UpdatePlayersAchievements()
    {
        var enemyScores = enemyInv.GetScores();
        var playerScores = playerInv.GetScores();
        canFinishGame = playerScores >= scoresToWin ||
                        enemyScores >= scoresToWin;

        playerScoresSlider.SetValue(playerScores / scoresToWin);
        enemyScoresSlider.SetValue(enemyScores / scoresToWin);
    }

    private void FinishGame()
    {
        finishScreen.Activate(playerInv.GetScores() > enemyInv.GetScores() ? FinishType.Win : FinishType.Defeat);
    }
}