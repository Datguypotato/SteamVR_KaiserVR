using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VR_PointerManager : MonoBehaviour
{
    UIpanel panel;

    VR_pointerObjectSnapper pointerObjectSnapper;
    VR_GridPointer gridpointer;

    private void Awake()
    {
        panel = FindObjectOfType<UIpanel>();

        gridpointer = GetComponent<VR_GridPointer>();
        pointerObjectSnapper = GetComponent<VR_pointerObjectSnapper>();
    }

    private void OnEnable()
    {
        panel.OntabSwitch += ActivatePointer;
    }

    private void Start()
    {
        pointerObjectSnapper.enabled = false;
        gridpointer.enabled = false;
    }

    void ActivatePointer(int index)
    {;
        if(index == 0)
        {
            gridpointer.enabled = false;
            pointerObjectSnapper.enabled = true;
        }
        else if(index == 2 || index == 3)
        {
            gridpointer.enabled = true;
            pointerObjectSnapper.enabled = false;
        }
    }
}
