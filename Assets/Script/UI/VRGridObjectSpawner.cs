using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRGridObjectSpawner : VRObjectSpawner
{

    VR_GridPointer gridpointer;

    public override void Setup(ViewportContent controller)
    {
        base.Setup(controller);

        gridpointer = FindObjectOfType<VR_GridPointer>();
    }

    public override void ButtonAction(int index, Vector3 pos)
    {
        gridpointer.selectedPrefab = objects[index].spawnObject;
    }

}
