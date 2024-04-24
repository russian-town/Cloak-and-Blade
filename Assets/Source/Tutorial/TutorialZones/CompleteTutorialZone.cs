using UnityEngine;

public class CompleteTutorialZone : MonoBehaviour, IDataWriter, IDataReader
{
    [SerializeField] private LevelExit _levelExit;

    private bool _isTutorialCompleted;

    private void OnEnable()
    {
        _levelExit.ExitOpened += OnLevelPassed;
    }

    private void OnDisable()
    {
        _levelExit.ExitOpened -= OnLevelPassed;
    }

    public void Write(PlayerData playerData)
    {
        playerData.IsTutorialCompleted = _isTutorialCompleted;
    }

    public void Read(PlayerData playerData)
    {
        _isTutorialCompleted = playerData.IsTutorialCompleted;
    }

    private void OnLevelPassed()
    {
        _isTutorialCompleted = true;
    }
}
