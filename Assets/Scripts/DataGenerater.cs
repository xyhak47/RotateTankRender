/**
*Author: xyhak47
*/

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;

public class DataGenerater : MonoBehaviour 
{
    public static DataGenerater Instance = null;
    public DataGenerater()
    {
        Instance = this;
    }

    private string tankdata;

    void Start () 
	{

	}

    private void Update()
    {
        // test
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            Save();
        }
    }

    public void AppendData(string pic_name, Vector3 direction)
    {
        tankdata += (pic_name + " " + direction.x + " " + direction.y + " " + direction.z + "\n");
    }

    public void Save()
    {
        Debug.Log(tankdata);
        string file = "tankdata.txt";
        string path = Application.dataPath + "/../TankData/";

        File.WriteAllText(path + file, tankdata, Encoding.UTF8);
    }
}
