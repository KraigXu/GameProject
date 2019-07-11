using System.Collections;
using System.Collections.Generic;
using GameSystem;
using Unity.Entities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;


public class UiArticleBox : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler
{


    [SerializeField]
    private Image _articlePicture;
    [SerializeField]
    private Text _articleNameTxt;

    public Image image;
    public Text NumberText;
    public Entity Entity;


    void Start()
    {
        ArticleItem articleItem= SystemManager.GetProperty<ArticleItem>(Entity);

        _articlePicture.sprite = StrategyAssetManager.GetArticleSprites(articleItem.SpriteId);
        ArticleItemFixed articleItemFixed = GameStaticData.ArticleDictionary[Entity];
        _articleNameTxt.text = articleItemFixed.Name;
        
    }


    public void OnDrag(PointerEventData eventData)
    {
    }

    public void OnPointerClick(PointerEventData eventData)
    {
    }

    public void OnPointerDown(PointerEventData eventData)
    {
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (Entity != Entity.Null)
        {
           RectTransform rectTransform=  WXPoolManager.Pools[Define.GeneratedPool].Spawn(StrategyAssetManager.UiArticleInfo) as RectTransform;

            //UiArticleInfo uiArticleInfo= rectTransform.GetComponent<UiArticleInfo>();
            //uiArticleInfo.

            //StrategyMouseInfo.Instance.InObject(gameObject);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Entity != Entity.Null)
        {
            StrategyMouseInfo.Instance.PutObject(gameObject);
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {

    }
}
