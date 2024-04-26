using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

[RequireComponent(typeof(CanvasGroup))]
public class Compass : MonoBehaviour
{
    private readonly int _defaultDivider = 2;

    [SerializeField] private RectTransform _compassTransform;
    [SerializeField] private List<RectTransform> _compassTargetIcons = new ();
    [SerializeField] private List<Transform> _compassTargetTransform = new ();
    [SerializeField] private Transform _cameraObjectiveTransform;
    [SerializeField] private Key _key;
    [SerializeField] private Treasure _treasure;
    [SerializeField] private DefaultLevelExit _levelExit;
    [SerializeField] private CameraControls _cameraControls;
    [SerializeField] private float _threshHold;
    [SerializeField] private float _fadeDuration;

    private List<ICompassTarget> _compassTargets;
    private int _currentTargetIndex = 0;
    private bool _cameraIsAngled = true;
    private CanvasGroup _canvasGroup;

    private void Start()
    {
        _compassTargets = new List<ICompassTarget> { _key, _treasure, _levelExit };
        _canvasGroup = GetComponent<CanvasGroup>();

        foreach (var compassTarget in _compassTargets)
            compassTarget.Disabled += OnTargetDisabled;
    }

    private void OnEnable()
    {
        _cameraControls.AngleChanged += OnCameraAngleChanged;
    }

    private void OnDisable()
    {
        _cameraControls.AngleChanged -= OnCameraAngleChanged;

        foreach (var compassTarget in _compassTargets)
            compassTarget.Disabled -= OnTargetDisabled;
    }

    private void Update()
    {
        if (_currentTargetIndex > _compassTargets.Count || _compassTargetIcons[_currentTargetIndex] == null
            || _compassTargetTransform[_currentTargetIndex] == null)
            return;

        SetMarkerPosition(_compassTargetIcons[_currentTargetIndex], _compassTargetTransform[_currentTargetIndex].position);
    }

    private void OnTargetDisabled()
    {
        if (_currentTargetIndex == _compassTargets.Count)
            return;

        _compassTargetIcons[_currentTargetIndex].gameObject.SetActive(false);
        _currentTargetIndex++;
        _compassTargetIcons[_currentTargetIndex].gameObject.SetActive(true);
    }

    private void SetMarkerPosition(RectTransform markerTransform, Vector3 worldPosition)
    {
        Vector3 directionToTarget = worldPosition - _cameraObjectiveTransform.position;
        float angle = Vector2.SignedAngle(
            new Vector2(directionToTarget.x, directionToTarget.z),
            new Vector2(
                _cameraObjectiveTransform.transform.forward.x,
            _cameraObjectiveTransform.transform.forward.z));
        float compassPositionX = Mathf.Clamp(_defaultDivider * angle / Camera.main.fieldOfView, -1, 1);
        float compassPositionMultiplier = (_compassTransform.rect.width / _defaultDivider) - _threshHold;
        markerTransform.anchoredPosition = new Vector2(compassPositionMultiplier * compassPositionX, 0);
    }

    private void OnCameraAngleChanged()
    {
        if (_cameraIsAngled)
        {
            _canvasGroup.DOFade(0, _fadeDuration);
            _cameraIsAngled = false;
        }
        else
        {
            _canvasGroup.DOFade(1, _fadeDuration);
            _cameraIsAngled = true;
        }
    }
}
