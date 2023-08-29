using UnityEngine;
using UnityEngine.UI;

public class CameraAngleChanger : MonoBehaviour
{
    [SerializeField] private CameraControls _controls;
    [SerializeField] private Button _button;

    private void OnEnable()
    {
        _button.onClick?.AddListener(_controls.ChangeCameraAngle);
    }

    private void OnDisable()
    {
        _button.onClick?.RemoveListener(_controls.ChangeCameraAngle);
    }
}
