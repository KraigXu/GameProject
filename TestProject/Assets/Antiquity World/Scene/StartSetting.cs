using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartSetting : MonoBehaviour
{

    private static StartSetting _instance;
    public static StartSetting Instance
    {
        get { return _instance;}

    }

    public TextAsset SkillConfig;

    void Awake()
    {

    }
}
