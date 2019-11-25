using System.Collections;
using System.Collections.Generic;
using Valve.VR;
using UnityEngine;

public class VR_PointTeleporter : MonoBehaviour
{
    SteamVR_Action_Boolean teleportButton;

    private void Update()
    {
        if (teleportButton.GetState(SteamVR_Input_Sources.RightHand))
        { 
            // show linerenderer

            // change color if teleportable

            
        }
        else if (teleportButton.GetStateUp(SteamVR_Input_Sources.RightHand))
        {
            // start fade

            // teleport

            
        }
    }
}
