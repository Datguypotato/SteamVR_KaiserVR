using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Special type of wrapper viewport container that can contain multiple sub-grids of type GridSpawnWindow
/// </summary>
public class GridContainerWindow : ViewportContent
{
    public enum SubTabAlignment
    {
        Horizontal,
        Vertical
    }

    [Header("Tab button settings")]
    [Tooltip("The button prefab to be used for each sub-grid's tab button")]
    public GameObject subTabButton;
    [Tooltip("The total (RectTransform) area for the tab buttons to be placed in")]
    public RectTransform subTabDiameters;
    [Tooltip("The actual object the tab button will be parented to, set to same object as subTabDiameters for most cases")]
    public GameObject subTabContainer;
    [Tooltip("The alignment in which the tab buttons will be placed")]
    public SubTabAlignment subTabAlignment = SubTabAlignment.Horizontal;

    private ButtonUIController[] tabButtons;

    VR_SwapHandModel swapHandModel;
    [Header("Hand model swap")]
    public GameObject leftHandPrefab;
    public GameObject rightHandPrefab;

    //doesn't work :(
    public delegate void OnWindow(int i);
    public event OnWindow Open;
    public event OnWindow Close;

    Canvas parentCanvas;

    public override void SetupContent(UIpanel controller, GameObject viewPort)
    {
        swapHandModel = FindObjectOfType<VR_SwapHandModel>();

        parentCanvas = GetComponentInParent<Canvas>();

        this.controller = controller;
        this.viewPort = viewPort;

        usePageCount = false;

        List<GameObject> gridPages = new List<GameObject>();

        foreach(Transform child in transform)
        {
            GridSpawnWindow window = child.GetComponent<GridSpawnWindow>();
            if(window != null)
            {
                window.SetupContent(controller, viewPort);
                gridPages.Add(window.gameObject);
            }
        }

        pages = gridPages.ToArray();
        generateSubTabs(subTabDiameters, subTabAlignment);
        SwitchToPage(0);
        
    }

    public override void OpenWindow()
    {
        if(parentCanvas.enabled)
        {
            swapHandModel.SetRenderModel(leftHandPrefab, rightHandPrefab);

            //Debug.Log("I am a stupid script and i am being stupid");
        }


        SwitchToPage(currentPage);
    }

    public override void CloseWindow()
    {
        Button nextButton = controller.NextButton.GetComponent<Button>();
        Button backButton = controller.BackButton.GetComponent<Button>();

        nextButton.onClick.RemoveAllListeners();
        backButton.onClick.RemoveAllListeners();

        //Debug.Log(currentPage);
        pages[currentPage].GetComponent<ViewportContent>().CloseWindow();

        //this is for VR_SwapHandModel
        Close?.Invoke(0);
    }


    public override void ScrollNext()
    {
        pages[currentPage].GetComponent<ViewportContent>().ScrollNext();
        Debug.Log("Scrol next");
    }

    public override void ScrollBack()
    {
        pages[currentPage].GetComponent<ViewportContent>().ScrollBack();
        Debug.Log("Scrol back");
    }

    /// <summary>
    /// Switches to a given page and enables/disables the next/back button if the pageNumber is the first or last page
    /// In addition, the fuction will also disable all pages and close the previously opened window
    /// </summary>
    /// <param name="pageNumber">the index of the page to switch to</param>
    protected override void SwitchToPage(int pageNumber)
    {
        Array.ForEach(pages, page => page.SetActive(false));
        pages[currentPage].GetComponent<ViewportContent>().CloseWindow();

        pages[pageNumber].SetActive(true);
        currentPage = pageNumber;

        ViewportContent controller = pages[currentPage].GetComponent<ViewportContent>();

        GameObject nextButton = this.controller.NextButton;
        GameObject backButton = this.controller.BackButton;

        Button next = nextButton.GetComponent<Button>();
        Button back = backButton.GetComponent<Button>();

        next.onClick.RemoveAllListeners();
        back.onClick.RemoveAllListeners();

        if (controller != null)
        {
            if(controller.PageCount > 1 && controller.UsePageCount)
            {
                nextButton.SetActive(true);
                backButton.SetActive(true);
                next.onClick.AddListener(controller.ScrollNext);
                back.onClick.AddListener(controller.ScrollBack);
            }
            else
            {
                nextButton.SetActive(false);
                backButton.SetActive(false);
            }

            controller.OpenWindow();
        }
        else
        {
            nextButton.SetActive(false);
            backButton.SetActive(false);
        }

        if(tabButtons.Length > 0)
        {
            Array.ForEach(tabButtons, obj => obj.SetHighLight(false));
            tabButtons[pageNumber].SetHighLight(true);
        }

        Open?.Invoke(pageNumber);
    }

    /// <summary>
    /// Generates the required subtabs for each of the GridSpawnWindow's
    /// </summary>
    /// <param name="container">STUMP</param>
    /// <param name="alignment">The alignment in which the tab buttons will be placed</param>
    private void generateSubTabs(RectTransform container, SubTabAlignment alignment)
    {
        Vector2 viewPortDiameters = container.sizeDelta;

        float gutter = 0;
        float center = 0;
        //float offset = 0;

        if (alignment == SubTabAlignment.Vertical)
        {
            gutter = viewPortDiameters.y / pages.Length;
            center = viewPortDiameters.y / 2;
            //offset = (viewPortDiameters.y / 2);
        }
        else if(alignment == SubTabAlignment.Horizontal)
        {
            gutter = viewPortDiameters.x / pages.Length;
            center = viewPortDiameters.x / 2;
            //offset = (viewPortDiameters.x / 2);
        }

        List<ButtonUIController> tabButtonList = new List<ButtonUIController>();
        for (int i = 0; i < pages.Length; i++)
        {
            GameObject subTab = Instantiate(subTabButton, subTabContainer.transform);
            float spacing = (gutter * i);

            if (alignment == SubTabAlignment.Vertical)
            {
                //STUMP
                throw new NotImplementedException();
                //subTab.transform.localPosition = new Vector3(0, spacing - center + (gutter / 2), 0);
            }
            else if(alignment == SubTabAlignment.Horizontal)
            {
                subTab.transform.localPosition = new Vector3(spacing - center + (gutter / 2), 0, 0);
            }
            
            subTab.name = "Subtab: " + i;

            ButtonUIController buttonController = subTab.GetComponent<ButtonUIController>();
            ViewportContent subWindowController = pages[i].GetComponent<ViewportContent>();


            if (subWindowController != null && buttonController != null)
            {
                tabButtonList.Add(buttonController);

                subTab.transform.localScale = subTab.transform.localScale * subWindowController.tabButtonScale;
                buttonController.SetImage(subWindowController.tabButtonImage);
                buttonController.SetName(subWindowController.tabName);
            }
            else
            {
                subTab.GetComponentInChildren<Text>().text = pages[i].name;
            }

            int index = i;
            Button button = subTab.GetComponent<Button>();
            button.onClick.AddListener(delegate { SwitchToPage(index); });
        }
        tabButtons = tabButtonList.ToArray();
    }
}
