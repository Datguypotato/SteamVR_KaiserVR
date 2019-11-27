using System.Collections;
using System.Collections.Generic;
using Valve.VR;
using UnityEngine;

/// <summary>
/// with this script you can delete object you grabbed with VR_FixedJointGrab
/// </summary>

[RequireComponent(typeof(VR_FixedJointGrab))]
public class VR_DeleteOnGrabbed : MonoBehaviour
{
    public SteamVR_Action_Boolean deleteButton;
    public SteamVR_Input_Sources hand = SteamVR_Input_Sources.RightHand;

    public GameObject particlePrefab;

    VR_FixedJointGrab fixedJointGrab;

    private void Awake()
    {
        fixedJointGrab = GetComponent<VR_FixedJointGrab>();
    }

    private void Update()
    {
        if (deleteButton.GetStateDown(hand) && fixedJointGrab.CurrentGrabbedObject != null)
        {
            Transform currentGo = fixedJointGrab.CurrentGrabbedObject.transform;

            if (particlePrefab)
                Instantiate(particlePrefab, currentGo.position, currentGo.rotation);

            Destroy(fixedJointGrab.CurrentGrabbedObject);
        }
    }
}
