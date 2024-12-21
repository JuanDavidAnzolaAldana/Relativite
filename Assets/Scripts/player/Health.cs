using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Health: MonoBehaviour {
    public float health;
    public float air;
    public bool drown;
    public Material effect;
    public Material effect2;
    Vector3 forc, rel;
    bool stillAlive;

    void Start() {
        stillAlive = true;
    }
    void FixedUpdate() {
        health -= Mathf.Max(rel.magnitude - forc.magnitude, 0) * Time.fixedDeltaTime / 5000;
        forc = Vector3.zero;
        rel = Vector3.zero;
    }
    void Update() {
        if(stillAlive) {
            if(air <= 0) {
                health -= Time.deltaTime / 8;
            }
            if(drown && air > 0) {
                air -= Time.deltaTime / 45;
            } else if(air < 1 && !drown) {
                air += Time.deltaTime / 3;
            }
            health = Mathf.Clamp01(health);
            air = Mathf.Clamp01(air);
            float x = (100 - Mathf.Pow(10, 2 * health)) / 99;
            effect.SetFloat("_Total", x);
            effect2.SetFloat("_Total", x);
        }
        if(health < -2) {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        } else if(health <= 0) {
            stillAlive = false;
            health -= Time.deltaTime;
            effect.SetFloat("_Total", 1 - health / 2);
            effect2.SetFloat("_Total", 1 - health / 2);
        } else if(health < 1 && air > 0) {
            health += air * Time.deltaTime / 10;
        }
    }
    void OnCollisionEnter(Collision coll) {
        health -= Mathf.Max(0, Vector3.Dot(coll.GetContact(0).normal, coll.relativeVelocity.normalized)) * (coll.relativeVelocity.magnitude - 10f) * (0.375f * Vector3.Dot(coll.GetContact(0).point - transform.position, transform.up) + 0.625f) / 50;
    }
    void OnCollisionStay(Collision coll) {
        forc += coll.impulse;
        rel += new Vector3(Mathf.Abs(coll.impulse.x), Mathf.Abs(coll.impulse.y), Mathf.Abs(coll.impulse.z));
    }
}
