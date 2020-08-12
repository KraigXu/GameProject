using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using Verse;
using System.Linq;
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
    //控制重复路径文件
    public List<string> urls2 = new List<string>();


    public DirectoryInfo directoryInfo = null;
    public FileInfo[] files = null;

    private string txt1 = "";
    private string txt2 = "";

    void Awake()
    {
        _instance = this;

        txt1 = Application.dataPath + "/Test/Test1.txt";
        txt2 = Application.dataPath + "/Test/Test2.txt";
    }

    void Start()
    {
        List<Texture2D> list = (from x in ContentFinder<Texture2D>.GetAllInFolder("World/Hills") orderby x.name select x).ToList();

        Debug.Log("TestLength>>>"+list.Count);
        string src = Application.dataPath + "/Test/Textures";
        directoryInfo = new DirectoryInfo(src);
        files = directoryInfo.GetFiles();
    }


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            RdFile();
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            MoveFiles2();
        }
    }



    /// <summary>
    /// 对应准确位置文件
    /// </summary>
    /// <param name="value"></param>
    public void InFile(string value)
    {
        StreamWriter sw;

        if (File.Exists(txt1))
        {
            sw = File.AppendText(txt1);
            sw.WriteLine(value);
            sw.Close();
        }
    }


    public void InFloder(string value)
    {
        return;
        StreamWriter sw;

        if (File.Exists(txt2))
        {
            sw = File.AppendText(txt2);
            sw.WriteLine(value);
            sw.Close();
        }
    }

    public void RdFile()
    {
        using (StreamReader sr = new StreamReader(txt1))
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                urls.Add(line);
            }
        }

        using (StreamReader sr = new StreamReader(txt2))
        {
            string line;
            while ((line = sr.ReadLine()) != null)
            {
                urls2.Add(line);
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
            url = Application.dataPath + "/Test/AbsFile";
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
                        }
                        if(file.Name==childs[j]+ ".png.meta")
                        {
                            file.MoveTo(Path.Combine(url, file.Name));
                        }
                    }
                }
            }
        }

        //for (int i = 0; i < urls2.Count; i++)
        //{
        //    childs = urls2[i].Split('/');
        //    url = Application.dataPath + "/Test/AbsFloder";
        //    for (int j = 0; j < childs.Length; j++)
        //    {
        //        url += "/" + childs[j];
        //        if (!Directory.Exists(url))//如果不存在就创建 dir 文件夹  
        //            Directory.CreateDirectory(url);

        //        if (j == childs.Length - 1)
        //        {

        //            foreach (FileInfo file in files)
        //            {
        //                if (file.Name == childs[j] + ".png")
        //                {
        //                    file.MoveTo(Path.Combine(url, file.Name));
        //                }
        //            }
        //        }
        //    }
        //}


    }





    
}
