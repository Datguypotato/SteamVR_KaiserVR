using System.Collections;
using System.Collections.Generic;
using Valve.VR.InteractionSystem;
using Valve.VR;
using Valve.VR.InteractionSystem.Sample;
using UnityEngine;


/// <summary>
/// Manager of swapping handmodel for steamVR
/// </summary>

public class VR_SwapHandModel : MonoBehaviour
{
    public GameObject defaultLeftHand;
    public GameObject defaultRightHand;

    GameObject recentRightModel;
    GameObject recentLeftModel;

    //public GridContainerWindow grid;
    public VRPanelMover panelMover;
    public SkeletonUIOptions SkeletonUIOptions;

    private void Awake()
    {
        //panelMover = FindObjectOfType<VRPanelMover>();
        //SkeletonUIOptions = FindObjectOfType<SkeletonUIOptions>();

        recentLeftModel = GetComponentInChildren<GridContainerWindow>().leftHandPrefab;
        recentRightModel = GetComponentInChildren<GridContainerWindow>().rightHandPrefab;
    }

    private void OnEnable()
    {
        panelMover.ClosePanel += ResetRendelModel;
        panelMover.OpenPanel += SetRecentModel;
    }

    public void ResetRendelModel()
    {
        SkeletonUIOptions.SetRenderModel(defaultLeftHand, defaultRightHand);
    }

    public void SetRenderModel(GameObject leftHand, GameObject rightHand)
    {
        SkeletonUIOptions.SetRenderModel(leftHand, rightHand);
        recentLeftModel = leftHand;
        recentRightModel = rightHand;
    }

    public void SetRecentModel()
    {
        if(recentLeftModel != null && recentRightModel != null)
            SkeletonUIOptions.SetRenderModel(recentLeftModel, recentRightModel);
    }

    #region useless
    //private void Awake()
    //{
    //    //grid = GetComponent<GridContainerWindow>();
    //    SkeletonUIOptions = this.GetComponentInParent<SkeletonUIOptions>();
    //    panelMover = FindObjectOfType<VRPanelMover>();

    //    //panelMover.ClosePanel += ResetRendelModel;
    //    //panelMover.OpenPanel += SetRenderModel;
    //}

    //private void OnEnable()
    //{
    //    panelMover.ClosePanel += ResetRendelModel;
    //    panelMover.OpenPanel += SetRenderModel;
    //}

    //private void OnDisable()
    //{
    //    panelMover.ClosePanel -= ResetRendelModel;
    //    panelMover.OpenPanel -= SetRenderModel;
    //}

    //IEnumerator DelayUnsub()
    //{
    //    yield return new WaitForSeconds(0.2f);
    //    panelMover.ClosePanel -= ResetRendelModel;
    //    panelMover.OpenPanel -= SetRenderModel;
    //}
    #endregion
}
