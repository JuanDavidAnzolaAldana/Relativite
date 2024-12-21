using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoidScript: MonoBehaviour {
    public portalscript portal1, portal2;
    public float amplitude = 10, frecuence = 1, offset = 50;
    float inpos1, inpos2;
    void Start() {
        inpos1 = Vector3.Dot(portal1.transform.position - transform.position, transform.forward);
        inpos2 = Vector3.Dot(portal2.transform.position - transform.position, transform.forward);
    }
    void OnTriggerExit(Collider other) {
        if(other.TryGetComponent(out PseudoGravity pse)) {
            pse.gravi = true;
        }
    }
    void OnTriggerEnter(Collider other) {
        if(other.TryGetComponent(out PseudoGravity pse)) {
            pse.gravi = false;
        }
    }
    void FixedUpdate() {
        float tcos = amplitude * (1 + Mathf.Cos(Time.time * frecuence)) + offset;
        Vector3 tsin = -amplitude * frecuence * Mathf.Sin(Time.time * frecuence) * transform.forward;
        transform.localScale = new Vector3(100, 100, 100 * tcos);
        portal1.vel = inpos1 * tsin;
        portal2.vel = inpos2 * tsin;
        portal1.transform.position = transform.position + (inpos1 * tcos * transform.forward);
        portal2.transform.position = transform.position + (inpos2 * tcos * transform.forward);
    }

}
