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
    private PlayerView _playerView;

    public bool Prepared { get; private set; }

    public override void Initialize(UpgradeSetter upgradeSetter, PlayerView playerView)
    {
        _attacker = GetComponent<PlayerAttacker>();
        _player = GetComponent<Player>();
        _upgradeSetter = upgradeSetter;
        _playerView = playerView;
        _useLimit += _upgradeSetter.Level;
        _maxUseLimit = _useLimit;
    }

    public override void Prepare()
    {
        if (Prepared == true)
            return;

        _playerView.DisableAbilityButton();
        _transformationEffect.Play();
        _basicModel.Hide();
        _transformationModel.Show();
        _attacker.Attack(AttackType.Blind);
        _currentCell = _player.CurrentCell;
        _currentCell.Content.BecomeWall();
        Prepared = true;
    }

    public override void Cancel()
    {
        _playerView.Cansel();
        _playerView.EnableAbilityButton();
        _currentCell.Content.BecomeEmpty();
        _attacker.Attack(AttackType.UnBlind);
        _transformationEffect.Play();
        _basicModel.Show();
        _transformationModel.Hide();
        Prepared = false;
    }

    public override bool CanUse()
    {
        return _useLimit > 0;
    }

    protected override void Action(Cell cell)
    {
        _useLimit--;
        _useLimit = Mathf.Clamp(_useLimit, 0, _maxUseLimit);
    }

    public override void ResetAbility()
    {
        _useLimit++;
    }
}
