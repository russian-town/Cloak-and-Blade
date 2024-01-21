using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemoRainbowObject : MonoBehaviour {
    public Vector3 eulerSpeed = Vector3.zero;

    // Update is called once per frame
    void Update() {
        transform.GetComponent<MeshRenderer>().material.SetColor("_Color", Color.HSVToRGB((Time.realtimeSinceStartup * 0.1f) % 1, 1, 1));
    }
}
