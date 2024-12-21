using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Platform : MonoBehaviour
{
    public Vector3[] pst;
    public Quaternion[] rtc;
    public float vel;
    public bool stepped=false, step=false;
    Rigidbody rb;
    float tie;
    int a = 1, b = 0, c = 1, d = 0;
    bool flick=false,nostep,active=false;
    void Start()
    {
        nostep = step;
        rb=this.GetComponent<Rigidbody>();
    }
    void Update()
    {
        if (nostep != step)
        {
            active = true;
            nostep = step;
        }
    }
    void FixedUpdate(){
        if (active || !stepped)
        {
            tie += Time.fixedDeltaTime * vel;
            if (!flick && tie >= Mathf.PI)
            {
                b = a + 1;
                d = c + 1;
                if (b >= pst.Length)
                {
                    b -= pst.Length;
                }
                if (d >= rtc.Length)
                {
                    d -= rtc.Length;
                }
                if (active && step)
                {
                    active = false;
                }
                flick = true;
            }
            else if (flick && tie >= 2 * Mathf.PI)
            {
                a = b + 1;
                c = d + 1;
                if (a >= pst.Length)
                {
                    a -= pst.Length;
                }
                if (c >= rtc.Length)
                {
                    c -= rtc.Length;
                }
                if (active && !step)
                {
                    active = false;
                }
                tie -= 2 * Mathf.PI;
                flick = false;
            }
            if (pst.Length > 1)
            {
                rb.MovePosition(pst[a] + ((1 + Mathf.Cos(tie)) * (pst[b] - pst[a]) / 2));
            }
            if (rtc.Length > 1)
            {
                rb.MoveRotation(Quaternion.Lerp(rtc[c], rtc[d], (1 + Mathf.Cos(tie)) / 2));
            }
        }
    }
}
