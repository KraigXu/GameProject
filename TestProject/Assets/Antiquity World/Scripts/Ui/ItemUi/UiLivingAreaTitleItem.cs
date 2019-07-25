using System.Collections;
using System.Collections.Generic;
using DataAccessObject;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 显示屏幕信息
/// </summary>
public class UiLivingAreaTitleItem : MonoBehaviour
{

    public string Name
    {
        get { return Text.text; }
        set { Text.text = value; }
    }


    public Entity ContetntEntity;
    public LivingAreaData Data;

    public Transform Target;
    public Text Text;

    public RectTransform Rect;

    public Image _typeImag;
    public Image _effectImag;
    public Image _usedImag;

    public Color color;
    void Awake()
    {
        Rect = (RectTransform) transform;
       // Text = transform.Find("Text").gameObject.GetComponent<Text>();
    }

    void Start()
    {
        


    }

    void Update()
    {

    }

    void OnEnable()
    {

    }

    void Disable()
    {

    }

}
