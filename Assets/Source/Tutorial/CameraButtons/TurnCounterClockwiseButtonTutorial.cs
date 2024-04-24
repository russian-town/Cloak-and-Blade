using UnityEngine;
using UnityEngine.EventSystems;

public class TurnCounterClockwiseButtonTutorial : TurnCounterclockwiseButton, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private ProgressBarFiller _barFiller;

    private bool _isFilled;

    public ProgressBarFiller ProgressBarFiller => _barFiller;

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

        ToggleRotating();
    }

    public new void OnPointerUp(PointerEventData eventData)
    {
        if (!_isFilled)
            _barFiller.ChangeFilling();

        ToggleRotating();
    }

    private void OnBarFilled() => _isFilled = true;
}
