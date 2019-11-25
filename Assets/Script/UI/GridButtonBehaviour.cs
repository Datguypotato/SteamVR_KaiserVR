using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public abstract class GridButtonBehaviour : MonoBehaviour
{
    //amount of objects so that the parent grid knows exactly how many buttons are needed,
    //needs to be set in the setup method
    public int NumberOfObjects { get; protected set; }

    //refference to the parent grid
    protected ViewportContent controller;

    //method for initial setup of script
    public abstract void Setup(ViewportContent controller);

    //method for modifying/setting up the current button
    public abstract void SetUpButton(GameObject buttonGameObject, int index);

    //method which will be executed in the buttons event
    public abstract void ButtonAction(int index);

    public abstract void ButtonAction(int index, Vector3 pos);

    //method that gets executed when window is openened
    public virtual void EnableBehaviour()
    {
        //standard implementation to be added later
    }

    //method that gets executed when window is closed
    public virtual void DisableBehaviour()
    {
        //standard implementation to be added later
    }
}
