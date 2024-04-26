using System.Collections.Generic;
using Source.Player.PlayerUI;
using Source.Tutorial.UI;
using UnityEngine;

namespace Source.Tutorial.TutorialElements
{
    public class ResetAbilityElement : BaseTutorialElement
    {
        [SerializeField] private Gameboard.Gameboard _gameboard;
        [SerializeField] private PlayerView _playerView;
        [SerializeField] private List<MainButton> _abilityButtons;
        [SerializeField] private Battery _battery;

        private Player.Player _player;

        public override void Show(Player.Player player)
        {
            _player = player;
            _player.Mover.MoveEnded += OnMoveEnded;
            _gameboard.Enable();
            player.GetAbilityCommand().Ability.ResetAbility();
            _playerView.ResetAbilityIcon();
            _battery.Enable();

            foreach (var button in _abilityButtons)
            {
                button.Open();
                button.Show();
            }
        }

        private void OnMoveEnded()
        {
            _player.Mover.MoveEnded -= OnMoveEnded;
            InvokeTutorialZoneCompleteAction();
        }
    }
}
