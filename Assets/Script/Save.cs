using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save
{
    //public float[] vectorX;
    public SerializableVector3[] allposition;
    public SerializableQuaternion[] allRotation;
    public GridTypes[] gridtype;

    //public int mesh;

    //public Transform[] gridColumTransform;
    //public Transform[] gridWallTransform;
    //public Transform[] gridFloorTransform;
}
