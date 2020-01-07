using System.Collections;
using System.Collections.Generic;
using Valve.VR;
using UnityEngine;

public class EditGridObject : MonoBehaviour
{
    public GameObject oldestObject;

    UiPointer pointer;

    Vector3 hidePos;
    Vector3 showPos;

    float lerpingPos;

    public SteamVR_Action_Boolean grip;

    private void Awake()
    {
        pointer = FindObjectOfType<UiPointer>();
    }

    private void Start()
    {
        showPos = transform.position;
        hidePos = new Vector3(transform.position.x, 0, transform.position.z);
        if(FindObjectOfType<VR_pointerObjectSnapper>().selectedObject == this.gameObject)
        {
            FindObjectOfType<VR_pointerObjectSnapper>().selectedObject = null;
        }
    }

    private void Update()
    {
        LerpButton();

        if (grip.GetLastStateDown(SteamVR_Input_Sources.RightHand) && lerpingPos > 0.8f)
        {
            oldestObject.transform.parent.position = new Vector3(0, 100, 0);
            FindObjectOfType<VR_pointerObjectSnapper>().selectedObject = oldestObject.transform.parent.gameObject;
            Destroy(oldestObject);
        }

        if (oldestObject == null)
        {
            Destroy(this.gameObject);
        }
    }

    void LerpButton()
    {
        showPos = pointer.hit.point;

        if (pointer.highlightedObject == oldestObject)
        {
            lerpingPos += Time.deltaTime;
        }
        else
        {
            lerpingPos -= Time.deltaTime;
        }

        lerpingPos = Mathf.Clamp01(lerpingPos);
        transform.position = Vector3.Lerp(hidePos, showPos, lerpingPos);
    }
}
