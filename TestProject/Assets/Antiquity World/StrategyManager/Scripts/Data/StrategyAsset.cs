using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "GameAsset/StrategyAsset")]
public class StrategyAsset : ScriptableObject
{
    public List<HexUnit> HexUnitPrefabs;

    public List<Sprite> ArticleSprites;

    public List<Sprite> CellFeatureSpt;
    public List<Sprite> CellTypeSprites;

    public List<Sprite> BiologicalAvatar;

    public RectTransform UiArticleBox;
    public RectTransform UiArticleView;
    public RectTransform UiArticleInfo;
    public RectTransform UiArticleLable;
    public GUISkin UiArticleSkin;

    public RectTransform UiFunctionButton;
    public RectTransform UiPersonButton;

    public RectTransform UiSpeciality;
    public RectTransform UiCellFeature;

    public RectTransform UiLivingAreaTitle;



}
