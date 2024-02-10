using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class TurnClockwiseButton : MainButton, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private CameraControls _controls;

    protected bool _isRotating;

    public event Action HoldComplete;

    private void Update()
    {
        if (_isRotating)
            _controls.TurnClockwise();
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
