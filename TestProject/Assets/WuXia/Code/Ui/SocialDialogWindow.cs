using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace WX.Ui
{

    public class SocialDialogWindow : UIWindowBase
    {
        
        [SerializeField]
        private GameObject _dialogPanel;

        [SerializeField]
        private Text _aText;
        [SerializeField]
        private Image _aAvatar;

        [SerializeField]
        private Text _bText;
        [SerializeField]
        private Image _bAvatar;

        [SerializeField]
        private Text _startTxt;

        [SerializeField]
        private List<GameObject> _items;

        [SerializeField]
        private int[] _currentItem;

        private SocialDialogWindowData _socialDialogWindowData;

        protected override void SetWindowId()
        {
            this.ID = WindowID.SocialDialogWindow;
        }

        protected override void InitWindowCoreData()
        {
            windowData.windowType = UIWindowType.NormalLayer;
            windowData.showMode = UIWindowShowMode.DoNothing;
            windowData.navigationMode = UIWindowNavigationMode.IgnoreNavigation;
            windowData.colliderMode = UIWindowColliderMode.None;
            windowData.closeModel = UIWindowCloseModel.Destory;
            windowData.animationType = UIWindowAnimationType.None;
            windowData.playAnimationModel = UIWindowPlayAnimationModel.Stretching;
        }
       

        public override void InitWindowOnAwake()
        {
            //_socialDialog=new SocialDialog();

            //_socialDialog.Aid = 1;
            //_socialDialog.Bid = 2;

            //_socialDialog.StartId = 2;
            //_socialDialog.StartlogId =new int[]{1,2,3};

            //_socialDialog.DialogEvent = SocialDialogCallBack;
            //log =new Dictionary<int, string>();
            //log.Add(1,"A{0}");
            //log.Add(2,"B1");
            //log.Add(3,"B2");
            //log.Add(4,"B3");
            //log.Add(5,"B4");
            //log.Add(6, "C1");
            //log.Add(7, "C2");
            //log.Add(8, "C3");


            //_dialogPanel.SetActive(true);
            //_aText.text = "A";
            //_bText.text = "B";

            //_startTxt.text = string.Format(log[_socialDialog.StartId]);
            //_currentItem = _socialDialog.StartlogId;
            //
        }

        //private SocialDialog _socialDialog;
        //private Dictionary<int,string> log=new Dictionary<int, string>();

        public class SocialDialog
        {
            public int Aid;
            public int Bid;

            public int StartId;
            public int[] StartlogId;
            public SocialDialogEvent DialogEvent;
        }

        public override void ShowWindow(BaseWindowContextData contextData)
        {
            if(contextData==null)return;
            base.ShowWindow(contextData);

            _socialDialogWindowData = (SocialDialogWindowData) contextData;

            Change();
        }

        public void ItemOnClick(GameObject go)
        {
            int id =Int32.Parse( go.name);
           _currentItem= _socialDialogWindowData.DialogEvent(id);
            Change();
        }

        public void Change()
        {

            for (int i = 0; i < _items.Count; i++)
            {
                _items[i].SetActive(false);
            }
           

            for (int i = 0; i < _currentItem.Length; i++)
            {
                _items[i].gameObject.name = _socialDialogWindowData.StartlogId[i].ToString();
                
                _items[i].GetComponentInChildren<Text>().text = string.Format(GameStaticData.SocialDialogNarration[_currentItem[i]],"张三"); 
                UIEventTriggerListener.Get(_items[i]).onClick = ItemOnClick;
            }
        }

        public int[] SocialDialogCallBack(int id)
        {
            int[] ids = null;
            if (id == 1)
            {
                ids=new int[]{3,4,5};
            }
            else
            {
                ids=new int[]{6};
            }

            return ids;
        }
    }
}