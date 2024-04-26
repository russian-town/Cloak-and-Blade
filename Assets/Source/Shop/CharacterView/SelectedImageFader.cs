using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Shop.CharacterView
{
    public class SelectedImageFader : MonoBehaviour
    {
        [SerializeField] private Image _image;
        [SerializeField] private float _speed;

        private Color _color;
        private Coroutine _fadeCroroutine;

        public void SetColor(Color color)
        {
            _image.color = color;
            _color = color;
        }

        public void ChangeAlpha(float value)
        {
            if (_fadeCroroutine != null)
            {
                StopCoroutine(_fadeCroroutine);
                _fadeCroroutine = null;
            }

            _fadeCroroutine = StartCoroutine(FadeOverTime(value));
        }

        private IEnumerator FadeOverTime(float value)
        {
            while (_image.color.a != value)
            {
                _color.a = Mathf.MoveTowards(_color.a, value, _speed * Time.deltaTime);
                _image.color = _color;
                yield return null;
            }
        }
    }
}
