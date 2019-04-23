using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrategyStyle : MonoBehaviour
{

    public static StrategyStyle Instance
    {
        get { return _instance; }
    }
    private static StrategyStyle _instance;


    public Transform ModelCityO1;
    public Transform ModelCity02;

    public RectTransform UiFunctionButton;
    public RectTransform UiPersonButton;
    public RectTransform UiEffectTip;
    public RectTransform UiLableTip;
    public RectTransform UiImageTips;
    

    void Awake()
    {
        _instance = this;
    }

}
