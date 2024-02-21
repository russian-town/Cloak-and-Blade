using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour, IDataReader, IDataWriter
{
    [SerializeField] private List<Level> _levels = new List<Level>();
    [SerializeField] private LevelView _levelViewTemplate;
    [SerializeField] private HorizontalLayoutGroup _levelViewParent;
    [SerializeField] private Knob _knobTemplate;
    [SerializeField] private AudioSource _levelViewScrollSource;
    [SerializeField] private HorizontalLayoutGroup _knobParent;
    [SerializeField] private CanvasGroup _levelViewScroll;
    [SerializeField] private ScrollIndicator _scrollIndicator;
    [SerializeField] private int _firstLevelIndex;

    private List<LevelView> _levelViews = new List<LevelView>();
    private List<Knob> _knobs = new List<Knob>();

    private void OnDisable()
    {
        foreach (var levelView in _levelViews)
            levelView.OpenLevelButtonClicked -= OnOpenLevelButtonClicked;
    }

    public void Read(PlayerData playerData)
    {
        foreach (var level in _levels)
            level.Read(playerData);

        if (playerData.FinishedLevelNames.Contains("Chapel"))
            print("FinishedLevels contains chapel");
        else
            print("No bueno");

        if (Constants.Tutorial == _levels[0].Name)
            print("_levels Contains the tutorial name");
    }

    public void Write(PlayerData playerData)
    {
        /*foreach (var level in _levels)
            level.Write(playerData);*/
    }

    public void Initialize()
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

    public void OnOpenLevelButtonClicked(Level level) => TryOpenLevel(level);

    private void TryOpenLevel(Level level)
    {
        if (level.IsOpen)
            SceneManager.LoadScene(level.Name);
    }
}
