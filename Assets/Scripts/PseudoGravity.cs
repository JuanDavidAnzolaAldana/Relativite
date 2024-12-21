using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PseudoGravity: MonoBehaviour {

    public Rigidbody rb;
    public float volume, mass;
    public Vector3 direction;
    public bool pickable = false, gravi = true;
    public Pick picked;
    public Transform[] sources;
    public static float magnitude = 18f, lenght = 24f;

    void Awake() {
        if(!rb && !this.TryGetComponent<Rigidbody>(out rb)) {
            rb = this.gameObject.AddComponent(typeof(Rigidbody)) as Rigidbody;
            rb.mass = mass;
            rb.useGravity = false;
            rb.isKinematic = false;
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        }
    }

    void FixedUpdate() {
        if(gravi) {
            rb.AddForce(magnitude * rb.mass * direction);
            foreach(Transform sample in sources) {
                rb.AddForce(lenght * rb.mass * (sample.position - transform.position).normalized / ((sample.position - transform.position).magnitude * (sample.position - transform.position).magnitude));
            }
        }
    }
    void OnCollisionEnter() {
        if(picked && picked.corrupt) {
            picked.Drop();
        }
    }
}
