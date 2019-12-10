using System.Collections;
using System.Collections.Generic;
using Valve.VR;
using Valve.VR.InteractionSystem;
using UnityEngine;

public class VR_GridPointer : MonoBehaviour
{
    UiPointer pointer;
        
    public SteamVR_Action_Boolean PlaceButton;
    public SteamVR_Action_Boolean RotateButton;

    public Shader ghostShader;

    public GameObject dustParticle;

    [SerializeField]
    GameObject selectedPrefab;

    Transform gridManager;
    GameObject tempObject;
    Transform spawnTransform;
    Vector3 desiredRotation = Vector3.zero;
    Vector3 spawnRotation;

    private bool needRotate;
    readonly float axisLimit = 0.05f;

    //Vector3 offset = new Vector3(0, 1, 0);

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
                    PlaceObject(selectedPrefab, spawnTransform);

                }
                else
                {
                    SetGhost(element ,selectedPrefab);
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

    //check multiple conditions
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


            return gridTransform.position;


    }

    void SetGhost(GridElement gridObject, GameObject GhostObject)
    {
        if(gridObject != null && GhostObject != null)
        {
            if (tempObject == null)
            {
                tempObject = Instantiate(GhostObject, gridManager);
                tempObject.layer = 2;

                SetGhostShaders(tempObject.GetComponent<MeshRenderer>().materials);


            }
            else if(gridObject.myType == GridTypes.Colum)
            {
                tempObject.transform.position = gridObject.transform.position;
                tempObject.transform.rotation = Quaternion.identity;
                //Debug.Log("Colum");
            }
            else
            {
                tempObject.transform.position = gridObject.transform.position;
                spawnRotation = gridObject.transform.rotation.eulerAngles + desiredRotation;
                tempObject.transform.eulerAngles = gridObject.transform.rotation.eulerAngles + desiredRotation;
                //Debug.Log("Comumn't");
            }
        }

    }

    void SetGhostShaders(Material[] mats)
    {
        for (int i = 0; i < mats.Length; i++)
        {
            mats[i].shader = ghostShader;
        }
    }

    void PlaceObject(GameObject placeable, Transform t)
    {
        if(placeable != null)
        {
            GameObject spawnedObject = Instantiate(placeable, t.position, Quaternion.Euler(spawnRotation), gridManager);

            //spawn the particle
            Instantiate(dustParticle, t.position, Quaternion.identity);

            //play sound
            spawnedObject.GetComponent<AudioSource>().Play();

            //special offset or default pos
            if (placeable.GetComponent<GridObject>().myType == GridTypes.Colum)
            {
                spawnedObject.transform.rotation = Quaternion.identity;
            }

            t.GetComponent<GridElement>().activeObject = spawnedObject;
            //spawnedObject.transform.SetParent(t);
            spawnedObject.layer = 0;


        }
    }

    public void UpdateSelection(GameObject selectedGo)
    {
        selectedPrefab = selectedGo;
    }
}
