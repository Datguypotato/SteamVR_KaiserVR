using System.Collections;
using System.Collections.Generic;
using Valve.VR;
using Valve.VR.InteractionSystem;
using UnityEngine;

/// <summary>
/// This class handle grabbing from the uiPointer using rigidbodys and fixed joints
/// 
/// <Requirement>
/// UiPointer
/// </Requirement>
/// </summary>
public class VR_FixedJointGrab : MonoBehaviour
{
    /// <summary>
    /// this is get emitted when you grab and ungrab something
    /// </summary>
    public delegate void OnJointEvent();
    public event OnJointEvent OnGrab;
    public event OnJointEvent UnGrab;

    [Header("grab properties")]
    public SteamVR_Action_Boolean grabButton;
    public SteamVR_Input_Sources handGrab;
    public GameObject CurrentGrabbedObject{ get; private set; }

    UiPointer pointer;
    GameObject dot;
    LineRenderer lineRenderer;

    private void Awake()
    {
        pointer = GetComponent<UiPointer>();
        dot = transform.GetChild(0).gameObject;
        lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update()
    {
        if (grabButton.GetStateDown(handGrab) && pointer.highlightedObject != null)
            GrabFromAttachPoint(pointer.highlightedObject.transform.root.gameObject);
        else if (grabButton.GetStateUp(handGrab))
            UngrabFromAttachPoint();
    }

    /// <summary>
    /// grabbing a object from the linerenderer 
    /// </summary>
    /// <param name="focusGo"></param>is the object that is going to be grabbed
    void GrabFromAttachPoint(GameObject focusGo)
    {
        pointer.isgrabbing = true;

        if (focusGo != null && focusGo.GetComponent<Rigidbody>() != null)
        {
            //event
            //if != null
            OnGrab?.Invoke();

            // setting variables
            CurrentGrabbedObject = focusGo;
            Rigidbody grabbedRb = focusGo.GetComponent<Rigidbody>();

            // Configure variables
            if (focusGo.GetComponent<FixedJoint>() == null)
            {
                FixedJoint grabbedJoint = focusGo.AddComponent<FixedJoint>();
                grabbedJoint.connectedBody = dot.GetComponent<Rigidbody>();
            }

            grabbedRb.useGravity = false;
            grabbedRb.isKinematic = false;
        }
    }

    /// <summary>
    /// ungrab the object and resetting values
    /// </summary>
    void UngrabFromAttachPoint()
    {
        //event
        //if != null
        UnGrab?.Invoke();

        pointer.isgrabbing = false;

        if (CurrentGrabbedObject != null && CurrentGrabbedObject.GetComponent<Rigidbody>() != null)
        {
            // set up the rotation
            Quaternion childRotation = CurrentGrabbedObject.transform.GetChild(0).rotation;
            Rigidbody grabbedRb = CurrentGrabbedObject.GetComponent<Rigidbody>();
            FixedJoint grabbedJoint = CurrentGrabbedObject.GetComponent<FixedJoint>();

            CurrentGrabbedObject.transform.rotation = childRotation;
            CurrentGrabbedObject.transform.GetChild(0).localEulerAngles = Vector3.zero;

            // empty variables
            Destroy(grabbedJoint);
            grabbedRb.useGravity = true;
            CurrentGrabbedObject = null;
        }
    }

    ///////////////////////////
    ///Deprecated
    ///////////////////////////
    //void DestroyObject(GameObject deleteObject)
    //{
    //    if(deleteObject != null && deleteObject.GetComponent<Interactable>())
    //    {
    //        Instantiate(particle, deleteObject.transform.position, deleteObject.transform.rotation);
    //        Destroy(deleteObject);
    //    }
    //}

    //public void ToggleDeleteBeam()
    //{
    //    deleteMode = !deleteMode;
    //    CurrentGrabbedObject = null;

    //    Debug.Log("Delete Mode: " + deleteMode);

    //    if (deleteMode)
    //    {
    //        lineRenderer.startColor = Color.red;
    //        lineRenderer.endColor = Color.red;

    //        dot.GetComponent<Renderer>().material.color = Color.red;
    //    }
    //    else
    //    {
    //        lineRenderer.startColor = Color.green;
    //        lineRenderer.endColor = Color.green;

    //        dot.GetComponent<Renderer>().material.color = Color.green;
    //    }
    //}
}
