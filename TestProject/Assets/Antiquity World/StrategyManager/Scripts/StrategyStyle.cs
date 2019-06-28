using System.Collections;
using System.Collections.Generic;
using Unity.Rendering;
using UnityEngine;

public class StrategyStyle : MonoBehaviour
{
    public static StrategyStyle Instance
    {
        get { return _instance;}
    }
    private static StrategyStyle _instance;

    public List<MeshInstanceRenderer> BiologicalRenderers;

    public List<HexUnit> HexUnitPrefabs;
    public RectTransform UiLivingAreaTitle;

    public RectTransform UiFunctionButton;
    public RectTransform UiPersonButton;
    public RectTransform UiArticleBox;
    public RectTransform UiArticleView;
    public RectTransform UiEffectTip;
    public RectTransform UiLableTip;
    public RectTransform UiImageTips;
    public RectTransform UiSpeciality;


    [SerializeField]
    private RectTransform _uiCellFeature;
    public static RectTransform UiCellFeature
    {
        get { return _instance._uiCellFeature; }
    }

    void Awake()
    {
        _instance = this;
    }






}
