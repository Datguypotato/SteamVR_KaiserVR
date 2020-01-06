using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEditor;
using System;

public class ButtonUIController : MonoBehaviour
{
    public Image ContentImage { get { return contentImage; } }

    [Tooltip("The name of the content.")]
    [SerializeField] private Text contentName;
    [Tooltip("The target image which the UI panel will use to assign custom sprites to.")]
    [SerializeField] private Image contentImage;
    [Obsolete("Still in use, but will be replaced with the image build in highlight color.")]
    [SerializeField] private Color highLightColor;

    public void SetHighLight(bool value)
    {
        contentImage.color = value ? highLightColor : Color.clear;
    }

    public void SetImage(Sprite sprite)
    {
        contentImage.sprite = sprite;
    }

    public void SetName(string name)
    {
        contentName.text = name;
    }
}
