using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventTester : MonoBehaviour
{
    public void Hovering()
    {
        Debug.Log("Hovering!");
    }
    public void CLicking()
    {
        Debug.Log("Click");
        //Animator anim = GetComponent<Animator>();
        //anim.Play(0);
    }
    public void Exiting()
    {
        Debug.Log("Exiting");
    }
}
