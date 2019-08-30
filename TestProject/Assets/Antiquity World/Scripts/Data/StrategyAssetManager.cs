using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 场景资源读取管理
/// </summary>
public class StrategyAssetManager : MonoBehaviour
{

    [SerializeField]
    private StrategyAsset _strategyAsset;

    public static StrategyAssetManager Instance
    {
        get { return _instance; }
    }
    private static StrategyAssetManager _instance;


    void Awake()
    {
        _instance = this;
    }

    void OnDestory()
    {
        Destroy(_instance);
    }

    public static Sprite GetArticleSprites(int id)
    {
        return _instance._strategyAsset.ArticleSprites[id];
    }

    public static HexUnit GetHexUnitPrefabs(int id)
    {
        return _instance._strategyAsset.HexUnitPrefabs[id];
    }

    public static Sprite GetCellFeatureSpt(int id)
    {
        return _instance._strategyAsset.CellFeatureSpt[id];
    }

    public static Sprite GetBiologicalAvatar(int id)
    {
        return _instance._strategyAsset.BiologicalAvatar[id];
    }

    public static Sprite GetCellTypeSprites(int id)
    {
        return _instance._strategyAsset.CellTypeSprites[id];
    }

    public static RectTransform UiFunctionButton { get { return _instance._strategyAsset.UiFunctionButton; } }
    public static RectTransform UiPersonButton { get { return _instance._strategyAsset.UiPersonButton; } }

    public static RectTransform UiCellFeature { get { return _instance._strategyAsset.UiCellFeature; } }

    public static RectTransform UiArticleView { get { return _instance._strategyAsset.UiArticleView; } }

    public static RectTransform UiArticleBox { get { return _instance._strategyAsset.UiArticleBox; } }

    public static RectTransform UiArticleLable { get { return _instance._strategyAsset.UiArticleLable; } }

    public static GUISkin UiArticleSkin { get { return _instance._strategyAsset.UiArticleSkin; } }

    public static RectTransform UiSpeciality { get { return _instance._strategyAsset.UiSpeciality; } }

    public static RectTransform UiLivingAreaTitle { get { return _instance._strategyAsset.UiLivingAreaTitle; } }

    public static RectTransform UiArticleInfo { get { return _instance._strategyAsset.UiArticleInfo; } }

    public static RectTransform UiCityTitle { get { return _instance._strategyAsset.UiCityTitle; } }

    public static RectTransform UiZigguratTitle { get { return _instance._strategyAsset.UiZigguratTitle; } }

    public static RectTransform UiOrganizationTitle { get { return _instance._strategyAsset.UiOrganizationTitle; } }

    public static RectTransform UiDialogNodeItem { get { return _instance._strategyAsset.UiDialogNodeItem; } }


}
