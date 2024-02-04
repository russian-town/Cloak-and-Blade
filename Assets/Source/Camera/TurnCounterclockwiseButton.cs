using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class TurnCounterclockwiseButton : MainButton, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private CameraControls _controls;
    [SerializeField] private float _timeToHold;

    private float _startTime;
    private bool _isTimeOver;
    private bool _isRotating;

    public event Action HoldSecondComplete;

    public void Start()
    {
        _startTime = _timeToHold;
    }

    private void Update()
    {
        if (_isRotating)
        {
            _controls.TurnCounterClockwise();

            if (_timeToHold > 0)
            {
                _timeToHold -= Time.deltaTime;
            }
            else
            {
                if (_isTimeOver)
                    return;

                HoldSecondComplete?.Invoke();
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
