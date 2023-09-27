using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerMover))]
public class TheWorld : Ability
{
    [SerializeField] private int _stepCount;

    private PlayerMover _mover;
    private int _currentStepCount;

    private void OnDisable() => _mover.MoveEnded -= IncreaseStepCount;

    public override void Initialize()
    {
        _mover = GetComponent<PlayerMover>();
        _mover.MoveEnded += IncreaseStepCount;
    }

    public override void Cancel()
    {
        throw new System.NotImplementedException();
    }

    public override void Prepare()
    {
        throw new System.NotImplementedException();
    }

    protected override void Action(Cell cell)
    {
        throw new System.NotImplementedException();
    }

    private void IncreaseStepCount()
    {
        if (_currentStepCount < _stepCount)
            _currentStepCount++;
        else
            Cancel();
    }
}
