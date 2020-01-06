using System.Collections;
using System.Collections.Generic;
using Valve.VR;
using Valve.VR.InteractionSystem;
using UnityEngine;

public class VR_Door : MonoBehaviour
{
    public SteamVR_Action_Boolean openDoorButton;

    bool isopen;

    private void OnTriggerStay(Collider other)
    {
        if(other.GetComponent<Hand>() != null && other.GetComponent<Hand>().handType == SteamVR_Input_Sources.RightHand)
        {
            if (openDoorButton.GetStateDown(SteamVR_Input_Sources.RightHand))
            {
                Debug.Log("Door interacted");
                Animator anim = GetComponent<Animator>();
                anim.Play(0);
                if (!isopen)
                    anim.SetFloat("Speed", 1);
                else
                    anim.SetFloat("Speed", -1);

                isopen = !isopen;
            }
        }
    }
}
