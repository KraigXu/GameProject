using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;
using GameSystem;

namespace GameSystem.Ui
{
    public class PlayerInfoWindow : UIWindowBase
    {
        public Image AvateImage;
        public Text NameText;

        public RectTransform RestBtns;
        public RectTransform TeamBtns;
        public RectTransform PersonBtns;
        public RectTransform LogBtns;
        public RectTransform IntelligenceBtns;
        public RectTransform OptionBtns;

        protected override void InitWindowData()
        {
            this.ID = WindowID.PlayerInfoWindow;

            windowData.windowType = UIWindowType.NormalLayer;
            windowData.showMode = UIWindowShowMode.DoNothing;
            windowData.navigationMode = UIWindowNavigationMode.IgnoreNavigation;
            windowData.colliderMode = UIWindowColliderMode.None;
            windowData.closeModel = UIWindowCloseModel.Destory;
        }

        public void SetRestTog(bool toggle)
        {
            RestBtns.gameObject.SetActive(toggle);
        }

        public void SetTeamTog(bool toggle)
        {
            TeamBtns.gameObject.SetActive(toggle);

        }

        public void SetPersonTog(bool toggle)
        {
            PersonBtns.gameObject.SetActive(toggle);
        }

        public void SetLogTog(bool toggle)
        {
            LogBtns.gameObject.SetActive(toggle);
        }

        public void SetIntelligenceTog(bool toggle)
        {
            IntelligenceBtns.gameObject.SetActive(toggle);
        }

        public void SetOptionTog(bool toggle)
        {
            OptionBtns.gameObject.SetActive(toggle);
        }

       
        public override void InitWindowOnAwake()
        {
            AvateImage.overrideSprite = StrategyScene.Instance.Player.AvatarSprite;
            NameText.text = StrategyScene.Instance.Player.SurName + StrategyScene.Instance.Player.Name;




        }

        

    }
}