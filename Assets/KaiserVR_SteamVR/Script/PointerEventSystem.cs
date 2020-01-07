using UnityEngine.Events;
using UnityEngine;

public static class PointerEventSystem
{
    [System.Serializable]
    public class PointerOnHoverEvent : UnityEvent<UiPointer>
    {
        
    }

    [System.Serializable]
    public class PointerOnClickEvent : UnityEvent<UiPointer>
    {

    }

    [System.Serializable]
    public class PointerOnExitEvent : UnityEvent<UiPointer>
    {

    }
}
