using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelsHandler : MonoBehaviour
{
    [SerializeField] private List<Level> _levels = new List<Level>();

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

        return null;
    }
}
