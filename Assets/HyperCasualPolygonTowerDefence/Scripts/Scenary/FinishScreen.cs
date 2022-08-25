using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HyperCasualPolygonTowerDefence.Scripts.Scenary
{
    internal class FinishScreen : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI defeatText;
        [SerializeField] private TextMeshProUGUI victoryText;
    
        public void Activate(FinishType finishType)
        {
            gameObject.SetActive(true);
            switch (finishType)
            {
                case FinishType.Defeat:
                    defeatText.enabled = true;
                    break;
                case FinishType.Win:
                    victoryText.enabled = true;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(finishType), finishType, null);
            }
        }

        public void Restart()
        {
            SceneManager.LoadScene(0);
        }
    }
}