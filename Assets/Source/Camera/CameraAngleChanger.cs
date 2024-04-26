using Source.Tutorial.UI;
using UnityEngine;
using UnityEngine.UI;

namespace Source.Camera
{
    public class CameraAngleChanger : MainButton
    {
        [SerializeField] private CameraControls _controls;
        [SerializeField] private Button _changeAngleButton;

        protected Button ChangeAngleButton => _changeAngleButton;

        protected CameraControls Controls => _controls;

        private void OnEnable()
        {
            _changeAngleButton.onClick?.AddListener(_controls.ChangeCameraAngle);
        }

        private void OnDisable()
        {
            _changeAngleButton.onClick?.RemoveListener(_controls.ChangeCameraAngle);
        }
    }
}
