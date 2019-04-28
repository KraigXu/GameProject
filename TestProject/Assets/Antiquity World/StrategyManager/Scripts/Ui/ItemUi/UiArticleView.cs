using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiArticleView : MonoBehaviour
{

    public RectTransform ThisRect;

    public RectTransform ContentRect;
    public Text Text;

    void Awake()
    {
       

    }


    void Start()
    {

    }

    void Update()
    {
        ThisRect.sizeDelta=new Vector2(ThisRect.sizeDelta.x, ContentRect.rect.height+30+30);

    }

}
