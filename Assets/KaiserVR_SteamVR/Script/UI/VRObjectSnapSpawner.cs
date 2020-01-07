using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRObjectSnapSpawner : VRBaseObjectsSpawner
{
    VR_pointerObjectSnapper pointerSnapper;

    public override void Setup(ViewportContent controller)
    {
        base.Setup(controller);

        pointerSnapper = FindObjectOfType<VR_pointerObjectSnapper>();
    }

    public override void SetUpButton(GameObject buttonGameObject, int index)
    {
        base.SetUpButton(buttonGameObject, index);

        button.onClick.AddListener(delegate { ButtonAction(index, Vector3.zero); });
    }

    public override void ButtonAction(int index, Vector3 iAmNotUsed)
    {
        pointerSnapper.selectedObject = spawnableObjects[index].spawnObject;
    }
}
