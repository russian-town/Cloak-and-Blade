using DG.Tweening;
using UnityEngine;

namespace Source.UiAnimations
{
    public class StandardImageFader : MonoBehaviour
    {
        [SerializeField] private CanvasGroup _canvasGroup;
        [SerializeField] private float _fadeDuration;

        private bool _canFadeIn = true;

        public void Fade()
        {
            if (_canFadeIn)
            {
                _canvasGroup.DOFade(1, _fadeDuration);
                _canFadeIn = false;
            }
            else
            {
                _canvasGroup.DOFade(0, _fadeDuration);
                _canFadeIn = true;
            }
        }
    }
}
