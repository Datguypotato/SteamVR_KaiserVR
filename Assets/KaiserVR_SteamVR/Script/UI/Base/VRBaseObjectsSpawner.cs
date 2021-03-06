﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

/// <summary>
/// base class for VGRidObjectSpawner and VRObjectSpawner
/// </summary>

public class VRBaseObjectsSpawner : GridButtonBehaviour
{
    public GameObject[] spawnableprefabs { get; private set; }
    public ObjectData[] spawnableObjects;

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

        spawnableprefabs = new GameObject[spawnableObjects.Length];

        for (int i = 0; i < spawnableObjects.Length; i++)
        {
            spawnableprefabs[i] = spawnableObjects[i].spawnObject;
        }

        NumberOfObjects = spawnableObjects.Length;
    }

    //creating buttons
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
