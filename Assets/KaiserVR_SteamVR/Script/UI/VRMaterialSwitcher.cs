using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Valve.VR;

/// <summary>
/// Replica of VRES_MaterialChanger
/// </summary>

public class VRMaterialSwitcher : GridButtonBehaviour
{
    [Tooltip("only objects with this tag can have their materials edited")]
    public string changeMaterialTag;
    [Tooltip("color to highlight selected material with in the button grid")]
    public Color selectedColor;
    [Tooltip("the list of available materials")]
    public MaterialData[] materials;

    private bool hasObjectSelected;

    //refferences to the currently selected button, material and object in the scene
    //also keeps track of the color of the button before it was selected
    private Material currentMaterial;
    public GameObject currentMaterialButton;
    private Color currentButtonColor;
    private RaycastHit currentlySelectedObject;

    private List<GameObject> buttons = new List<GameObject>();

    UiPointer pointer;
    public SteamVR_Action_Boolean ChangeMatButton;

    //for VR_MaterialPicker
    public delegate void OnChangeMaterial();
    public event OnChangeMaterial OnChange;


    public override void Setup(ViewportContent controller)
    {
        this.controller = controller;
        NumberOfObjects = materials.Length;
        pointer = FindObjectOfType<UiPointer>();

        if (NumberOfObjects > 0)
        {
            currentMaterial = materials[0].material; 
        }
    }

    public override void SetUpButton(GameObject buttonGameObject, int index)
    {
        MaterialData currentMaterial = materials[index];
        Button button = buttonGameObject.GetComponent<Button>();
        Text buttonText = buttonGameObject.GetComponentInChildren<Text>();

        CreateMaterialPreview(currentMaterial.material, button.image);

        if (currentMaterial.displayName)
        {
            buttonText.text = currentMaterial.material.name;
        }
        else
        {
            buttonText.text = "";
        }

        button.onClick.AddListener(delegate { ButtonAction(index); });
        buttons.Add(buttonGameObject);
    }

    public override void ButtonAction(int index)
    {
        Debug.Log("material");
        if (currentMaterialButton != null && currentMaterialButton.GetComponent<Image>() != null)
        {
            currentMaterialButton.GetComponent<Image>().color = currentButtonColor;
        }

        currentMaterialButton = buttons[index];
        //currentButtonColor = currentMaterialButton.GetComponent<Image>().color;
        currentMaterial = materials[index].material;

        //currentMaterialButton.GetComponent<Image>().color = selectedColor;
    }

    private void Update()
    {
        if(pointer.hit.collider != null)
        {
            SelectObject(pointer.hit);
        }
        else
        {
            DeselectObject();
        }

        if (ChangeMatButton.GetStateDown(SteamVR_Input_Sources.RightHand))
        {
            ChangeSubMaterial();
        }
    }

    //sets the sprite and color of an image to a material preview
    private void CreateMaterialPreview(Material material, Image image)
    {
        Texture2D mainTexture = material.mainTexture as Texture2D;

        if(mainTexture != null)
        {
            Sprite preview = Sprite.Create(mainTexture, new Rect(0.0f, 0.0f, mainTexture.width, mainTexture.height), new Vector2(0.5f, 0.5f), 100.0f);
            image.sprite = preview;
        }

        if(material.HasProperty("_Color"))
        {
            Color previewColor = material.color;
            image.color = previewColor;
        }

    }

    //method to be executed when the confirmation button is pressed, will only change materials
    //if the pointer is currently pointing at a valid colision with the specified tag
    private void ChangeSubMaterial()
    {
        if (hasObjectSelected)
        {
            RaycastHit hit = currentlySelectedObject;
            if (hit.collider.CompareTag(changeMaterialTag) && hit.collider is MeshCollider)
            {

                Mesh mesh = hit.collider.gameObject.GetComponent<MeshFilter>().mesh;

                //Debug.Log(mesh.triangles.Length);
                //Debug.Log(hit.triangleIndex);

                MeshRenderer renderer = hit.collider.gameObject.GetComponent<MeshRenderer>();
                Material[] objectMaterials = hit.collider.gameObject.GetComponent<MeshRenderer>().materials;

                int triangleIndex = hit.triangleIndex;
                int lookupIndexX1 = mesh.triangles[triangleIndex * 3];
                int lookupIndexX2 = mesh.triangles[triangleIndex * 3 + 1];
                int lookupIndexX3 = mesh.triangles[triangleIndex * 3 + 2];


                //iterate through the different submeshes to find the correct material index
                for (int i = 0; i < mesh.subMeshCount; i++)
                {
                    int[] triangles = mesh.GetTriangles(i);

                    for (int j = 0; j < triangles.Length; j += 3)
                    {
                        if(triangles[j] == lookupIndexX1 && triangles[j+1] == lookupIndexX2 && triangles[j + 2] == lookupIndexX3)
                        {
                            objectMaterials[i] = currentMaterial;

                            renderer.materials = objectMaterials;

                            //this is for VR_MaterialPicker
                            OnChange?.Invoke();

                            return;
                        }
                    }
                }
                
            }
            else if(hit.collider.CompareTag(changeMaterialTag) && !(hit.collider is MeshCollider))
            {
                Debug.LogWarning("VRMaterialSwitcher can only be used on meshcolliders");
            }
        }
    }

    //methods for correctly setting hasObjectSelected
    private void SelectObject(RaycastHit highlighObj)
    {
        //check if has same tag
        if (highlighObj.collider.CompareTag(changeMaterialTag))
        {
            hasObjectSelected = true;
            currentlySelectedObject = highlighObj;
        }
        else
        {
            hasObjectSelected = false;
        }
    }

    private void DeselectObject()
    {
        hasObjectSelected = false;
    }

    public override void ButtonAction(int index, Vector3 pos)
    {
        throw new System.NotImplementedException();
    }
}

#region TestingDifferentMethod
//GameObject target = hit.collider.transform.GetChild(0).gameObject;

//Mesh mesh = target.GetComponent<Mesh>();

//Debug.Log(mesh.triangles.Length);
//                Debug.Log(hit.triangleIndex);

//                MeshRenderer renderer = target.GetComponent<MeshRenderer>();
//Material[] objectMaterials = target.GetComponent<MeshRenderer>().materials;
#endregion

[System.Serializable]
public struct MaterialData
{
    public Material material;
    public bool displayName;
}
