using System.Collections;
using System.Collections.Generic;
using Valve.VR;
using UnityEngine.SceneManagement;
using UnityEngine;

/// <summary>
/// this class handles Spawning object on grid ground
/// </summary>
public class VR_pointerObjectSnapper : MonoBehaviour
{
    UiPointer pointer;
    VRPanelMover panelMover;

    public GameObject selectedObject;
    public SteamVR_Action_Boolean placeObjectButton;
    public SteamVR_Action_Boolean RotateButton;

    public Material ghostMaterial;
    public float offset = 0.6f;

    GameObject ghostObject;

    List<Transform> childList;
    Vector3 desiredRotation;

    bool changeSnapDir;
    private readonly string objectTag = "GridObject";

    private void Awake()
    {
        pointer = GetComponent<UiPointer>();
        panelMover = FindObjectOfType<VRPanelMover>();
    }

    private void Update()
    {
        if (IsGround() && selectedObject != null)
        {
            SetGhost(pointer.hit.point);

            PlaceObject(pointer.hit.point, Quaternion.identity);

        }
        else if (pointer.highlightedObject != null && pointer.highlightedObject.GetComponent<GridObjectPlaceAble>() != null)
        {
            Debug.Log("0");
                SideSnap();

        }
        else if (ghostObject != null)
        {
            Destroy(ghostObject);
        }


        if (RotateButton.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            changeSnapDir = !changeSnapDir;
            desiredRotation += new Vector3(0, 90, 0);
        }
    }

    //check multiple conditions
    bool IsGround()
    {
        if (pointer.highlightedObject != null)
                if (pointer.highlightedObject.GetComponent<GridObject>() != null)
                    return true;

        return false;
    }

    void SetGhost(Vector3 spawnOffset)
    {
        if (ghostObject == null && selectedObject != null)
        {
            ghostObject = Instantiate(selectedObject, spawnOffset, Quaternion.Euler(desiredRotation));
            ghostObject.layer = 2;

            childList.Clear();
            GetAllChildren(ghostObject);
            for (int i = 0; i < childList.Count; i++)
            {
                childList[i].gameObject.layer = 2;
            }
        }
        else if (ghostObject != null)
        {
            ghostObject.transform.rotation = Quaternion.Euler(desiredRotation);
            ghostObject.transform.position = spawnOffset;
        }
    }

    void SetGhostShaders(Material[] mats)
    {
        for (int i = 0; i < mats.Length; i++)
        {
            mats[i] = ghostMaterial;
        }
    }
    
    void SideSnap()
    {
        Vector3 normalHit = pointer.hit.normal;
        Vector3 snapPoint = Vector3.zero;

        snapPoint = new Vector3(offset * normalHit.x, 0, offset * normalHit.z) + pointer.hit.transform.position;
        desiredRotation = pointer.hit.transform.rotation.eulerAngles;

        if (snapPoint != pointer.hit.transform.position)
        {
            SetGhost(snapPoint);
            PlaceObject(snapPoint, pointer.hit.transform.rotation, pointer.hit.transform);
        }
    }

    //if snap to side 
    void PlaceObject(Vector3 pos, Quaternion spawnRotation, Transform hitTransform)
    {
        if (placeObjectButton.GetStateDown(SteamVR_Input_Sources.RightHand) && selectedObject != null)
        {
            GameObject spawnObject;
            if (hitTransform != null)
            {
                spawnObject = Instantiate(selectedObject, pos, Quaternion.Euler(desiredRotation));
            }
            else
            {
                spawnObject = Instantiate(selectedObject, pos, Quaternion.Euler(desiredRotation));
                Debug.Log("There was no parent");
            }
            spawnObject.tag = objectTag;
            
        }
    }

    //if snap to ground
    void PlaceObject(Vector3 pos, Quaternion spawnRotation)
    {
        //this need rework😀😶😏😴😫🙂☺😐🤐😪😴🙃🤔🤔
        if (placeObjectButton.GetStateDown(SteamVR_Input_Sources.RightHand) && selectedObject != null)
        {
            GameObject spawnObject = Instantiate(selectedObject, pos, Quaternion.Euler(desiredRotation));
            spawnObject.tag = objectTag;
        }
    }

    void GetAllChildren(GameObject g)
    {
        int childCount = g.transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            GameObject temp = g.transform.GetChild(i).gameObject;
            childList.Add(temp.transform);
            if (temp.transform.childCount > 0)
            {
                GetAllChildren(temp);
            }
        }
    }
}