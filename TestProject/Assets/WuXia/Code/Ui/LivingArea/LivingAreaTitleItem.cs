using System.Collections;
using System.Collections.Generic;
using LivingArea;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 显示居住地标题
/// </summary>
public class LivingAreaTitleItem : BaseCorrespondenceByModelControl
{

    [SerializeField]
    private Text _titleText;
    [SerializeField]
    private RectTransform _buffParent;

    public void Init(Transform target)
    {
        this.Target = target;
        LivingAreaNode node=  target.GetComponent<LivingAreaNode>();
        _titleText.text = node.Name;
        LivingAreaState[] states = node.Groups;
        if (states.Length > 0)
        {
            GameObject buffPrefab = Define.Value.UiLivingAreaBuff;
            for (int i = 0; i < states.Length; i++)
            {
                GameObject item = UGUITools.AddChild(_buffParent.gameObject, buffPrefab);
                ThumbnailsInfo thumbnails= item.GetComponent<ThumbnailsInfo>();
                thumbnails.ThumbnailsImage.overrideSprite = states[i].Icon;
                thumbnails.ThumbnailsText.text = states[i].Description;
            }
        }
     

    }
}
