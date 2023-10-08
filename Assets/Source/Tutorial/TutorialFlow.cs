using UnityEngine;

public class TutorialFlow : MonoBehaviour
{
    [SerializeField] private TutorialButtonHider _skipButton;
    [SerializeField] private TutorialButtonHider _cameraMode;
    [SerializeField] private TutorialButtonHider _cameraTurnClock;
    [SerializeField] private TutorialButtonHider _cameraTurnCounter;

    private void Start()
    {
        _skipButton.Hide();
        _cameraMode.Hide();
        _cameraTurnClock.Hide();
        _cameraTurnCounter.Hide();
    }
}
