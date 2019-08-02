using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StreamingAssetsTest : MonoBehaviour
{

    public RawImage Image;
    void Awake()
    {
        string path =
#if UNITY_ANDROID && !UNITY_EDITOR
        Application.streamingAssetsPath + "/Josn/modelname.json";
#elif UNITY_IPHONE && !UNITY_EDITOR
        "file://" + Application.streamingAssetsPath + "/Josn/modelname.json";
#elif UNITY_STANDLONE_WIN||UNITY_EDITOR
            "file://" + Application.streamingAssetsPath + "/2.png";
#else
        string.Empty;
#endif
        StartCoroutine(ReadData(path));
    }

    IEnumerator ReadData(string path)
    {
        WWW www = new WWW(path);
        yield return www;
        while (www.isDone == false)
        {
            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForSeconds(0.5f);
        Image.texture = www.texture;
        yield return new WaitForEndOfFrame();
    }
}
