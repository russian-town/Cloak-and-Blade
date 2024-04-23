using Agava.WebUtility;
using Cinemachine;
using UnityEngine;
using UnityEngine.Events;

public class CameraControls : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _angledCamera;
    [SerializeField] private CinemachineVirtualCamera _straightCamera;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private float _angledCameraValue;
    [SerializeField] private int _mobileFOV = 40;
    [SerializeField] private int _desktopFOV = 60;

    private readonly int _maxRotation = 180;
    private readonly int _minRotation = -180;

    private CinemachineOrbitalTransposer _angledCameraOrbitalTransposer;
    private bool _cameraIsStraight;

    public event UnityAction AngleChanged;

    private void Awake()
    {
        _angledCameraOrbitalTransposer =
            _angledCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
    }

    private void Start()
    {
        _angledCamera.Priority = 1;
        _straightCamera.Priority = 0;
        _angledCameraOrbitalTransposer.m_FollowOffset.z = _angledCameraValue;

#if UNITY_WEBGL && !UNITY_EDITOR
        if (Device.IsMobile)
            _angledCamera.m_Lens.FieldOfView = _mobileFOV;
        else
            _angledCamera.m_Lens.FieldOfView = _desktopFOV;
#endif

        _cameraIsStraight = false;
    }

    public void ChangeCameraAngle()
    {
        AngleChanged?.Invoke();

        if (!_cameraIsStraight)
        {
            _angledCamera.Priority = 0;
            _straightCamera.Priority = 1;
            _cameraIsStraight = true;
        }
        else
        {
            _straightCamera.Priority = 0;
            _angledCamera.Priority = 1;
            _cameraIsStraight = false;
        }
    }

    public void TurnClockwise()
    {
        if (_angledCameraOrbitalTransposer.m_Heading.m_Bias >= _maxRotation)
            _angledCameraOrbitalTransposer.m_Heading.m_Bias = _minRotation;

        _angledCameraOrbitalTransposer.m_Heading.m_Bias += _rotationSpeed * Time.deltaTime;
    }

    public void TurnCounterClockwise()
    {
        if (_angledCameraOrbitalTransposer.m_Heading.m_Bias <= _minRotation)
            _angledCameraOrbitalTransposer.m_Heading.m_Bias = _maxRotation;

        _angledCameraOrbitalTransposer.m_Heading.m_Bias -= _rotationSpeed * Time.deltaTime;
    }
}
