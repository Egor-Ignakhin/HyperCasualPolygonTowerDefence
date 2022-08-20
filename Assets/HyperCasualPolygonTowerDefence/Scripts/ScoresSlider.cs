using UnityEngine;
using UnityEngine.UI;

public class ScoresSlider : MonoBehaviour
{
    [SerializeField] private Slider slider;

    public void SetValue(float value)
    {
        slider.SetValueWithoutNotify(value);
    }
}