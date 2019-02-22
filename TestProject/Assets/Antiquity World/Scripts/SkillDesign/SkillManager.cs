using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 单一对象的skill管理
/// </summary>
public class SkillManager : MonoBehaviour {



    public List<SkillInstance> SkillInstances=new List<SkillInstance>();
    public TextAsset Texts1;
    public TextAsset Texts2;
    public TextAsset Texts3;
    public TextAsset Texts4;
    public TextAsset Texts5;

    void Awake()
    {

    }

    void Start()
    {
        if (Texts1 != null)
        {
          //  SkillInstances.Add(Texts1);
        }

        if (Texts2 != null)
        {

        }



    }

    void Update()
    {

    }
}
