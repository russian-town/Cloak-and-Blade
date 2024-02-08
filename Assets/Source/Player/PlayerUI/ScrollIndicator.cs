using System.Collections.Generic;
using Tayx.Graphy.Utils.NumString;
using UnityEngine;
using UnityEngine.Analytics;
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
    private float[] _positions;
    private bool _isScrolling;
    private bool _isInitialized;
    private float _time;
    private float _distance;

    private readonly List<Knob> _knobs = new List<Knob>();
    private readonly List<LevelView> _levelViews = new List<LevelView>();

    private void Update()
    {
        if (_isInitialized == false)
            return;

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
        _scrollbar.onValueChanged.AddListener(OnScrollbarValueChanged);

        for (int i = 0; i < _positions.Length; i++)
            _positions[i] = _distance * i;
    }

    public void KnobClicked(Knob knob)
    {
        if(_knobs.Contains(knob))
        {
            _currentPositionIndex = _knobs.IndexOf(knob);
            _time = 0;
            _isScrolling = true;
        }
    }
    
    public void OnScrollbarValueChanged(float value)
    {

        for (int i = 0; i < _positions.Length; i++)
        {
            if (_scrollbar.value < _positions[i] + (_distance / 2) && _scrollbar.value > _positions[i] - (_distance / 2))
            {
                _levelViews[i].Focus();
                _knobs[i].Focus();
            }
            else
            {
                _levelViews[i].Unfocus();
                _knobs[i].Unfocus();
            }
        }
    }

    private void DragViewToClosestPosition()
    {
        for (int i = 0; i < _positions.Length; i++)
            if (_scrollbar.value < _positions[i] + (_distance / 2) && _scrollbar.value > _positions[i] - (_distance / 2))
                _scrollbar.value = Mathf.Lerp(_scrollbar.value, _positions[i], _scrollSpeed * Time.deltaTime);
    }

    
}
