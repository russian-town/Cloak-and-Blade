using UnityEngine;

[RequireComponent(typeof(RectTransform))]
public class ArrowPointer : MonoBehaviour
{
    [SerializeField] private Transform _target;
    [SerializeField] private RectTransform _canvas;
    [SerializeField] private Vector2 _offSet;

    private RectTransform _rectTransform;
    private Vector3 _halfScale;
    private Camera _camera;
    private Player _player;
    private bool _isInitialize;

    public void Initialize(Player player)
    {
        _rectTransform = GetComponent<RectTransform>();
        _camera = Camera.main;
        _player = player;
        _halfScale = _rectTransform.sizeDelta / 2f;
        _isInitialize = true;
    }

    public void Update()
    {
        if (_isInitialize == false)
            return;

        Vector3 to = _target.position;
        Vector3 from = _player.transform.position;
        from.y = 0f;
        to.y = 0f;
        Vector3 direction = (to - from).normalized;
        float angle = Mathf.Atan2(direction.z, direction.x) * Mathf.Rad2Deg % 360f;
        _rectTransform.localEulerAngles = new Vector3(0f, 0f, angle);
        Vector3 screenPoint = _camera.WorldToScreenPoint(_target.transform.position);

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvas, screenPoint, null, out Vector2 localPoint))
        {
            localPoint.x = Mathf.Clamp(localPoint.x, -Screen.width / 2f + _halfScale.x, Screen.width / 2f - _halfScale.x);
            localPoint.y = Mathf.Clamp(localPoint.y, -Screen.height / 2f + _halfScale.y, Screen.height / 2f - _halfScale.y);
            Vector3 targetPositon = Vector3.zero;

            if (localPoint.y < 0f && localPoint.x < 0f)
                targetPositon = new Vector2(localPoint.x + _halfScale.x + _offSet.x, localPoint.y + _halfScale.y + _offSet.y);
            else if(localPoint.y > 0f && localPoint.x > 0f)
                targetPositon = new Vector2(localPoint.x - _halfScale.x - _offSet.x, localPoint.y - _halfScale.y - _offSet.y);
            else if(localPoint.y < 0f && localPoint.x > 0f)
                targetPositon = new Vector2(localPoint.x - _halfScale.x - _offSet.x, localPoint.y + _halfScale.y + _offSet.y);
            else if (localPoint.y > 0f && localPoint.x < 0f)
                targetPositon = new Vector2(localPoint.x + _halfScale.x + _offSet.x, localPoint.y - _halfScale.y - _offSet.y);

            _rectTransform.anchoredPosition = targetPositon;
        }
    }
}
