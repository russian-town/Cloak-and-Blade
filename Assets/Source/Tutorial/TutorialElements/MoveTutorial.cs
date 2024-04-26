using System.Collections.Generic;
using Source.Tutorial.UI;
using UnityEngine;

namespace Source.Tutorial.TutorialElements
{
    public class MoveTutorial : BaseTutorialElement
    {
        [SerializeField] private List<MainButton> _mainButtons = new List<MainButton>();
        [SerializeField] private Gameboard.Gameboard _gameboard;

        public override void Show(Player.Player player)
        {
            _gameboard.Enable();

            foreach (var button in _mainButtons)
            {
                button.Open();
                button.Show();
            }
        }
    }
}
