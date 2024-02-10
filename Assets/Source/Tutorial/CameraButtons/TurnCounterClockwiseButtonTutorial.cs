using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TurnCounterClockwiseButtonTutorial : TurnCounterclockwiseButton, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private ProgressBarFiller _barFiller;

    public ProgressBarFiller ProgressBarFiller => _barFiller;

    private bool _isFilled;

    private void Start()
    {
        _barFiller.Initialize();
        _barFiller.ProgressBarFilled += OnBarFilled;
    }

    private void OnDisable()
    {
        _barFiller.ProgressBarFilled -= OnBarFilled;
    }

    public new void OnPointerDown(PointerEventData eventData)
    {
        if (!_isFilled)
            _barFiller.ChangeFilling();

        _isRotating = true;
    }

    public new void OnPointerUp(PointerEventData eventData)
    {
        if (!_isFilled)
            _barFiller.ChangeFilling();

        _isRotating = false;
    }

    private void OnBarFilled() => _isFilled = true;
}
