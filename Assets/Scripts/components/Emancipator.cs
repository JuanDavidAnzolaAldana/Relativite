using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Emancipator: MonoBehaviour {
    public bool objects, player;
    public float tie;
    public Material deploy;
    void OnTriggerStay(Collider other) {
        if(other.TryGetComponent(out PseudoGravity pse)) {
            if(other.TryGetComponent(out Health life)) {
                if(player) {
                    if(tie == 0) {
                        life.health = 0;
                    } else {
                        life.health -= Time.fixedDeltaTime / tie;
                    }
                }
            } else {
                if(objects) {
                    if(pse.picked != null) {
                        pse.picked.Drop();
                    }
                    pse.pickable = false;
                    pse.gravi = false;
                    pse.rb.angularVelocity = Random.insideUnitSphere * 1.5f;
                    other.GetComponent<MeshRenderer>().material = deploy;
                    Destroy(other.gameObject, 3f);
                }
            }
        } else if(other.GetComponent<PortalOrb>()) {
            Destroy(other.gameObject);
        }
    }
}
