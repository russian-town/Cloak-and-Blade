using UnityEngine;

[RequireComponent(typeof(Player), typeof(Navigator))]
public class Blink : Ability
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private int _blinkRange = 4;
    [SerializeField] private ParticleSystem _prepareEffect;
    [SerializeField] private ParticleSystem _actionEffect;
    [SerializeField] private AudioSource _source;
    [SerializeField] private AudioClip _prepareSound;
    [SerializeField] private AudioClip _actionSound;

    private UpgradeSetter _upgradeSetter;
    private Player _player;
    private Navigator _navigator;
    private bool _canUse = true;

    public override void Initialize(UpgradeSetter upgradeSetter)
    {
        _player = GetComponent<Player>();
        _navigator = GetComponent<Navigator>();
        _upgradeSetter = upgradeSetter;
        _blinkRange += _upgradeSetter.Level;
    }

    public override void Prepare()
    {
        _navigator.RefillAvailableCells(_player.CurrentCell, false, _blinkRange);
        ShowBlinkRange();
        _source.clip = _prepareSound;
        _source.Play();
        _prepareEffect.Play();
    }

    public override void Cancel()
    {
        HideBlinkRange();
        _prepareEffect.Stop();
        _source.Stop();
    }

    public override bool CanUse()
    {
        return _canUse;
    }

    protected override void Action(Cell cell)
    {
        if (_player.TryMoveToCell(cell, _moveSpeed, _rotationSpeed))
        {
            _canUse = false;
            Cancel();
            _source.clip = _actionSound;
            _source.Play();
            _actionEffect.Play();
        }
    }

    private void ShowBlinkRange()
    {
        foreach (var cell in _navigator.AvailableCells)
            if (cell.Content.Type != CellContentType.Wall)
                cell.View.PlayAbilityRangeEffect();
    }

    private void HideBlinkRange()
    {
        foreach (var cell in _navigator.AvailableCells)
            cell.View.StopAbilityRangeEffect();
    }
}
