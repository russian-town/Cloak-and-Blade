using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Compass : MonoBehaviour
{
    [SerializeField] private RectTransform _compassTransform;
    [SerializeField] private RectTransform _keyIconRectTransform;
    [SerializeField] private RectTransform _chestIconRectTransform;
    [SerializeField] private RectTransform _exitIconRectTransform;
    [SerializeField] private Transform _keyTransform;
    [SerializeField] private Transform _chestTransform;
    [SerializeField] private Transform _exitTransform;
    [SerializeField] private Transform _cameraObjectiveTransform;

    private void Update()
    {
        SetMarkerPosition(_keyIconRectTransform, _keyTransform.position);
    }

    private void SetMarkerPosition(RectTransform markerTransform, Vector3 worldPosition)
    {
        Vector3 directionToTarget = worldPosition - _cameraObjectiveTransform.position;
        float angle = Vector2.SignedAngle(new Vector2(directionToTarget.x, directionToTarget.z), new Vector2(_cameraObjectiveTransform.transform.forward.x, _cameraObjectiveTransform.transform.forward.z));
        float compassPositionX = Mathf.Clamp(2 * angle / Camera.main.fieldOfView, -1, 1);
        markerTransform.anchoredPosition = new Vector2(_compassTransform.rect.width / 2 * compassPositionX, 0);
    }
}
