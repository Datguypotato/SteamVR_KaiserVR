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
    public int yStartSize;
    
    public float startRange = 1;

    [Header("Slider")]
    public Slider xSlider;
    public Slider ySlider;
    public Slider rangeSlider;

    Text xSliderText;
    Text ySliderText;
    Text rangeSlidertext;

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
        isActive = new bool[transform.childCount];

        pointsholder = new Transform[transform.childCount];
        for (int i = 0; i < transform.childCount; i++)
        {
            pointsholder[i] = transform.GetChild(i);
        }

        xSliderText = xSlider.GetComponentInChildren<Text>();
        ySliderText = ySlider.GetComponentInChildren<Text>();
        rangeSlidertext = rangeSlider.GetComponentInChildren<Text>();
        
        CreateGrid(xStartSize, yStartSize, startRange);
    }

    private void Update()
    {
        if (disableOnTeleport)
        {
            if (teleportButton.GetStateDown(SteamVR_Input_Sources.Any))
            {
                
                for (int i = 0; i < transform.childCount; i++)
                {
                    isActive[i] = transform.GetChild(i).gameObject.activeSelf;
                    transform.GetChild(i).gameObject.SetActive(false);
                }
            }
            else if (teleportButton.GetStateUp(SteamVR_Input_Sources.Any))
            {
                for (int i = 0; i < transform.childCount; i++)
                {
                    transform.GetChild(i).gameObject.SetActive(isActive[i]);
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

    void CreateGrid(int xSize, int ySize, float range)
    {
        float offset = range / 2;

        for (int x = 0; x < xSize; x++)
        {
            for (int y = 0; y < ySize; y++)
            {
                float xRanged = x * range;
                float yRanged = y * range;

                Vector3 xOffset = new Vector3(xRanged - offset, 0, yRanged);
                Vector3 yOffset = new Vector3(xRanged, 0, yRanged - offset);

                Vector3 firstLayerX = new Vector3(xRanged - offset, 0, 0);
                Vector3 firstLayerY = new Vector3(0, 0, yRanged - offset);


                //pillar point
                GameObject crosses = Instantiate(columPoint, transform.position + new Vector3(xRanged, 0, yRanged), transform.rotation, pointsholder[0]);

                crosses.name = "Grid X: " + x + " Y: " + y;

                //mid point
                if (x > 0 && y > 0)
                {
                    
                    Instantiate(wallPoint, transform.position + xOffset, transform.rotation, pointsholder[1]).name = "Midpoint: X " + x + " Y: " + y;
                    Instantiate(wallPoint, transform.position + yOffset, Quaternion.Euler(0, 90, 0), pointsholder[1]).name = "Midpoint: X " + x + " Y: " + y;

                    Instantiate(wallPoint, transform.position + firstLayerX, transform.rotation, pointsholder[1]).name = "Midpoint: X " + x + " Y: " + y;
                    Instantiate(wallPoint, transform.position + firstLayerY, Quaternion.Euler(0, 90, 0), pointsholder[1]).name = "Midpoint: X " + x + " Y: " + y;

                //Floor point

                    Vector3 FloorPos = new Vector3(xRanged - offset, 0, yRanged - offset);
                    Instantiate(floorPoints, transform.position + FloorPos, Quaternion.identity, pointsholder[2]);
                }

                //create more grid points if needed

            }
        }
    }

    public void OnSliderValueChange()
    {
        xSliderText.text = xSlider.value.ToString();
        ySliderText.text = ySlider.value.ToString();
        rangeSlidertext.text = rangeSlider.value.ToString();
        Invoke("UpdateGrid", 1);
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
    
    void UpdateGrid()
    {
        ResetGrid();
        CreateGrid((int)xSlider.value, (int)ySlider.value, rangeSlider.value);

        transform.localScale = new Vector3(rangeSlider.value, rangeSlider.value, rangeSlider.value);
    }

}
