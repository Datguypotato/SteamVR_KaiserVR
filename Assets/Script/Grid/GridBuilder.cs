using System.Collections;
using System.Collections.Generic;
using System.IO;
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

    [Header("Slider")]
    public Slider xSlider;
    public Slider ySlider;
    public Text zText;

    public Slider rangeSlider;

    [Space(10)]

    [Tooltip("Enable this if you want to hide the grid on Teleport")]
    public bool disableOnTeleport;
    public SteamVR_Action_Boolean teleportButton;
    
    private int lastTabIndex;
    private int activeMenu;
    private bool[] isActive;

    [HideInInspector]
    //this get assigned in the VRGridObjectSpawner
    public VRGridObjectSpawner objectSpawner;
    UIpanel panel;

    public VRGridObjectSpawner[] gridObjectSpawners;
    private Transform[] pointsholder;

    private List<Transform> columHolder;
    private List<Transform> wallHolder;
    private List<Transform> floorHolder;

    [Space(5)]

    public AudioSource audioSource;

    private void Awake()
    {
        panelMover = FindObjectOfType<VRPanelMover>();
        panel = FindObjectOfType<UIpanel>();

        isActive = new bool[transform.childCount];
        pointsholder = new Transform[transform.childCount];

        for (int i = 0; i < transform.childCount; i++)
        {
            pointsholder[i] = transform.GetChild(i);
        }

        //gridObjectSpawners = FindObjectsOfType<VRGridObjectSpawner>();
    }

    private void OnEnable()
    {
        containerWindow.Open += ShowPoint;

        panelMover.ClosePanel += HideGrid;
        panelMover.OpenPanel += LastShowGrid;

        panel.OntabSwitch += Panel_OntabSwitch;

    }

    private void OnDisable()
    {
        containerWindow.Open -= ShowPoint;

        panelMover.ClosePanel -= HideGrid;
        panelMover.OpenPanel -= LastShowGrid;

        panel.OntabSwitch -= Panel_OntabSwitch;
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

        if (Input.GetKeyDown(KeyCode.Space))
        {
            SaveBuild();
        }
    }

    private void Panel_OntabSwitch(int index)
    {
        activeMenu = index;
        if (index == 2 || index == 3 || index == 4 || index == 5)
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
        else
        {
            HideGrid();
        }
    }

    void LastShowGrid()
    {
        Panel_OntabSwitch(activeMenu);
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

    //y and z is switched because this used to be a only one floor
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
                    float zRanged = z * 3;

                    Vector3 xOffset = new Vector3(xRanged - offset, 0, yRanged);
                    Vector3 yOffset = new Vector3(xRanged, 0, yRanged - offset);
                    Vector3 zOffset = new Vector3(0, zRanged, 0);

                    Vector3 firstLayerY = new Vector3(0, 0, yRanged - offset);
                    Vector3 firstLayerX = new Vector3(xRanged - offset, 0, 0);


                    //pillar point
                    GameObject crosses = Instantiate(columPoint, transform.position + new Vector3(xRanged, 0, yRanged) + zOffset, transform.rotation, pointsholder[0]);

                    crosses.name = "Grid X: " + x + " Y: " + y;
                    //mid point
                    if(x != 0)
                        Instantiate(wallPoint, transform.position + xOffset + zOffset, transform.rotation, pointsholder[1]).name = "Midpoint: X " + x + " Y: " + y + " 1";
                    if(y != 0)
                        Instantiate(wallPoint, transform.position + yOffset + zOffset, Quaternion.Euler(0, 90, 0), pointsholder[1]).name = "Midpoint: X " + x + " Y: " + y + " 2 ";
                    if (x > 0 && y > 0)
                    {
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
        //the only active gameobject with a GridContainerWindow script will be the save tab
        //So I don't have to check if I have the right one
        GridContainerWindow settingWindow = FindObjectOfType<GridContainerWindow>();
        settingWindow.gameObject.SetActive(false);

        // creating directory if needed
        if (!Directory.Exists(Application.dataPath + "/Saves"))
        {
            Directory.CreateDirectory(Application.dataPath + "/Saves");
        }

        Debug.Log("I got here");
        if (!Directory.Exists(Application.dataPath + "/Resources/Thumbnail"))
        {
            Debug.Log("Creating directory");
            Directory.CreateDirectory(Application.dataPath + "/Resources/Thumbnail");
        }

        // serialize
        Save save = CreateSaveGameObject();

        // creating file
        BinaryFormatter bf = new BinaryFormatter();

        FileStream file = File.Create(Application.dataPath + "/Saves/" + DateTime.Now.ToString("MM/dd/yyyy HHmm") + ".Kaiser");
        bf.Serialize(file, save);
        file.Close();

        // todo
        // show something to the user that the save happend
        Debug.Log("Saved file");
        settingWindow.gameObject.SetActive(true);
    }

    public void LoadBuild(string filePath)
    {
        //the only active gameobject with a GridContainerWindow script will be the save tab
        //So I don't have to check if I have the right one
        GridContainerWindow settingWindow = FindObjectOfType<GridContainerWindow>();
        settingWindow.gameObject.SetActive(false);

        if (File.Exists(filePath))
        {
            
            // Getting save
            BinaryFormatter bf = new BinaryFormatter();
            FileStream file = File.Open(filePath, FileMode.Open);
            Save save = (Save)bf.Deserialize(file);
            file.Close();

            // setting gridsetting
            xSlider.value = save.xGrid;
            ySlider.value = save.yGrid;
            zText.text = save.zGrid.ToString();

            rangeSlider.value = save.gridRange;

            // setting grid acording to the settings
            UpdateGrid();

            //restting objects
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).GetComponent<GridObject>() != null)
                    Destroy(transform.GetChild(i).gameObject);
            }

            // getting available objects
            GameObject[] columColl = new GameObject[gridObjectSpawners[0].spawnableprefabs.Length];
            GameObject[] wallColl = new GameObject[gridObjectSpawners[1].spawnableprefabs.Length];
            GameObject[] floorColl = new GameObject[gridObjectSpawners[2].spawnableprefabs.Length];

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
                        wallColl = gridObjectSpawners[i].spawnableprefabs;
                        break;
                }
            }

            // getting all transfor of the pointHolders
            columHolder = GetChildren(pointsholder[0]);
            floorHolder = GetChildren(pointsholder[1]);
            wallHolder = GetChildren(pointsholder[2]);


            // creating objects from save
            for (int i = 0; i < save.allposition.Length; i++)
            {
                //decide what Object need to spawn

                switch (save.gridtype[i])
                {
                    case (GridTypes.Colum):
                        SpawnSaveObject(columColl[save.objectIndex[i]], GridTypes.Colum, save, i);
                        break;
                    case (GridTypes.Floor):
                        SpawnSaveObject(floorColl[save.objectIndex[i]], GridTypes.Floor, save, i);
                        break;
                    case (GridTypes.wall):
                        SpawnSaveObject(wallColl[save.objectIndex[i]], GridTypes.wall, save, i);
                        break;
                    default:
                        Debug.Log("Something is really fucked");
                        break;

                }
            }

            Debug.Log("Save loaded");
        }
        else
        {
            Debug.Log("No saved file founded");
        }
        settingWindow.gameObject.SetActive(true);
    }

    void SpawnSaveObject(GameObject go, GridTypes type, Save save, int loopIndex)
    {
        GameObject savedGo = Instantiate(go, save.allposition[loopIndex], save.allRotation[loopIndex], transform);

        switch (type)
        {
            case GridTypes.Colum:
                SetComponents(columHolder[save.childTransformIndex[loopIndex]].GetComponent<GridElement>(), savedGo,
                    save.objectIndex[loopIndex], save.childTransformIndex[loopIndex]);
                break;
            case GridTypes.Floor:
                SetComponents(wallHolder[save.childTransformIndex[loopIndex]].GetComponent<GridElement>(),
                    savedGo, save.objectIndex[loopIndex], save.childTransformIndex[loopIndex]);
                break;
            case GridTypes.wall:
                SetComponents(floorHolder[save.childTransformIndex[loopIndex]].GetComponent<GridElement>(),
                    savedGo, save.objectIndex[loopIndex], save.childTransformIndex[loopIndex]);
                break;
            default:
                Debug.Log("Something went wrong");
                break;
        }
    }

    void SetComponents(GridElement element, GameObject g, int objInt, int childInt)
    {
        element.activeObject = g;

        GridObject gridobject = g.GetComponent<GridObject>();
        gridobject.objectIndex = objInt;
        gridobject.childIndex = childInt;
    }

    // I am not sure why yet but the first half of the list is missing transform
    // My fix is simply remove the first half 
    // I think it has the do with the fact that I destroy all children
    List<Transform> GetChildren(Transform t)
    {
        List<Transform> returnTrans = new List<Transform>();
        for (int i = 0; i < t.childCount; i++)
        {
            returnTrans.Add(t.GetChild(i));
        }

        returnTrans.RemoveRange(0, returnTrans.Count / 2);

        return returnTrans;
    }

    private Save CreateSaveGameObject()
    {
        // create screenshot for thumbnail
        string screenshotPath = Application.dataPath + "/Resources/Thumbnail/" + DateTime.Now.ToString("MM/dd/yyyy HHmm") + "_Thumbnail.png";
        StartCoroutine(TakeScreenShot(screenshotPath));

        // creating variables
        Save save = new Save();

        GridObject[] gridObjects = FindObjectsOfType<GridObject>();
        List<Transform> placeableTransform = new List<Transform>();
        List<GridTypes> placeableType = new List<GridTypes>();
        List<int> saveObjectIndex = new List<int>();
        List<int> saveTransformIndex = new List<int>();

        // filter out not relevant GridObject
        for (int i = 0; i < gridObjects.Length; i++)
        {
            if(gridObjects[i].myType != GridTypes.objectsnap)
            {
                placeableTransform.Add(gridObjects[i].transform);
                placeableType.Add(gridObjects[i].myType);
                saveObjectIndex.Add(gridObjects[i].objectIndex);
                saveTransformIndex.Add(gridObjects[i].childIndex);
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
        save.childTransformIndex = saveTransformIndex.ToArray();
        

        // assigning grid info
        save.xGrid = (int)xSlider.value;
        save.yGrid = (int)ySlider.value;
        save.zGrid = int.Parse(zText.text);

        save.gridRange = rangeSlider.value;

        save.pathThumbnail = screenshotPath;

        return save;
    }

    IEnumerator TakeScreenShot(string path)
    {
        VRPanelMover panelMover = FindObjectOfType<VRPanelMover>();
        panelMover.EnableCanvas(false);
        audioSource.PlayOneShot(audioSource.clip);
        yield return new WaitForSeconds(1);
        ScreenCapture.CaptureScreenshot(path, ScreenCapture.StereoScreenCaptureMode.RightEye);
        yield return new WaitForEndOfFrame();
        panelMover.EnableCanvas(true);
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
