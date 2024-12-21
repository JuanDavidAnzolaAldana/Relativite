using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserBeam : MonoBehaviour
{
    public Vector3 poit=Vector3.zero, point, direction, directio;
    LineRenderer lr;
    public Transform[] portals;
    public LaserBeam[] partners;
    public Transform burn;
    public bool source=false;
    int b=-1;
    Transform job=null;
    void OnDisable()
    {
      Deploy();
      lr.enabled=false;
    }
    void OnEnable()
    {
      lr=this.GetComponent<LineRenderer>();
      lr.enabled=true;
    }
    void Update()
    {
      if (source) {
        direction=transform.TransformDirection(directio.normalized);
        point=transform.rotation*poit;
      }
      lr.SetPosition(0,transform.position+(point));
      RaycastHit hat;
      if (Physics.Raycast(transform.position+point, direction, out hat,Mathf.Infinity,~(1<<1),QueryTriggerInteraction.Collide)) {
        if (hat.transform!=job) {//no esta tocando el mismo objeto
          if (b!=-1&&hat.transform!=portals[b]) {//no esta tocando el mismo portal
            Deploy();
          }
          if (job!=null) {//estaba tocando un objeto
            job.GetComponent<Activate>().interruptor=false;
            job=null;
          }
          if (hat.transform.tag=="Laser Receptor") {//está tocando un objeto diferente
            b=-1;
            lr.SetPosition(1,hat.point);
            burn.position=hat.point-(direction*0.01f);
            burn.rotation=Quaternion.FromToRotation(burn.forward,-direction)*burn.rotation;
            hat.transform.GetComponent<Activate>().interruptor=true;
            job=hat.transform;
          }else if (hat.transform.tag=="portal") {//está tocando un supuesto portal
            int a=-1;
            for (int i=0; i<portals.Length; i++) {//buscar el portal
              if (portals[i]==hat.transform) {
                a=i;
              }
            }
            if (a!=-1) {//activar el portal si existe
              b=a;
              Quaternion rot=partners[a].transform.rotation*Quaternion.Euler((Quaternion.Inverse(portals[a].rotation)*transform.rotation).eulerAngles+new Vector3(0,180,0));
              partners[a].enabled=true;
              partners[a].burn=burn;
              partners[a].point=rot*transform.InverseTransformDirection(hat.point+(0.1f*direction)-portals[a].position);
              partners[a].direction=rot*transform.InverseTransformDirection(direction);
            }
            lr.SetPosition(1,transform.position+point+(direction*(hat.distance+2f)));
          }else if (hat.transform.TryGetComponent(out Health life)) {
            life.health-=Time.deltaTime/4;
            b=-1;
            lr.SetPosition(1,hat.point);
            burn.position=hat.point-(direction*0.01f);
            burn.rotation=Quaternion.FromToRotation(burn.forward,-direction)*burn.rotation;
          }else{
            b=-1;
            lr.SetPosition(1,hat.point);
            burn.position=hat.point-(direction*0.01f);
            burn.rotation=Quaternion.FromToRotation(burn.forward,-direction)*burn.rotation;
          }
        }else {
          b=-1;
          lr.SetPosition(1,hat.point);
          burn.position=hat.point-(direction*0.01f);
          burn.rotation=Quaternion.FromToRotation(burn.forward,-direction)*burn.rotation;
        }

      }else {
        lr.SetPosition(1,transform.position+point+(direction*50f));
        burn.position=transform.position+point+(direction*50f);
        burn.rotation=Quaternion.FromToRotation(burn.forward,-direction)*burn.rotation;
        if (job!=null) {
          job.GetComponent<Activate>().interruptor=false;
          job=null;
        }
      }
    }
    void Deploy(){
      if (b!=-1&&partners[b]!=null) {
        partners[b].burn=null;
        partners[b].enabled=false;
      }
      if (job!=null) {
        job.GetComponent<Activate>().interruptor=false;
        job=null;
      }
    }
}
