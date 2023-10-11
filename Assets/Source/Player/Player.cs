using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerMover), typeof(PlayerAttacker))]
public class Player : Ghost, IPauseHandler
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private Ability _ability;
    [SerializeField] private ItemsInHold _itemsInHold;
    [SerializeField] private ParticleSystem _diedParticle;
    [SerializeField] private PlayerModel _model;

    private PlayerMover _mover;
    private PlayerAttacker _attacker;
    private IEnemyTurnHandler _enemyTurnHandler;
    private Cell _startCell;
    private MoveCommand _moveCommand;
    private AbilityCommand _abilityCommand;
    private SkipCommand _skipCommand;
    private Navigator _navigator;
    private AnimationClip _hourglassAnimation;
    private Animator _hourglassAnimator;
    private CanvasGroup _hourglass;
    private PlayerAnimationHandler _animationHandler;
    private Command _currentCommand;
    private PlayerView _playerView;
    private List<Enemy> _enemies = new List<Enemy>();
    private Command _deferredCommand;

    public Cell CurrentCell => _mover.CurrentCell;
    public ItemsInHold ItemsInHold => _itemsInHold;
    public MoveCommand Move => _moveCommand;
    public Command NextCommand { get; private set; }

    public event UnityAction StepEnded;
    public event UnityAction Died;

    public void Unsubscribe() => _mover.MoveEnded -= OnMoveEnded;

    public void Initialize(Cell startCell, AnimationClip hourglassAnimation, Animator hourglassAnimator, CanvasGroup hourglass, IEnemyTurnHandler enemyTurnHandler, PlayerView playerView)
    {
        _startCell = startCell;
        _mover = GetComponent<PlayerMover>();
        _navigator = GetComponent<Navigator>();
        _attacker = GetComponent<PlayerAttacker>();
        _playerView = playerView;
        _animationHandler = GetComponent<PlayerAnimationHandler>();
        _enemyTurnHandler = enemyTurnHandler;
        _mover.Initialize(_startCell, _animationHandler);
        _ability.Initialize();
        _mover.MoveEnded += OnMoveEnded;
        _hourglass = hourglass;
        _hourglassAnimator = hourglassAnimator;
        _hourglassAnimation = hourglassAnimation;
        _moveCommand = new MoveCommand(this, _mover, _playerView, _navigator, _moveSpeed, _rotationSpeed);
        _abilityCommand = new AbilityCommand(_ability);
        _skipCommand = new SkipCommand(this, _hourglassAnimator, this, _hourglass, _enemyTurnHandler.WaitForEnemies(), _animationHandler, _hourglassAnimation);
    }

    public void SetTargets(List<Enemy> enemies)
    {
        _enemies.AddRange(enemies);
        _attacker.Initialize(_enemies);
    }

    public void PrepareAbility() => SwitchCurrentCommand(_abilityCommand);

    public void PrepareMove() => SwitchCurrentCommand(_moveCommand);

    public void PrepareSkip()
    {
        SwitchCurrentCommand(_skipCommand);
        ExecuteCurrentCommand(CurrentCell);
    }

    public void SkipTurn() => StepEnded?.Invoke();

    public bool TryMoveToCell(Cell targetCell, float moveSpeed, float rotationSpeed)
    {
        if (_navigator.CanMoveToCell(targetCell) && targetCell.IsOccupied == false && targetCell.Content.Type != CellContentType.Wall)
        {
            _mover.Move(targetCell, moveSpeed, rotationSpeed);
            _startCell = _mover.CurrentCell;
            return true;
        }

        return false;
    }

    public void ExecuteCurrentCommand(Cell cell)
    {
        if (_currentCommand?.IsExecuting == true)
            return;

        if (_currentCommand == null)
        {
            if (_deferredCommand != null)
                SwitchCurrentCommand(_deferredCommand);
            else
                return;
        }

        StartCoroutine(_currentCommand.Execute(cell, this));
    }

    public void Die() => StartCoroutine(MakeDeath());

    public void SetPause(bool isPause)
    {
        _mover.SetPause(isPause);

        if (isPause == true)
            _animationHandler.StopAnimation();
        else
            _animationHandler.StartAnimation();
    }

    public bool ResetCommand()
    {
        if (_currentCommand is not SkipCommand)
            _deferredCommand = null;

        if (_currentCommand is not IUnmissable)
        {
            _currentCommand = null;
            return true;
        }

        return false;
    }

    private IEnumerator MakeDeath()
    {
        _currentCommand = null;
        _model.Hide();
        _diedParticle.Play();
        yield return new WaitUntil(() => !_diedParticle.isPlaying);
        Died?.Invoke();
    }

    private void SwitchCurrentCommand(Command command)
    {
        if (_currentCommand is SkipCommand)
            return;

        if (_currentCommand != null && _currentCommand.IsExecuting)
            return;

        if (command == _currentCommand)
            return;

        if (command is AbilityCommand abilityCommand && abilityCommand.Ability is IDeferredAbility)
            _deferredCommand = abilityCommand;

        NextCommand = command;
        _currentCommand?.Cancel();
        _currentCommand = command;
        StartCoroutine(_currentCommand.Prepare(this));
    }

    private void OnMoveEnded()
    {
        if (!ResetCommand())
            StartCoroutine(_currentCommand.Prepare(this));

        StepEnded?.Invoke();
    }
}
