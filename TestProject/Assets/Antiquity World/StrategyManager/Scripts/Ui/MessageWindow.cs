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
        public List<Text> Texts;

        public Dictionary<Text, Sequence> TextSequences=new Dictionary<Text, Sequence>();

        protected override void InitWindowData()
        {
            this.ID = WindowID.MessageWindow;

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
            for (int i = 0; i < Texts.Count; i++)
            {
                //Sequence mySequence = DOTween.Sequence();
                
                //mySequence.intId = i;
                //mySequence.Append(Texts[i].DOFade(0f, 3f).OnStepComplete(() => TextAnimation(Texts[i])).Pause());
                //TextSequences.Add(Texts[i], mySequence);

            }
        }

        void TextAnimation(Text text)
        {

            text.gameObject.SetActive(false);
            text.color=Color.black;
            Debug.Log(text.gameObject.name);
        }





        public void Log(string value)
        {
            Text current = null;
            for (int i = 0; i < Texts.Count; i++)
            {
                if (Texts[i].gameObject.activeSelf == false)
                {
                    current = Texts[i];
                    Debug.Log(i);
                    break;
                }
            }

            if (current != null)
            {
                current.text = value;
                current.gameObject.SetActive(true);
                current.transform.SetAsLastSibling();
                current.DOFade(0, 3f).OnComplete(() => TextAnimation(current));
            }
            else  //如何等于NULL 则需要生成新的GO
            {

            }
        }


    }

}

