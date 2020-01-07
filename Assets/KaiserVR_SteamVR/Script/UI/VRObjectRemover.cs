using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Controller class for removing pointed-at objects with a given tag
/// </summary>
public class VRObjectRemover : MonoBehaviour
{
    [Tooltip("The button which is used for triggering the deleting objects function")]
    //public VRTK_ControllerEvents.ButtonAlias deletionButton;
    public string deleteObjectTag;

    //private VRTK_Pointer pointer;
    //private VRTK_ControllerEvents events;

    private GameObject targetedObject;

    private void Awake()
    {
        //pointer = GetComponent<VRTK_Pointer>();
        //events = GetComponent<VRTK_ControllerEvents>();
    }

    private void OnEnable()
    {
        //events.SubscribeToButtonAliasEvent(deletionButton, true, ButtonHandler);
        //pointer.PointerStateValid += PointerStateValidHandler;
    }

    private void OnDisable()
    {
        //events.UnsubscribeToButtonAliasEvent(deletionButton, true, ButtonHandler);
        //pointer.PointerStateValid -= PointerStateValidHandler;
    }

    public void DeleteAllObjects()
    {
        GameObject[] objects = GameObject.FindGameObjectsWithTag(deleteObjectTag);

        Array.ForEach(objects, obj => Destroy(obj));
    }

    public void PointerStateValidHandler(object sender)
    {
        //targetedObject = e.target.gameObject;
    }

    public void ButtonHandler(object sender)
    {
        //if (pointer.IsStateValid())
        //{
        //    if (targetedObject.CompareTag(deleteObjectTag))
        //    {
        //        Destroy(targetedObject);
        //    }
        //}
        //else
        //{
        //    Debug.Log("pointerstate is not valid");
        //}
    }
}
