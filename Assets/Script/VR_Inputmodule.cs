using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Valve.VR;

/// <summary>
/// NGL i also have no idea what this all mean
/// i got it from this https://www.youtube.com/watch?v=3mRI1hu9Y3w
/// </summary>

public class VR_Inputmodule : BaseInputModule
{
    public Camera m_camera;
    public SteamVR_Input_Sources targetSource;
    public SteamVR_Action_Boolean clickAction;

    GameObject currentGo = null;
    PointerEventData data = null;

    protected override void Awake()
    {
        base.Awake();

        data = new PointerEventData(eventSystem);
    }

    public override void Process()
    {
        //reset data
        data.Reset();

        data.position = new Vector2(m_camera.pixelWidth / 2, m_camera.pixelHeight / 2);

        //raycast
        eventSystem.RaycastAll(data, m_RaycastResultCache);
        data.pointerCurrentRaycast = FindFirstRaycast(m_RaycastResultCache);
        currentGo = data.pointerCurrentRaycast.gameObject;

        //clear
        m_RaycastResultCache.Clear();

        //hover
        HandlePointerExitAndEnter(data, currentGo);

        //press
        if (clickAction.GetStateDown(targetSource))
            ProcessPress(data);

        //release
        if (clickAction.GetStateUp(targetSource))
            ProcessRelease(data);
    }

    public PointerEventData GetData()
    {
        return data;
    }

    void ProcessPress(PointerEventData _data)
    {
        // set raycast
        _data.pointerPressRaycast = data.pointerPressRaycast;

        // check for object hit
        // get the downhandler
        GameObject newPointerPress = ExecuteEvents.ExecuteHierarchy(currentGo, _data, ExecuteEvents.pointerDownHandler);

        // if no downhandler 
        // get clickhandler
        if (newPointerPress == null)
            newPointerPress = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentGo);

        // set data
        _data.pressPosition = _data.position;
        _data.pointerPress = newPointerPress;
        _data.rawPointerPress = currentGo;
    }

    void ProcessRelease(PointerEventData _data)
    {
        // excute pointer up
        ExecuteEvents.Execute(_data.pointerPress, _data, ExecuteEvents.pointerUpHandler);

        // check for click handler
        GameObject pointerUpHandler = ExecuteEvents.GetEventHandler<IPointerClickHandler>(currentGo);

        //check if actual
        if(_data.pointerPress == pointerUpHandler)
        {
            ExecuteEvents.Execute(_data.pointerPress, _data, ExecuteEvents.pointerClickHandler);
        }

        // clear selected gameobject
        eventSystem.SetSelectedGameObject(null);

        //reset data
        _data.pressPosition = Vector2.zero;
        _data.pointerPress = null;
        _data.rawPointerPress = null;
    }
}
