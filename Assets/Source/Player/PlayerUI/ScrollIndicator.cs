using System.Collections;
using System.Collections.Generic;
using Source.LevelLoader.Knob;
using Tayx.Graphy.Utils.NumString;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Player.PlayerUI
{
    [RequireComponent(typeof(HorizontalLayoutGroup))]
    public class ScrollIndicator : MonoBehaviour
    {
        private readonly float _startDelay = .6f;
        private readonly int _defaultDivider = 2;
        private readonly List<Knob> _knobs = new ();
        private readonly List<LevelView.LevelView> _levelViews = new ();

        [SerializeField] private Scrollbar _scrollbar;
        [SerializeField] private RectTransform _viewPort;
        [SerializeField] private int _contentWidth;
        [SerializeField] private float _scrollSpeed;
        [SerializeField] private float _stopScrollTimer;
        [SerializeField] private float _focusThreshHold;

        private HorizontalLayoutGroup _layout;
        private int _currentPositionIndex;
        private int _lastOpenedLevelIndex;
        private float[] _positions;
        private bool _isScrolling;
        private bool _isInitialized;
        private float _time;
        private float _distance;
        private float _previousScrollbarPosition;
        private Vector2 _screenPos;
        private LevelView.LevelView _lastFocusedLevelView;
        private Knob _lastFocusedKnob;
        private WaitForSeconds _startDelayWaitForSeconds;

        private void Update()
        {
            if (_isInitialized == false)
                return;

            _screenPos = new Vector2(Screen.width / _defaultDivider, Screen.height / _defaultDivider);

            if (_isScrolling)
            {
                _time += Time.deltaTime;
                _scrollbar.value = Mathf.Lerp(_scrollbar.value, _positions[_currentPositionIndex], _scrollSpeed * Time.deltaTime);

                if (_time > _stopScrollTimer)
                {
                    _time = 0;
                    _isScrolling = false;
                }
            }

            if (_isScrolling)
                return;

            if (!Input.GetMouseButton(0))
                DragViewToClosestPosition();
        }

        private void OnDisable()
        {
            _scrollbar.onValueChanged.RemoveListener(OnScrollbarValueChanged);
        }

        public void Initialize(List<LevelView.LevelView> levelViews, List<Knob> knobs)
        {
            _layout = GetComponent<HorizontalLayoutGroup>();
            _levelViews.AddRange(levelViews);
            _layout.padding.left = (_viewPort.rect.width / _defaultDivider).ToInt() - _contentWidth;
            _layout.padding.right = _layout.padding.left;
            _knobs.AddRange(knobs);
            _isInitialized = true;
            _positions = new float[transform.childCount];
            _distance = 1f / (_positions.Length - 1);

            for (int i = 0; i < _positions.Length; i++)
                _positions[i] = _distance * i;

            for (int i = 0; i < _positions.Length; i++)
            {
                _levelViews[i].Unfocus();
                _knobs[i].Unfocus();
            }

            _startDelayWaitForSeconds = new WaitForSeconds(_startDelay);
            StartCoroutine(WaitToSetScrollbar());
            _scrollbar.onValueChanged.AddListener(OnScrollbarValueChanged);
        }

        public void KnobClicked(Knob knob)
        {
            if (_knobs.Contains(knob))
            {
                _currentPositionIndex = _knobs.IndexOf(knob);
                _time = 0;
                _isScrolling = true;
            }
        }

        public void OnScrollbarValueChanged(float value)
        {
            if (Mathf.Abs(_scrollbar.value - _previousScrollbarPosition) < _focusThreshHold)
                return;

            _previousScrollbarPosition = _scrollbar.value;

            for (int i = 0; i < _positions.Length; i++)
            {
                if (_levelViews[i].IsPointInsideImage(_screenPos))
                {
                    SetLastFocusedObject(i);
                }
            }
        }

        public void SetLastOpenedLevelIndex(int index) => _lastOpenedLevelIndex = index;

        private void DragViewToClosestPosition()
        {
            for (int i = 0; i < _positions.Length; i++)
            {
                if (_scrollbar.value < _positions[i] + (_distance / _defaultDivider) && _scrollbar.value > _positions[i] - (_distance / _defaultDivider))
                {
                    _scrollbar.value = Mathf.Lerp(_scrollbar.value, _positions[i], _scrollSpeed * Time.deltaTime);
                }
            }
        }

        private void SetLastFocusedObject(int index)
        {
            if (_lastFocusedLevelView != null && _lastFocusedKnob != null)
            {
                if (_lastFocusedLevelView != _levelViews[index] && _lastFocusedKnob != _knobs[index])
                {
                    _lastFocusedLevelView.Unfocus();
                    _lastFocusedKnob.Unfocus();
                }
            }

            _lastFocusedLevelView = _levelViews[index];
            _lastFocusedKnob = _knobs[index];
            _lastFocusedLevelView.Focus();
            _lastFocusedKnob.Focus();
        }

        private IEnumerator WaitToSetScrollbar()
        {
            yield return _startDelayWaitForSeconds;
            SetLastFocusedObject(_lastOpenedLevelIndex);
            _scrollbar.value = _positions[_lastOpenedLevelIndex];
        }
    }
}
