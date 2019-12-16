using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// This is for giving multiple objects who are snapped the same materials
/// Might wanna change firstMat and secondMat to a list/array
/// 
/// 
/// You must create a Resources folder for all the material
/// </summary>

public class VR_MaterialPicker : MonoBehaviour
{
    public GameObject materialPicker;
    public GameObject secondMaterialPicker;

    [Space(5)]
    private List<GameObject> snappedObjects;
    private VR_SnapZone[] snapzones;

    private Material[] resourceMaterials;
    private List<Renderer> snappedRenderer;

    private List<GameObject> childList;
    public VRMaterialSwitcher switcher;

    public int arrayTest;
    public int secondArrayTest;

    private void Awake()
    {
        snapzones = FindObjectsOfType<VR_SnapZone>();

        for (int i = 0; i < snapzones.Length; i++)
        {
            snapzones[i].OnSnap += SetMaterials;
            snapzones[i].UnSnap += SetMaterials;
        }

        switcher.OnChange += SetMaterials;

        resourceMaterials = Resources.LoadAll("Materials", typeof(Material)).Cast<Material>().ToArray();
    }

    private void SetMaterials()
    {

        ResetLists();

        GetAllObjects();

        CompareMaterials();

        AssignMaterials();
    }

    private void Update()
    {
        
    }

    void GetAllObjects()
    {

        for (int i = 0; i < snapzones.Length; i++)
        {
            if (snapzones[i].SnappedObject != null)
            {
                snappedObjects.Add(snapzones[i].SnappedObject);
            }
        }

        // get all children of snapped objects
        for (int i = 0; i < snappedObjects.Count; i++)
        {
            GetAllChildren(snappedObjects[i]);
        }
    }

    void CompareMaterials()
    {

        for (int i = 0; i < resourceMaterials.Length; i++)
        {
            //check if the material is in the resource folder
            if(materialPicker.GetComponent<Renderer>().material.mainTexture == resourceMaterials[i].mainTexture)
            {
                arrayTest = i;
            }

            if(secondMaterialPicker.GetComponent<Renderer>().material.mainTexture == resourceMaterials[i].mainTexture)
            {
                secondArrayTest = i;
            }
        }
    }

    void AssignMaterials()
    {
        for (int i = 0; i < childList.Count; i++)
        {
            if(childList[i].GetComponent<Renderer>() != null)
                snappedRenderer.Add(childList[i].GetComponent<Renderer>());
        }

        for (int i = 0; i < snappedRenderer.Count; i++)
        {
            snappedRenderer[i].materials[0].mainTexture = resourceMaterials[arrayTest].mainTexture;
            snappedRenderer[i].materials[1].mainTexture = resourceMaterials[secondArrayTest].mainTexture;
        }

    }

    void GetAllChildren(GameObject g)
    {
        int childCount = g.transform.childCount;

        for (int i = 0; i < childCount; i++)
        {
            GameObject temp = g.transform.GetChild(i).gameObject;
            childList.Add(temp);
            if (temp.transform.childCount > 0)
            {
                GetAllChildren(temp);
            }
        }
    }

    void ResetLists()
    {
        snappedObjects.Clear();
        childList.Clear();
        snappedRenderer.Clear();
    }
}