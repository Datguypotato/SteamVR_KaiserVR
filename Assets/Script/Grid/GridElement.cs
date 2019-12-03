using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridElement : MonoBehaviour
{
    public GameObject prefab;
    public GameObject ghostPrefab;

    public GameObject activeObject;

    public Collider[] colliders;
    public MeshRenderer meshrender;

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
