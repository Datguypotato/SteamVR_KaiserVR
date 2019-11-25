using System.Collections;
using System.Collections.Generic;
using Valve.VR.InteractionSystem;
using Valve.VR;
using UnityEngine;

public class VR_SwapHandModel : MonoBehaviour
{
    public GameObject leftHand;
    public GameObject rightHand;

    public GridContainerWindow grid;

    private void OnEnable()
    {
        grid.GetComponent<GridContainerWindow>();
        grid.open += SetRenderModel;
    }

    public void SetRenderModel()
    {
        for (int handIndex = 0; handIndex < Player.instance.hands.Length; handIndex++)
        {
            Hand hand = Player.instance.hands[handIndex];
            if (hand != null)
            {
                if (hand.handType == SteamVR_Input_Sources.RightHand)
                    hand.SetRenderModel(rightHand);
                if (hand.handType == SteamVR_Input_Sources.LeftHand)
                    hand.SetRenderModel(leftHand);
            }
        }
    }
}
