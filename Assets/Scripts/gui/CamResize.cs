using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamResize: MonoBehaviour {
    public float scale;
    public Camera cam;
    void Update() {
        cam.orthographicSize = Mathf.Max(scale, 16 * Screen.height * scale / (9 * Screen.width));
    }
}
