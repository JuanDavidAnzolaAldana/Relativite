using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvenTimer : MonoBehaviour
{
    public float total=10,tie=10;
    public Activate act;
    public int index;
    Animator anim;
    bool undo;
    public EvenTimer[] incompatible;
    void Start()
    {
        anim=this.GetComponentInChildren<Animator>();
        tie=total+1;
    }
    void Update()
    {
        if (act!=null)
        {
            if (tie<total)
            {
                tie+=Time.deltaTime;
                anim.SetBool("pressed",true);
                act.interruption[index]=true;
                undo=true;
            }else if (undo)
            {
                Restart();
            }
        }
    }
    public void Restart()
    {
        tie = total + 1;
        anim.SetBool("pressed", false);
        act.interruption[index] = false;
        undo = false;
    }
}
