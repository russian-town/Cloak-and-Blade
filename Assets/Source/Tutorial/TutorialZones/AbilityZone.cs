using Source.Tutorial.TutorialPlayer;
using UnityEngine;

namespace Source.Tutorial.TutorialZones
{
    public class AbilityZone : TutorialZone
    {
        [SerializeField] private Side _side;

        public override void Interact()
        {
            if (Player.TryGetComponent(out TutorialCharacter character))
            {
                Player.CommandExecuter.ResetCommand();
                character.SetSide(_side);
                base.Interact();
            }
        }
    }
}
