using System.Collections;
using System.Collections.Generic;
using Tayx.Graphy.Utils.NumString;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(HorizontalLayoutGroup))]
public class ScrollIndicator : MonoBehaviour
{
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
    private LevelView _lastFocusedLevelView;
    private Knob _lastFocusedKnob;

    private readonly List<Knob> _knobs = new List<Knob>();
    private readonly List<LevelView> _levelViews = new List<LevelView>();

    private void Update()
    {
        if (_isInitialized == false)
            return;

        _screenPos = new Vector2(Screen.width / 2, Screen.height / 2);

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

    public void Initialize(List<LevelView> levelViews, List<Knob> knobs)
    {
        _layout = GetComponent<HorizontalLayoutGroup>();
        _levelViews.AddRange(levelViews);
        _layout.padding.left = (_viewPort.rect.width / 2).ToInt() - _contentWidth;
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
            if (_levelViews[i].IsPointInsideImage(_screenPos))
                SetLastFocusedObject(i);
    }

    public void SetLastOpenedLevelIndex(int index) => _lastOpenedLevelIndex = index;

    private void DragViewToClosestPosition()
    {
        for (int i = 0; i < _positions.Length; i++)
            if (_scrollbar.value < _positions[i] + (_distance / 2) && _scrollbar.value > _positions[i] - (_distance / 2))
                _scrollbar.value = Mathf.Lerp(_scrollbar.value, _positions[i], _scrollSpeed * Time.deltaTime);
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
        yield return new WaitForSeconds(.6f);
        SetLastFocusedObject(_lastOpenedLevelIndex);
        _scrollbar.value = _positions[_lastOpenedLevelIndex];
    }
}
