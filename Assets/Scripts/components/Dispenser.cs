using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dispenser : MonoBehaviour
{
    public GameObject obj;
    public bool active=false,force=false;
    public Transform[] sources;
    public Material deploy;
    public portalClone clone;
    bool forced=false;
    GameObject inside, outside;
    Animator anim;

    void Start(){
      anim=this.GetComponent<Animator>();
      inside=Instantiate(obj,transform.position-(transform.forward*2),Random.rotation);
      PseudoGravity pse=inside.GetComponent<PseudoGravity>();
      pse.direction=-transform.forward;
      pse.sources=sources;
    }

    void Update(){
      if (!force) {
        forced=true;
      }
      if (inside!=null) {
        if (outside==null) {
          if (active||(force&&forced)) {
            forced=false;
            anim.SetBool("MustOpen",true);
            outside=inside;
            inside=null;
            StartCoroutine(Recharge());
          }
        }else if (force&&forced) {
          forced=false;
          PseudoGravity pso=outside.GetComponent<PseudoGravity>();
          if (pso.picked!=null) {
            pso.picked.Drop();
          }
          pso.pickable=false;
          pso.gravi=false;
          pso.rb.angularVelocity=Random.insideUnitSphere*1.5f;
          outside.GetComponent<MeshRenderer>().material=deploy;
          Destroy(outside,3f);
          anim.SetBool("MustOpen",true);
          outside=inside;
          inside=null;
          StartCoroutine(Recharge());
        }
      }
      if (clone) {
        clone.clone=outside.transform;
      }
    }

    IEnumerator Recharge(){
      yield return new WaitForSeconds(2);
      anim.SetBool("MustOpen",false);
      yield return new WaitForSeconds(3);
      inside=Instantiate(obj,transform.position-(transform.forward*2),Random.rotation);
      PseudoGravity pse=inside.GetComponent<PseudoGravity>();
      pse.direction=-transform.forward;
      pse.sources=sources;
    }
}
