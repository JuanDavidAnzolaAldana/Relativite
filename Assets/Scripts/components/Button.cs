using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button: MonoBehaviour {
    public Activate act;
    public int index;
    public string mask = null;
    Animator anim;
    void Start() {
        anim = this.GetComponent<Animator>();
    }
    void OnTriggerStay(Collider other) {
        if(act != null && !other.isTrigger && (mask == "" || mask == other.name)) {
            act.interruption[index] = true;
            anim.SetBool("pressed", true);
        }
    }

    void OnTriggerExit(Collider other) {
        if(act != null && !other.isTrigger && (mask == "" || mask == other.name)) {
            act.interruption[index] = false;
            anim.SetBool("pressed", false);
        }
    }
}
