using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class ArrowPointer : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private RectTransform _canvas;

    private RectTransform _rectTransform;
    private Camera _camera;
    private Player _player;
    private bool _isInitialize;

    private void Awake()
    {
        Initialize();
    }

    public void Initialize(/*Player player*/)
    {
        _rectTransform = GetComponent<RectTransform>();
        _camera = Camera.main;
       /* _player = player;*/
        _isInitialize = true;
    }

    public void Update()
    {
        if (_isInitialize == false)
            return;

        /*Vector3 to = _target.position;
        Vector3 from = _player.transform.position;
        Vector3 direction = (to - from).normalized;
        float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg % 360f;
        _rectTransform.localEulerAngles = new Vector3(0f, 0f, angle);*/
        Vector3 screenPoint = _camera.WorldToScreenPoint(new Vector3(_target.position.x, 0f, _target.position.z));

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas, screenPoint, null, out Vector2 localPoint))
        {
            localPoint.x = Mathf.Clamp(localPoint.x, -Screen.width / 2f, Screen.width / 2f);
            localPoint.y = Mathf.Clamp(localPoint.y, -Screen.height / 2f, Screen.height / 2f);
            _rectTransform.anchoredPosition = localPoint;
        }
    }
}
