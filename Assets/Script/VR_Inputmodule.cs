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
    [SerializeField] private UiPointer pointer = null;

    public PointerEventData Data { get; private set; } = null;
    public SteamVR_Input_Sources m_Source = SteamVR_Input_Sources.RightHand;
    public SteamVR_Action_Boolean m_Click = null;

    protected override void Start()
    {
        Data = new PointerEventData(eventSystem);
        Data.position = new Vector2(pointer.Camera.pixelWidth / 2, pointer.Camera.pixelHeight / 2);
    }

    public override void Process()
    {
        eventSystem.RaycastAll(Data, m_RaycastResultCache);
        Data.pointerCurrentRaycast = FindFirstRaycast(m_RaycastResultCache);

        HandlePointerExitAndEnter(Data, Data.pointerCurrentRaycast.gameObject);

        // Press
        if (m_Click.GetStateDown(m_Source))
            Press();
        // Release
        if (m_Click.GetStateUp(m_Source))
            Release();

        ExecuteEvents.Execute(Data.pointerDrag, Data, ExecuteEvents.dragHandler);
    }

    public void Press()
    {
        Data.pointerPressRaycast = Data.pointerCurrentRaycast;

        Data.pointerPress = ExecuteEvents.GetEventHandler<IPointerClickHandler>(Data.pointerPressRaycast.gameObject);
        Data.pointerDrag = ExecuteEvents.GetEventHandler<IDragHandler>(Data.pointerPressRaycast.gameObject);

        ExecuteEvents.Execute(Data.pointerPress, Data, ExecuteEvents.pointerDownHandler);
        ExecuteEvents.Execute(Data.pointerDrag, Data, ExecuteEvents.beginDragHandler);
    }

    public void Release()
    {
        GameObject pointerRelease = ExecuteEvents.GetEventHandler<IPointerClickHandler>(Data.pointerCurrentRaycast.gameObject);

        if (Data.pointerPress == pointerRelease)
            ExecuteEvents.Execute(Data.pointerPress, Data, ExecuteEvents.pointerClickHandler);

        ExecuteEvents.Execute(Data.pointerPress, Data, ExecuteEvents.pointerUpHandler);
        ExecuteEvents.Execute(Data.pointerDrag, Data, ExecuteEvents.endDragHandler);

        Data.pointerPress = null;
        Data.pointerDrag = null;

        Data.pointerCurrentRaycast.Clear();
    }
}
