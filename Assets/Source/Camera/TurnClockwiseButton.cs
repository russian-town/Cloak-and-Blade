using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class TurnClockwiseButton : MainButton, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private CameraControls _controls;
    [SerializeField] private float _timeToHold;

    private float _startTime;
    private bool _isRotating;
    private bool _isTimeOver;

    public event Action HoldComplete;

    private void Start()
    {
        _startTime = _timeToHold;
    }

    private void Update()
    {
        if (_isRotating)
        {
            _controls.TurnClockwise();

            if (_timeToHold > 0)
            {
                _timeToHold -= Time.deltaTime;
            }
            else
            {
                if (_isTimeOver)
                    return;

                HoldComplete?.Invoke();
                _isTimeOver = true;
            }
        }
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isRotating = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isRotating = false;
        _timeToHold = _startTime;
    }
}
