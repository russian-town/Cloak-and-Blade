using Cinemachine;
using UnityEngine;

namespace Source.Camera
{
    public class CameraOffsetSetter : MonoBehaviour
    {
        [SerializeField] private CinemachineVirtualCamera _camera;

        private CinemachineOrbitalTransposer _cameraOrbitalTransposer;
        private float _cameraFollowOffset = 7.5f;

        private void Awake()
        {
            _cameraOrbitalTransposer =
                _camera.GetCinemachineComponent<CinemachineOrbitalTransposer>();
            _cameraOrbitalTransposer.m_FollowOffset.z = _cameraFollowOffset;
        }
    }
}
