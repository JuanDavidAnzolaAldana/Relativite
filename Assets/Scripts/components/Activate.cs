using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Activate : MonoBehaviour
{
    public bool interruptor=false;
    public bool[] interruption;
    public LaserBeam laser=null;
    public GameObject obj=null;
    public Dispenser suply,regenerator;
    public Platform slider;
    public SpriteRenderer[] sr;
    public Doors exit;
    public Activate act;
    public int acts;

    // Update is called once per frame
    void Update()
    {
        bool check=interruptor;
        foreach (bool a in interruption) {
            check=check&&a;
        }
        if (laser) {
            laser.enabled=check;
        }
        if (obj) {
            obj.SetActive(check);
        }
        if (suply) {
            suply.active=check;
        }
        if (regenerator) {
            regenerator.force=check;
        }
        if (exit) {
            exit.active=check;
        }
        if (slider)
        {
            slider.step = check;
        }
        if (act)
        {
            act.interruption[acts] = check;
        }
        foreach (SpriteRenderer a in sr) {
            a.color=check?new Color(0.73f,1,0):new Color(0.5f,0,1);
        }
    }
}
