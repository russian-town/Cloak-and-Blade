using System;
using UnityEngine;
using UnityEngine.UI;

public class ChangeCameraAngleButtonTutorial : CameraAngleChanger
{
    private int _clickCount;

    private readonly int _targetClickCount = 2;

    public event Action DoubleClickComplete;

    private void OnEnable()
    {
        ChangeAngleButton.onClick?.AddListener(Clicked);
        ChangeAngleButton.onClick?.AddListener(Controls.ChangeCameraAngle);
    }

    private void OnDisable()
    {
        ChangeAngleButton.onClick?.RemoveListener(Clicked);
        ChangeAngleButton.onClick?.RemoveListener(Controls.ChangeCameraAngle);
    }

    private void Clicked()
    {
        _clickCount++;

        if (_clickCount >= _targetClickCount)
            DoubleClickComplete?.Invoke();
    }
}
