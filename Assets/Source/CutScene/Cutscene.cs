using System.Collections.Generic;
using UnityEngine;

public class Cutscene : MonoBehaviour, IActiveScene, IAutoContinuer
{
    [SerializeField] private CutsceneScenario _scenario;
    [SerializeField] private FocusHandler _focusHandler;

    private Pause _pause;

    private void Start()
    {
        _focusHandler.SetActiveScene(this);
        List<IPauseHandler> pauseHandlers = new List<IPauseHandler>();
        pauseHandlers.Add(_scenario);
        _pause = new Pause(pauseHandlers);
    }

    public void SetPause() => _pause.Enable();

    public void Continue() => _pause.Disable();
}
