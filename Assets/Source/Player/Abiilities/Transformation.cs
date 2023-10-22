using UnityEngine;

[RequireComponent(typeof(PlayerAttacker))]
public class Transformation : Ability, IDeferredCommand
{
    [SerializeField] private PlayerModel _basicModel;
    [SerializeField] private PlayerModel _transformationModel;
    [SerializeField] private ParticleSystem _transformationEffect;

    private PlayerAttacker _attacker;
    private PlayerMover _mover;
    private Player _player;
    private CommandExecuter _commandExecuter;
    private bool _isTransformation;
    private Cell _currentCell;
    private Coroutine _prepareCoroutine;
    private Coroutine _executeCoroutine;

    private void OnDisable()
    {
        if (_mover == null)
            return;

        _mover.MoveEnded -= Cancel;
    }

    public override void Initialize()
    {
        _attacker = GetComponent<PlayerAttacker>();
        _mover = GetComponent<PlayerMover>();
        _player = GetComponent<Player>();
        _commandExecuter = GetComponent<CommandExecuter>();
    }

    public override void Prepare()
    {
        if (_isTransformation)
            return;

        _mover.MoveEnded += Cancel;
        _transformationEffect.Play();
        _basicModel.Hide();
        _transformationModel.Show();
        _attacker.Attack(AttackType.Blind);
        _prepareCoroutine = _commandExecuter.StartCoroutine(_player.Move.Prepare(_commandExecuter));
        _currentCell = _player.CurrentCell;
        _currentCell.Content.BecomeWall();
        _isTransformation = true;
    }

    protected override void Action(Cell cell)
    {
        _executeCoroutine = _commandExecuter.StartCoroutine(_player.Move.Execute(cell, _commandExecuter));
    }

    public override void Cancel()
    {
        if (_commandExecuter.NextCommand is SkipCommand)
            return;

        _commandExecuter.ResetDeferredCommand();
        _currentCell.Content.BecomeEmpty();
        _attacker.Attack(AttackType.UnBlind);
        _player.Move.Cancel(_commandExecuter);
        _mover.MoveEnded -= Cancel;
        _transformationEffect.Play();
        _basicModel.Show();
        _transformationModel.Hide();
        _isTransformation = false;

        if(_executeCoroutine != null)
        {
            _commandExecuter.StopCoroutine(_executeCoroutine);
            _executeCoroutine = null;
        }

        if(_prepareCoroutine != null) 
        {
            _player.StopCoroutine( _prepareCoroutine);
            _prepareCoroutine = null;
        }
    }
}
