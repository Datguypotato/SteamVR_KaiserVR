using System.Collections;
using System.Collections.Generic;
using Valve.VR;
using UnityEngine;

public class VR_GridPointer : MonoBehaviour
{
    UiPointer pointer;
    public SteamVR_Action_Boolean PlaceButton;
    public GameObject wallPrefab;

    private void Awake()
    {
        pointer = GetComponent<UiPointer>();
    }

    private void Update()
    {
        if (PlaceButton.GetStateDown(SteamVR_Input_Sources.RightHand) && IsObjectGrid())
        {
            Debug.Log("Placeing wall");
            Instantiate(wallPrefab, GetGhostPos(pointer.hit.point, pointer.highlightedObject), Quaternion.identity);
        }
    }

    bool IsObjectGrid()
    {
        if (pointer.highlightedObject != null)
            if (pointer.hit.collider != null)
                return true;

        return false;
    }

    Vector3 GetGhostPos(Vector3 hitPoint, GameObject gridObject)
    {
        //create either 2 offset variables or a float offset
        Transform gridTransform = gridObject.transform;
        

        if (gridTransform.lossyScale.x > gridTransform.lossyScale.z)
        {
            if(hitPoint.x > gridTransform.position.x)
            {
                return gridTransform.position + new Vector3(0.5f, 0, 0);
            }
            else
            {
                return gridTransform.position - new Vector3(0.5f, 0, 0);
            }
        }
        else
        {
            if(hitPoint.z > gridTransform.position.z)
            {
                return gridTransform.position + new Vector3(0, 0, 0.5f);
            }
            else
            {
                return gridTransform.position - new Vector3(0, 0, 0.5f);
            }
        }
    }
}
