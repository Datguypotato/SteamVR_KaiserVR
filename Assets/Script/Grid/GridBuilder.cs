using System.Collections;
using System.Collections.Generic;
using Valve.VR;
using UnityEngine;

public class GridBuilder : MonoBehaviour
{
    public SteamVR_Action_Boolean teleportButton;

    public GameObject prefab;
    public GameObject midPoint;

    public int xSize;
    public int ySize;
    
    public float range = 1;

    public bool disableOnTeleport;

    private void Awake()
    {
        float Xoffset = range / 2;
        float Yoffset = range / 2;

        //create pillar grid
        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                //pillar point
                GameObject temp = Instantiate(prefab, transform.position + new Vector3(x * range, 0, y * range), transform.rotation, this.transform);
                
                temp.name = "Grid X: " + x + " Y: " + y;

                //mid point
                Vector3 xOffset = new Vector3((x * range) - (range / 2), 0, (y * range));
                Vector3 yOffset = new Vector3((x * range), 0, (y * range) - (range / 2));

                if (x > 0 && y > 0)
                {
                    //Vector3 offsetStarterPoint = transform.position + new Vector3(xRange / 2, 0, (y * yRange));
                    Instantiate(midPoint, transform.position + xOffset, transform.rotation, this.transform).name = "Midpoint: " + Yoffset.ToString();
                    Instantiate(midPoint, transform.position + yOffset, transform.rotation, this.transform).name = "Midpoint: " + xOffset.ToString();

                    Instantiate(midPoint, transform.position + new Vector3((x * range) - (range / 2), 0, 0), transform.rotation, this.transform).name = "Midpoint: " + new Vector3((x * range) - (range / 2), 0, 0).ToString();
                    Instantiate(midPoint, transform.position + new Vector3(0, 0, (y * range) - (range / 2)), transform.rotation, this.transform).name = "Midpoint: " + new Vector3((x * range) - (range / 2), 0, 0).ToString();
                }
            }
        }
    }

    private void Update()
    {
        if (disableOnTeleport)
        {
            if (teleportButton.GetStateDown(SteamVR_Input_Sources.Any))
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(false);
                }
            }
            else if (teleportButton.GetStateUp(SteamVR_Input_Sources.Any))
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(true);
                }
            }
        }
    }
    
}
