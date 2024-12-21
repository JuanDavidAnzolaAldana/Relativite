using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Graviter: MonoBehaviour {
    public float scale;
    public ParticleSystem pop;
    public Collider coll;
    void OnDisable() {
        transform.localScale = Vector3.zero;
        pop.Play();
        coll.enabled = false;
    }
    void OnTriggerStay(Collider other) {
        if(other.TryGetComponent(out PseudoGravity pse)) {
            if(pse.TryGetComponent(out Movement move)) {
                if(Vector3.Dot(-pse.transform.up, transform.up) > 0.996f) {
                    if(-pse.transform.up==transform.up) {
                        float t = Random.Range(0, Mathf.PI);
                        pse.transform.rotation = pse.transform.rotation * new Quaternion(Mathf.Cos(t) * 0.08987855f, 0, Mathf.Sin(t) * 0.08987855f, 0.99595273f);
                    } else {
                        pse.transform.rotation = Quaternion.AngleAxis(5.4f,Vector3.Cross(pse.transform.up, transform.up).normalized) * pse.transform.rotation;
                    }
                }
                if(move.eyes.TryGetComponent(out PostProcessing process)) {
                    if((move.eyes.position - transform.position).sqrMagnitude < 0.25f * transform.lossyScale.x * transform.lossyScale.x) {
                        process.filter[2] = true;
                    }else {
                        process.filter[2] = false;
                    }
                }
            }
            pse.direction = -transform.up;
        }
    }
    void Update() {
        transform.localScale = (scale - Mathf.Pow(0.02f, Time.deltaTime) * (scale - transform.localScale.x)) * Vector3.one;
        coll.enabled = true;
    }
    void OnTriggerExit(Collider other) {
        if(other.TryGetComponent(out Movement move)&&move.eyes.TryGetComponent(out PostProcessing process)) {
                process.filter[2] = false;
        }
    }
}
