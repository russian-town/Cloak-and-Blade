using System.Collections;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class LightButtonEffectHandler : MonoBehaviour
{
    [SerializeField] private Image _effectImage;
    [SerializeField] private float _appearingSpeed;
    [SerializeField] private Color _startColor;
    [SerializeField] private Color _secondColor;
    [SerializeField] private float _fadeDuration;

    private CanvasGroup _canvasGroup;
    private Sequence _pulsatingSequence;

    private void Start()
    {
        _canvasGroup = GetComponent<CanvasGroup>();
        PlayLightEffect();
    }
    
    public void PlayLightEffect()
    {
        if (_pulsatingSequence != null)
            return;

        _canvasGroup.alpha = 1;
        _pulsatingSequence = DOTween.Sequence();
        _pulsatingSequence
            .Append(_effectImage.DOColor(_secondColor, _fadeDuration))
            .Append(_effectImage.DOColor(_startColor, _fadeDuration))
            .SetLoops(-1);
    }

    public void StopLightEffect()
    {
        if (_pulsatingSequence == null)
            return;

        _canvasGroup.alpha = 0;
        _pulsatingSequence.Kill();
        _pulsatingSequence = null;
    }
}
