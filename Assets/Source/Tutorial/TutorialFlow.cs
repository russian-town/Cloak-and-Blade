using UnityEngine;

public class TutorialFlow : MonoBehaviour, IDataWriter
{
    [SerializeField] private TutorialButtonHider _skipButton;
    [SerializeField] private TutorialButtonHider _cameraMode;
    [SerializeField] private TutorialButtonHider _cameraTurnClock;
    [SerializeField] private TutorialButtonHider _cameraTurnCounter;
    [SerializeField] private TutorialButtonHider _abilityButton;
    [SerializeField] private LevelExit _levelExit;
    [SerializeField] private Root _root;

    private bool _isTutorialCompleted = false;

    private void OnEnable() => _levelExit.LevelPassed += OnLevelPassed;

    private void OnDisable() => _levelExit.LevelPassed -= OnLevelPassed;

    private void Start()
    {
        _root.Saver.AddDataWriters(new IDataWriter[] {this});
        _skipButton.Hide();
        _cameraMode.Hide();
        _cameraTurnClock.Hide();
        _cameraTurnCounter.Hide();
        _abilityButton.Hide();
    }

    public void Write(PlayerData playerData) => playerData.IsTutorialCompleted = _isTutorialCompleted;

    private void OnLevelPassed()
    {
        _isTutorialCompleted = true;
        _root.Saver.Save();
    }
}
