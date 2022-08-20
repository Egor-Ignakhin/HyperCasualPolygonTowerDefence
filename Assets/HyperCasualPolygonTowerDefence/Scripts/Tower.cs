using UnityEngine;

public class Tower : MonoBehaviour
{
    [SerializeField] private float scoresPerSecond = 1;

    private Inventory invaderInventory;

    public void SetInvaderInventory(Inventory inventory)
    {
        invaderInventory = inventory;
    }
    
    private void Update()
    {
        if (invaderInventory != null)
            invaderInventory.AddScores(scoresPerSecond * Time.deltaTime);
    }
}