using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour, IDataReader
{
    private readonly List<LevelView> _levelViews = new();
    private readonly List<Knob> _knobs = new();

    [SerializeField] private List<Level> _levels = new ();
    [SerializeField] private LevelView _levelViewTemplate;
    [SerializeField] private HorizontalLayoutGroup _levelViewParent;
    [SerializeField] private Knob _knobTemplate;
    [SerializeField] private AudioSource _levelViewScrollSource;
    [SerializeField] private HorizontalLayoutGroup _knobParent;
    [SerializeField] private CanvasGroup _levelViewScroll;
    [SerializeField] private ScrollIndicator _scrollIndicator;
    [SerializeField] private int _firstLevelIndex;

    private LevelsHandler _levelsHandler;

    private void OnDisable()
    {
        foreach (var levelView in _levelViews)
            levelView.OpenLevelButtonClicked -= OnOpenLevelButtonClicked;
    }

    public void Read(PlayerData playerData)
    {
        foreach (var level in _levels)
            level.Read(playerData);
    }

    public void Initialize(LevelsHandler levelsHandler)
    {
        for (int i = 0; i < _levels.Count; i++)
        {
            if (i == _firstLevelIndex)
                _levels[i].Open();

            if (i < _levels.Count - 1)
            {
                if (_levels[i].IsCompleted)
                {
                    _levels[i + 1].Open();
                    _scrollIndicator.SetLastOpenedLevelIndex(i + 1);
                }
            }

            _levelsHandler = levelsHandler;
            LevelView levelView = Instantiate(_levelViewTemplate, _levelViewParent.transform);
            levelView.Render(_levels[i]);
            _levelViews.Add(levelView);
            levelView.OpenLevelButtonClicked += OnOpenLevelButtonClicked;
            Knob knob = Instantiate(_knobTemplate, _knobParent.transform);
            knob.Initialize(_scrollIndicator, _levelViewScrollSource, _levelViewScroll);
            _knobs.Add(knob);
        }

        _scrollIndicator.Initialize(_levelViews, _knobs);
    }

    public void OnOpenLevelButtonClicked(Level level)
        => OpenUnlockedLevel(level);

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(_levelsHandler.GetNextLevel().Name);
    }

    public void BackToMainMenu()
    {
        SceneManager.LoadScene(Constants.MainMenu);
    }

    public void RestartLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    private void OpenUnlockedLevel(Level level)
    {
        if (!level.IsOpen)
            return;

        SceneManager.LoadScene(level.Name);
    }
}
