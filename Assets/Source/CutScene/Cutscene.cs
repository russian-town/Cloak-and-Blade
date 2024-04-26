using System.Collections.Generic;
using Source.Game;
using Source.Pause;
using UnityEngine;

namespace Source.CutScene
{
    public class Cutscene : MonoBehaviour, IActiveScene, IAutoContinuer
    {
        [SerializeField] private CutsceneScenario _scenario;
        [SerializeField] private FocusHandler.FocusHandler _focusHandler;

        private Pause.Pause _pause;

        private void Start()
        {
            _focusHandler.SetActiveScene(this);
            List<IPauseHandler> pauseHandlers = new ();
            pauseHandlers.Add(_scenario);
            _pause = new Pause.Pause(pauseHandlers);
        }

        public void SetPause()
            => _pause.Enable();

        public void Continue()
            => _pause.Disable();
    }
}
