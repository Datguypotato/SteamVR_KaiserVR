using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Valve.VR;
using UnityEngine.UI;
using UnityEngine;
using System;

public class GridBuilder : MonoBehaviour
{
    public GridContainerWindow containerWindow;
    VRPanelMover panelMover;

    [Header("Grid element prefabs")]
    public GameObject columPoint;
    public GameObject wallPoint;
    public GameObject floorPoints;

    //[Header("Grid size")]
    //public int xStartSize;
    //public int zStartSize;
    
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
    int activeMenu;

    [HideInInspector]
    //this get assigned in the VRGridObjectSpawner
    public VRGridObjectSpawner objectSpawner;

    UIpanel panel;
    private void Awake()
    {
        panelMover = FindObjectOfType<VRPanelMover>();
        panel = FindObjectOfType<UIpanel>();

        pointsholder = new Transform[transform.childCount];
        isActive = new bool[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            pointsholder[i] = transform.GetChild(i);
        }
        //CreateGrid(xStartSize, 0, zStartSize, startRange);
    }

    private void OnEnable()
    {
        containerWindow.Open += ShowPoint;
        containerWindow.Close += HideGridWrapper;

        panelMover.ClosePanel += HideGrid;
        panelMover.OpenPanel += LastShowGrid;

        panel.OntabSwitch += Panel_OntabSwitch;

    }

    private void Panel_OntabSwitch(int index)
    {
        activeMenu = index;
    }

    private void OnDisable()
    {
        containerWindow.Open -= ShowPoint;
        containerWindow.Close -= HideGridWrapper;

        panelMover.ClosePanel -= HideGrid;
        panelMover.OpenPanel -= LastShowGrid;
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
   

    void ShowPoint(int index)
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
        if (activeMenu == 2)
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
    }

    private void HideGridWrapper(int i)
    {
        HideGrid();
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

    #region savingSystem

    public void SaveBuild()
    {
        // serialize
        Save save = CreateSaveGameObject();

        // creating file
        BinaryFormatter bf = new BinaryFormatter();
        FileStream file = File.Create(Application.persistentDataPath + "/BuildingSave.Kaiser");
        bf.Serialize(file, save);
        file.Close();

        Debug.Log("Saved file");
    }

    public void LoadBuild()
    {
        if(File.Exists(Application.persistentDataPath + "/BuildingSave.Kaiser"))
        {

            // Getting save
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(Application.persistentDataPath + "/BuildingSave.Kaiser", FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();

            // getting available objects
            VRGridObjectSpawner[] gridObjectSpawners = FindObjectsOfType<VRGridObjectSpawner>();

            //save.allposition.Length should not be the lenght need gridobject lenght
            //todo make it spawn the right object these array are empty for some reason
            GameObject[] columColl = new GameObject[save.allposition.Length];
            GameObject[] wallColl = new GameObject[save.allposition.Length];
            GameObject[] floorColl = new GameObject[save.allposition.Length];

            for (int i = 0; i < gridObjectSpawners.Length; i++)
            {
                switch (gridObjectSpawners[i].tabType)
                {
                    case (GridTypes.Colum):
                        columColl = gridObjectSpawners[i].spawnableprefabs;
                        break;
                    case (GridTypes.Floor):
                        floorColl = gridObjectSpawners[i].spawnableprefabs;
                        break;
                    case (GridTypes.wall):
                        Debug.Log(gridObjectSpawners[i].spawnableprefabs[i].name);
                        wallColl = gridObjectSpawners[i].spawnableprefabs;
                        break;
                }
            }

            // creating objects from save

            for (int i = 0; i < save.allposition.Length; i++)
            {
                GameObject go = new GameObject();
                switch (save.gridtype[i])
                {
                    case (GridTypes.Colum):
                        Instantiate(columColl[save.objectIndex[i]], save.allposition[i], save.allRotation[i]);
                        break;
                    case (GridTypes.Floor):
                        Instantiate(floorColl[save.objectIndex[i]], save.allposition[i], save.allRotation[i]);
                        break;
                    case (GridTypes.wall):
                        Debug.Log(save.objectIndex[i])
                        Debug.Log(wallColl[save.objectIndex[i]]);
                        Debug.Log(save.allposition[i]);
                        Debug.Log(save.allRotation[i]);
                        Instantiate(wallColl[save.objectIndex[i]], save.allposition[i], save.allRotation[i]);
                        break;

                }
                
            }

            // setting gridsetting
            xSlider.value = save.xGrid;
            ySlider.value = save.yGrid;
            zText.text = save.zGrid.ToString();

            rangeSlider.value = save.gridRange;

            // setting grid acording to the settings
            UpdateGrid();

            Debug.Log("Save loaded");
        }
        else
        {
            Debug.Log("No saved file founded");
        }
    }

    private Save CreateSaveGameObject()
    {
        // creating variables
        Save save = new Save();

        GridObject[] gridObjects = FindObjectsOfType<GridObject>();
        List<Transform> placeableTransform = new List<Transform>();
        List<GridTypes> placeableType = new List<GridTypes>();
        List<int> saveObjectIndex = new List<int>();

        // filter out not relevant GridObject
        for (int i = 0; i < gridObjects.Length; i++)
        {
            if(gridObjects[i].myType != GridTypes.objectsnap)
            {
                placeableTransform.Add(gridObjects[i].transform);
                placeableType.Add(gridObjects[i].myType);
                saveObjectIndex.Add(gridObjects[i].objectIndex);
            }
        }

        // Creating seriazable variables
        SerializableVector3[] placeablePos = new SerializableVector3[placeableTransform.Count];
        SerializableQuaternion[] placeableRot = new SerializableQuaternion[placeableTransform.Count];

        // splitting data
        for (int i = 0; i < placeableTransform.Count; i++)
        {
            placeablePos[i] = placeableTransform[i].position;
            placeableRot[i] = placeableTransform[i].rotation;
        }

        // assigning data
        save.allposition = placeablePos;
        save.allRotation = placeableRot;
        save.gridtype = placeableType.ToArray();
        save.objectIndex = saveObjectIndex.ToArray();

        // grid info
        save.xGrid = (int)xSlider.value;
        save.yGrid = (int)ySlider.value;
        save.zGrid = int.Parse(zText.text);

        save.gridRange = rangeSlider.value;

        return save;
    }
    #endregion
}

/// <summary>
/// Since unity doesn't flag the Vector3 as serializable, we
/// need to create our own version. This one will automatically convert
/// between Vector3 and SerializableVector3
/// </summary>
[System.Serializable]
public struct SerializableVector3
{
    /// <summary>
    /// x component
    /// </summary>
    public float x;

    /// <summary>
    /// y component
    /// </summary>
    public float y;

    /// <summary>
    /// z component
    /// </summary>
    public float z;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="rX"></param>
    /// <param name="rY"></param>
    /// <param name="rZ"></param>
    public SerializableVector3(float rX, float rY, float rZ)
    {
        x = rX;
        y = rY;
        z = rZ;
    }

    /// <summary>
    /// Returns a string representation of the object
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return String.Format("[{0}, {1}, {2}]", x, y, z);
    }

    /// <summary>
    /// Automatic conversion from SerializableVector3 to Vector3
    /// </summary>
    /// <param name="rValue"></param>
    /// <returns></returns>
    public static implicit operator Vector3(SerializableVector3 rValue)
    {
        return new Vector3(rValue.x, rValue.y, rValue.z);
    }

    /// <summary>
    /// Automatic conversion from Vector3 to SerializableVector3
    /// </summary>
    /// <param name="rValue"></param>
    /// <returns></returns>
    public static implicit operator SerializableVector3(Vector3 rValue)
    {
        return new SerializableVector3(rValue.x, rValue.y, rValue.z);
    }
}

/// <summary>
/// Since unity doesn't flag the Quaternion as serializable, we
/// need to create our own version. This one will automatically convert
/// between Quaternion and SerializableQuaternion
/// </summary>
[System.Serializable]
public struct SerializableQuaternion
{
    /// <summary>
    /// x component
    /// </summary>
    public float x;

    /// <summary>
    /// y component
    /// </summary>
    public float y;

    /// <summary>
    /// z component
    /// </summary>
    public float z;

    /// <summary>
    /// w component
    /// </summary>
    public float w;

    /// <summary>
    /// Constructor
    /// </summary>
    /// <param name="rX"></param>
    /// <param name="rY"></param>
    /// <param name="rZ"></param>
    /// <param name="rW"></param>
    public SerializableQuaternion(float rX, float rY, float rZ, float rW)
    {
        x = rX;
        y = rY;
        z = rZ;
        w = rW;
    }

    /// <summary>
    /// Returns a string representation of the object
    /// </summary>
    /// <returns></returns>
    public override string ToString()
    {
        return String.Format("[{0}, {1}, {2}, {3}]", x, y, z, w);
    }

    /// <summary>
    /// Automatic conversion from SerializableQuaternion to Quaternion
    /// </summary>
    /// <param name="rValue"></param>
    /// <returns></returns>
    public static implicit operator Quaternion(SerializableQuaternion rValue)
    {
        return new Quaternion(rValue.x, rValue.y, rValue.z, rValue.w);
    }

    /// <summary>
    /// Automatic conversion from Quaternion to SerializableQuaternion
    /// </summary>
    /// <param name="rValue"></param>
    /// <returns></returns>
    public static implicit operator SerializableQuaternion(Quaternion rValue)
    {
        return new SerializableQuaternion(rValue.x, rValue.y, rValue.z, rValue.w);
    }
}
