using System.Collections;
using System.Collections.Generic;
using Valve.VR;
using UnityEditor;
using UnityEngine;

public class GridBuilder : MonoBehaviour
{
    public SteamVR_Action_Boolean teleportButton;
    public GridContainerWindow containerWindow;

    public GameObject pillarPoint;
    public GameObject midPoint;

    public int xSize;
    public int ySize;
    
    public float range = 1;

    public bool disableOnTeleport;
    public Transform[] pointsholder;

    [HideInInspector]
    public VRGridObjectSpawner objectSpawner;

    private void OnEnable()
    {
        containerWindow.Open += ShowGrid;
    }

    private void OnDisable()
    {
        containerWindow.Open -= ShowGrid;
    }

    private void Awake()
    {
        //CreateGrid();
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
    
    void ShowGrid(int index)
    {
        for (int i = 0; i < pointsholder.Length; i++)
        {
            pointsholder[i].gameObject.SetActive(false);
        }
        pointsholder[index].gameObject.SetActive(true);

        //set default object
        if(objectSpawner != null)
        {
            objectSpawner.ButtonAction(0);
        }
    }

    //void ResetGrid()
    //{
    //    for (int i = 0; i < pointsholder.Length; i++)
    //    {
    //        for (int x = 0; x < pointsholder[i].childCount; x++)
    //        {
    //            DestroyImmediate(pointsholder[i].GetChild(x));
    //        }
    //    }
    //}

    void CreateGrid()
    {

        float Xoffset = range / 2;
        float Yoffset = range / 2;

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                //pillar point
                GameObject crosses = Instantiate(pillarPoint, transform.position + new Vector3(x * range, 0, y * range), transform.rotation, pointsholder[0]);

                crosses.name = "Grid X: " + x + " Y: " + y;

                //mid point
                Vector3 xOffset = new Vector3((x * range) - (range / 2), 0, (y * range));
                Vector3 yOffset = new Vector3((x * range), 0, (y * range) - (range / 2));

                Vector3 firstLayerX = new Vector3((x * range) - (range / 2), 0, 0);
                Vector3 firstLayerY = new Vector3(0, 0, (y * range) - (range / 2));

                if (x > 0 && y > 0)
                {
                    //Vector3 offsetStarterPoint = transform.position + new Vector3(xRange / 2, 0, (y * yRange));
                    Instantiate(midPoint, transform.position + xOffset, transform.rotation, pointsholder[1]).name = "Midpoint: X " + x + " Y: " + y;
                    Instantiate(midPoint, transform.position + yOffset, Quaternion.Euler(0, 90, 0), pointsholder[1]).name = "Midpoint: X " + x + " Y: " + y;

                    Instantiate(midPoint, transform.position + firstLayerX, transform.rotation, pointsholder[1]).name = "Midpoint: X " + x + " Y: " + y;
                    Instantiate(midPoint, transform.position + firstLayerY, Quaternion.Euler(0, 90, 0), pointsholder[1]).name = "Midpoint: X " + x + " Y: " + y;
                }
            }
        }
    }
    

}
