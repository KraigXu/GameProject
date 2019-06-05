using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Unity.Entities;

namespace GameSystem.Ui
{
    /// <summary>
    /// 消息界面 显示提示消息的界面
    /// </summary>
    public class MessageWindow : UIWindowBase
    {

        public RectTransform ContentParent;
        public GameObject Prefab;
        public List<Text> Texts;

        public Dictionary<Text, Sequence> TextSequences=new Dictionary<Text, Sequence>();

        public Queue<RectTransform> overflows =new Queue<RectTransform>();

        public int MaxNumber;
        public float Time=5f;


        protected override void InitWindowData()
        {
            this.ID = WindowID.MessageWindow;

            windowData.windowType = UIWindowType.NormalLayer;
            windowData.showMode = UIWindowShowMode.DoNothing;
            windowData.navigationMode = UIWindowNavigationMode.IgnoreNavigation;
            windowData.colliderMode = UIWindowColliderMode.None;
            windowData.closeModel = UIWindowCloseModel.Destory;
            windowData.animationType = UIWindowAnimationType.None;
        }

        public override void InitWindowOnAwake()
        {
        }

        void TextAnimation(Text text)
        {
            text.gameObject.SetActive(false);
            text.color=Color.black;
            Debug.Log(text.gameObject.name);
        }

        void DestoryText()
        {

            GameObject.Destroy(overflows.Dequeue().gameObject);
        }

        public void Log(string value)
        {
            Text current = null;
            for (int i = 0; i < Texts.Count; i++)
            {
                if (Texts[i].gameObject.activeSelf == false)
                {
                    current = Texts[i];
                    break;
                }
            }

            if (current != null)
            {
                current.text = value;
                current.gameObject.SetActive(true);
                current.transform.SetAsLastSibling();
                current.DOFade(0, Time).OnComplete(() => TextAnimation(current));
            }
            else  
            {
                GameObject go =  GameObject.Instantiate(Prefab) as GameObject;
                RectTransform rect= go.GetComponent<RectTransform>();
                rect.SetParent(ContentParent);
                overflows.Enqueue(rect);
                Text text = go.GetComponent<Text>();
                text.text = value;
                text.color=Color.black;


                text.DOFade(0, Time).OnComplete((() => DestoryText()));
                go.SetActive(true);
            }
        }


    }

}

