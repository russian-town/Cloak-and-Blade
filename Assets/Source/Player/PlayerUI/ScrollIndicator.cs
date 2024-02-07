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

    private HorizontalLayoutGroup _layout;
    private float _currentScrollbarPosition;
    private float[] _positions;
    private bool _isScrolling;
    private bool _isInitialized;
    private float _time;

    private readonly int _buttonIndex;
    private readonly List<Knob> _knobs = new List<Knob>();
    private readonly List<LevelView> _levelViews = new List<LevelView>();

    private void Update()
    {
        if (_isInitialized == false)
            return;

        _positions = new float[transform.childCount];
        float distance = 1f / (_positions.Length - 1f);

        if (_isScrolling)
        {
            GecisiDuzenle(distance, _positions);
            _time += Time.deltaTime;

            if (_time > 1f)
            {
                _time = 0;
                _isScrolling = false;
            }
        }

        for (int i = 0; i < _positions.Length; i++)
            _positions[i] = distance * i;

        if (Input.GetMouseButton(0))
            _currentScrollbarPosition = _scrollbar.value;
        else
            for (int i = 0; i < _positions.Length; i++)
                if (_currentScrollbarPosition < _positions[i] + (distance / 2) && _currentScrollbarPosition > _positions[i] - (distance / 2))
                    _scrollbar.value = Mathf.Lerp(_scrollbar.value, _positions[i], 0.1f);

        for (int i = 0; i < _positions.Length; i++)
        {
            if (_currentScrollbarPosition < _positions[i] + (distance / 2) && _currentScrollbarPosition > _positions[i] - (distance / 2))
            {
                _levelViews[i].Focus();
                _knobs[i].Focuse();

                for (int j = 0; j < _positions.Length; j++)
                {
                    if (j != i)
                    {
                        _knobs[j].Unfocuse();
                        _levelViews[j].Unfocus();
                    }
                }
            }
        }
    }

    public void Initialize(List<LevelView> levelViews, List<Knob> knobs)
    {
        _layout = GetComponent<HorizontalLayoutGroup>();
        _levelViews.AddRange(levelViews);
        _layout.padding.left = (_viewPort.rect.width / 2).ToInt() - _contentWidth;
        _layout.padding.right = _layout.padding.left;
        _knobs.AddRange(knobs);
        _isInitialized = true;
    }

    public void WhichButtonClicked(Knob knob)
    {
        if(_knobs.Contains(knob))
        {
            int index = _knobs.IndexOf(knob);
            _time = 0;
            _currentScrollbarPosition = _positions[index];
            _isScrolling = true;
        }
    }

    private void GecisiDuzenle(float distance, float[] positions)
    {
        for (int i = 0; i < positions.Length; i++)
            if (_currentScrollbarPosition < positions[i] + (distance / 2) && _currentScrollbarPosition > positions[i] - (distance / 2))
                _scrollbar.value = Mathf.Lerp(_scrollbar.value, positions[_buttonIndex], 1f * Time.deltaTime);
    }
}
