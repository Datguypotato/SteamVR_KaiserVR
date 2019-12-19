using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class VRSlider : MonoBehaviour
{
    public Text sliderText;
    public bool hasUnit;
    Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();

        OnSliderChange();
    }

    public void OnSliderChange()
    {
        float newValue = slider.value;
        if (hasUnit)
        {
            newValue = newValue / 10;
            sliderText.text = newValue.ToString("0.0") + "M";
        }
        else
        {
            sliderText.text = newValue.ToString();
        }
    }
}
