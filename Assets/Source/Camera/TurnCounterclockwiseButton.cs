using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class TurnCounterclockwiseButton : MainButton, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private CameraControls _controls;

    protected bool _isRotating;

    public event Action HoldSecondComplete;

    private void Update()
    {
        if (_isRotating)
            _controls.TurnCounterClockwise();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isRotating = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isRotating = false;
    }
}
