using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

public class UiBuildingItem : MonoBehaviour
{

    public Text Name;
    public Entity BuildingEntity;
    public RectTransform Rect;
    public Button EnterButton;
    public OnBuildingClick OnBuildingEnter;


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
