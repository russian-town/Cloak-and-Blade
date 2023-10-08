using UnityEngine;

[RequireComponent(typeof(PlayerMover))]
public class TheWorld : Ability
{
    [SerializeField] private int _maxStepCount;

    private PlayerAttacker _attacker;
    private Player _player;
    private bool _isActive;
    private int _currentStepCount;

    public int CurrentStepCount => _currentStepCount;
    public int MaxStepCount => _maxStepCount;

    private void OnDisable()
    {
        _player.StepEnded -= IncreaseCurrentStepCount;
    }

    public override void Initialize()
    {
        _attacker = GetComponent<PlayerAttacker>();
        _player = GetComponent<Player>();
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
        _player.StepEnded += IncreaseCurrentStepCount;
        _attacker.Attack(this);
    }

    private void IncreaseCurrentStepCount()
    {
        _currentStepCount++;

        if (_currentStepCount >= _maxStepCount)
        {
            _isActive = false;
            _player.StepEnded -= IncreaseCurrentStepCount;
        }
    }
}
