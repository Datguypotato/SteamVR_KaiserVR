using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRGridObjectSpawner : VRBaseObjectsSpawner
{

    VR_GridPointer gridpointer;

    public override void Setup(ViewportContent controller)
    {
        base.Setup(controller);

        gridpointer = FindObjectOfType<VR_GridPointer>();
    }

    public override void ButtonAction(int index)
    {
        gridpointer.selectedPrefab = spawnableObjects[index].spawnObject;
    }

    public override void SetUpButton(GameObject buttonGameObject, int index)
    {
        base.SetUpButton(buttonGameObject, index);

        button.onClick.AddListener(delegate { ButtonAction(index); });
    }

}
