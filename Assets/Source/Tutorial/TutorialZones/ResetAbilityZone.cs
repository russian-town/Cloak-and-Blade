using Source.InteractiveObjects.Objects.Treasure;
using Source.Tutorial.TutorialPlayer;
using UnityEngine;

namespace Source.Tutorial.TutorialZones
{
    public class ResetAbilityZone : TutorialZone
    {
        [SerializeField] private Treasure _treasure;
        [SerializeField] private Side _side;

        public override void Interact()
        {
            if (Player.ItemsInHold.FindItemInList(_treasure))
            {
                Player.CommandExecuter.ResetCommand();

                if (Player.TryGetComponent(out TutorialCharacter character))
                    character.SetSide(_side);

                base.Interact();
            }
        }
    }
}
