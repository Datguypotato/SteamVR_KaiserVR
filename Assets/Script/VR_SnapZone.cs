using System.Collections;
using System.Collections.Generic;
using Valve.VR.InteractionSystem;
using UnityEngine;

/// <summary>
/// This is replica of VRTK_SnapZone
/// to make sure it snaps correct make sure the gameobject pivot point is on the bottom
/// The highlighobject must be a child of the object this script is on
/// </summary>


public class VR_SnapZone : MonoBehaviour
{
    public GameObject SnappedObject; //{ get; private set;} //readonly for other scripts to make sure nothing funny happends
    [Tooltip("if this is true then the highligh of the snapzones is always visible")]
    public bool alwaysShowHighlight;
    [Tooltip("how long it takes to lerp the object in snapzone")]
    public float lerpDuration;

    public delegate void SnapEvents();
    public event SnapEvents OnSnap;
    public event SnapEvents UnSnap;

    GameObject highlightObject;
    VR_FixedJointGrab fixedJointGrab;

    private void Awake()
    {
        //setting up variables
        fixedJointGrab = FindObjectOfType<VR_FixedJointGrab>();
        highlightObject = transform.GetChild(0).gameObject;

        //deactive highlighter if alwaysShowHighlight is false
        highlightObject.SetActive(alwaysShowHighlight);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (fixedJointGrab.CurrentGrabbedObject == other.gameObject)
        {
            //activate highlighter
            highlightObject.SetActive(true);
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.GetComponent<Interactable>() != null)
        {
            GameObject lerpObject = other.gameObject;

            //lerp
            if (fixedJointGrab.grabButton.GetStateUp(fixedJointGrab.handGrab) && SnappedObject == null)
            {
                StartCoroutine(UpdateTransformDimensions(lerpObject, this.gameObject, lerpDuration));
                highlightObject.SetActive(false);

                SnappedObject = lerpObject;
            }

            //make sure you can instantly snap back if you grab to it
            if (SnappedObject == fixedJointGrab.CurrentGrabbedObject)
            {
                SnappedObject = null;
            }
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.GetComponent<Interactable>() != null)
        {
            //deactivate highlighter
            if (!alwaysShowHighlight)
            {
                highlightObject.SetActive(false);
            }

            SnappedObject = null;

            //if unspan != null invoke
            UnSnap?.Invoke();
        }
    }

    //IEnumerator is lerping
    protected virtual IEnumerator UpdateTransformDimensions(GameObject ioCheck, GameObject endSettings, float duration)
    {
        float elapsedTime = 0f;
        Transform ioTransform = ioCheck.transform;
        Vector3 startPosition = ioTransform.position;
        Quaternion startRotation = ioTransform.rotation;
        Vector3 startScale = ioTransform.localScale;

        ioCheck.GetComponent<Rigidbody>().isKinematic = true;

        while (elapsedTime <= duration)
        {
            elapsedTime += Time.deltaTime;
            if (ioTransform != null && endSettings != null)
            {
                ioTransform.position = Vector3.Lerp(startPosition, endSettings.transform.position, (elapsedTime / duration));
                ioTransform.rotation = Quaternion.Lerp(startRotation, endSettings.transform.rotation, (elapsedTime / duration));
                ioTransform.localScale = Vector3.Lerp(startScale, endSettings.transform.localScale, (elapsedTime / duration));
            }
            yield return null;
        }
        //if onsnap != null invoke
        OnSnap?.Invoke();
    }
}
