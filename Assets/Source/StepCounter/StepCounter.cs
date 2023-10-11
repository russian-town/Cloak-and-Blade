using UnityEngine;

[RequireComponent (typeof(StepCounterView))]
public class StepCounter : MonoBehaviour
{
    private Player _player;
    private int _stepCount;
    private StepCounterView _view;

    public int CurrentStepCount => _stepCount;

    private void OnDisable()
    {
        _player.StepEnded -= OnStepEnded;
    }

    public void Initialize(Player player)
    {
        _view = GetComponent<StepCounterView>();
        _player = player;
        _player.StepEnded += OnStepEnded;
    }

    private void OnStepEnded()
    {
        IncreaseStepCount();
        _view.UpdateView(_stepCount);
    }

    private void IncreaseStepCount() => _stepCount++;
}
