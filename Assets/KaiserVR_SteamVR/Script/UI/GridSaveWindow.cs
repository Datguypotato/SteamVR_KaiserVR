using System.Collections;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class GridSaveWindow : GridButtonBehaviour
{
    string[] filenames;
    string[] path;

    public string[] thumbnailpaths;

    Sprite[] thumbnailSprits;

    //public Image image;
    //public Rect rect;
    public Vector4 border;

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

        // getting save files
        path = Directory.GetFiles(Application.dataPath + "/Saves", "*.Kaiser");
 
        filenames = new string[path.Length];
        for (int i = 0; i < path.Length; i++)
        {
            filenames[i] = Path.GetFileNameWithoutExtension(path[i]);
        }

        // getting thumbnails
        Texture2D[] textures = Resources.LoadAll("Thumbnail", typeof(Texture2D)).Cast<Texture2D>().ToArray();
        thumbnailSprits = new Sprite[textures.Length];

        for (int i = 0; i < textures.Length; i++)
        {
            thumbnailSprits[i] = Sprite.Create(textures[i], new Rect(0, 0, 400, 400), Vector2.zero, 50, 0, SpriteMeshType.FullRect, border);
        }

        NumberOfObjects = filenames.Length;
    }

    public override void SetUpButton(GameObject buttonGameObject, int index)
    {
        // getting variables
        Button btn = buttonGameObject.GetComponent<Button>();
        ButtonUIController buttonUI = buttonGameObject.GetComponent<ButtonUIController>();

        // setting thumbnail
        // if this get a out of index exception then it means there are more save files then thumbnail
        buttonGameObject.GetComponent<ButtonUIController>().ContentImage.sprite = thumbnailSprits[index];

        buttonGameObject.GetComponentInChildren<Text>().text = filenames[index];
        btn.onClick.AddListener(delegate { ButtonAction(index); });
    }
}
