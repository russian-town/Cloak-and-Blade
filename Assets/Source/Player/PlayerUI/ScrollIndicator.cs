using System.Collections.Generic;
using Tayx.Graphy.Utils.NumString;
using UnityEngine;
using UnityEngine.UI;

public class ScrollIndicator : MonoBehaviour
{
    /*public Color[] colors;
    public GameObject scrollbar, imageContent;
    private float scroll_pos = 0;
    float[] _positions;
    private bool runIt = false;
    private float time;
    private Button takeTheBtn;
    int btnNumber;*/

    [SerializeField] private Scrollbar _scrollbar;
    [SerializeField] private Transform _navigationButtons;
    [SerializeField] private Transform _content;
    [SerializeField] private RectTransform _viewPort;
    [SerializeField] private HorizontalLayoutGroup _layout;
    [SerializeField] private Color _defaultButtonColor;
    [SerializeField] private Color _focusedButtonColor;
    [SerializeField] private int _contentWidth;
    [SerializeField] private float _focusSpeed;

    private float _currentScrollbarPosition;
    private float[] _positions;
    private bool _isScrolling;
    private Button _pressedButton;
    private int _buttonIndex;
    private float _time;

    private void Start()
    {
        print(_viewPort.rect.width.ToInt() / 2);
        _layout.padding.left = (_viewPort.rect.width / 2).ToInt() - _contentWidth;
        _layout.padding.right = (_viewPort.rect.width/ 2).ToInt() - _contentWidth;
    }

    void Update()
    {
        _positions = new float[transform.childCount];
        float distance = 1f / (_positions.Length - 1f);

        if (_isScrolling)
        {
            GecisiDuzenle(distance, _positions, _pressedButton);
            _time += Time.deltaTime;

            if (_time > 1f)
            {
                _time = 0;
                _isScrolling = false;
            }
        }

        for (int i = 0; i < _positions.Length; i++)
        {
            _positions[i] = distance * i;
        }

        if (Input.GetMouseButton(0))
        {
            _currentScrollbarPosition = _scrollbar.value;
        }

        else
        {
            for (int i = 0; i < _positions.Length; i++)
            {
                if (_currentScrollbarPosition < _positions[i] + (distance / 2) && _currentScrollbarPosition > _positions[i] - (distance / 2))
                {
                    _scrollbar.value = Mathf.Lerp(_scrollbar.value, _positions[i], 0.1f);
                }
            }
        }

        for (int i = 0; i < _positions.Length; i++)
        {
            if (_currentScrollbarPosition < _positions[i] + (distance / 2) && _currentScrollbarPosition > _positions[i] - (distance / 2))
            {
                Debug.LogWarning("Current Selected Level" + i);
                transform.GetChild(i).localScale = Vector2.Lerp(transform.GetChild(i).localScale, new Vector2(1f, 1f), 0.1f);
                _navigationButtons.GetChild(i).localScale = Vector2.Lerp(_navigationButtons.GetChild(i).localScale, new Vector2(1.2f, 1.2f), 0.1f);
                _navigationButtons.GetChild(i).GetComponent<Image>().color = _focusedButtonColor;
                for (int j = 0; j < _positions.Length; j++)
                {
                    if (j != i)
                    {
                        _navigationButtons.GetChild(j).GetComponent<Image>().color = _defaultButtonColor;
                        _navigationButtons.GetChild(j).localScale = Vector2.Lerp(_navigationButtons.GetChild(j).localScale, new Vector2(0.8f, 0.8f), 0.1f);
                        transform.GetChild(j).localScale = Vector2.Lerp(transform.GetChild(j).localScale, new Vector2(0.8f, 0.8f), 0.1f);
                    }
                }
            }
        }
    }

    private void GecisiDuzenle(float distance, float[] pos, Button btn)
    {
        for (int i = 0; i < pos.Length; i++)
        {
            if (_currentScrollbarPosition < pos[i] + (distance / 2) && _currentScrollbarPosition > pos[i] - (distance / 2))
            {
                _scrollbar.value = Mathf.Lerp(_scrollbar.value, pos[_buttonIndex], 1f * Time.deltaTime);
            }
        }

        for (int i = 0; i < btn.transform.parent.transform.childCount; i++)
        {
            btn.transform.name = ".";
        }

    }
    public void WhichBtnClicked(Button btn)
    {
        btn.transform.name = "clicked";
        for (int i = 0; i < btn.transform.parent.transform.childCount; i++)
        {
            if (btn.transform.parent.transform.GetChild(i).transform.name == "clicked")
            {
                _buttonIndex = i;
                _pressedButton = btn;
                _time = 0;
                _currentScrollbarPosition = (_positions[_buttonIndex]);
                _isScrolling = true;
            }
        }
    }
    /*[SerializeField] private Scrollbar _scrollbar;

    private void OnEnable()
    {
        _scrollbar.onValueChanged.AddListener(OnScrollbarValueChanged);
    }

    private void OnDisable()
    {
        _scrollbar.onValueChanged.RemoveListener(OnScrollbarValueChanged);
    }*/
}
