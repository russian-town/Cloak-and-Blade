using UnityEngine;

[RequireComponent(typeof(PlayerAttacker))]
public class Transformation : Ability, IDeferredAbility
{
    [SerializeField] private PlayerModel _basicModel;
    [SerializeField] private PlayerModel _transformationModel;
    [SerializeField] private ParticleSystem _transformationEffect;

    private PlayerAttacker _attacker;
    private PlayerMover _mover;
    private Player _player;
    private bool _isTransformation;
    private Cell _currentCell;

    private void OnDisable() => _mover.MoveEnded -= Cancel;

    public override void Initialize()
    {
        _attacker = GetComponent<PlayerAttacker>();
        _mover = GetComponent<PlayerMover>();
        _player = GetComponent<Player>();
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
        StartCoroutine(_player.Move.Prepare(this));
        _currentCell = _player.CurrentCell;
        _currentCell.Content.BecomeWall();
        _isTransformation = true;
    }

    protected override void Action(Cell cell)
    {
        StartCoroutine(_player.Move.Execute(cell, this));
    }

    public override void Cancel()
    {
        if (_player.NextCommand is SkipCommand)
            return;

        _currentCell.Content.BecomeEmpty();
        _attacker.Attack(AttackType.UnBlind);
        _player.Move.Cancel();
        _mover.MoveEnded -= Cancel;
        _transformationEffect.Play();
        _basicModel.Show();
        _transformationModel.Hide();
        _isTransformation = false;
    }
}
