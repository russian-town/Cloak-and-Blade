using System.Collections;
using System.Linq;
using Source.Player.Abiilities;
using UnityEngine;

namespace Source.Player.Commands
{
    public class TheWorldCommand : AbilityCommand
    {
        private TheWorld _theWorld;
        private YellowGhost.YellowGhost _yellowGhost;

        public TheWorldCommand(TheWorld theWorld, CommandExecuter.CommandExecuter executer, YellowGhost.YellowGhost yellowGhost)
            : base(theWorld, executer)
        {
            _theWorld = theWorld;
            _yellowGhost = yellowGhost;
            _theWorld.AddSceneEffectsToChange(
                _yellowGhost.SceneEffects.ToList(),
                _yellowGhost.SceneSounds.ToList(),
                _yellowGhost.SceneSplines.ToList(),
                _yellowGhost.SceneAnimations.ToList());
        }

        protected override IEnumerator WaitOfExecute()
        {
            yield break;
        }

        protected override IEnumerator ExecuteAction()
        {
            yield return new WaitUntil(() => _theWorld.Cast(null));
            yield break;
        }

        protected override IEnumerator PrepareAction()
        {
            _theWorld.Prepare();
            yield break;
        }
    }
}
