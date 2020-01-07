using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// DEPRECATED
/// BEING REWORKED
/// </summary>
public class VRObjectSpawner : VRBaseObjectsSpawner
{
    public string spawnedObjectTag;

    Vector3 spawnOffset = new Vector3(0, -0.5f, 0);

    public override void SetUpButton(GameObject buttonGameObject, int index)
    {
        base.SetUpButton(buttonGameObject, index);

        button.onClick.AddListener(delegate { ButtonAction(index, button.transform.position + spawnOffset); });
    }

    public override void ButtonAction(int index, Vector3 pos)
    {
        GameObject instantiatedObject = Instantiate(spawnableObjects[index].spawnObject, pos, Quaternion.identity);

        if(spawnedObjectTag != "")
        {
            instantiatedObject.tag = spawnedObjectTag;
        }
    }


    //public override void Setup(ViewportContent controller)
    //{
    //    throw new NotImplementedException();
    //}
}


//[System.Serializable]
//public struct SpawnObjectData
//{
//    public GameObject spawnObject;
//    public Sprite objectImage;
//    public bool displayName;

//    public SpawnObjectData(GameObject g, bool b)
//    {
//        spawnObject = g;
//        objectImage = null;
//        displayName = b;

//    }
//}
