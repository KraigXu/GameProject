using System.Collections;
using System.Collections.Generic;
using GameSystem;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;

namespace GameSystem
{
    public class UiBiologicalAvatarItem : MonoBehaviour
    {

        public Text Text;
        public Image AvatarImage;
        public Button Button;
        public int Key;
        public Entity Entity;
        public bool IsEnable;

        public EntityCallBack ClickCallBack;

        void Awake()
        {
            Button.onClick.AddListener(ButtonClic);
        }

        private void ButtonClic()
        {
            if (ClickCallBack != null)
            {
                ClickCallBack.Invoke(Entity, Key);
            }

        }

        public void SetupData(Entity entity)
        {
            this.Entity = entity;


        }

        public void Change()
        {
            BiologicalFixed biologicalFixed = GameStaticData.BiologicalRunDic[Entity];
            Text.text = string.Format("{0}.{1}", biologicalFixed.Name,biologicalFixed.Surname);
            AvatarImage.sprite = biologicalFixed.Sprite;




        }






    }

}

