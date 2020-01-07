using System.Collections;
using System.Collections.Generic;
using Valve.VR;
using Valve.VR.InteractionSystem;
using UnityEngine;

/// <summary>
/// this class is a pointer used for spawning objects on Grid
/// </summary>
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
    int Index;

    //Vector3 offset = new Vector3(0, 1, 0);

    private void Awake()
    {
        pointer = GetComponent<UiPointer>();

        if(FindObjectOfType<GridBuilder>() != null)
        {
            gridManager = FindObjectOfType<GridBuilder>().transform;
        }
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
                    Destroy(tempObject);
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

        if (RotateButton.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            desiredRotation = needRotate ? Vector3.zero : new Vector3(0, 180, 0);
            needRotate = !needRotate;
        }
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

    void SetGhost(GridElement gridObject, GameObject GhostObject)
    {
        if(gridObject != null && GhostObject != null)
        {
            // check for special occasion 
            if (tempObject == null)
            {
                tempObject = Instantiate(GhostObject, new Vector3(1000, 1000, 1000), Quaternion.Euler(0,0,0), gridManager);
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

    /// <summary>
    /// Deprecated
    /// </summary>
    /// <param name="mats"> material that gonna swap shader</param>
    void SetGhostShaders(Material[] mats)
    {
        for (int i = 0; i < mats.Length; i++)
        {
            mats[i].shader = Shader.Find("Legacy Shaders/Transparent/Diffuse");
            mats[i].color = new Color(mats[i].color.r, mats[i].color.g, mats[i].color.b, 0.5f);
        }
    }

    void PlaceObject(GameObject placeable, Transform elementTransform)
    {
        if(placeable != null)
        {
            GameObject spawnedObject = Instantiate(placeable, elementTransform.position, Quaternion.Euler(spawnRotation), gridManager);

            // spawn the particle
            Instantiate(dustParticle, elementTransform.position, Quaternion.identity);

            // play sound
            if(spawnedObject.GetComponent<AudioSource>() != null)
                spawnedObject.GetComponent<AudioSource>().Play();

            // special offset or default pos
            if (placeable.GetComponent<GridObject>().myType == GridTypes.Colum)
            {
                spawnedObject.transform.rotation = Quaternion.identity;
            }


            // configure spawned gridobject
            elementTransform.GetComponent<GridElement>().activeObject = spawnedObject;
            spawnedObject.layer = 0;
            spawnedObject.tag = "GridObject";
            spawnedObject.GetComponent<GridObject>().objectIndex = Index;
            spawnedObject.GetComponent<GridObject>().childIndex = elementTransform.GetSiblingIndex();

        }
    }

    /// <summary>
    /// Function used to update selection
    /// this get called in VRGridObjectSpawner usually located in
    /// UIcanvas/UIpanel/Main viewport/GridBuider/
    /// </summary>
    /// <param name="selectedGo">object to spawn</param>
    /// <param name="arrayIndex">a int used for saving the build</param>
    public void UpdateSelection(GameObject selectedGo, int arrayIndex)
    {
        selectedPrefab = selectedGo;
        Index = arrayIndex;
    }
}
