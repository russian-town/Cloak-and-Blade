using UnityEngine;

[RequireComponent(typeof(PlayerMover))]
public class TheWorld : Ability
{
    [SerializeField] private int _maxStepCount;

    private PlayerAttacker _attacker;
    private PlayerMover _mover;
    private bool _isActive;
    private int _currentStepCount;

    public int CurrentStepCount => _currentStepCount;
    public int MaxStepCount => _maxStepCount;

    private void OnDisable()
    {
        _mover.MoveEnded -= IncreaseCurrentStepCount;
    }

    public override void Initialize()
    {
        _attacker = GetComponent<PlayerAttacker>();
        _mover = GetComponent<PlayerMover>();
    }

    public override void Cancel() 
    {
        _isActive = false;
        _currentStepCount = 0;
    }

    public override void Prepare() { }

    protected override void Action(Cell cell)
    {
        if (_isActive)
            return;

        _isActive = true;
        _currentStepCount = 0;
        _mover.MoveEnded += IncreaseCurrentStepCount;
        _attacker.Attack(this);
    }

    private void IncreaseCurrentStepCount()
    {
        _currentStepCount++;
        Debug.Log(_currentStepCount);

        if (_currentStepCount >= _maxStepCount)
        {
            _isActive = false;
            _mover.MoveEnded -= IncreaseCurrentStepCount;
        }
    }
}
