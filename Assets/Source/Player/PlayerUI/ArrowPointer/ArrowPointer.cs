using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class ArrowPointer : MonoBehaviour
{
    private readonly float _standardDivider = 2f;

    [SerializeField] private Transform _target;
    [SerializeField] private RectTransform _canvas;

    private RectTransform _rectTransform;
    private Camera _camera;
    private bool _isInitialize;

    private void Awake()
    {
        Initialize();
    }

    public void Initialize()
    {
        _rectTransform = GetComponent<RectTransform>();
        _camera = Camera.main;
        _isInitialize = true;
    }

    public void Update()
    {
        if (_isInitialize == false)
            return;

        Vector3 screenPoint = _camera.WorldToScreenPoint(new Vector3(_target.position.x, 0f, _target.position.z));

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas, screenPoint, null, out Vector2 localPoint))
        {
            localPoint.x = Mathf.Clamp(localPoint.x, -Screen.width / _standardDivider, Screen.width / _standardDivider);
            localPoint.y = Mathf.Clamp(localPoint.y, -Screen.height / _standardDivider, Screen.height / _standardDivider);
            _rectTransform.anchoredPosition = localPoint;
        }
    }
}
