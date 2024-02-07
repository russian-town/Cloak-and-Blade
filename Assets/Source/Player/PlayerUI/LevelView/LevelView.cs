using DG.Tweening;
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
    [SerializeField] private float _focusScale;
    [SerializeField] private float _unfocusScale;
    [SerializeField] private float _changeFocusScale;
    [SerializeField] private Color _focusedColor;
    [SerializeField] private Color _unFocusedColor;

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
        string levelName = Lean.Localization.LeanLocalization.GetTranslationText(level.Name);
        _levelName.text = levelName;

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

    public void Focus()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(_focusScale, _focusScale, 1), _changeFocusScale);
        _preview.DOColor(_focusedColor, .1f).SetEase(Ease.InSine);
    }

    public void Unfocus()
    {
        transform.localScale = Vector3.Lerp(transform.localScale, new Vector3(_unfocusScale, _unfocusScale, 1), _changeFocusScale);
        _preview.DOColor(_unFocusedColor, .1f).SetEase(Ease.InOutQuad);
    }
}
