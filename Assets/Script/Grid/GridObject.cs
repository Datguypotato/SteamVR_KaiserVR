using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject : MonoBehaviour
{
    public GridTypes myType;
}
public enum GridTypes
{
    wall,
    Colum,
    Floor
}
