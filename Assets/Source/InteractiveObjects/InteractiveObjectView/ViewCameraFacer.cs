using UnityEngine;

public class ViewCameraFacer : MonoBehaviour
{
    private readonly int _lookAtMultiplier = 2;

    [SerializeField] private Transform _lookAt;

    private void Update()
    {
        if (_lookAt)
            transform.LookAt(transform.position * _lookAtMultiplier - _lookAt.position);
    }
}
