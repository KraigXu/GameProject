using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UiLivingAreaTitleItem : MonoBehaviour
{

    public string Name;
    public string Contetnt;

    public Transform Target;

    public RectTransform Rect;

    public Text Text;

    void Awake()
    {
        Rect = (RectTransform) transform;
    }

}
