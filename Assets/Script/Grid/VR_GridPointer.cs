using System.Collections;
using System.Collections.Generic;
using Valve.VR;
using UnityEngine;

public class VR_GridPointer : MonoBehaviour
{
    UiPointer pointer;
    public SteamVR_Action_Boolean PlaceButton;
    public SteamVR_Action_Boolean RotateButton;
    public GameObject wallPrefab;
    //public GameObject ghostWallPrefab;

    //WIP
    public GameObject selectedPrefab;
    //_WIP

    Transform gridManager;

    GameObject tempObject;
    Transform spawnTransform;
    Vector3 desiredRotation = Vector3.zero;
    Vector3 spawnRotation;

    bool needRotate;
    readonly float axisLimit = 0.05f;

    Vector3 offset = new Vector3(0, 1, 0);

    private void Awake()
    {
        pointer = GetComponent<UiPointer>();
        gridManager = FindObjectOfType<GridBuilder>().transform;
    }

    private void Update()
    {
        if (IsObjectGrid())
        {
            GridElement element = null;

            //get element
            if(pointer.highlightedObject.GetComponent<GridElement>() != null)
            {
                element = pointer.highlightedObject.GetComponent<GridElement>();
                spawnTransform = element.transform;
            }

            if(element != null)
            {
                if (PlaceButton.GetStateDown(SteamVR_Input_Sources.RightHand))
                {
                    PlaceObject(element.prefab, spawnTransform);

                }
                else
                {
                    SetGhost(element);
                }
            }
        }
        else if(tempObject != null)
        {
            Destroy(tempObject);
        }

        //if (RotateButton.GetStateDown(SteamVR_Input_Sources.RightHand))
        //{
        //    desiredRotation = needRotate ? Vector3.zero : new Vector3(0, 90, 0);
        //    needRotate = !needRotate;
        //}
    }

    //check multiple condition
    bool IsObjectGrid()
    {
        if (pointer.highlightedObject != null)
            if (pointer.hit.collider != null)
                if (pointer.highlightedObject.transform.IsChildOf(gridManager))
                    return true;
                

        return false;
    }

    //i was overthinking this lol
    //so this is completly useless
    Vector3 GetGhostPos(Vector3 hitPoint, GameObject gridObject)
    {
        //todo
        //create either 2 offset variables or a float offset
        Transform gridTransform = gridObject.transform;

        float deltaZ = hitPoint.z - gridTransform.position.z;
        float deltaX = hitPoint.x - gridTransform.position.x;

        if (deltaZ > deltaX)
        {
            if (deltaZ > axisLimit)
            {
                return gridTransform.position - new Vector3(0, 0, -0.5f);

            }
            else if (deltaZ < -axisLimit)
            {
                return gridTransform.position - new Vector3(0, 0, 0.5f);
            }
        }
        else
        {
            if (deltaX > axisLimit)
            {
                return gridTransform.position - new Vector3(-0.5f, 0, 0);
            }
            else if (deltaX < -axisLimit)
            {
                return gridTransform.position - new Vector3(0.5f, 0, 0);
            }
        }
            //}

            //Debug.Log(deltaZ + " - " + deltaX);
            //Debug.Log(deltaZ - deltaX);
            //if (deltaZ > axisLimit && deltaX > axisLimit)
            //{
            //    if(deltaZ - deltaX > 0)
            //    {
            //        return gridTransform.position - new Vector3(0, 0, 0.5f);
            //    }
            //    else
            //    {
            //        return gridTransform.position - new Vector3(0.5f, 0, 0);
            //    }
            //}
            //else if(deltaX < -axisLimit && deltaZ < -axisLimit)
            //{

            //    if (deltaZ - deltaX < 0)
            //    {
            //        return gridTransform.position - new Vector3(-0.5f, 0, 0);
            //    }
            //    else
            //    {
            //        return gridTransform.position - new Vector3(0.5f, 0, 0);
            //    }
            //}

            return gridTransform.position;


    }

    void SetGhost(GridElement element)
    {
        if(element != null)
        {
            if (tempObject == null)
            {
                tempObject = Instantiate(element.ghostPrefab);
            }
            else if (element.prefab == wallPrefab)
            {
                tempObject.transform.position = element.transform.position + offset;
                spawnRotation = element.transform.rotation.eulerAngles + desiredRotation;
                tempObject.transform.eulerAngles = element.transform.rotation.eulerAngles + desiredRotation;
            }
            else
            {
                tempObject.transform.position = element.transform.position + offset;
                spawnRotation = desiredRotation;
                //tempObject.transform.eulerAngles = element.transform.rotation.eulerAngles;
            }
        }

    }

    void PlaceObject(GameObject placeable, Transform t)
    {
        if(placeable != null)
        {
            GameObject spawnedObject = Instantiate(placeable, t.position + offset, Quaternion.Euler(spawnRotation));
            spawnedObject.transform.SetParent(t);
            spawnedObject.layer = 0;
        }
    }

}
