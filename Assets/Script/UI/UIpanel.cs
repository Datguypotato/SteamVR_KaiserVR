using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

/// <summary>
/// Main class for the UIpanel, manages state of the panel and setup and navigation of each individual sub-window
/// </summary>
public class UIpanel : MonoBehaviour
{
    [Tooltip("Prefab that will be used for the creation of navigational tab button on the panel itself")]
    public GameObject tabButton;

    private GameObject tabViewport, mainViewport, navigationalViewport, categoryViewPort;
    private GameObject nextButton;
    private GameObject backButton;

    private ViewportContent[] windowControllers;
    private ButtonUIController[] tabButtons;
    private GameObject[] windows;

    public GameObject NextButton
    {
        get
        {
            if(nextButton == null)
            {
                GenerateReferences();
            }
            return nextButton;
        }
    }

    public GameObject BackButton
    {
        get
        {
            if(backButton == null)
            {
                GenerateReferences();
            }
            return backButton;
        }
    }

    private void Awake()
    {
        GenerateReferences();
        GeneratePanel();
    }

    /// <summary>
    /// Generates all necessary references and throws a MissingReferenceException if the required components couldn't be found within the panel
    /// </summary>
    private void GenerateReferences()
    {
        tabViewport = transform.Find("Tab viewport").gameObject;
        if (tabViewport == null)
            throw new MissingReferenceException("The Tab viewport couldn't be found");

        mainViewport = transform.Find("Main viewport").gameObject;
        if (mainViewport == null)
            throw new MissingReferenceException("The Main viewport couldn't be found");

        navigationalViewport = transform.Find("Navigational viewport").gameObject;
        if(navigationalViewport == null)
            throw new MissingReferenceException("The Navigational viewport couldn't be found");

        nextButton = navigationalViewport.transform.Find("NextButton").gameObject;
        if (nextButton == null)
            throw new MissingReferenceException("The NextButton couldn't be found");

        backButton = navigationalViewport.transform.Find("BackButton").gameObject;
        if (backButton == null)
            throw new MissingReferenceException("THe BackButton couldn't be found");
    }

    /// <summary>
    /// Constructor function for panel
    /// </summary>
    private void GeneratePanel()
    {
        List<GameObject> windowsInMainPort = new List<GameObject>();

        foreach(Transform window in mainViewport.transform)
        {
            windowsInMainPort.Add(window.gameObject);
        }

        windows = windowsInMainPort.ToArray();

        windowControllers = GetComponentsInChildren<ViewportContent>();

        if(windows.Length < 1)
        {
            Debug.LogWarning("UIpanel has no tabs in it");
            return;
        }

        GenerateTabs();
        GenerateWindows();
        SwitchTabs(0);
    }

    /// <summary>
    /// Generates the tab buttons for each of the windows
    /// </summary>
    private void GenerateTabs()
    {
        Vector2 parentDiameters = tabViewport.GetComponent<RectTransform>().sizeDelta;
        float gutter = parentDiameters.x / windows.Length;
        float center = parentDiameters.y / 2;

        List<ButtonUIController> tabButtonList = new List<ButtonUIController>();
        for (int i = 0; i < windows.Length; i++)
        {
            GameObject tab = Instantiate(tabButton, tabViewport.transform);
            float spacing = (gutter * i);
            float offset = (parentDiameters.x / 2);
            tab.transform.localPosition = new Vector3(spacing - offset + (gutter / 2), 0, 0);

            tab.name = "Tab: " + i;

            ViewportContent windowController = windows[i].GetComponent<ViewportContent>();
            ButtonUIController buttonController = tab.GetComponent<ButtonUIController>();

            if (windowController != null && buttonController != null)
            {
                tabButtonList.Add(buttonController);
                tab.transform.localScale = tab.transform.localScale * windowController.tabButtonScale;
                buttonController.SetImage(windowController.tabButtonImage);
                buttonController.SetName(windowController.tabName);
            }
            else
            {
                tab.GetComponentInChildren<Text>().text = windows[i].name;
            }

            int index = i;
            Button button = tab.GetComponent<Button>();
            button.onClick.AddListener(delegate { SwitchTabs(index); });
        }
        tabButtons = tabButtonList.ToArray();
    }

    /// <summary>
    /// Generates the windows by calling the SetupContent() method for each, if a controller is found
    /// </summary>
    private void GenerateWindows()
    {
        foreach(GameObject window in windows)
        {
            ResizeWindow(window.GetComponent<RectTransform>());
            ViewportContent windowController = window.GetComponent<ViewportContent>();
            
            if(windowController != null)
            {
                windowController.SetupContent(this, mainViewport);
                windowController.CloseWindow();
            }

            window.SetActive(false);
        }
    }

    /// <summary>
    /// Sets the currently active window to the given index
    /// </summary>
    /// <param name="index">The window to be set active</param>
    public void SwitchTabs(int index)
    {
        GameObject currentWindow = windows.FirstOrDefault(panel => panel.activeInHierarchy == true);
        if(currentWindow != null)
        {
            ViewportContent closingWindowController = currentWindow.GetComponent<ViewportContent>();

            if (closingWindowController != null)
            {
                closingWindowController.CloseWindow();
            }

            currentWindow.SetActive(false);
        }

        Button next = NextButton.GetComponent<Button>();
        Button back = BackButton.GetComponent<Button>();

        next.onClick.RemoveAllListeners();
        back.onClick.RemoveAllListeners();

        ViewportContent windowController = windows[index].GetComponent<ViewportContent>();

        if (windowController != null)
        {
            if (windowController.PageCount > 1 && windowController.UsePageCount)
            {
                NextButton.SetActive(true);
                BackButton.SetActive(true);
                next.onClick.AddListener(windowController.ScrollNext);
                back.onClick.AddListener(windowController.ScrollBack);
            }
            else
            {
                NextButton.SetActive(false);
                BackButton.SetActive(false);
            }

            windowController.OpenWindow();
        }
        else
        {
            NextButton.SetActive(false);
            BackButton.SetActive(false);
        }

        if(tabButtons.Length > 0)
        {
            Array.ForEach(tabButtons, obj => obj.SetHighLight(false));
            tabButtons[index].SetHighLight(true);
        }

        windows[index].SetActive(true);
    }

    /// <summary>
    /// Stretches the given window to the maintransform's width and height as well as setting its rotation to 0 and scale to 1
    /// </summary>
    /// <param name="window">The window to resize</param>
    private void ResizeWindow(RectTransform window)
    {
        window.rotation = new Quaternion(0, 0, 0, 0);
        window.sizeDelta = mainViewport.GetComponent<RectTransform>().sizeDelta;
        window.localScale = new Vector3(1, 1, 1);
        window.localPosition = new Vector3(0, 0, 0);
    }
}

public static class TransFormExtention
{
    /// <summary>
    /// Finds the centerpoint of a transform relative to its bounding box
    /// PLEASE NOTE: currently only works vertically with the button as transform, and the text of the button as a child located somewhere under the button
    /// </summary>
    /// <param name="parentTransform">This RectTransform</param>
    /// <returns>The offset of the transform's pivot point to the true center point</returns>
    public static Vector3 FindVerticalButtonCentroid(this RectTransform parentTransform)
    {
        Vector3 centroid = new Vector3(0, 0, 0);
        centroid.y += parentTransform.sizeDelta.y / 2;

        Vector3 correctedChildPos = parentTransform.GetChild(0).localPosition;
        correctedChildPos.y -= parentTransform.GetChild(0).GetComponent<RectTransform>().sizeDelta.y / 2;

        centroid += correctedChildPos;

        centroid /= 2;

        centroid *= parentTransform.localScale.y;

        return centroid;
    }
}
