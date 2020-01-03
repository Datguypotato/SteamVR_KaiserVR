using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Save
{
    /// <summary>
    /// transform
    /// </summary>
    public SerializableVector3[] allposition;
    public SerializableQuaternion[] allRotation;

    /// <summary>
    /// gridsetting
    /// </summary>
    public int xGrid;
    public int yGrid;
    public int zGrid;

    public float gridRange;

    /// <summary>
    /// Object
    /// </summary>
    public GridTypes[] gridtype;
    public int[] objectIndex;
    public int[] childTransformIndex;

    /// <summary>
    /// thumbnail
    /// </summary>
    public string pathThumbnail;
}
