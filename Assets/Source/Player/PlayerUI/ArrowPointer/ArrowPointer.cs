using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(RequireComponent))]
public class ArrowPointer : MonoBehaviour
{
    [SerializeField] private Transform _target;

    private RectTransform _rectTransform;
    private Camera _camera;

    private void Awake()
    {
        _rectTransform = GetComponent<RectTransform>();
        _camera = Camera.main;
    }

    public void Update()
    {
        Vector3 to = _target.position;
        Vector3 from = _camera.transform.position;
        from.z = 0;
        Vector3 direction = (to - from).normalized;
        float angle = Mathf.Atan2(direction.x, direction.y) * Mathf.Rad2Deg;
        _rectTransform.localEulerAngles = new Vector3(0f, 0f, angle);
    }
}
