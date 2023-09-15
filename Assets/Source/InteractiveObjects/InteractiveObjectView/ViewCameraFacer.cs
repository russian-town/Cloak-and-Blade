using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ViewCameraFacer : MonoBehaviour
{
    [SerializeField] private Transform _lookAt;

    private void Update()
    {
        if (_lookAt)
            transform.LookAt(2 * transform.position - _lookAt.position);
    }
}
