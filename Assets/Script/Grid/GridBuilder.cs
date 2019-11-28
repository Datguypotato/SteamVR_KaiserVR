using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBuilder : MonoBehaviour
{
    public GameObject prefab;

    public int xSize;
    public int ySize;

    public float xRange = 1;
    public float yRange = 1;


    private void Awake()
    {
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                GameObject temp = Instantiate(prefab, transform.position + new Vector3(x * xRange, 0, y * yRange), transform.rotation, this.transform);
                //temp.transform.SetParent(this.transform);
                temp.name = "Grid X: " + x + " Y: " + y;
            }
        }
    }
}
