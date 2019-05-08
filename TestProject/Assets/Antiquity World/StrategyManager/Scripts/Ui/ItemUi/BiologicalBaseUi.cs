using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameSystem
{
    public class BiologicalBaseUi : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler, 
        IPointerDownHandler, IDragHandler
    {

        public Entity Entity;


        public Text NameTex;
        public Image HeadImg;

        public int Id;

        void Awake()
        {
            HeadImg = transform.Find("Head").GetComponent<Image>();
            NameTex = transform.Find("Name").GetComponent<Text>();
        }

        void Start()
        {
            Biological biological = SystemManager.GetProperty<Biological>(Entity);

            NameTex.text = GameStaticData.BiologicalNameDic[biological.BiologicalId];
            HeadImg.overrideSprite = GameStaticData.BiologicalAvatar[biological.AvatarId];
        }

        void Update()
        {

        }

        void IDragHandler.OnDrag(PointerEventData eventData)
        {
        
        }

        public void OnPointerDown(PointerEventData eventData)
        {

        }

        public void OnPointerExit(PointerEventData eventData)
        {

        }

        public void OnPointerEnter(PointerEventData eventData)
        {

        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.pointerId == -1)
            {
                Debug.Log("Left Mouse Clicked.");
            }
            else if (eventData.pointerId == -2)
            {
                Debug.Log("Right Mouse Clicked.");
            }
        }
    }

}