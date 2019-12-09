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
    }

    public void OnSliderChange()
    {
        sliderText.text = slider.value.ToString();
    }
}
