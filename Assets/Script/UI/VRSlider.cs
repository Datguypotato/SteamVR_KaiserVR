using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;

public class VRSlider : MonoBehaviour
{
    public void OnClick()
    {
        Debug.Log("PointerOnClick");
    }

    public void OnDown()
    {
        Debug.Log("PointerOnDown");
    }
}
