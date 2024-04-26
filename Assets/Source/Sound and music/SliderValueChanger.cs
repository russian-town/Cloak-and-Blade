using UnityEngine;
using UnityEngine.UI;

namespace Source.Sound_and_music
{
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
}
