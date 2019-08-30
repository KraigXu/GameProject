using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace GameSystem.Ui
{
    public class SocialDialogWindow : UIWindowBase
    {
        public RectTransform QuizzerParent;
        public RectTransform ReplierParent;

        public int CurDialogId=0;
        public Text AsideTxt;

        [SerializeField]
        private RectTransform  _dialogParent;
        private List<RectTransform> _curDialogItem;
        private SocialDialogWindowData _socialDialogWindowData;

        protected override void InitWindowData()
        {
            this.ID = WindowID.SocialDialogWindow;

            windowData.windowType = UIWindowType.NormalLayer;
            windowData.showMode = UIWindowShowMode.DoNothing;
            windowData.navigationMode = UIWindowNavigationMode.IgnoreNavigation;
            windowData.colliderMode = UIWindowColliderMode.None;
            windowData.closeModel = UIWindowCloseModel.Destory;
            windowData.playAnimationModel = UIWindowPlayAnimationModel.Stretching;
        }
        public override void InitWindowOnAwake()
        {
        }

        public override void ShowWindow(BaseWindowContextData contextData)
        {
            if (contextData == null)
            {
                return;
            }
              
            base.ShowWindow(contextData);
            _socialDialogWindowData = (SocialDialogWindowData) contextData;

            if (SystemManager.Contains<Biological>(_socialDialogWindowData.OnSelfEntity))
            {
                Biological biologicalOnSelf = SystemManager.GetProperty<Biological>(_socialDialogWindowData.OnSelfEntity);
                BiologicalFixed biologicalosFixed = GameStaticData.BiologicalDictionary[_socialDialogWindowData.OnSelfEntity];

                RectTransform personRect = WXPoolManager.Pools[Define.GeneratedPool].Spawn(StrategyAssetManager.UiPersonButton, QuizzerParent);
                BiologicalBaseUi bui = personRect.GetComponent<BiologicalBaseUi>();
                bui.Avatar = StrategyAssetManager.GetBiologicalAvatar(biologicalOnSelf.AvatarId);
                bui.PersonName = biologicalosFixed.Surname + biologicalosFixed.Name;
                bui.Entity = _socialDialogWindowData.OnSelfEntity;
            }

            if (SystemManager.Contains<Biological>(_socialDialogWindowData.OtherEntity))
            {
                Biological biologicalReplier = SystemManager.GetProperty<Biological>(_socialDialogWindowData.OtherEntity);
                BiologicalFixed biologicalFixedRr = GameStaticData.BiologicalDictionary[_socialDialogWindowData.OtherEntity];

                RectTransform personRect = WXPoolManager.Pools[Define.GeneratedPool].Spawn(StrategyAssetManager.UiPersonButton, ReplierParent);
                BiologicalBaseUi bui = personRect.GetComponent<BiologicalBaseUi>();
                bui.Avatar = StrategyAssetManager.GetBiologicalAvatar(biologicalReplier.AvatarId);
                bui.PersonName = biologicalFixedRr.Surname + biologicalFixedRr.Name;
                bui.Entity = _socialDialogWindowData.OtherEntity;
            }

            AsideTxt.text = _socialDialogWindowData.Aside;

            for (int i = 0; i < _socialDialogWindowData.Other.Count; i++)
            {
                var node = _socialDialogWindowData.Other[i];

                var rect = WXPoolManager.Pools[Define.GeneratedPool].Spawn(StrategyAssetManager.UiDialogNodeItem, _dialogParent);


                UiDialogNodeItem item = rect.GetComponent<UiDialogNodeItem>();
                item.ContentTxt.text = node.Content;
                item.EventBtn.onClick.AddListener(delegate () { DialogIdEvent(node.Id); });

                _curDialogItem.Add(rect);
            }

            for (int i = 0; i < _socialDialogWindowData.OnSelf.Count; i++)
            {
                var node = _socialDialogWindowData.OnSelf[i];

                var rect=WXPoolManager.Pools[Define.GeneratedPool].Spawn(StrategyAssetManager.UiDialogNodeItem, _dialogParent);

                UiDialogNodeItem item= rect.GetComponent<UiDialogNodeItem>();
                item.ContentTxt.text = node.Content;
                item.EventBtn.onClick.AddListener(delegate() { DialogIdEvent(node.Id); });

                _curDialogItem.Add(rect);
            }
        }

        public void DialogIdEvent(int id)
        {
            DialogNode dialogNode=null;
            for (int i = 0; i < _socialDialogWindowData.OnSelf.Count; i++)
            {
                for (int j = 0; j < _socialDialogWindowData.OnSelf[i].Child.Count; j++)
                {
                    if (id == _socialDialogWindowData.OnSelf[i].Child[j].Id)
                    {
                        dialogNode = _socialDialogWindowData.OnSelf[i].Child[j];
                    }
                }
            }

            for (int i = 0; i < _socialDialogWindowData.Other.Count; i++)
            {
                for (int j = 0; j < _socialDialogWindowData.Other[i].Child.Count; j++)
                {
                    if (id == _socialDialogWindowData.Other[i].Child[j].Id)
                    {
                        dialogNode = _socialDialogWindowData.Other[i].Child[j];
                    }
                }
            }


            if (dialogNode != null)
            {
                AsideTxt.text = dialogNode.Content;

                for (int i = 0; i < dialogNode.Child.Count; i++)
                {
                    var node = dialogNode.Child[i];

                    var rect = WXPoolManager.Pools[Define.GeneratedPool].Spawn(StrategyAssetManager.UiDialogNodeItem, _dialogParent);


                    UiDialogNodeItem item = rect.GetComponent<UiDialogNodeItem>();
                    item.ContentTxt.text = node.Content;
                    item.EventBtn.onClick.AddListener(delegate () { DialogIdEvent(node.Id); });

                    _curDialogItem.Add(rect);
                }
            }
            else  //执行EventCode
            {
                Debug.Log("EventCode");

                switch (dialogNode.EventCode)
                {
                    case 99:



                        break;
                    case 98:
                        break;
                    case 97:
                        break;
                    case 80:
                        break;
                    case 70:
                        break;
                    case 60:
                        break;
                    default:
                        break;
                }


            }

        }
    }
}