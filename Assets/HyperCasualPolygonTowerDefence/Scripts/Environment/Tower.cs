using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace HyperCasualPolygonTowerDefence.Scripts.Environment
{
    public class Tower : MonoBehaviour
    {
        private static readonly List<Tower> Towers = new();
        [SerializeField] private float scoresPerSecond = 1;
        private IInvader currentInvader;
        private Image image;

        private Inventory invaderInventory;

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

        public IInvader GetInvader()
        {
            return currentInvader;
        }

        public void SetInvader(IInvader invader)
        {
            currentInvader = invader;
        }
    }
}