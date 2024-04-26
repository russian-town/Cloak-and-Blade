using Source.InteractiveObjects.Objects.Key;
using UnityEngine;

namespace Source.Tutorial.TutorialElements
{
    public class KeyElement : BaseTutorialElement
    {
        [SerializeField] private Gameboard.Gameboard _gameboard;
        [SerializeField] private Key _key;

        public override void Show(Player.Player player)
        {
            _key.PickedUp += OnPickedUp;
        }

        private void OnPickedUp()
        {
            _gameboard.Disable();
            _key.PickedUp -= OnPickedUp;
            InvokeTutorialZoneCompleteAction();
        }
    }
}
