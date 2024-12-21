using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PortalGun: MonoBehaviour {
    public Material[] colors = new Material[2];
    public Transform[] objects = new Transform[2];
    public GameObject bullet;
    public float vel;
    Transform eyes;

    void Start() {
        eyes = this.GetComponent<Movement>().eyes;
    }

    void Update() {
        if(Input.GetButton("Swap") && Input.GetButtonDown("Fire1") && GameObject.Find("proyectile(Clone)") == null) {
            GameObject proyectile = Instantiate(bullet, eyes.position + eyes.forward, eyes.rotation * new Quaternion(0, 1, 0, 0));
            proyectile.transform.localScale = 0.5f * Vector3.one;
            proyectile.GetComponent<MeshRenderer>().material = colors[0];
            proyectile.GetComponent<Rigidbody>().velocity = vel * eyes.forward;
            proyectile.GetComponent<PortalOrb>().portal = objects[0];
        }
        if(Input.GetButton("Swap") && Input.GetButtonDown("Fire2") && GameObject.Find("proyectile(Clone)") == null) {
            GameObject proyectile = Instantiate(bullet, eyes.position + eyes.forward, eyes.rotation * new Quaternion(0, 1, 0, 0));
            proyectile.transform.localScale = 0.5f * Vector3.one;
            proyectile.GetComponent<MeshRenderer>().material = colors[1];
            proyectile.GetComponent<Rigidbody>().velocity = vel * eyes.forward;
            proyectile.GetComponent<PortalOrb>().portal = objects[1];
        }
    }
}
