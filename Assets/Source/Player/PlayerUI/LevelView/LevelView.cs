using System;
using System.Collections.Generic;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Player.PlayerUI.LevelView
{
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

        private Level.Level _level;
        private bool _isFocused;
        private bool _isUnFocused;
        private UnityEngine.Camera _camera;

        public event Action<Level.Level> OpenLevelButtonClicked;

        private void OnEnable()
        {
            _openLevel.onClick.AddListener(() => OpenLevelButtonClicked?.Invoke(_level));
        }

        private void OnDisable()
        {
            _openLevel.onClick.RemoveListener(() => OpenLevelButtonClicked?.Invoke(_level));
        }

        public void Render(Level.Level level)
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
            if (_isFocused)
                return;

            transform.DOScale(_focusScale, _changeFocusScale);
            _preview.DOColor(_focusedColor, .6f).SetEase(Ease.InOutSine);
            _isUnFocused = false;
            _isFocused = true;
        }

        public void Unfocus()
        {
            if (_isUnFocused)
                return;

            transform.DOScale(_unfocusScale, _changeFocusScale);
            _preview.DOColor(_unFocusedColor, .6f).SetEase(Ease.InOutQuad);
            _isFocused = false;
            _isUnFocused = true;
        }

        public bool IsPointInsideImage(Vector2 screenPos)
        {
            if (_preview != null)
            {
                RectTransform rectTransform = _preview.rectTransform;
                Vector2 localPoint;

                if (RectTransformUtility.ScreenPointToLocalPointInRectangle(rectTransform, screenPos, UnityEngine.Camera.main, out localPoint))
                    return rectTransform.rect.Contains(localPoint);
            }

            return false;
        }
    }
}
