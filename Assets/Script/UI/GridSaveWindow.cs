using System.Collections;
using System.IO;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class GridSaveWindow : GridButtonBehaviour
{
    string[] filenames;
    string[] path;

    [SerializeField]
    public Texture2D texst;

    public Sprite[] thumbnailSprits;
    public override void ButtonAction(int index)
    {
        GridBuilder builder = FindObjectOfType<GridBuilder>();

        builder.LoadBuild(path[index]);
    }

    public override void ButtonAction(int index, Vector3 pos)
    {
        throw new System.NotImplementedException();
    }

    public override void Setup(ViewportContent controller)
    {
        this.controller = controller;

        path = Directory.GetFiles(Application.dataPath + "/Saves", "*.Kaiser");

        filenames = new string[path.Length];
        for (int i = 0; i < path.Length; i++)
        {
            filenames[i] = Path.GetFileNameWithoutExtension(path[i]);
        }
        NumberOfObjects = filenames.Length;
    }

    public override void SetUpButton(GameObject buttonGameObject, int index)
    {
        // getting variables
        Button btn = buttonGameObject.GetComponent<Button>();
        ButtonUIController buttonUI = buttonGameObject.GetComponent<ButtonUIController>();

        thumbnailSprits = Resources.LoadAll("/Thumbnail") as Sprite[];

        //Sprite thumbnail = Sprite.Create(tex, buttonGameObject.GetComponent<RectTransform>().rect, Vector2.zero);
        //buttonUI.ContentImage.sprite = thumbnail;

        buttonGameObject.GetComponentInChildren<Text>().text = filenames[index];
        btn.onClick.AddListener(delegate { ButtonAction(index); });
    }
}
