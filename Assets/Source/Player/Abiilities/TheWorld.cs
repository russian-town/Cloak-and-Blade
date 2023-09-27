using UnityEngine;

[RequireComponent(typeof(PlayerMover))]
public class TheWorld : Ability
{
    [SerializeField] private int _stepCount;

    private PlayerMover _mover;
    private PlayerAttacker _attacker;
    private int _currentStepCount;

    private void OnDisable() => _mover.MoveEnded -= IncreaseStepCount;

    public override void Initialize()
    {
        _mover = GetComponent<PlayerMover>();
        _attacker = GetComponent<PlayerAttacker>();
        _mover.MoveEnded += IncreaseStepCount;
    }

    public override void Cancel()
    {
    }

    public override void Prepare()
    {
    }

    protected override void Action(Cell cell)
    {
        if (_currentStepCount <= _stepCount)
        {
            _attacker.Attack(this);
        }
        else
        {
            _attacker.Attack(null);
            Cancel();
        }
    }

    private void IncreaseStepCount()
    {
        _currentStepCount++;
    }
}
