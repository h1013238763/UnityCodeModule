using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;

public class JsonController : BaseController<JsonController>
{
    
    /// <summary>
    /// Write data into Json file
    /// </summary>
    /// <param name="data">data object</param>
    /// <param name="file_name">file name</param>
    /// <param name="dir">the sub folder</param>
    public void WriteFile(object data, string file_name, string file_extension, string dir = "")
    {
        // get the save path
        string path = Application.persistentDataPath + "/" + dir;
        // path checking
        if(!Directory.Exists(path))
            Directory.CreateDirectory(path);
        path += "/" + file_name + file_extension;

        File.WriteAllText(path, JsonUtility.ToJson(data));
    }

    public object ReadFile<T>(string file_name, string file_extension, string dir = "")
    {
        // try to find file in two paths
        string path = Application.persistentDataPath + "/" + dir + file_name + ".xml";
        if(!File.Exists(path))
            path = Application.streamingAssetsPath + "/" + dir + file_name + ".xml";
        if(!File.Exists(path))
            path = dir + file_name + ".xml";
        if(!File.Exists(path))
        {
            Debug.LogError( new FileNotFoundException() );
            return null;  // return a default file if not found
        }
    
        try{
            using (StreamReader reader = new StreamReader(path))
            {
                return JsonUtility.FromJson<T>(reader.ReadToEnd());
            }
        }
        catch( Exception e )
        {
            Debug.LogError(e);
            return null;
        }
    }

    public void DeleteFile(string file_name, string file_extension, string dir = "")
    {
        string path = Application.persistentDataPath + "/" + dir + file_name + file_extension;

        if(File.Exists(path))
        {
            File.Delete(path);
        }
    }
}