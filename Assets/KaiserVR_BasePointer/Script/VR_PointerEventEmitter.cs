using System.Collections;
using System.Collections.Generic;
using Valve.VR;
using UnityEngine;


/// <summary>
/// this class is used to emit all the event from PointerEventSystem
/// and also create highlight around objects
/// </summary>
public class VR_PointerEventEmitter : MonoBehaviour
{
    public string tagChecker;
    public SteamVR_Action_Boolean activateAnimationButton;
    public SteamVR_Input_Sources hand;
    public Material HoverMat;

    UiPointer pointer;
    GameObject _highlightedObject;

    PointerEvent pointerEvent;

    private void Awake()
    {
        pointer = FindObjectOfType<UiPointer>();
    }

    private void Update()
    {
        if (pointer.highlightedObject != null && pointer.highlightedObject.CompareTag(tagChecker))
        {
            pointerEvent = pointer.highlightedObject.GetComponent<PointerEvent>();


            // Invoke alls the event function here
            // Heck PointerEventSystem and PointerEvent for more information
            pointerEvent.onHoverEvent.Invoke(pointer);

            Animator anim = pointer.highlightedObject.GetComponent<Animator>();
            if (_highlightedObject == null && anim.GetCurrentAnimatorStateInfo(0).normalizedTime > 1 && !anim.IsInTransition(0))
            {
                Transform highlightedTransform = pointer.highlightedObject.transform;

                //setting up highlight object
                _highlightedObject = Instantiate(pointer.highlightedObject, highlightedTransform.position, highlightedTransform.rotation);
                Destroy(_highlightedObject.GetComponent<Animator>());
                _highlightedObject.transform.parent = highlightedTransform;
                _highlightedObject.layer = 2; //make it ignore uiipointer
                _highlightedObject.GetComponent<MeshRenderer>().material = HoverMat;
            }

            if (activateAnimationButton.GetStateDown(hand))
            {
                pointerEvent.OnClickEvent.Invoke(pointer);
                Destroy(_highlightedObject);
            }
        }
        else
        {
            // resetting values
            if(pointerEvent != null)
            {
                pointerEvent.OnExitEvent.Invoke(pointer);
                pointerEvent = null;
            }

            if(_highlightedObject != null)
                Destroy(_highlightedObject);
        }

    }
}
