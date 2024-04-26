using Source.Gameboard.Cell;
using Source.Gameboard.Cell.CellContent;
using Source.Player.PlayerUI;
using Source.Upgrader;
using UnityEngine;

namespace Source.Player.Abiilities
{
    [RequireComponent(typeof(Player), typeof(Navigator.Navigator))]
    public class Blink : Ability
    {
        [SerializeField] private float _moveSpeed;
        [SerializeField] private float _rotationSpeed;
        [SerializeField] private int _blinkRange = 4;
        [SerializeField] private ParticleSystem _prepareEffect;
        [SerializeField] private ParticleSystem _actionEffect;
        [SerializeField] private ParticleSystem _trailEffectMain;
        [SerializeField] private ParticleSystem _trailEffectChildren;
        [SerializeField] private AudioSource _source;
        [SerializeField] private AudioClip _prepareSound;
        [SerializeField] private AudioClip _actionSound;

        private UpgradeSetter _upgradeSetter;
        private PlayerView _playerView;
        private Player _player;
        private Navigator.Navigator _navigator;
        private bool _canUse = true;

        protected Navigator.Navigator Navigator => _navigator;

        protected Player Player => _player;

        protected int BlinkRange => _blinkRange;

        public override void Initialize(UpgradeSetter upgradeSetter, PlayerView playerView)
        {
            _player = GetComponent<Player>();
            _navigator = GetComponent<Navigator.Navigator>();
            _upgradeSetter = upgradeSetter;
            _playerView = playerView;
            _blinkRange += _upgradeSetter.Level;
        }

        public override void Prepare()
        {
            _playerView.DisableAbilityButton();
            RefillNavigatorCells();
            ShowBlinkRange();
            _source.clip = _prepareSound;
            _source.Play();
            _prepareEffect.Play();
        }

        public override void Cancel()
        {
            _playerView.Cancel();
            _playerView.EnableAbilityButton();
            HideBlinkRange();
            _prepareEffect.Stop();
            _source.Stop();
        }

        public override bool CanUse()
            => _canUse;

        public override void ResetAbility()
        {
            if (_canUse == true)
                return;

            _canUse = true;
        }

        public virtual void RefillNavigatorCells()
        {
            _navigator.RefillAvailableCells(_player.CurrentCell, _blinkRange);
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
                _trailEffectChildren.startSpeed = Vector3.Distance(cell.transform.position, _player.CurrentCell.transform.position) * 5;
                Vector3 targetEffectRotation = cell.transform.position - _trailEffectMain.transform.position;
                _trailEffectMain.transform.rotation = Quaternion.LookRotation(targetEffectRotation);
                _trailEffectMain.Play();
            }
        }

        private void ShowBlinkRange()
        {
            foreach (var cell in _navigator.AvailableCells)
            {
                if (cell.Content.Type != CellContentType.Wall)
                {
                    cell.View.PlayAbilityRangeEffect();
                    cell.View.Show();
                }
            }
        }

        private void HideBlinkRange()
        {
            foreach (var cell in _navigator.AvailableCells)
            {
                cell.View.StopAbilityRangeEffect();
                cell.View.Hide();
            }
        }
    }
}
