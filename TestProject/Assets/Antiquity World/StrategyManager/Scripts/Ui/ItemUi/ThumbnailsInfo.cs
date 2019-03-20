using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
/// <summary>
/// 缩略图显示
/// </summary>
public class ThumbnailsInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image ThumbnailsImage;
    public Text ThumbnailsText;

    public void OnPointerExit(PointerEventData eventData)
    {
        ThumbnailsText.enabled = false;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        ThumbnailsText.enabled = true;
        //throw new System.NotImplementedException();
    }
}
