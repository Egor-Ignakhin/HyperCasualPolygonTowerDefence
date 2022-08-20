using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Tower : MonoBehaviour
{
    private static readonly List<Tower> Towers = new();
    [SerializeField] private float scoresPerSecond = 1;

    private Inventory invaderInventory;
    private Image image;

    private void Start()
    {
        image = GetComponent<Image>();
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

    public void SetInvaderColor(Color color)
    {
        image.color = color;
    }

    public static List<Tower> GetTowers()
    {
        return Towers;
    }
}