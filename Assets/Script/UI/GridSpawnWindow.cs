using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// Viewport class that generates a grid of options/functions based on the given GridButtonBehaviour
/// This will overflow and create a new page if the maximum grid size per page is reached
/// </summary>
public class GridSpawnWindow : ViewportContent
{
    [Header("Grid dimension settings")]
    [Tooltip("Amount of horizontally allowed items")]
    public int width;
    [Tooltip("Amount of vertically allowed items")]
    public int height;
    [Tooltip("Prefab used for each button in the grid")]
    public GameObject gridButton;
    public float gridButtonScale;
    private GridButtonBehaviour buttonBehaviourScript;
    private int numberOfButtons;

    List<GameObject> spawnedPages = new List<GameObject>();
    List<GameObject> Buttons = new List<GameObject>();

    public override void SetupContent(UIpanel controller, GameObject viewPort)
    {
        buttonBehaviourScript = GetComponent<GridButtonBehaviour>();
        if(buttonBehaviourScript == null)
        {
            throw new MissingComponentException("GridSpawnWindow of " + gameObject.name + " has no behavioural script attached to it");
        }

        buttonBehaviourScript.Setup(this);
        numberOfButtons = buttonBehaviourScript.NumberOfObjects;

        this.controller = controller;
        this.viewPort = viewPort;
        backGroundImage = GetComponent<Image>();

        GenerateButtonGrid();

        SwitchToPage(0);
	}

    public override void OpenWindow()
    {
        base.OpenWindow();
        buttonBehaviourScript.EnableBehaviour();
    }

    public override void CloseWindow()
    {
        base.CloseWindow();
        buttonBehaviourScript.DisableBehaviour();
    }

    /// <summary>
    /// Generates the actual button grid
    /// If the grid is larger than the specified dimensions the grid will be continued on a new page
    /// </summary>
    private void GenerateButtonGrid()
    {
        Vector2 parentDiameters = this.GetComponent<RectTransform>().sizeDelta;
        Vector2 centerPos = new Vector2(parentDiameters.x / 2, parentDiameters.y / 2);

        float Xgutter = parentDiameters.x / width;
        float Ygutter = parentDiameters.y / height;

        int buttonCount = 0;
        List<GameObject> pageList = new List<GameObject>();

        List<ButtonUIController> buttonList = new List<ButtonUIController>();
        for (int i = 0; buttonCount < numberOfButtons; i++)
        {
            GameObject page = new GameObject("GridPage: " + i, typeof(RectTransform));
            spawnedPages.Add(page);
            RectTransform pageTransForm = page.GetComponent<RectTransform>();

            pageTransForm.SetParent(gameObject.transform);
            pageTransForm.sizeDelta = parentDiameters;
            pageTransForm.localPosition = new Vector3(0, 0, 0);
            pageTransForm.rotation = new Quaternion(0, 0, 0, 0);
            pageTransForm.localScale = new Vector3(1, 1, 1);

            page.SetActive(false);

            pageList.Add(page);

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    GameObject button = Instantiate(gridButton, page.transform);
                    Buttons.Add(button);
                    RectTransform buttonPosition = button.GetComponent<RectTransform>();

                    buttonBehaviourScript.SetUpButton(button, buttonCount);

                    float Xspacing = (Xgutter * x);
                    float Yspacing = (Ygutter * y);

                    buttonPosition.localPosition = new Vector3(Xspacing - centerPos.x + (Xgutter / 2), -(Yspacing - centerPos.y + (Ygutter / 2)), 0);
                    buttonPosition.localScale = new Vector3(gridButtonScale, gridButtonScale, gridButtonScale);
                    buttonPosition.localPosition -= buttonPosition.FindVerticalButtonCentroid();

                    buttonCount++;
                    if(buttonCount >= numberOfButtons)
                    {
                        pages = pageList.ToArray();
                        return;
                    }
                }
            }
        }
    }

    //no longer needed 
    //public void ResetButtonGrid()
    //{
    //    // destroy old buttons and pages

    //    for (int i = 0; i < spawnedPages.Count; i++)
    //    {
    //        Destroy(spawnedPages[i]);
    //    }

    //    for (int i = 0; i < Buttons.Count; i++)
    //    {
    //        Destroy(Buttons[i]);
    //    }

    //    // clear list

    //    spawnedPages.Clear();
    //    Buttons.Clear();

    //}
}
