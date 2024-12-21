using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserTurret : MonoBehaviour
{
    LaserBeam laser;
    public Transform player;
    Vector3 dir;
    void Start(){
      laser=this.GetComponent<LaserBeam>();
      dir=transform.forward;
    }
    void Update(){
      laser.point=transform.rotation*laser.poit;
      Vector3 pos=transform.position+(transform.rotation*laser.poit);
      if (Vector3.Dot((player.position-pos).normalized,transform.forward)>0.7071) {
        RaycastHit hut;
        if (Physics.Raycast(pos,(player.position-pos).normalized,out hut,Mathf.Infinity,~(1<<1),QueryTriggerInteraction.Ignore)) {
          dir+=((hut.point-pos).normalized-dir)/10;
        }else {
          dir+=(transform.forward-dir)/10;
        }
      }else{
        dir+=(transform.forward-dir)/10;
      }
      laser.directio=transform.InverseTransformDirection(dir);
    }
}
