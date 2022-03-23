using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class SaveData : MonoBehaviour
{
    private string mobilePath;
    public void SaveIntoJson(List<MeasurementData> mes)
    {
        mobilePath = Application.persistentDataPath;
        string json = "*"+JsonHelper.ToJson(mes,true)+"*";        
        TextWriter tsw = new StreamWriter(mobilePath + "/MeasureData.json", true);
        tsw.WriteLine(json);
        tsw.Close();
    }

    public void SaveIntoJson2(MeasurementData mes,string fileName)
    {
        mobilePath = Application.persistentDataPath;
        string json = JsonHelper.ToJson2(mes, true);
        File.WriteAllText(mobilePath + "/"+fileName+".json",json);       
    }
}
[System.Serializable]
public class MeasurementData
{
    public string objectName;
    public List<string> objectDimension;    

    public MeasurementData(string a, List<string> b)
    {
        objectName = a;
        objectDimension = b;
    }

}

[System.Serializable]
public class MesurementList
{
    public List<MeasurementData> mesureData;
}

public static class JsonHelper
{
    public static List<MeasurementData> FromJson(string jsonFile)
    {
        MesurementList wrapper = JsonUtility.FromJson<MesurementList>(jsonFile);
        return wrapper.mesureData;
    }
    public static MeasurementData FromJson2(string jsonFile)
    {        
        return JsonUtility.FromJson<MeasurementData>(jsonFile);
    }
    public static string ToJson(List<MeasurementData> list, bool prettyPrint)
    {
        MesurementList wrapper = new MesurementList();
        wrapper.mesureData = list;
        return JsonUtility.ToJson(wrapper, prettyPrint);
    }

    public static string ToJson2(MeasurementData m, bool prettyPrint)
    {
        return JsonUtility.ToJson(m, prettyPrint);
    }

}