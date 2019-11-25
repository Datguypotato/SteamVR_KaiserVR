using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR;
using Valve.VR.InteractionSystem;

/// <summary>
/// create the linerenderer and that you can react to UI 
/// this also gives a raycasthit to vrmaterialchanger
/// 
/// <requirements>
/// gameobject the script is on should be the child of hand gameobject
/// Linerenderer on the same object as this script
/// Sphere gameobject child gameobject this script is on
/// Inputmodule must be in this scene
/// 
/// Camera with the following setting:
/// Clear flags = don't cleat
/// culling mask = nothing
/// FOV = 0
/// clippingmask near = 0.01
/// and disable the component
/// 
/// </requirements>
/// </summary>


public class UiPointer : MonoBehaviour
{
    [Header("laser properties")]
    public RaycastHit hit;
    public float range = 10;
    public float UiOffset;
    
    public GameObject highlightedObject; //{ get; private set; }

    PointerEventData data;

    public Rigidbody highlightedRb;

    //get changed in VR_FixedJointGrab
    public bool isgrabbing;

    float latestFloat;

    //variables for linerenderer
    LineRenderer linerenderer;
    GameObject dot;
    Vector3 endLinePos;
    VR_Inputmodule inputmodule;

    //grabbing
    Hand hand;
    FixedJoint grabbedJoint;
    Rigidbody grabbedRb;

    private void Awake()
    {
        linerenderer = GetComponent<LineRenderer>();
        dot = transform.GetChild(0).gameObject;
        inputmodule = FindObjectOfType<VR_Inputmodule>();
        hand = GetComponentInParent<Hand>();
    }
    
    void Update()
    {
        //only update line when nothing is grabbed
        hit = CreateRayCast(range);
        data = inputmodule.GetData();

        endLinePos = transform.position + (transform.forward * GetLaserPos(data));

        dot.transform.position = endLinePos;
        linerenderer.SetPosition(0, transform.parent.transform.position);
        linerenderer.SetPosition(1, endLinePos);

        //if(highlightedObject != null)
        //    SetKinemetic();
    }

    /// <summary>
    /// Used to create the raycast that is used for grabing and creating the posistion for the linerenderer
    /// </summary>
    /// <param name="lenght"> is the range of the raycast</param>
    RaycastHit CreateRayCast(float lenght)
    {
        RaycastHit _hit;
        Ray ray = new Ray(transform.position, transform.forward);

        Physics.Raycast(ray, out _hit, lenght);

        //update higlightObject
        highlightedObject = _hit.collider != null ? highlightedObject = _hit.collider.gameObject : highlightedObject = null; 

        return _hit;
    }

    /// <summary>
    /// Calculating the range of the linerenderer and dot
    /// ignore calculating if the player is grabbing something
    /// </summary>
    /// <param name="_data">is for UI elements collission</param>
    float GetLaserPos(PointerEventData _data)
    {
        if (!isgrabbing)
        {
            if (_data.pointerCurrentRaycast.distance == 0)
            {
                if (range < hit.distance)
                {
                    latestFloat = range;
                }
                else
                {
                    //latestFloat = hit.distance == 0 ? range : hit.distance;
                    if(hit.distance == 0)
                    {
                        latestFloat = range;
                        //Debug.Log("0");
                    }
                    else
                    {
                        latestFloat = hit.distance;
                        //Debug.Log("1");
                    }
                }
            }
            else
            {
                latestFloat = _data.pointerCurrentRaycast.distance - UiOffset;
            }
        }
        return latestFloat;
    }



    //void SetKinemetic()
    //{
    //    //highlightedRb = highlightedObject.GetComponent<Rigidbody>() != null ? highlightedRb = highlightedObject.GetComponent<Rigidbody>() : highlightedRb = null;

    //    if(highlightedObject.GetComponent<Rigidbody>() != null)
    //    {
    //        if (MaterialTab.activeInHierarchy)
    //        {
    //            highlightedRb = highlightedObject.GetComponent<Rigidbody>();
    //            highlightedRb.isKinematic = true;
    //        }
    //        else
    //        {
    //            highlightedRb.isKinematic = false;
                
    //        }
    //    }
    //    else
    //    {
    //        highlightedRb = null;
    //    }
    //}
    
}