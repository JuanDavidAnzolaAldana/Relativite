using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectable : MonoBehaviour
{
    public Pick picker;
    public SimpleGravity graviter;
    public bool cPick,cGrav;
    public GameObject visual,noVisual;
    public CrossCursor cros;
    void OnTriggerEnter(Collider other){
        if (other.gameObject!=this.gameObject&&other.gameObject.GetComponent<Movement>()) {
            if (picker) {
                picker.enabled=true;
                picker.corrupt=cPick;
            }
            if (graviter) {
                graviter.enabled=true;
                graviter.corrupt=cGrav;
            }
            if (visual) {
                visual.SetActive(true);
                if (picker) {
                    picker.lr=visual.GetComponentInChildren(typeof(LineRenderer)) as LineRenderer;
                    picker.lr.enabled=false;
                }
            }
            if (noVisual) {
                noVisual.SetActive(false);
            }
            if (cros) {
                cros.render=visual.GetComponent<MeshRenderer>();
                cros.mat=cros.render.materials;
            }
            Destroy(this.gameObject);
        }
    }
}
