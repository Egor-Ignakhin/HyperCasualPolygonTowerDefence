using UnityEngine;

public class Inventory : MonoBehaviour
{
    private float scores;

    public void AddScores(float value)
    {
        scores += value;
    }

    public float GetScores()
    {
        return scores;
    }
}