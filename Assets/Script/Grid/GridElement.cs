using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridElement : Grid
{
    //get assigned in VR_GridPointer
    public GameObject activeObject;

    Collider[] colliders;
    MeshRenderer meshrender;

    private void Awake()
    {
        colliders = GetComponents<Collider>();
        meshrender = GetComponent<MeshRenderer>();
    }

    private void Update()
    {
        if(activeObject != null)
        {
            if (colliders[0].enabled)
            {
                DissableComponents(false);
            }

        }
        else if(!colliders[0].enabled)
        {
            DissableComponents(true);
        }
    }

    void DissableComponents(bool b)
    {
        meshrender.enabled = b;

        for (int i = 0; i < colliders.Length; i++)
        {
            colliders[i].enabled = b;

        }
    }
}