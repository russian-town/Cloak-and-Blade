using System;
using System.Collections.Generic;
using UnityEngine;

public class CameraTutorial : BaseTutorialElement
{
    [SerializeField] private TurnClockwiseButton _turnClockwiseButton;
    [SerializeField] private TurnCounterclockwiseButton _turnCounterclockwiseButton;
    [SerializeField] private CameraAngleChanger _cameraAngleChanger;

    [SerializeField] private Gameboard _gameboard;

    public override void Show(Player player)
    {
        _turnClockwiseButton.Show();
        _turnClockwiseButton.Open();
        _turnClockwiseButton.HoldComplete += OnHoldComplete;
    }

    private void OnHoldComplete()
    {
        _turnClockwiseButton.Hide();
        _turnClockwiseButton.EffectHandler.StopLightEffect();
        _turnCounterclockwiseButton.Show();
        _turnCounterclockwiseButton.Open();
        _turnCounterclockwiseButton.HoldSecondComplete += OnHoldSecondComplete;
        _turnClockwiseButton.HoldComplete -= OnHoldComplete;
    }

    private void OnHoldSecondComplete()
    {
        _turnCounterclockwiseButton.Hide();
        _turnCounterclockwiseButton.EffectHandler.StopLightEffect();
        _cameraAngleChanger.Show();
        _cameraAngleChanger.Open();
        _cameraAngleChanger.DoubleClickComplete += OnDoubleClickComplete;
        _turnCounterclockwiseButton.HoldSecondComplete -= OnHoldSecondComplete;
    }

    private void OnDoubleClickComplete()
    {
        _cameraAngleChanger.EffectHandler.StopLightEffect();
        InvokeTutorialZoneCompleteAction();
        _cameraAngleChanger.DoubleClickComplete -= OnDoubleClickComplete;
    }
}
