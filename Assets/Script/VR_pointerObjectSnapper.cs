using System.Collections;
using System.Collections.Generic;
using Valve.VR;
using Valve.VR.InteractionSystem;
using UnityEngine;

public class VR_pointerObjectSnapper : MonoBehaviour
{
    UiPointer pointer;
    public GameObject selectedObject;
    public SteamVR_Action_Boolean placeObjectButton;
    public SteamVR_Action_Boolean RotateButton;


    public Shader ghostShader;
    public float offset = 0.6f;

    GameObject ghostObject;

    public Vector3 desiredRotation;
    public List<Transform> childList;

    private void Awake()
    {
        pointer = GetComponent<UiPointer>();
        
    }

    private void Update()
    {
        if (IsGround() && selectedObject != null)
        {
            SetGhost(pointer.hit.point);

            placeObject(pointer.hit.point, Quaternion.identity);

        }
        else if (pointer.highlightedObject != null && pointer.highlightedObject.GetComponent<GridObject>() != null)
        {
            //if(pointer.highlightedObject.GetComponent<GridObject>().myType == GridTypes.objectsnap)
                SideSnap();

        }
        else if (ghostObject != null)
        {
            Destroy(ghostObject);
        }


        if (RotateButton.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            desiredRotation += new Vector3(0, 90, 0);
        }
    }

    //check multiple conditions
    bool IsGround()
    {
        if (pointer.highlightedObject != null)
            if (pointer.highlightedObject.tag == "GridObject")
                if (pointer.highlightedObject.GetComponent<GridObject>().myType == GridTypes.Floor)
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
                //SetGhostShaders(childList[i].GetComponent<MeshRenderer>().materials);
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
        //for (int i = 0; i < mats.Length; i++)
        //{
        //    StandardShaderUtils.ChangeRenderMode(mats[i], StandardShaderUtils.BlendMode.Transparent);
        //    //mats[i].shader = ghostShader;
        //}
    }
    
    void SideSnap()
    {
        Vector3 normalHit = pointer.hit.normal;
        normalHit = pointer.hit.transform.TransformDirection(normalHit);

        Vector3 snapPoint = Vector3.zero;

        if (normalHit == pointer.hit.transform.right)
        {
            snapPoint = new Vector3(offset, 0, 0) + pointer.hit.transform.position;
            SetGhost(snapPoint);

        }
        else if (normalHit == -pointer.hit.transform.right)
        {
            snapPoint = new Vector3(-offset, 0, 0) + pointer.hit.transform.position;
            SetGhost(snapPoint);
        }

        if (snapPoint != Vector3.zero)
        {
            placeObject(snapPoint, pointer.hit.transform.rotation);
        }
    }

    void placeObject(Vector3 pos, Quaternion spawnRotation)
    {
        if (placeObjectButton.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            GameObject spawnenObject = Instantiate(selectedObject, pos, spawnRotation);
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
