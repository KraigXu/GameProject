
using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class WXPoolManagerDB : ScriptableObject
{
    public List<string> poolsName;
    public List<WXPoolContainer> pools;
    public string namer = "default";
}