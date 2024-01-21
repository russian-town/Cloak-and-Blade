using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoRotatingObject : MonoBehaviour {
    public Vector3 eulerSpeed = Vector3.zero;

    // Update is called once per frame
    void Update() {
        transform.Rotate(eulerSpeed * Time.deltaTime * 180);
    }
}
