﻿using System.Collections;
using System.Collections.Generic;
using Valve.VR;
using UnityEngine;

public class VR_RotateMoveObject : MonoBehaviour
{
    public float rotateSpeed;
    public float moveSpeed;

    public SteamVR_Action_Vector2 joystick;
    public Vector2 joystickPos;
    
    VR_FixedJointGrab fixedJointGrab;

    private void Awake()
    {
        fixedJointGrab = GetComponent<VR_FixedJointGrab>();
    }

    private void Update()
    {
        joystickPos = joystick.GetAxis(SteamVR_Input_Sources.RightHand);

        if(fixedJointGrab.CurrentGrabbedObject != null)
        {
            RotateObject(fixedJointGrab.CurrentGrabbedObject.transform.GetChild(0).gameObject);
            MoveObject(gameObject);
        }
        else
        {
            transform.localPosition = Vector3.zero;
        }
        
    }

    void MoveObject(GameObject moveObject)
    {

        if(joystickPos.y > 0.8f || joystickPos.y < -0.8f)
        {
            moveObject.transform.Translate(Vector3.forward * joystickPos.y * moveSpeed * Time.deltaTime);
        }
    }

    void RotateObject(GameObject rotateObject)
    {
        if(joystickPos.x > 0.8f || joystickPos.x < -0.8f)
        {
            rotateObject.transform.Rotate(Vector3.up * joystickPos * rotateSpeed, Space.World);
            
        }
    }
}
