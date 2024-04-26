using System.Collections.Generic;
using Source.Tutorial.UI;
using UnityEngine;

namespace Source.Tutorial.TutorialElements
{
    public class AbilityTutorial : BaseTutorialElement
    {
        [SerializeField] private List<MainButton> _abilityButtons;
        [SerializeField] private Gameboard.Gameboard _gameboard;

        private Player.Player _player;

        public override void Show(Player.Player player)
        {
            _player = player;
            _player.Mover.MoveEnded += OnMoveEnded;
            _gameboard.Enable();

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
