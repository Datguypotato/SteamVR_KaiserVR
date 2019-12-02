using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class VRBaseObjectsSpawner : GridButtonBehaviour
{

    public GameObject[] spawnableprefabs;
    public ObjectData[] spawnableObjects;

    public bool showName;

    protected Button button;

    //for VROGridbjectSpawner
    public override void ButtonAction(int index)
    {
        throw new System.NotImplementedException();
    }

    //for VRObjectSpawner
    public override void ButtonAction(int index, Vector3 pos)
    {
        throw new System.NotImplementedException();
    }

    public override void Setup(ViewportContent controller)
    {
        this.controller = controller;

        spawnableObjects = new ObjectData[spawnableprefabs.Length];
        for (int i = 0; i < spawnableprefabs.Length; i++)
        {
            spawnableObjects[i] = new ObjectData(spawnableprefabs[i], showName);
        }

        NumberOfObjects = spawnableObjects.Length;
    }

    public override void SetUpButton(GameObject buttonGameObject, int index)
    {
        ObjectData currentObject = spawnableObjects[index];
        button = buttonGameObject.GetComponent<Button>();
        Text buttontext = buttonGameObject.GetComponentInChildren<Text>();

        ButtonUIController buttonController = buttonGameObject.GetComponent<ButtonUIController>();
        if (buttonController != null)
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
    }
}

[Serializable]
public struct ObjectData
{
    public GameObject spawnObject;
    public Sprite objectImage;
    public bool displayName;

    public ObjectData(GameObject g, bool b)
    {
        spawnObject = g;
        objectImage = null;
        displayName = b;
    }
}
