using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

public class UiBuildingItem : MonoBehaviour
{

    public OnBuildingClick OnBuildingEnter;
    public Entity BuildingEntity;
    public RectTransform Rect;

    public string Value
    {
        get { return Textval.text; }
        set { Textval.text = value; }
    }


    [SerializeField]
    private Text Textval;
    [SerializeField]
    private Button EnterButton;

    void Start()
    {
        EnterButton.onClick.AddListener(ButtonClick);
    }


    private void ButtonClick()
    {
        if (OnBuildingEnter!=null)
        {
            OnBuildingEnter(BuildingEntity);
        }
    }


}
