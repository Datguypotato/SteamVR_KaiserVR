using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Spawner : MonoBehaviour {

    public GameObject spawnedObject;
    private Button button;
    private Text buttonText;

	// Use this for initialization
	void Start ()
    {
        if(spawnedObject == null)
        {
            Debug.LogWarning("spawnedObject object is null");
        }
        else
        {
            button = GetComponent<Button>();
            buttonText = GetComponentInChildren<Text>();
            buttonText.text = spawnedObject.name;
        }
    }

    public void SetSpawnableObject(GameObject obj)
    {
        if(obj == null)
        {
            Debug.LogWarning("spawnedObject object is null");
        }
        else
        {
            spawnedObject = obj;
            button = GetComponent<Button>();
            buttonText = GetComponentInChildren<Text>();
            buttonText.text = spawnedObject.name;
        }
    }
	
    public void SpawnObject()
    {
        Instantiate(spawnedObject, gameObject.transform.position, new Quaternion(0,0,0,0));
    }

}
