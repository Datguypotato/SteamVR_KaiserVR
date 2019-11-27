using System.Collections;
using System.Collections.Generic;
using Valve.VR;
using UnityEngine;

public class VRPanelMover : MonoBehaviour
{
    [Header("Transform settings")]
    public bool disableOnStart;
    [Tooltip("The object that the panel needs to track, like the camera object")]
    public GameObject player;
    public Vector3 offsetFromPlayer;
    [Tooltip("Modify the rotation to point in the oposite direction of normal direction")]
    public bool inverseX, inverseY, inverseZ;
    [Tooltip("When set to true, the Y position of the panel will always be equal to the height of the player, regardless of wether they're looking down or up")]
    public bool retainYposition;

    public SteamVR_Action_Boolean panelButton;
    public SteamVR_Input_Sources hand;

    public delegate void PanelEvents();
    public event PanelEvents ClosePanel;
    public event PanelEvents OpenPanel;

    //[Header("Interaction settings")]
    //[Tooltip("Button to enable/disable the UIPanel with")]
    //public VRTK_ControllerEvents.ButtonAlias key;
    //[Tooltip("which controller to listen to")]
    //public VRTK_ControllerEvents buttonEvent;

    public bool isEnabled { get; private set; }

    void Start()
    {
        isEnabled = false;

        EnableCanvas(!disableOnStart);

        GotoPosition();
    }

    private void Update()
    {
        if (panelButton.GetStateDown(hand))
        {
            ToggleVRPanel();
        }
    }

    public void GotoPosition()
    {
        gameObject.transform.position = player.transform.position;
        gameObject.transform.rotation = player.transform.rotation;

        gameObject.transform.Translate(offsetFromPlayer);

        if (retainYposition)
        {
            gameObject.transform.position = new Vector3(gameObject.transform.position.x, player.transform.position.y + offsetFromPlayer.y, gameObject.transform.position.z);
        }

        gameObject.transform.LookAt(player.transform);

        Vector3 rotationInverse = new Vector3();

        if (inverseX)
        {
            rotationInverse.x = 180;
        }
        if (inverseY)
        {
            rotationInverse.y = 180;
        }
        if (inverseZ)
        {
            rotationInverse.z = 180;
        }

        gameObject.transform.Rotate(rotationInverse);
    }

    public void EnableCanvas(bool value)
    {
        isEnabled = value;
        GetComponent<Canvas>().enabled = isEnabled;
        GotoPosition();
            
        if (!isEnabled)
            ClosePanel?.Invoke();
        else
            OpenPanel?.Invoke();

        //any other stuff required for disabling/enabling canvas goes here
    }

    private void ToggleVRPanel()
    {
        bool newValue = !isEnabled;
        EnableCanvas(newValue);
    }

    public void InvokeOpenPanel()
    {
        Debug.Log("Invoking Succesfull");
        OpenPanel?.Invoke();
    }
}
