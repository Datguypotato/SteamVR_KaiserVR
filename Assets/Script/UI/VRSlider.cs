using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class VRSlider : MonoBehaviour
{
    public Text sliderText;
    Slider slider;

    private void Awake()
    {
        slider = GetComponent<Slider>();

        OnSliderChange();
    }

    public void OnSliderChange()
    {
        if (slider.wholeNumbers)
        {
            sliderText.text = slider.value.ToString();
        }
        else
        {
            float newValue = slider.value / 10f;
            sliderText.text = newValue.ToString("0.0") + "M";
        }
    }
}
