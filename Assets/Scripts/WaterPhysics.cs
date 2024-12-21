using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterPhysics: MonoBehaviour {
    public int shade;
    public Vector3 direction;
    public float density, viscosity;
    Bounds space;
    Vector3 s, c;
    void Start() {
        space = this.GetComponent<Collider>().bounds;
        s = space.max;
        c = space.min;
    }
    void OnTriggerStay(Collider other) {
        if(other.TryGetComponent(out Movement move)) {
            move.walk = move.eyes;
            if(space.Contains(move.eyes.position)) {
                if(move.eyes.TryGetComponent(out PostProcessing process)) {
                    process.filter[shade] = true;
                }
                if(move.TryGetComponent(out Health health)) {
                    health.drown = true;
                }
            } else {
                if(move.eyes.TryGetComponent(out PostProcessing process)) {
                    process.filter[shade] = false;
                }
                if(move.TryGetComponent(out Health health)) {
                    health.drown = false;
                }
            }
        }
        if(other.TryGetComponent(out PseudoGravity pse)) {
            Bounds jail = other.bounds;
            float v = Volume(Vector3.Max(Vector3.Min(jail.max, s) - Vector3.Max(jail.min, c), Vector3.zero));
            pse.rb.AddForce(-density * pse.volume * PseudoGravity.magnitude * v * direction / (Volume(jail.size)));
            pse.rb.AddForce(-viscosity * density * Mathf.Pow(pse.volume, 2 / 3) * pse.rb.velocity.magnitude * pse.rb.velocity / 2);
        }
    }
    void OnTriggerExit(Collider other) {
        if(other.TryGetComponent(out Movement move)) {
            move.walk = move.transform;
            if(move.eyes.TryGetComponent(out PostProcessing process)) {
                process.filter[shade] = false;
            }
            if(move.TryGetComponent(out Health health)) {
                health.drown = false;
            }
        }
    }
    float Volume(Vector3 a) {
        return a.x * a.y * a.z;
    }
}
