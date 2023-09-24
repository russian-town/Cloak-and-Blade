using Cinemachine;
using UnityEngine;

public class CameraControls : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _angledCamera;
    [SerializeField] private CinemachineVirtualCamera _straightCamera;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private int _angledCameraValue;

    private CinemachineOrbitalTransposer _angledCameraOrbitalTransposer;
    private int _minRotation = -180;
    private int _maxRotation = 180;
    private bool _cameraIsStraight;

    private void Awake()
    {
        _angledCameraOrbitalTransposer = _angledCamera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
    }

    private void Start()
    {
        _angledCamera.Priority = 1;
        _straightCamera.Priority = 0;
        _angledCameraOrbitalTransposer.m_FollowOffset.z = _angledCameraValue;
        _cameraIsStraight = false;
    }

    public void ChangeCameraAngle()
    {
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
