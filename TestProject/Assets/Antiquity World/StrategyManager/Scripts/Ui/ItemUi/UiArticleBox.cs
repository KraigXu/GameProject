using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UiArticleBox : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IDragHandler
{

    public Image image;
    public Text NumberText;
    public Entity Entity;


    void Start()
    {
    }

    void Update()
    {
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
            StrategyMouseInfo.Instance.InObject(gameObject);
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (Entity != Entity.Null)
        {
            StrategyMouseInfo.Instance.PutObject(gameObject);
        }
    }
}
