using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors: MonoBehaviour {
    public bool active;
    Animator anim;
    void Start() {
        anim = this.GetComponent<Animator>();
    }
    void OnTriggerStay(Collider other) {
        if(other.GetComponent<Movement>() && active) {
            anim.SetBool("mustOpen", true);
        }
    }
    void OnTriggerExit(Collider other) {
        if(other.GetComponent<Movement>()) {
            anim.SetBool("mustOpen", false);
        }
    }
}
