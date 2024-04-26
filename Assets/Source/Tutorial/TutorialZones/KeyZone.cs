using UnityEngine;

namespace Source.Tutorial.TutorialZones
{
    public class KeyZone : TutorialZone
    {
        [SerializeField] private Gameboard.Gameboard _gameboard;

        public override void Interact()
        {
            _gameboard.Disable();
            base.Interact();
            Element.Show(Player);
        }
    }
}
