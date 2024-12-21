using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement: MonoBehaviour {
    public Transform eyes, walk;
    PseudoGravity pg;
    public float velocity = 1000, jump = 10;
    public static float look;
    float speed = 1, tie = 0;
    public bool doLook = true, jumpLock;
    public static bool play = true;
    public Vector3 velocit = Vector3.zero;
    readonly Collider[] colliders = new Collider[10];
    int i, o;
    void Start() {
        pg = GetComponent<PseudoGravity>();
        pg.rb.constraints = RigidbodyConstraints.FreezeRotation;
        walk = transform;
        look = PlayerPrefs.GetFloat("Raton", 0.32768f) * 2000;
    }
    void Update() {
        if(play) {
            transform.Rotate(look * Input.GetAxis("Mouse X") * Time.deltaTime * Vector3.up);
            if(doLook) {
                if(Vector3.Dot(eyes.forward, transform.forward) >= 0) {
                    eyes.Rotate(-look * Input.GetAxis("Mouse Y") * Time.deltaTime * Vector3.right);
                } else {
                    if(Vector3.Dot(eyes.forward, transform.up) > 0) {
                        eyes.Rotate(Vector3.right);
                    } else {
                        eyes.Rotate(Vector3.left);
                    }
                }
            }
            if(-pg.direction != transform.up) {
                transform.rotation = Util.AngleVector(1.75f * Time.deltaTime * Vector3.Cross(transform.up, -pg.direction)) * transform.rotation;
            }
            if(Input.GetButton("Jump") && tie < 0.2f) {
                Debug.Log("a");
                tie = 1;
                pg.rb.velocity += transform.up * (jump);
            }
            jumpLock = false;
            o = Physics.OverlapSphereNonAlloc(transform.position - transform.up * 0.6f, 0.44f, colliders, ~1024, QueryTriggerInteraction.Ignore);
            for(i=0; i<o; i++) {
                if(Mathf.Abs(Vector3.Dot(pg.rb.velocity - (colliders[i].attachedRigidbody ? colliders[i].attachedRigidbody.velocity : Vector3.zero), transform.up)) < 2) {
                    jumpLock = true;
                }
            }
            if(jumpLock) {
                tie = 0;
                if(Input.GetButton("Crouch")) {
                    speed = 0.5f;
                } else {
                    speed = 1;
                }
            } else {
                speed = 0.1f;
                tie += Time.deltaTime;
            }
        }
    }
    void FixedUpdate() {
        pg.rb.AddForce(speed * velocity * walk.TransformDirection(new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"))).normalized, ForceMode.Force);
    }
}
