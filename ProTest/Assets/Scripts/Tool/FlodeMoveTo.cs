using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class FlodeMoveTo : MonoBehaviour
{

    private static FlodeMoveTo _instance;
    public static FlodeMoveTo Instance
    {
        get
        {
            return _instance;
        }
    }


    public List<string> urls = new List<string>();
    public DirectoryInfo directoryInfo = null;
    public FileInfo[] files = null;
    private void Awake()
    {       
        _instance = this;
        return;
        Debug.Log(Application.dataPath + ">>>");
        string src = Application.dataPath + "/TestD/Texture2D1";
        directoryInfo = new DirectoryInfo(src);
        files = directoryInfo.GetFiles();
        return;
    }

    
    void Update()
    {
        return;
        if (Input.GetKeyDown(KeyCode.Q))
        {
            RdFile();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            MoveFiles2();
        }
    }

    public void InFile(string value)
    {
        return;
        string txt = Application.streamingAssetsPath + "/Test.txt";
        StreamWriter sw;

        if (File.Exists(txt))
        {
            sw = File.AppendText(txt);
            sw.WriteLine(value);
            sw.Close();
        }
    }
    
    public void InFloder(string value)
    {
        return;
        string txt = Application.streamingAssetsPath + "/Test.txt";
        StreamWriter sw;

        if (File.Exists(txt))
        {
            sw = File.AppendText(txt);
            sw.WriteLine(value);
            sw.Close();
        }
        return;
    }

    public void RdFile()
    {
        string txt = Application.streamingAssetsPath + "/Test.txt";

        using (StreamReader sr = new StreamReader(txt))
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                urls.Add(line);
            }
        }
    }

    public void MoveFiles2()
    {
        string url;
        string[] childs;
        for (int i = 0; i < urls.Count; i++)
        {
            childs = urls[i].Split('/');
            url = Application.dataPath + "/TestD/Move";
            for (int j = 0; j < childs.Length; j++)
            {

                url += "/" + childs[j];

                if (!Directory.Exists(url))//如果不存在就创建 dir 文件夹  
                    Directory.CreateDirectory(url);

                if (j == childs.Length - 1)
                {
                    foreach (FileInfo file in files)
                    {
                        if (file.Name.Contains(childs[j]))
                        {
                            file.MoveTo(Path.Combine(url, file.Name));
                        }
                    }
                }
            }

        }

    }


    public void MoveFiles()
    {

        Debug.Log(urls.Count);
        //Textures/UI/Overlays/ReservedForWork
        string url;
        string[] childs;
        for (int i = 0; i < urls.Count; i++)
        {
            childs = urls[i].Split('/');
            url = Application.dataPath + "/TestD/Move";
            for (int j = 0; j < childs.Length; j++)
            {
                if (j != childs.Length - 1)
                {
                    url += "/" + childs[j];

                    if (!Directory.Exists(url))//如果不存在就创建 dir 文件夹  
                        Directory.CreateDirectory(url);
                }
                else
                {
                    foreach (FileInfo file in files)
                    {
                        if (file.Name == childs[j] + ".png")
                        {
                            file.MoveTo(Path.Combine(url, file.Name));
                            // return;
                        }
                    }
                    //if(!Directory.Exists(Path.Combine(url, childs[j] + ".png")))
                    //{
                    //    Debug.Log("$$$$$$$"+ Path.Combine(url, childs[j] + ".png"));
                    //}
                }
            }

        }



    }
}
