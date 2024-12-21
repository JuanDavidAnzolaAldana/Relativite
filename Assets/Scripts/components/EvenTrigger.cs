using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EvenTrigger : MonoBehaviour
{
    public Activate act;
    public int index;
    public string obj;
    private void OnTriggerEnter(Collider other)
    {
        if (act != null && other.tag == obj)
        {
            act.interruption[index] = true;
        }
    }
}
