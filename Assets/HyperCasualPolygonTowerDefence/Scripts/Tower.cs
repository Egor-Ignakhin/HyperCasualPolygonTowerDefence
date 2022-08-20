using System.Collections.Generic;
using UnityEngine;

public class Tower : MonoBehaviour
{
    private static readonly List<Tower> Towers = new();
    [SerializeField] private float scoresPerSecond = 1;

    private Inventory invaderInventory;

    private void Start()
    {
        Towers.Add(this);
    }

    private void Update()
    {
        if (invaderInventory != null)
            invaderInventory.AddScores(scoresPerSecond * Time.deltaTime);
    }

    public void SetInvaderInventory(Inventory inventory)
    {
        invaderInventory = inventory;
    }

    public static List<Tower> GetTowers()
    {
        return Towers;
    }
}