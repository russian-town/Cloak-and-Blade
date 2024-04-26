using System.Collections;
using Source.Player.Abiilities;

namespace Source.Player.Commands
{
    public class AbilityCommand : Command
    {
        private readonly Ability _ability;

        public Ability Ability
        {
            get
            {
                return _ability;
            }
        }

        public AbilityCommand(Ability ability, CommandExecuter.CommandExecuter executer)
            : base(executer)
            => _ability = ability;

        public bool IsUsed => _ability.CanUse() == false;

        protected override IEnumerator WaitOfExecute()
        {
            yield return null;
        }

        protected override IEnumerator ExecuteAction()
        {
            yield return null;
        }

        protected override IEnumerator PrepareAction()
        {
            yield return null;
        }

        protected override void OnCommandChanged(Command command)
            => Cancel();

        protected override void Cancel()
            => base.Cancel();
    }
}
