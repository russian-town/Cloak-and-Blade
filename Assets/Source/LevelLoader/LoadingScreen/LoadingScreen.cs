using System.Collections;
using UnityEngine;

namespace Source.LevelLoader.LoadingScreen
{
    public class LoadingScreen : MonoBehaviour
    {
        [SerializeField] private float _speed;
        [SerializeField] private PSXEffects _psXEffects;
        [SerializeField] private RetroFXFilter _retroFXFilter;

        private IFader _fader;
        private float _currentValue;

        public void Initialize()
        {
#if UNITY_WEBGL && !UNITY_EDITOR
        if (Device.IsMobile)
            _fader = _psXEffects;
        else
            _fader = _retroFXFilter;
#else
            _fader = _retroFXFilter;
#endif
        }

        public Coroutine StartFade(float value)
        {
            return StartCoroutine(Fade(value));
        }

        public void SetFade(float value)
            => _fader.SetFade(value);

        private IEnumerator Fade(float value)
        {
            _currentValue = _fader.Fade;

            while (Mathf.Approximately(_currentValue, value) == false)
            {
                _currentValue = Mathf.MoveTowards(_currentValue, value, Time.deltaTime * _speed);
                _fader.SetFade(_currentValue);
                yield return null;
            }
        }
    }
}
