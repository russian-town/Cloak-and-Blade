using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour, IDataReader
{
    [SerializeField] private List<Level> _levels = new List<Level>();
    [SerializeField] private LevelView _levelViewTemplate;
    [SerializeField] private GridLayoutGroup _parent;

    private List<LevelView> _levelViews = new List<LevelView>();

    private bool _tutorialCompleted;

    private void OnDisable()
    {
        foreach (var levelView in _levelViews)
            levelView.OpenLevelButtonClicked -= OnOpenLevelButtonClicked;
    }

    public void Read(PlayerData playerData) => _tutorialCompleted = playerData.IsTutorialCompleted;

    public void Initialize()
    {
        foreach (var level in _levels)
        {
            LevelView levelView = Instantiate(_levelViewTemplate, _parent.transform);
            levelView.Render(level);
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
        /*if (level.IsOpen)*/
            SceneManager.LoadScene(level.Name);
    }
}
