using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LevelView : MonoBehaviour
{
    [SerializeField] private Button _openLevel;
    [SerializeField] private TextMeshProUGUI _levelName;
    [SerializeField] private Image _preview;
    [SerializeField] private List<Image> _stars = new List<Image>();
    [SerializeField] private Image _lock;

    private Level _level;

    public event Action<Level> OpenLevelButtonClicked;

    private void OnEnable()
    {
        _openLevel.onClick.AddListener(() => OpenLevelButtonClicked?.Invoke(_level));
    }

    private void OnDisable()
    {
        _openLevel.onClick.RemoveListener(() => OpenLevelButtonClicked?.Invoke(_level));
    }

    public void Render(Level level)
    {
        _level = level;
        _levelName.text = level.name;

        if (_level.IsOpen)
            _preview.sprite = _level.UnlockedPreview;
        else
            _preview.sprite = _level.LockedPreview;

        _lock.gameObject.SetActive(!_level.IsOpen);

        if (_level.IsCompleted == false)
            return;

        for (int i = 0; i < _level.StarsCount; i++)
            _stars[i].gameObject.SetActive(true);
    }
}
