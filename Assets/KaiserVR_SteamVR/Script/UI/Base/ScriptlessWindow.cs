using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class ScriptlessWindow : ViewportContent {

    //todo make fade work
    public GameObject playerObj;
    public Transform VrCam;

    [Tooltip("If it is true that means you can teleport from something that you TeleportToTransformChild to. It is however not recommend since it doesn't reset the playerObj parent")]
    public bool enableTeleportWhileRiding;

    Transform playerParentTransform;

    Animator anim;

    //this is kinda like void Start()
    public override void SetupContent(UIpanel controller, GameObject viewPort)
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {


    }

    public void TeleportToTransform(Transform destination)
    {
        if(destination == null)
        {
            Debug.LogError("You forgot to fill the parameters of onclick() you dummy");
        }

        playerObj.transform.position = destination.position;

        gameObject.SetActive(false);
        GameObject.FindObjectOfType<VRPanelMover>().EnableCanvas(true);


        playerObj.transform.SetParent(playerParentTransform);

    }

    public void TeleportToTransformChild(Transform destination)
    {
        if (destination == null)
        {
            Debug.LogError("You forgot to fill the parameters of onclick() you dummy");
        }

        playerParentTransform = playerObj.transform.parent;

        playerObj.transform.position = destination.position;
        playerObj.transform.SetParent(destination);

        gameObject.SetActive(false);
        GameObject.FindObjectOfType<VRPanelMover>().EnableCanvas(true);
        

    }
}
