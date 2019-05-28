using System.Collections;
using System.Collections.Generic;
using GameSystem;
using UnityEngine;
using UnityEngine.UI;

namespace AntiquityWorld.ReadyManager
{
    public class StartSelectWindow : MonoBehaviour
    {

        public Button StartBtn;
        public Button ExitBtn;

        void Start()
        {
            StartBtn.onClick.AddListener(StartBtnEvent);
        }
        void Update()
        {

        }

        private void StartBtnEvent()
        {

            GameSceneInit.Load("DemoScene");

        }
    }

}

