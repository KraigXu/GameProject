using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UIRelationNode : MonoBehaviour,IPointerEnterHandler,IPointerExitHandler
{
    public RelationMapWindow Window;
    public RectTransform NodeRect;
    public List<UIRelationNode> AssociationNodes = new List<UIRelationNode>();

    public int Id;

    public Entity Entity;
    public Button Button;
    public Text Text;

    public delegate void ThisClick(UIRelationNode uiRelationNode);

    public ThisClick callback;
    void Start()
    {
        Button.onClick.AddListener(OnClickCallback);
        Text.text = Id.ToString();
    }

    void Update()
    {

    }

    public void OnClickCallback()
    {
        if (callback != null)
        {
            callback(this);
        }




    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Window.ShowInfo(this);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        Window.CloseInfo(this);
    }
}
