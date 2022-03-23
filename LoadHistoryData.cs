using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class LoadHistoryData : MonoBehaviour
{    
    [SerializeField]
    private Transform content;
    [SerializeField]
    private GameObject textItem;
    [SerializeField]
    private TMP_InputField inputFieldDel;
    private GameObject textF;
    private bool updateContent;
     
    private void OnEnable()
    {
        updateContent = true;         
    }

    private void Update()
    {
        if (updateContent)
        {
            showHistoryData();
            updateContent = false;
        }
    }


    //Delete an object from history
    public void deleteObject()
    {
        string path = Application.persistentDataPath + "/" + inputFieldDel.text + ".json";
        if (File.Exists(path))
        {
            File.Delete(path);
        }
        int childsCount = content.childCount;
        for(int i = 0; i < childsCount; i++)
        {
            GameObject.Destroy(content.transform.GetChild(i).gameObject);
        }
        updateContent = true;
    }

    //Delete all history
    public void deleteAllObject()
    {
        string[] fileEntries = Directory.GetFiles(Application.persistentDataPath + "/");
        foreach (string fileName in fileEntries)
        {
            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
        }
        int childsCount = content.childCount;
        for (int i = 0; i < childsCount; i++)
        {
            GameObject.Destroy(content.transform.GetChild(i).gameObject);
        }
        updateContent = true;
    }


    private List<MeasurementData> ProcessDirectory()
    {
        List<MeasurementData> list = new List<MeasurementData>();
        string[] fileEntries = Directory.GetFiles(Application.persistentDataPath+"/");
        foreach (string fileName in fileEntries)
        {
            //read all lines from the json file  
            var lines = File.ReadAllText(fileName);
            //JSON deserialization using the static method JsonUtility.FromJson()
            var mData = JsonHelper.FromJson2(lines);
            if(!list.Contains(mData))
            {
                list.Add(mData);
            }                       
        }
        return list;
    }

    //Display measurements data in history
    private void showHistoryData()
    {
        foreach (MeasurementData m in ProcessDirectory())
        {
            textF = (GameObject)Instantiate(textItem);
            string res = "";
            if (textF != null)
            {
                res += m.objectName + "{";
                for (int i = 0; i < m.objectDimension.Count; i++)
                {
                    if (i < m.objectDimension.Count - 1)
                    {
                        res += m.objectDimension[i] + " , ";
                    }

                    if (i == m.objectDimension.Count - 1)
                    {
                        res += m.objectDimension[i] + "";
                    }
                }

                res += "}\n";
                textF.GetComponent<Text>().text = res;
                textF.transform.SetParent(content);
            }
        }    
    }
}
