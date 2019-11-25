using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//todo make this work in UIpointer

public class SetKinemetics : MonoBehaviour
{
    Rigidbody rb;
    VR_FixedJointGrab fixedJointGrab;
    //MeshCollider meshCol;

    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        fixedJointGrab = FindObjectOfType<VR_FixedJointGrab>();
        //meshCol = GetComponent<MeshCollider>();
        //meshCol.convex = true;
    }

    private void OnEnable()
    {
        fixedJointGrab.OnGrab += FixedJointGrab_OnGrab;
        fixedJointGrab.UnGrab += FixedJointGrab_UnGrab;
    }

    private void OnDisable()
    {
        fixedJointGrab.OnGrab -= FixedJointGrab_OnGrab;
        fixedJointGrab.UnGrab -= FixedJointGrab_UnGrab;
    }

    private void FixedJointGrab_UnGrab()
    {
        StartCoroutine(DelaySetKinemetic());
    }

    private void FixedJointGrab_OnGrab()
    {
        rb.isKinematic = false;
        StopAllCoroutines();
    }

    IEnumerator DelaySetKinemetic()
    {
        yield return new WaitForSeconds(1.5f);
        rb.isKinematic = true;
        //meshCol.convex = false;
    }

    void SetConvex()
    {
        
    }
}
