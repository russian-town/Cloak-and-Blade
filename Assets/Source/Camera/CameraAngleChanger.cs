using System;
using UnityEngine;
using UnityEngine.UI;

public class CameraAngleChanger : MainButton
{
    [SerializeField] private CameraControls _controls;
    [SerializeField] private Button _changeAngleButton;

    private int _clickCount;

    private readonly int _targetClickCount = 2;

    public event Action DoubleClickComplete;

    private void OnEnable()
    {
        _changeAngleButton.onClick?.AddListener(_controls.ChangeCameraAngle);
        _changeAngleButton.onClick?.AddListener(Clicked);
    }

    private void OnDisable()
    {
        _changeAngleButton.onClick?.RemoveListener(_controls.ChangeCameraAngle);
        _changeAngleButton.onClick?.RemoveListener(Clicked);
    }

    private void Clicked()
    {
        _clickCount++;

        if (_clickCount >= _targetClickCount)
            DoubleClickComplete?.Invoke();
    }
}
