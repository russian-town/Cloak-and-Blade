using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(PlayerMover), typeof(PlayerAttacker), typeof(CommandExecuter))]
public abstract class Player : Ghost, IPauseHandler
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private ItemsInHold _itemsInHold;
    [SerializeField] private ParticleSystem _diedParticle;
    [SerializeField] private PlayerModel _model;
    [SerializeField] private Sprite _abilityIcon;

    private PlayerMover _mover;
    private PlayerAttacker _attacker;
    private IEnemyTurnHandler _enemyTurnHandler;
    private Cell _startCell;
    private MoveCommand _moveCommand;
    private SkipCommand _skipCommand;
    private Navigator _navigator;
    private Hourglass _hourglass;
    private PlayerAnimationHandler _animationHandler;
    private PlayerView _playerView;
    private List<Enemy> _enemies = new List<Enemy>();
    private Gameboard _gameboard;
    private CommandExecuter _commandExecuter;

    public Coroutine MoveCoroutine { get; private set; }
    public Cell CurrentCell => _mover.CurrentCell;
    public ItemsInHold ItemsInHold => _itemsInHold;
    public MoveCommand Move => _moveCommand;
    protected Navigator Navigator => _navigator;
    protected Gameboard Gameboard => _gameboard;
    protected CommandExecuter CommandExecuter => _commandExecuter;

    public event UnityAction StepEnded;
    public event UnityAction Died;

    public void Unsubscribe() => _mover.MoveEnded -= OnMoveEnded;

    public void Initialize(Cell startCell, Hourglass hourglass, IEnemyTurnHandler enemyTurnHandler, PlayerView playerView, Gameboard gameboard)
    {
        _startCell = startCell;
        _mover = GetComponent<PlayerMover>();
        _navigator = GetComponent<Navigator>();
        _attacker = GetComponent<PlayerAttacker>();
        _commandExecuter = GetComponent<CommandExecuter>();
        _playerView = playerView;
        _animationHandler = GetComponent<PlayerAnimationHandler>();
        _enemyTurnHandler = enemyTurnHandler;
        _mover.Initialize(_startCell, _animationHandler);
        _mover.MoveEnded += OnMoveEnded;
        _hourglass = hourglass;
        _gameboard = gameboard;
        _moveCommand = new MoveCommand(this, _mover, _playerView, _navigator, _moveSpeed, _rotationSpeed, _gameboard, _commandExecuter);
        _skipCommand = new SkipCommand(this, _enemyTurnHandler.WaitForEnemies(), _animationHandler, _hourglass, _commandExecuter);
    }

    public void SetTargets(List<Enemy> enemies)
    {
        _enemies.AddRange(enemies);
        _attacker.Initialize(_enemies);
    }

    public void PrepareAbility() => _commandExecuter.PrepareCommand(AbilityCommand());

    public void PrepareMove() => _commandExecuter.PrepareCommand(_moveCommand);

    public void PrepareSkip() => _commandExecuter.PrepareCommand(_skipCommand);

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

    protected abstract Command AbilityCommand();

    private IEnumerator MakeDeath()
    {
        _commandExecuter.ResetCommand();
        _model.Hide();
        _diedParticle.Play();
        yield return new WaitUntil(() => !_diedParticle.isPlaying);
        Died?.Invoke();
    }

    private void OnMoveEnded()
    {
        _commandExecuter.UpdateLastCommand();
        StepEnded?.Invoke();
    }
}
