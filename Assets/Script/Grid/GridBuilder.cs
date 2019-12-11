using System.Collections;
using System.Collections.Generic;
using Valve.VR;
using UnityEngine.UI;
using UnityEngine;

public class GridBuilder : MonoBehaviour
{
    public GridContainerWindow containerWindow;
    VRPanelMover panelMover;

    [Header("Grid element prefabs")]
    public GameObject columPoint;
    public GameObject wallPoint;
    public GameObject floorPoints;

    [Header("Grid size")]
    public int xStartSize;
    public int zStartSize;
    
    public float startRange = 1;

    [Header("Slider")]
    public Slider xSlider;
    public Slider ySlider;
    public Text zText;

    public Slider rangeSlider;


    [Space(10)]

    [Tooltip("Enable this if you want to hide the grid on Teleport")]
    public bool disableOnTeleport;
    public SteamVR_Action_Boolean teleportButton;

    Transform[] pointsholder;

    bool[] isActive;
    int lastTabIndex;

    [HideInInspector]
    //this get assigned in the VRGridObjectSpawner
    public VRGridObjectSpawner objectSpawner;

    private void OnEnable()
    {
        containerWindow.Open += ShowGrid;
        
        panelMover.ClosePanel += HideGrid;
        panelMover.OpenPanel += LastShowGrid;

    }

    private void OnDisable()
    {
        containerWindow.Open -= ShowGrid;

        panelMover.ClosePanel -= HideGrid;
        panelMover.OpenPanel -= LastShowGrid;
    }

    private void Awake()
    {
        panelMover = FindObjectOfType<VRPanelMover>();

        pointsholder = new Transform[transform.childCount];
        isActive = new bool[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            pointsholder[i] = transform.GetChild(i);
        }
        //CreateGrid(xStartSize, 0, zStartSize, startRange);
    }

    private void Start()
    {
        UpdateGrid();
    }

    private void Update()
    {
        if (disableOnTeleport)
        {
            if (teleportButton.GetStateDown(SteamVR_Input_Sources.Any))
            {
                for (int i = 0; i < pointsholder.Length; i++)
                {
                    if (transform.GetChild(i) != null)
                    {
                        isActive[i] = transform.GetChild(i).gameObject.activeSelf;
                        transform.GetChild(i).gameObject.SetActive(false);
                    }
                }
            }
            else if (teleportButton.GetStateUp(SteamVR_Input_Sources.Any))
            {
                for (int i = 0; i < pointsholder.Length; i++)
                {
                    if(transform.GetChild(i) != null)
                    {
                        transform.GetChild(i).gameObject.SetActive(isActive[i]);
                    }
                }
            }
        }
    }
   

    void ShowGrid(int index)
    {
        lastTabIndex = index;
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

    void LastShowGrid()
    {
        for (int i = 0; i < pointsholder.Length; i++)
        {
            pointsholder[i].gameObject.SetActive(false);
        }
        pointsholder[lastTabIndex].gameObject.SetActive(true);

        //set default object
        if (objectSpawner != null)
        {
            objectSpawner.ButtonAction(0);
        }
    }

    private void HideGrid()
    {
        for (int i = 0; i < pointsholder.Length; i++)
        {
            pointsholder[i].gameObject.SetActive(false);
        }
    }

    void CreateGrid(int xSize, int ySize, int zSize,float range)
    {
        float offset = range / 2;

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < zSize; y++)
            {
                for (int z = 0; z < ySize; z++)
                {
                    float xRanged = x * range;
                    float yRanged = y * range;
                    float zRanged = z * range * 3;

                    Vector3 xOffset = new Vector3(xRanged - offset, 0, yRanged);
                    Vector3 yOffset = new Vector3(xRanged, 0, yRanged - offset);
                    Vector3 zOffset = new Vector3(0, zRanged, 0);

                    Vector3 firstLayerX = new Vector3(xRanged - offset, 0, 0);
                    Vector3 firstLayerY = new Vector3(0, 0, yRanged - offset);


                    //pillar point
                    GameObject crosses = Instantiate(columPoint, transform.position + new Vector3(xRanged, 0, yRanged) + zOffset, transform.rotation, pointsholder[0]);

                    crosses.name = "Grid X: " + x + " Y: " + y;

                    //mid point
                    if (x > 0 && y > 0)
                    {

                        Instantiate(wallPoint, transform.position + xOffset + zOffset, transform.rotation, pointsholder[1]).name = "Midpoint: X " + x + " Y: " + y;
                        Instantiate(wallPoint, transform.position + yOffset + zOffset, Quaternion.Euler(0, 90, 0), pointsholder[1]).name = "Midpoint: X " + x + " Y: " + y;

                        Instantiate(wallPoint, transform.position + firstLayerX + zOffset, transform.rotation, pointsholder[1]).name = "Midpoint: X " + x + " Y: " + y;
                        Instantiate(wallPoint, transform.position + firstLayerY + zOffset, Quaternion.Euler(0, 90, 0), pointsholder[1]).name = "Midpoint: X " + x + " Y: " + y;

                        //Floor point

                        Vector3 FloorPos = new Vector3(xRanged - offset, 0, yRanged - offset);
                        Instantiate(floorPoints, transform.position + FloorPos + zOffset, Quaternion.identity, pointsholder[2]);
                    }

                    //create more grid points if needed

                }
            }
        }
    }

    void ResetGrid()
    {
        for (int i = 0; i < pointsholder.Length; i++)
        {
            for (int x = 0; x < pointsholder[i].childCount; x++)
            {
                Destroy(pointsholder[i].GetChild(x).gameObject);
            }
        }
    }
    
    public void UpdateGrid()
    {
        ResetGrid();
        float rangeSliderDivided = rangeSlider.value / 10;
        transform.localScale = new Vector3(rangeSliderDivided, 1, rangeSliderDivided);
        CreateGrid((int)xSlider.value, int.Parse(zText.text), (int)ySlider.value, rangeSliderDivided);
    }

}
