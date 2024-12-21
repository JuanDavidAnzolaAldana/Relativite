using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portalClone: MonoBehaviour {
    public Transform clone;
    public Transform[] portals, partners;
    float dist;
    MeshRenderer render;
    void Start() {
        render = this.GetComponent<MeshRenderer>();
    }
    void Update() {
        if(clone) {
            dist = 2;
            int a = -1;
            for(int i = 0; i < portals.Length; i++) {
                if((clone.position - portals[i].position).magnitude < dist) {
                    dist = (clone.position - portals[i].position).magnitude;
                    a = i;
                }
            }
            if(a == -1) {
                render.enabled = false;
            } else {
                render.enabled = true;
                Quaternion euler = partners[a].rotation * new Quaternion(0, 1, 0, 0) * Quaternion.Inverse(portals[a].rotation);
                transform.SetPositionAndRotation(partners[a].position + (euler * (clone.position - portals[a].position)), euler * clone.rotation);
            }
        } else {
            render.enabled = false;
        }
    }
}
