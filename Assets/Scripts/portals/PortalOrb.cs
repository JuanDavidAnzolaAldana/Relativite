using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalOrb: MonoBehaviour {
    public Transform portal;
    float tie = 0f;
    int choques = 0, estado = 0;
    Vector3 pos, off;
    ContactPoint[] points = new ContactPoint[10];
    void OnDisable() {
        portal.localScale = 100f * Vector3.one;
        Destroy(this.gameObject);
    }
    void Update() {
        pos += off;
        transform.position += off;
        off = Vector3.zero;
        switch(estado) {
            case 0:
                tie += Time.deltaTime;
                if(tie >= 10f) {
                    estado = 1;
                }
                break;
            case 1:
                transform.localScale *= Mathf.Pow(0.025f, Time.deltaTime);
                portal.localScale = 100f * Vector3.one;
                if(transform.localScale.x < 0.005f) {
                    Destroy(this.gameObject);
                }
                break;
            case 2:
                transform.position += (pos - transform.position) * (1 - Mathf.Pow(0.1f, Time.deltaTime));
                transform.localScale += new Vector3(4.5f - transform.localScale.x, 4.5f - transform.localScale.y, 1 - transform.localScale.z) * (1 - Mathf.Pow(0.02f, Time.deltaTime));
                portal.localScale *= Mathf.Pow(0.00035f, Time.deltaTime);
                if(transform.localScale.x >= 4.3f && portal.localScale.x <= 0.1f) {
                    portal.SetPositionAndRotation(pos, transform.rotation);
                    estado = 3;
                }
                break;
            case 3:
                portal.localScale += (100 - portal.localScale.x) * (1 - Mathf.Pow(0.02f, Time.deltaTime)) * Vector3.one;
                transform.localScale *= Mathf.Pow(0.01f, Time.deltaTime);
                if(portal.localScale.x >= 99 && transform.localScale.x <= 0.05f) {
                    portal.localScale = 100f * Vector3.one;
                    Destroy(this.gameObject);
                }
                break;
        }
    }
    void OnTriggerStay(Collider other) {
        if(estado < 3 && other.gameObject.CompareTag("portal")) {
            portal.localScale = 100f * Vector3.one;
            Destroy(this.gameObject);
        }
    }
    void OnCollisionEnter(Collision coll) {
        if(estado == 0) {
            if(coll.transform.CompareTag("Portalizable")) {
                GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                transform.rotation = Quaternion.FromToRotation(transform.forward, -coll.transform.forward) * transform.rotation;
                pos = transform.position + (transform.forward * (0.01f - Vector3.Dot(transform.forward, transform.position - coll.transform.position)));
                estado = 2;
            } else {
                choques++;
                if(choques >= 3) {
                    estado = 1;
                }
            }
        }
    }
    void OnCollisionStay(Collision coll) {
        if(estado == 2) {//esta en un mal lugar?
            if((!coll.transform.CompareTag("Portalizable") || -Vector3.Dot(transform.forward, coll.transform.forward) < 0.9f) && coll.transform.GetComponent<PseudoGravity>() == null) {
                int a = 0;
                for(int i = 0; i < coll.GetContacts(points); i++) {
                    a = points[i].separation < points[a].separation ? i : a;//buscar la prioridad
                }
                Vector3 distance = (points[a].separation) * (points[a].point - transform.position).normalized;//hallar la dirección de la colisión
                distance -= (transform.forward * Vector3.Dot(distance, transform.forward));//eliminar el componente vertical
                if(Vector3.Dot(off.normalized, distance.normalized) > -0.7071f || distance.magnitude < 0.1f) {
                    off += distance;
                } else {
                    estado = 1;
                }
            }
        }
    }
    void FixedUpdate() {
        pos += off;
        transform.position += off;
        off = Vector3.zero;
    }
}
