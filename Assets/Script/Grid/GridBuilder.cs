using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridBuilder : MonoBehaviour
{
    public GameObject prefab;
    public GameObject midPoint;

    public int xSize;
    public int ySize;
    
    public float range = 1;

    private void Awake()
    {
        float Xoffset = range / 2;
        float Yoffset = range / 2;
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                GameObject temp = Instantiate(prefab, transform.position + new Vector3(x * range, 0, y * range), transform.rotation, this.transform);
                

                //create the mid gridpoint not needed yet
                //if(x != 0 && y != 0 && x != xSize && y != ySize)
                //    Instantiate(midPoint, transform.position + new Vector3((x * xRange) - Xoffset, 0, (y * yRange) - Yoffset), transform.rotation);

                //temp.transform.SetParent(this.transform);
                temp.name = "Grid X: " + x + " Y: " + y;
            }
        }

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                //Instantiate(midPoint, transform.position + new Vector3(x / 2 * xRange, 0, y / 2 * yRange), transform.rotation);
                Vector3 xOffset = new Vector3((x * range) - (range / 2), 0, (y * range));
                Vector3 yOffset = new Vector3((x * range), 0, (y * range) - (range / 2));

                if (x > 0 && y > 0)
                {
                    //Vector3 offsetStarterPoint = transform.position + new Vector3(xRange / 2, 0, (y * yRange));
                    Instantiate(midPoint, transform.position + xOffset, transform.rotation, this.transform);
                    Instantiate(midPoint, transform.position + yOffset, transform.rotation, this.transform);

                    Instantiate(midPoint, transform.position + new Vector3((x * range) - (range / 2), 0, 0), transform.rotation);
                    Instantiate(midPoint, transform.position + new Vector3(0, 0, (y * range) - (range / 2)), transform.rotation);
                }


            }
        }
    }
}
