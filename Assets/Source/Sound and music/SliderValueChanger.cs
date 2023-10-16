using UnityEngine;
using UnityEngine.UI;

public class SliderValueChanger : MonoBehaviour
{
    [SerializeField] private float _value;
    [SerializeField] private Slider _slider;

    public void IncreaseSliderValue()
    {
        _slider.value += _value;
    }

    public void DecreaseSliderValue()
    {
        _slider.value -= _value;
    }
}
