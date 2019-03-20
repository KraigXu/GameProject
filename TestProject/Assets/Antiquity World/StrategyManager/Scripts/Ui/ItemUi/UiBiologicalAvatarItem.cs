using System.Collections;
using System.Collections.Generic;
using GameSystem;
using UnityEngine;
using UnityEngine.UI;

namespace GameSystem
{
    public class UiBiologicalAvatarItem : MonoBehaviour
    {

        public Image AvatarImage;
        public Button Button;
        public int Key;

        public SingleParameterEvent ClickCallBack;

        void Awake()
        {
            Button.onClick.AddListener(ButtonClic);
        }

        private void ButtonClic()
        {
            if (ClickCallBack != null)
            {
                ClickCallBack.Invoke(Key);
            }

        }

    }

}

