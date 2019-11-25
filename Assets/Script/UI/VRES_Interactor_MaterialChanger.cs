using System.Collections;
using System.Collections.Generic;
using Valve.VR.InteractionSystem;
using Valve.VR;
using UnityEngine;

public class VRES_Interactor_MaterialChanger : MonoBehaviour
{
    public int selectedMaterialIndex { get; set; }

    public SteamVR_Action_Boolean ChangeMatButton;

    private GameObject panelCanvas;
    private UIpanel panelcontroller;
    private VRPanelMover panelMover;

    private void Awake()
    {
        panelcontroller = panelCanvas.GetComponentInChildren<UIpanel>();
        panelMover = panelCanvas.GetComponent<VRPanelMover>();    }

    //public void ButtonHandler(object sender, ControllerInteractionEventArgs e)
    //{
    //    if (pointer.IsStateValid())
    //    {
    //        if (eventArgs.target.GetComponent<VRES_MaterialChanger>())
    //        {
    //            eventArgs.target.GetComponent<VRES_MaterialChanger>().Change(eventArgs.raycastHit.triangleIndex, selectedMaterialIndex);
    //        }
    //    }
    //    else
    //    {
    //        Debug.Log("pointer state is not valid");
    //    }
    //}
}
