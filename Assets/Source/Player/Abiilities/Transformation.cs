using UnityEngine;
using UnityEngine.Events;

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

    public event UnityAction TransformationEnded;

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
        _attacker.Attack(this);
        StartCoroutine(_player.Move.Prepare(this));
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

        TransformationEnded?.Invoke();
        _player.Move.Cancel();
        _mover.MoveEnded -= Cancel;
        _transformationEffect.Play();
        _basicModel.Show();
        _transformationModel.Hide();
        _isTransformation = false;
    }
}
