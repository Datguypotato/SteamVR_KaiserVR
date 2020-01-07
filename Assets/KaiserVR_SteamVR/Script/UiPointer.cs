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
/// Clear flags = don't clear
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
    public float UiOffset;
    public RaycastHit hit { get; private set; }

    [SerializeField] private float defaultLength = 5.0f;
    [SerializeField] private GameObject dot = null;

    public Camera Camera { get; private set; } = null;

    private LineRenderer lineRenderer = null;
    private VR_Inputmodule inputModule = null;

    float targetLength;
    readonly float targetOffset = 0.1f;

    PointerEventData data;
    
    public GameObject highlightedObject { get; private set; }

    //get changed in VR_FixedJointGrab
    public bool isgrabbing;

    Vector3 startScale;
    public Transform fingerOffset;

    private void Awake()
    {
        Camera = GetComponent<Camera>();
        Camera.enabled = false;

        lineRenderer = GetComponent<LineRenderer>();
    }


    private void Start()
    {
        // current.currentInputModule does not work
        inputModule = EventSystem.current.gameObject.GetComponent<VR_Inputmodule>();

        startScale = dot.transform.localScale;
    }

    private void Update()
    {
        UpdateLine();
    }

    private void UpdateLine()
    {
        // Use default or distance
        PointerEventData data = inputModule.Data;
        hit = CreateRaycast(targetLength + targetOffset);

        // filling variables
        highlightedObject = hit.collider != null ? highlightedObject = hit.collider.gameObject : highlightedObject = null;

        if (!isgrabbing)
        {
            // If nothing is hit, set do default length
            float colliderDistance = hit.distance == 0 ? defaultLength : hit.distance;
            float canvasDistance = data.pointerCurrentRaycast.distance == 0 ? defaultLength : data.pointerCurrentRaycast.distance - UiOffset;

            // Get the closest one
            targetLength = Mathf.Min(colliderDistance, canvasDistance);

            UpdateDot(targetLength, canvasDistance);
        }

        // Default
        Vector3 endPosition = transform.position + (transform.forward * targetLength);

        // Set position of the dot
        dot.transform.position = endPosition;

        // Set linerenderer
        lineRenderer.SetPosition(0, fingerOffset.position);
        lineRenderer.SetPosition(1, endPosition);
    }

    void UpdateDot(float length, float canvas)
    {
        if(length != 0 && length == canvas)
        {
            dot.transform.localScale = startScale / 2;
            //Debug.Log("0");
        }
        else
        {
            dot.transform.localScale = startScale;
            //Debug.Log("1");
        }
    }

    private RaycastHit CreateRaycast(float range)
    {
        RaycastHit hit;
        Ray ray = new Ray(fingerOffset.position, transform.forward);
        Physics.Raycast(ray, out hit, range);

        return hit;
    }

}