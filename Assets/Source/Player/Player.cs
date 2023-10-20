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
    [SerializeField] private Sprite _abilityIcon;

    private PlayerMover _mover;
    private PlayerAttacker _attacker;
    private IEnemyTurnHandler _enemyTurnHandler;
    private Cell _startCell;
    private MoveCommand _moveCommand;
    private AbilityCommand _abilityCommand;
    private SkipCommand _skipCommand;
    private Navigator _navigator;
    private Hourglass _hourglass;
    private PlayerAnimationHandler _animationHandler;
    private Command _currentCommand;
    private PlayerView _playerView;
    private List<Enemy> _enemies = new List<Enemy>();
    private Command _deferredCommand;
    private List<EffectChangeHanldler> _sceneEffects = new List<EffectChangeHanldler>();
    private Gameboard _gameboard;
    private PlayerInput _input;
    private Coroutine _prepare;
    private Coroutine _execute;
    private Coroutine _waitOfExecute;
    private Coroutine _switchCommand;

    public Sprite AbilityIcon => _abilityIcon;
    public Coroutine MoveCoroutine { get; private set; }
    public Cell CurrentCell => _mover.CurrentCell;
    public ItemsInHold ItemsInHold => _itemsInHold;
    public MoveCommand Move => _moveCommand;
    public Command NextCommand { get; private set; }
    public IReadOnlyList<EffectChangeHanldler> SceneEffects => _sceneEffects;

    public event UnityAction StepEnded;
    public event UnityAction Died;

    public void Unsubscribe() => _mover.MoveEnded -= OnMoveEnded;

    public void Initialize(Cell startCell, Hourglass hourglass, IEnemyTurnHandler enemyTurnHandler, PlayerView playerView, Gameboard gameboard, PlayerInput input)
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
        _gameboard = gameboard;
        _input = input;
        _moveCommand = new MoveCommand(this, _mover, _playerView, _navigator, _moveSpeed, _rotationSpeed, _input, _gameboard);
        _abilityCommand = new AbilityCommand(_ability, _input, _gameboard, this, _navigator);
        _skipCommand = new SkipCommand(this, _enemyTurnHandler.WaitForEnemies(), _animationHandler, _hourglass, this);
    }

    public void AddEffects(List<EffectChangeHanldler> sceneEffect)
    {
        if (sceneEffect == null)
            return;

        if (sceneEffect.Count > 0)
            _sceneEffects.AddRange(sceneEffect);
    }

    public void SetTargets(List<Enemy> enemies)
    {
        _enemies.AddRange(enemies);
        _attacker.Initialize(_enemies);
    }

    public void PrepareAbility()
    {
        if (_switchCommand != null)
        {
            StopCoroutine(_switchCommand);
            _switchCommand = null;
        }

        StartCoroutine(SwitchCurrentCommand(_abilityCommand));
    }

    public void PrepareMove()
    {
        if (_switchCommand != null)
        {
            StopCoroutine(_switchCommand);
            _switchCommand = null;
        }

        StartCoroutine(SwitchCurrentCommand(_moveCommand));
    }

    public void PrepareSkip()
    {
        if (_switchCommand != null)
        {
            StopCoroutine(_switchCommand);
            _switchCommand = null;
        }

        StartCoroutine(SwitchCurrentCommand(_skipCommand));
    }

    public void SkipTurn() => StepEnded?.Invoke();

    public bool TryMoveToCell(Cell targetCell, float moveSpeed, float rotationSpeed)
    {
        if (_navigator.CanMoveToCell(targetCell) && targetCell.IsOccupied == false && targetCell.Content.Type != CellContentType.Wall)
        {
            _mover.StartMoveTo(targetCell, moveSpeed, rotationSpeed);
            _startCell = _mover.CurrentCell;
            return true;
        }

        return false;
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

    private IEnumerator SwitchCurrentCommand(Command command)
    {
        //if (_currentCommand is SkipCommand)
        //    return;

        //if (_currentCommand != null && _currentCommand.IsExecuting)
        //    return;

        //if (command == _currentCommand)
        //    return;

        //if (command is AbilityCommand abilityCommand && abilityCommand.Ability is IDeferredAbility)
        //    _deferredCommand = abilityCommand;

        if (command == _currentCommand)
            yield break;

        if (_currentCommand != null && _currentCommand.IsExecuting)
            yield break;

        if(_prepare != null)
        {
            StopCoroutine(_prepare);
            _prepare = null;
        }

        if(_waitOfExecute != null)
        {
            StopCoroutine(_waitOfExecute);
            _waitOfExecute = null;
        }

        NextCommand = command;
        _currentCommand?.Cancel(this);
        _currentCommand = command;

        _prepare = StartCoroutine(_currentCommand.Prepare(this));
        yield return _prepare;

        _waitOfExecute = StartCoroutine(_currentCommand.WaitOfExecute());
        yield return _waitOfExecute;
    }

    private void OnMoveEnded()
    {
        if (!ResetCommand())
        {
            _prepare = StartCoroutine(_currentCommand.Prepare(this));
            _waitOfExecute = StartCoroutine(_currentCommand.WaitOfExecute());
        }

        StepEnded?.Invoke();
    }
}
