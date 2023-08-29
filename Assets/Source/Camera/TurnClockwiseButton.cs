using UnityEngine;
using UnityEngine.EventSystems;

public class TurnClockwiseButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    [SerializeField] private CameraControls _controls;
    private bool _isRotating;

    private void Update()
    {
        if (_isRotating)
            _controls.TurnClockwise();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        _isRotating = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        _isRotating = false;
    }
}
