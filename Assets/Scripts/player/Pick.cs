using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pick: MonoBehaviour {
    Transform front;
    RaycastHit hot;
    CrossCursor crcu;
    PseudoGravity rb;
    public PseudoGravity pse;
    public bool corrupt = false, change = false;
    public LineRenderer lr;
    float tie = 1;
    void Awake() {
        front = this.GetComponent<Movement>().eyes;
        crcu = this.GetComponent<CrossCursor>();
        rb = this.GetComponent<PseudoGravity>();
    }
    void Update() {
        if(Movement.play) {//El juego corre
            if(pse) {//Tengo algo
                Draw(pse.transform.position, false, false);
                if(Input.GetButtonDown("Fire1") && !Input.GetButton("Swap")) {
                    Drop();
                }
            } else if(Physics.Raycast(front.position, front.forward, out hot, 6f, ~(1 << 2), QueryTriggerInteraction.Ignore)) { //Estoy mirando algo
                tie += Time.deltaTime;
                if(hot.collider.TryGetComponent(out PseudoGravity ps) && ps.pickable) {//Es agarrable
                    if(Input.GetButton("Fire1") && !Input.GetButton("Swap") && tie > 0.5f) {//Lo estoy agarrando
                        crcu.select = false;
                        ps.picked = this;
                        ps.gravi = false;
                        Draw(ps.transform.position, false, false);
                        if(corrupt) {
                            ps.rb.angularVelocity += Random.insideUnitSphere * 1.5f;
                        }
                        pse = ps;
                    } else {//No lo estoy agarrando
                        Select();
                    }
                } else if(hot.collider.TryGetComponent(out EvenTimer et)) {//es interactivo
                    Select();
                    if(Input.GetButtonDown("Fire1") && !Input.GetButton("Swap")) {//Lo estoy usando
                        et.tie = 0;
                        foreach(EvenTimer e in et.incompatible) {
                            e.Restart();
                        }
                    }
                } else {//Es otra cosa
                    crcu.select = false;
                    if(Input.GetButton("Fire1") && !Input.GetButton("Swap")) {//La intento usar
                        Draw(hot.point, false, false);
                    } else {
                        lr.enabled = false;
                        crcu.g2 = false;
                    }
                }
            } else {//No veo nada
                tie += Time.deltaTime;
                crcu.select = false;
                if(Input.GetButton("Fire1") && !Input.GetButton("Swap")) {
                    Draw(front.position + (front.forward * 8), true, true);
                } else {
                    lr.enabled = false;
                    crcu.g2 = false;
                }
            }
        }
    }
    void FixedUpdate() {
        if(pse) {
            pse.rb.AddForce((transform.position + front.forward + transform.rotation * new Vector3(0, 0.5f, 1.5f) - pse.transform.position) * 2700 + (rb.rb.velocity - pse.rb.velocity) * 450);
            if(!corrupt) {
                pse.rb.AddTorque(Vector3.Cross(pse.transform.up, transform.up) * 250 - pse.rb.angularVelocity * 35);
            }
        }
    }
    public void Drop() {
        tie = 0;
        pse.picked = null;
        if(corrupt) {
            pse.rb.velocity += Random.insideUnitSphere * 2f;
        }
        if(change) {
            pse.direction = rb.direction;
        }
        pse.gravi = true;
        pse = null;
    }
    void Select() {
        crcu.select = true;
        crcu.g2 = false;
        lr.enabled = false;
    }
    void Draw(Vector3 pos, bool shake, bool gradient) {
        lr.enabled = true;
        crcu.g2 = true;
        float dist = (pos - lr.transform.position).magnitude;
        int ite = (int) Mathf.Ceil(dist * 1.5f) + 2;
        lr.positionCount = ite;
        lr.SetPosition(0, lr.transform.position);
        for(int i = 1; i < ite - 1; i++) {
            lr.SetPosition(i, lr.transform.position + ((pos - lr.transform.position) * i / (ite - 1)) + Random.insideUnitSphere / 2.5f);
        }
        lr.SetPosition(ite - 1, shake ? pos + Random.insideUnitSphere / 3 : pos);
        lr.endColor = gradient ? Color.clear : Color.white;
    }
    void OnEnable() {
        crcu.g1 = true;
    }
    void OnDisable() {
        crcu.g1 = false;
    }
}
