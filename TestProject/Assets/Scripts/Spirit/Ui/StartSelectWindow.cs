using System.Collections;
using System.Collections.Generic;
using GameSystem;
using UnityEngine;
using UnityEngine.UI;

namespace AntiquityWorld
{
    public class StartSelectWindow : MonoBehaviour
    {
        [SerializeField]
        private RectTransform _newGame;
        [SerializeField]
        private RectTransform _continue;
        [SerializeField]
        private RectTransform _load;
        [SerializeField]
        private RectTransform _gameSetting;
        [SerializeField]
        private RectTransform _exit;

        void Start()
        {
        }

        void OnEnable()
        {

        }

        void OnDisable()
        {

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

