using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class ArrowPointer : MonoBehaviour
{
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
            localPoint.x = Mathf.Clamp(localPoint.x, -Screen.width / 2f, Screen.width / 2f);
            localPoint.y = Mathf.Clamp(localPoint.y, -Screen.height / 2f, Screen.height / 2f);
            _rectTransform.anchoredPosition = localPoint;
        }
    }
}
