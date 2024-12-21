using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalView: MonoBehaviour {
    public portalscript[] holes;
    Camera mycam;
    void Start() {
        mycam = this.GetComponent<Camera>();
    }

    // Update is called once per frame
    void OnPreRender() {
        Plane[] viewport = GeometryUtility.CalculateFrustumPlanes(mycam);
        foreach(portalscript hole in holes) {
            hole.SetRender(transform, viewport, 0);
        }
    }
}
