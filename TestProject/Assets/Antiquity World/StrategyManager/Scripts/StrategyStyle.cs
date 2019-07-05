using System.Collections;
using System.Collections.Generic;
using Unity.Rendering;
using UnityEngine;

public class StrategyStyle : MonoBehaviour
{
    public static StrategyStyle Instance
    {
        get { return _instance; }
    }
    private static StrategyStyle _instance;

    public List<MeshInstanceRenderer> BiologicalRenderers;

    public List<HexUnit> HexUnitPrefabs;
    public RectTransform UiLivingAreaTitle;

    [SerializeField]
    private RectTransform _uiFunctionButton;
    [SerializeField]
    private RectTransform _uiPersonButton;


    public RectTransform UiArticleBox;
    public RectTransform UiArticleView;
    public RectTransform UiEffectTip;
    public RectTransform UiLableTip;
    public RectTransform UiImageTips;
    public RectTransform UiSpeciality;

    [SerializeField]
    private RectTransform _uiCellFeature;

    [SerializeField]
    private RectTransform _uiBuildStatus;


    public static RectTransform UiFunctionButton { get { return _instance._uiFunctionButton; } }
    public static RectTransform UiPersonButton { get { return _instance._uiPersonButton; } }
    public static RectTransform UiCellFeature { get { return _instance._uiCellFeature; } }
    public static RectTransform UiBuildStatus { get { return _instance._uiBuildStatus; } }


    void Awake()
    {
        _instance = this;
    }






}
