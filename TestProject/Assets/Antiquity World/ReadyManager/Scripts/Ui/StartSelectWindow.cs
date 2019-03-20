using System.Collections;
using System.Collections.Generic;
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

            WXSceneManager.Load("DemoScene");

        }
    }

}

