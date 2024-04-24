using UnityEngine;

public class CameraTutorial : BaseTutorialElement
{
    [SerializeField] private TurnClockwiseButtonTutorial _turnClockwiseButton;
    [SerializeField] private TurnCounterClockwiseButtonTutorial _turnCounterclockwiseButton;
    [SerializeField] private ChangeCameraAngleButtonTutorial _cameraAngleChanger;

    [SerializeField] private Gameboard _gameboard;

    public override void Show(Player player)
    {
        _turnClockwiseButton.Show();
        _turnClockwiseButton.Open();
        _turnClockwiseButton.ProgressBarFiller.ProgressBarFilled += OnHoldComplete;
    }

    private void OnHoldComplete()
    {
        _turnClockwiseButton.Hide();
        _turnClockwiseButton.EffectHandler.StopLightEffect();
        _turnCounterclockwiseButton.Show();
        _turnCounterclockwiseButton.Open();
        _turnCounterclockwiseButton.ProgressBarFiller.ProgressBarFilled += OnHoldSecondComplete;
        _turnClockwiseButton.ProgressBarFiller.ProgressBarFilled -= OnHoldComplete;
    }

    private void OnHoldSecondComplete()
    {
        _turnCounterclockwiseButton.Hide();
        _turnCounterclockwiseButton.EffectHandler.StopLightEffect();
        _cameraAngleChanger.Show();
        _cameraAngleChanger.Open();
        _cameraAngleChanger.DoubleClickComplete += OnDoubleClickComplete;
        _turnCounterclockwiseButton.ProgressBarFiller.ProgressBarFilled -= OnHoldSecondComplete;
    }

    private void OnDoubleClickComplete()
    {
        _cameraAngleChanger.EffectHandler.StopLightEffect();
        InvokeTutorialZoneCompleteAction();
        _cameraAngleChanger.DoubleClickComplete -= OnDoubleClickComplete;
    }
}
