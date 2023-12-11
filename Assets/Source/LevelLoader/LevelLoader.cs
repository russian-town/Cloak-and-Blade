using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour, IDataReader, IDataWriter
{
    [SerializeField] private LevelsHandler _levelsHandler;
    [SerializeField] private LevelView _levelViewTemplate;
    [SerializeField] private GridLayoutGroup _parent;

    private List<LevelView> _levelViews = new List<LevelView>();

    private bool _tutorialCompleted;

    private void OnDisable()
    {
        foreach (var levelView in _levelViews)
            levelView.OpenLevelButtonClicked -= OnOpenLevelButtonClicked;
    }

    public void Read(PlayerData playerData)
    {
        _tutorialCompleted = playerData.IsTutorialCompleted;

        foreach (var level in _levelsHandler.Levels)
            level.Read(playerData);
    }

    public void Write(PlayerData playerData)
    {
        foreach (var level in _levelsHandler.Levels)
            level.Write(playerData);
    }

    public void Initialize()
    {
        for (int i = 0; i < _levelsHandler.Levels.Count; i++)
        {
            if (i == 0)
                _levelsHandler.Levels[i].Open();

            if (i < _levelsHandler.Levels.Count - 1)
                if (_levelsHandler.Levels[i].IsCompleted)
                    _levelsHandler.Levels[i + 1].Open();

            LevelView levelView = Instantiate(_levelViewTemplate, _parent.transform);
            levelView.Render(_levelsHandler.Levels[i]);
            _levelViews.Add(levelView);
            levelView.OpenLevelButtonClicked += OnOpenLevelButtonClicked;
        }
    }

    public bool TryTutorialLoad()
    {
        if (_tutorialCompleted == false)
        {
            SceneManager.LoadScene(Constants.Tutorial);
            return true;
        }

        return false;
    }

    public void OnOpenLevelButtonClicked(Level level) => TryOpenLevel(level);

    private void TryOpenLevel(Level level)
    {
        if (level.IsOpen)
            SceneManager.LoadScene(level.Name);
    }
}
