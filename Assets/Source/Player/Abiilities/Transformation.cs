using UnityEngine;

[RequireComponent(typeof(PlayerAttacker))]
public class Transformation : Ability
{
    [SerializeField] private PlayerModel _basicModel;
    [SerializeField] private PlayerModel _transformationModel;
    [SerializeField] private ParticleSystem _transformationEffect;
    [SerializeField] private int _useLimit = 1;

    private PlayerAttacker _attacker;
    private Player _player;
    private Cell _currentCell;
    private int _maxUseLimit;
    private UpgradeSetter _upgradeSetter;

    public bool Prepared { get; private set; }

    public override void Initialize(UpgradeSetter upgradeSetter)
    {
        _attacker = GetComponent<PlayerAttacker>();
        _player = GetComponent<Player>();
        _upgradeSetter = upgradeSetter;
        _useLimit += _upgradeSetter.Level;
        _maxUseLimit = _useLimit;
        Debug.Log($"{this} initialazed.");
    }

    public override void Prepare()
    {
        if (Prepared == true)
            return;

        _transformationEffect.Play();
        _basicModel.Hide();
        _transformationModel.Show();
        _attacker.Attack(AttackType.Blind);
        _currentCell = _player.CurrentCell;
        _currentCell.Content.BecomeWall();
        Prepared = true;
        Debug.Log($"{this} prepare.");
    }

    public override void Cancel()
    {
        _currentCell.Content.BecomeEmpty();
        _attacker.Attack(AttackType.UnBlind);
        _transformationEffect.Play();
        _basicModel.Show();
        _transformationModel.Hide();
        Prepared = false;
        Debug.Log($"{this} cansel.");
    }

    public override bool CanUse()
    {
        Debug.Log($"{this} can use = {_useLimit > 0}.");
        return _useLimit > 0;
    }

    protected override void Action(Cell cell)
    {
        Debug.Log($"{this} decrease useLimit.");
        _useLimit--;
        _useLimit = Mathf.Clamp(_useLimit, 0, _maxUseLimit);
    }
}
