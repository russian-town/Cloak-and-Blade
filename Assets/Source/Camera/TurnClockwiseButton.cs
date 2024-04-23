using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class TurnClockwiseButton : MainButton, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private CameraControls _controls;

    private bool _isRotating;

    public event Action HoldComplete;

    private void Update()
    {
        if (_isRotating)
            _controls.TurnClockwise();
    }

    public void OnPointerDown(PointerEventData eventData)
        => ToggleRotating();

    public void OnPointerUp(PointerEventData eventData)
        => ToggleRotating();

    public void ToggleRotating()
    {
        if (_isRotating)
            _isRotating = false;
        else
            _isRotating = true;
    }
}
