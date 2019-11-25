using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// DEPRECATED
/// BEING REWORKED
/// </summary>
public class VRObjectSpawner : GridButtonBehaviour
{
    public string spawnedObjectTag;
    public SpawnObjectData[] objects;

    Vector3 spawnOffset = new Vector3(0, -0.5f, 0);

    public override void Setup(ViewportContent controller)
    {
        this.controller = controller;
        NumberOfObjects = objects.Length;
    }

    public override void SetUpButton(GameObject buttonGameObject, int index)
    {
        SpawnObjectData currentObject = objects[index];
        Button button = buttonGameObject.GetComponent<Button>();
        Text buttontext = buttonGameObject.GetComponentInChildren<Text>();

        ButtonUIController buttonController = buttonGameObject.GetComponent<ButtonUIController>();
        if(buttonController != null)
        {
            if (currentObject.objectImage != null)
            {
                buttonController.SetImage(currentObject.objectImage);
            }
            if (currentObject.displayName)
            {
                buttonController.SetName(currentObject.spawnObject.name);
            }
        }
        else
        {
            if (currentObject.objectImage != null)
            {
                button.image.sprite = currentObject.objectImage;
            }

            if (currentObject.displayName)
            {
                buttontext.text = currentObject.spawnObject.name;
            }
            else
            {
                buttontext.text = "";
            }
        }

        button.onClick.AddListener(delegate { ButtonAction(index, button.transform.position + spawnOffset); });
    }

    public override void ButtonAction(int index, Vector3 pos)
    {
        GameObject instantiatedObject = Instantiate(objects[index].spawnObject, pos, Quaternion.identity);

        if(spawnedObjectTag != "")
        {
            instantiatedObject.tag = spawnedObjectTag;
        }
    }

    public override void ButtonAction(int index)
    {
        throw new NotImplementedException();
    }
}


[System.Serializable]
public struct SpawnObjectData
{
    public GameObject spawnObject;
    public Sprite objectImage;
    public bool displayName;
}
