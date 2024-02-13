using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsHandler : MonoBehaviour, IDataReader, IDataWriter
{
    [SerializeField] private List<Level> _levels = new List<Level>();

    private bool _tutorialCompleted;

    public IReadOnlyList<Level> Levels => _levels;

    public string GetFirstLevel() => _levels[0].Name;

    public Level GetCurrentLevel()
    {
        for (int i = 0; i < _levels.Count; i++)
            if (_levels[i].Name == SceneManager.GetActiveScene().name)
                return _levels[i];

        return null;
    }

    public Level GetNextLevel()
    {
        for (int i = 0; i < _levels.Count; i++)
            if (_levels[i].Name == SceneManager.GetActiveScene().name)
                if (i + 1 < _levels.Count)
                    return _levels[i + 1];
                else
                    return _levels[0];

        return null;
    }

    public bool TryLoadTutorial()
    {
        if (_tutorialCompleted == false)
        {
            SceneManager.LoadScene(Constants.Tutorial);
            return true;
        }
        else
        {
            SceneManager.LoadScene(Constants.MainMenu);
            return false;
        }
    }

    public void Read(PlayerData playerData)
    {
        foreach (var level in _levels)
            level.Read(playerData);
    }

    public void Write(PlayerData playerData)
    {
        foreach (var level in _levels)
            level.Write(playerData);
    }
}
