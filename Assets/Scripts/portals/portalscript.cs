using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class portalscript: MonoBehaviour {
    public Transform other;
    public Camera mine;
    public portalscript test;
    public RenderTexture show;
    public Vector3 vel = Vector3.zero;
    public int maxIterations = 5;
    MeshRenderer otherview;
    float raduis = 1.7507f;

    void Start() {
        show.width = Screen.width;
        show.height = Screen.height;
        mine.ResetAspect();
        otherview = other.GetComponent<MeshRenderer>();
        mine.enabled = false;
    }
    public void SetRender(Transform tran, Plane[] frustrum, int iteration) {
        if(iteration <= maxIterations && otherview.isVisible && GeometryUtility.TestPlanesAABB(frustrum, otherview.bounds) && (tran.position - otherview.transform.position).magnitude <= 110) {
            mine.enabled = true;
            Quaternion relation = transform.rotation * new Quaternion(0, 1, 0, 0) * Quaternion.Inverse(other.rotation);
            Vector3 pos = transform.position + (relation * (tran.position - other.position));
            mine.transform.position = pos;
            Quaternion rot = relation * tran.rotation;
            mine.transform.rotation = rot;
            Vector3 BounOff = new Vector3(Mathf.Sqrt(1 - Mathf.Pow(Vector3.Dot(transform.forward, mine.transform.right), 2)) * raduis, Mathf.Sqrt(1 - Mathf.Pow(Vector3.Dot(transform.forward, mine.transform.up), 2)) * raduis, Mathf.Sqrt(1 - Mathf.Pow(Vector3.Dot(transform.forward, mine.transform.forward), 2)) * raduis);
            float dist = Vector3.Dot(transform.position - mine.transform.position, mine.transform.forward) - BounOff.z;
            mine.nearClipPlane = Mathf.Max(0.01f, dist);
            Plane[] frustration = GeometryUtility.CalculateFrustumPlanes(mine);
            frustration[0] = frustration[0].GetSide(transform.position - mine.transform.rotation * BounOff) ? new Plane(mine.transform.position, mine.transform.position + mine.transform.up, transform.position - mine.transform.rotation * BounOff) : frustration[0];
            frustration[1] = frustration[1].GetSide(transform.position + mine.transform.rotation * new Vector3(BounOff.x, BounOff.y, -BounOff.z)) ? new Plane(mine.transform.position, mine.transform.position - mine.transform.up, transform.position + mine.transform.rotation * new Vector3(BounOff.x, BounOff.y, -BounOff.z)) : frustration[1];
            frustration[2] = frustration[2].GetSide(transform.position - mine.transform.rotation * BounOff) ? new Plane(mine.transform.position, mine.transform.position - mine.transform.right, transform.position - mine.transform.rotation * BounOff) : frustration[2];
            frustration[3] = frustration[3].GetSide(transform.position + mine.transform.rotation * new Vector3(BounOff.x, BounOff.y, -BounOff.z)) ? new Plane(mine.transform.position, mine.transform.position + mine.transform.right, transform.position + mine.transform.rotation * new Vector3(BounOff.x, BounOff.y, -BounOff.z)) : frustration[3];
            test.SetRender(mine.transform, frustration, iteration + 1);
            mine.enabled = true;
            mine.transform.SetPositionAndRotation(pos, rot);
            mine.nearClipPlane = Mathf.Max(0.01f, dist);
            mine.Render();
            mine.enabled = false;
        }
    }
    void OnTriggerStay(Collider oter) {
        if(oter.TryGetComponent(out PseudoGravity pse)) {
            oter.gameObject.layer = 8;
            if(Vector3.Dot(transform.forward, oter.transform.position - transform.position) < 0 && pse.picked == null && (oter.transform.position - transform.position).magnitude < 3) {
                Teleport(oter.transform);
                if(oter.TryGetComponent(out Pick pic)) {
                    if(pic.pse != null) {
                        Teleport(pic.pse.transform);
                    }
                }
            }
        }
    }
    void OnTriggerExit(Collider oter) {
        if(oter.TryGetComponent(out PseudoGravity pse)) {
            oter.gameObject.layer = 0;
            if(pse.picked != null) {
                if(Vector3.Dot(transform.forward, oter.transform.position - transform.position) < 0 && Vector3.Dot(transform.forward, pse.picked.transform.position - transform.position) > 0) {
                    pse.picked.Drop();
                    Teleport(oter.transform);
                }
            }
        }
    }
    void Teleport(Transform obj) {
        Quaternion relation = other.rotation * new Quaternion(0, 1, 0, 0) * Quaternion.Inverse(transform.rotation);
        obj.SetPositionAndRotation(other.position + (relation * (obj.position - transform.position)), relation * obj.rotation);
        obj.GetComponent<Rigidbody>().velocity = other.GetComponent<portalscript>().vel + (relation * (obj.GetComponent<Rigidbody>().velocity - vel));
        obj.GetComponent<PseudoGravity>().direction = relation * (obj.GetComponent<PseudoGravity>().direction);
    }
}
