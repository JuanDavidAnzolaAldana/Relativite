using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portalizer : MonoBehaviour
{
    public bool does;
    public Transform portal,fakePortal;
    bool done,finished=true;
    Animator anim;
    float tie;
    void Start(){
        anim=this.GetComponent<Animator>();
    }
    void Update(){
        if (!does) {
          done=true;
        }
        if (done&&does&&finished&&portal.position!=fakePortal.position) {
          StartCoroutine(Portalize());
        }
    }
    IEnumerator Portalize(){
      done=false;
      finished=false;
      anim.SetBool("instantiate",true);
      while (tie<1) {
        tie+=Time.deltaTime;
        portal.localScale*=Mathf.Pow(0.0001f,Time.deltaTime);
        yield return null;
      }
      portal.parent=fakePortal;
      portal.localScale=Vector3.one;
      portal.localPosition=Vector3.zero;
      portal.localRotation=Quaternion.identity;
      yield return new WaitForSeconds(2);
      anim.SetBool("instantiate",false);
      yield return new WaitForSeconds(2);
      portal.parent=null;
      finished=true;
    }
}
