using UnityEngine;
using UnityEngine.UI;

namespace HyperCasualPolygonTowerDefence.Scripts.Environment
{
    public class ScoresSlider : MonoBehaviour
    {
        [SerializeField] private Slider slider;

        public void SetValue(float value)
        {
            slider.SetValueWithoutNotify(value);
        }
    }
}