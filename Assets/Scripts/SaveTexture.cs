
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using UnityEngine.UI;
using System;


public class SaveTexture : MonoBehaviour
{
    public static SaveTexture Instance = null;
    public SaveTexture()
    {
        Instance = this;
    }

    private Camera PictureCamera;

    RenderTexture rt;
    Texture2D tex;
    Rect rect;
    string path;


    private void Start()
    {
        PictureCamera = Camera.main;
        Init();

        Tank.Instance.BeginRotate();
    }

    private void Init()
    {
        tex = new Texture2D(Screen.width, Screen.height, TextureFormat.ARGB32, false);
        rect = new Rect(0, 0, Screen.width, Screen.height);
        path = Application.dataPath + "/../TankData/pics/";

        if (!Directory.Exists(path))
        {
            Directory.CreateDirectory(path);
        }
    }

    int index = 0;

    public string TakePicture()
    {
        RenderTexture.active = PictureCamera.targetTexture;

        tex.ReadPixels(rect, 0, 0);
        tex.Apply();

        byte[] bytes = tex.EncodeToJPG();

        //string pic_name = PictureCamera.transform.rotation + ".png";
        string pic_name = index++ +".jpg";

        File.WriteAllBytes(path + pic_name, bytes);

        return pic_name;
    }
}
