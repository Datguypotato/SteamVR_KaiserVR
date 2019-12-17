using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    public GridTypes myType;
}

[System.Serializable]
public enum GridTypes
{
    wall,
    Colum,
    Floor,
    objectsnap
}
