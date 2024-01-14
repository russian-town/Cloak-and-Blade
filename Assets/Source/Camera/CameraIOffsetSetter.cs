using Cinemachine;
using UnityEngine;

public class CameraIOffsetSetter : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _camera;

    private CinemachineOrbitalTransposer _cameraOrbitalTransposer;

    private void Awake()
    {
        _cameraOrbitalTransposer = _camera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
        _cameraOrbitalTransposer.m_FollowOffset.z = 7.5f;

    }

/*    private void Update()
    {
    }*/
}
