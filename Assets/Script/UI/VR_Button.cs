using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class VR_Button : MonoBehaviour
{
    public int max;
    public int min = 1;

    Text floorText;

    private void Awake()
    {
        floorText = GetComponent<Text>();
    }

    public void FloorButton(int i)
    {
        if(i == 1 && int.Parse(floorText.text) < max)
        {
            floorText.text = (int.Parse(floorText.text) + i).ToString();
        }
        else if (i == -1 && int.Parse(floorText.text) > min)
        {
            floorText.text = (int.Parse(floorText.text) + i).ToString();
        }
        
    }
}
