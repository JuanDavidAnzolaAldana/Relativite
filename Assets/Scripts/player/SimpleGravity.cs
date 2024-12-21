using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleGravity: MonoBehaviour {
    Movement move;
    PseudoGravity mygrav;
    public bool corrupt = true;
    RaycastHit hit;
    CrossCursor cros;
    PostProcessing process;
    bool block;
    void Awake() {
        move = this.GetComponent<Movement>();
        mygrav = this.GetComponent<PseudoGravity>();
        cros = this.GetComponent<CrossCursor>();
        process = move.eyes.GetComponent<PostProcessing>();
    }
    void Update() {
        if(Movement.play) {
            if((Input.GetButtonUp("Stabilize") || Input.GetButtonUp("Fire2")) && block) {
                move.doLook = true;
                cros.p2 = false;
                process.filter[2] = false;
                block = false;
                if(corrupt) {
                    mygrav.direction = (0.3f * Random.insideUnitSphere - transform.up).normalized;
                }
            } else if(Input.GetButton("Stabilize") && Physics.Raycast(transform.position - transform.up * 0.5f, -transform.up, out hit, 1, ~1024, QueryTriggerInteraction.Ignore)) {
                cros.p2 = true;
                mygrav.direction = -hit.normal;
                move.doLook = true;
                process.filter[2] = true;
                block = true;
            } else if(Input.GetButton("Fire2") && !Input.GetButton("Swap")) {
                cros.p2 = true;
                move.doLook = false;
                mygrav.direction = -transform.up;
                process.filter[2] = true;
                block = true;
                transform.Rotate(-Movement.look * Input.GetAxis("Mouse Y") * Time.deltaTime * Vector3.right);
            }
        }
    }
    void OnEnable() {
        cros.p1 = true;
    }
    void OnDisable() {
        cros.p1 = false;
    }
}
