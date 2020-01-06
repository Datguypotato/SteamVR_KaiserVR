using System.Collections;
using System.Collections.Generic;
using Valve.VR;
using UnityEngine;

/// <summary>
/// with this script you can delete object you grabbed with VR_FixedJointGrab
/// and grid objects
/// </summary>

[RequireComponent(typeof(VR_FixedJointGrab))]
public class VR_DeleteOnGrabbed : MonoBehaviour
{
    public SteamVR_Action_Boolean deleteButton;
    public SteamVR_Input_Sources hand = SteamVR_Input_Sources.RightHand;

    public GameObject particlePrefab;
    GameObject currentGo;

    VR_FixedJointGrab fixedJointGrab;
    UiPointer pointer;

    private void Awake()
    {
        fixedJointGrab = GetComponent<VR_FixedJointGrab>();
        pointer = GetComponent<UiPointer>();
    }

    private void Update()
    {
        if (deleteButton.GetStateDown(hand))
        {
            //GameObject currentGo = new GameObject();

            if (fixedJointGrab.CurrentGrabbedObject != null)
            {
                currentGo = fixedJointGrab.CurrentGrabbedObject;
            }
            else if (pointer.highlightedObject != null && pointer.highlightedObject.GetComponent<GridObjectPlaceAble>() != null)
            {
                currentGo = pointer.highlightedObject;
            }

            if (particlePrefab && currentGo != null)
            {
                Instantiate(particlePrefab, currentGo.transform.position, currentGo.transform.rotation);
                Destroy(currentGo);
            }
        }
    }
}
