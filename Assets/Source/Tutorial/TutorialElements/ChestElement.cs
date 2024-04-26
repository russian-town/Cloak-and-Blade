using Source.InteractiveObjects.Objects.Treasure;
using UnityEngine;

namespace Source.Tutorial.TutorialElements
{
    public class ChestElement : BaseTutorialElement
    {
        [SerializeField] private Gameboard.Gameboard _gameboard;
        [SerializeField] private Treasure _treasure;

        public override void Show(Player.Player player)
        {
            _treasure.Opened += OnTreasureOpened;
        }

        private void OnTreasureOpened()
        {
            _gameboard.Disable();
            _treasure.Opened -= OnTreasureOpened;
            InvokeTutorialZoneCompleteAction();
        }
    }
}
