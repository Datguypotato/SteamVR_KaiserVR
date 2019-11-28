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
    public GameObject ghostWallPrefab;

    //WIP
    public GameObject selectedPrefab;
    //_WIP

    Transform gridManager;

    GameObject tempObject;
    bool needRotate;
    Vector3 desiredRotation = Vector3.zero;
    readonly float axisLimit = 0.05f;

    private void Awake()
    {
        pointer = GetComponent<UiPointer>();
        gridManager = FindObjectOfType<GridBuilder>().transform;
    }

    private void Update()
    {
        if (IsObjectGrid())
        {
            Transform spawnTransform = pointer.highlightedObject.GetComponent<GridDistanceChecker>().GetClosestTarget(pointer.hit.point);
            if (PlaceButton.GetStateDown(SteamVR_Input_Sources.RightHand))
            {
                //spawn new object
                if(spawnTransform.childCount == 0)
                {
                    PlaceObject(wallPrefab, spawnTransform);
                }
                else //replace object mesh
                {
                    MeshFilter childMeshFilter = spawnTransform.GetChild(0).GetComponent<MeshFilter>();

                    if(childMeshFilter.mesh != selectedPrefab.GetComponent<MeshFilter>().mesh)
                    {
                        PlaceObject(selectedPrefab, spawnTransform);
                    }
                }
                
            }
            else
            {
                SetGhost(pointer.highlightedObject.GetComponent<GridDistanceChecker>());
            }
        }
        else if(tempObject != null)
        {
            Destroy(tempObject);
        }

        if (RotateButton.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            desiredRotation = needRotate ? Vector3.zero : new Vector3(0, 90, 0);
            needRotate = !needRotate;
        }
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

    void SetGhost(GridDistanceChecker distanceChecker)
    {
        if(tempObject == null)
        {
            tempObject = Instantiate(ghostWallPrefab);
        }
        else
        {
            tempObject.transform.position = distanceChecker.GetClosestTarget(pointer.hit.point).position + new Vector3(0, 1, 0);
            tempObject.transform.eulerAngles = desiredRotation;

        }

    }

    void PlaceObject(GameObject placeable, Transform t)
    {
        GameObject spawnedObject = Instantiate(placeable, t.position + new Vector3(0, 1, 0), Quaternion.Euler(desiredRotation));
        spawnedObject.transform.SetParent(t);
        spawnedObject.layer = 0;
    }
}
