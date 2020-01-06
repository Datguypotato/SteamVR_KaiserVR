using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Abstact base class that all viewport windows for UIpanel need to implement
/// </summary>
public abstract class ViewportContent : MonoBehaviour {

    [Header("Tab settings")]
    public string tabName = "new tab";
    public Sprite tabButtonImage;
    public float tabButtonScale = 1;

    protected bool usePageCount = true;
    public bool UsePageCount { get { return usePageCount; } private set { usePageCount = value; } }
    public int PageCount { get { return pages != null ? pages.Length : -1; } }

    protected int currentPage = 0;
    protected GameObject[] pages;

    protected GameObject viewPort;
    protected Image backGroundImage;
    protected UIpanel controller;

    /// <summary>
    /// Constructor method which gets called by the UIpanel class that controls the panel
    /// </summary>
    /// <param name="controller">The controller that called this method</param>
    /// <param name="viewPort">The viewport this content is in, this is usually the main viewport</param>
    public abstract void SetupContent(UIpanel controller, GameObject viewPort);

    /// <summary>
    /// Method which gets called when the window is opened
    /// </summary>
    public virtual void OpenWindow()
    {
        if(PageCount > 1)
        {
            SwitchToPage(currentPage);
        }
    }

    /// <summary>
    /// Method which gets called when the window is closed
    /// </summary>
    public virtual void CloseWindow()
    {
        //standard implementation to be added later
    }

    /// <summary>
    /// Scrolls 1 page forward
    /// </summary>
    public virtual void ScrollNext()
    {
        SwitchToPage(currentPage + 1);
    }

    /// <summary>
    /// Scrolls 1 page backward
    /// </summary>
    public virtual void ScrollBack()
    {
        SwitchToPage(currentPage - 1);
    }

    /// <summary>
    /// Switches to a given page and enables/disables the next/back button if the pageNumber is the first or last page
    /// </summary>
    /// <param name="pageNumber">the index of the page to switch to</param>
    protected virtual void SwitchToPage(int pageNumber)
    {
        if (pageNumber >= 0 && pageNumber < PageCount)
        {
            pages[currentPage].SetActive(false);
            pages[pageNumber].SetActive(true);
            currentPage = pageNumber;

            if (currentPage == 0)
            {
                controller.BackButton.SetActive(false);
                controller.NextButton.SetActive(true);
            }

            else if (currentPage == pages.Length - 1)
            {
                controller.NextButton.SetActive(false);
                controller.BackButton.SetActive(true);
            }

            else
            {
                controller.BackButton.SetActive(true);
                controller.NextButton.SetActive(true);
            }
        }
    }
}
