using System.Collections;
using System.Collections.Generic;
using TinyFrameWork;
using UnityEngine;
using UnityEngine.UI;

public class StrategyTimeWindow : UIWindowBase
{
    public Text text1;
    public Text text2;
    public Text text3;
    public Text text4;
    protected override void SetWindowId()
    {
        this.ID = WindowID.StrategyTimeWindow;
    }
    protected override void InitWindowCoreData()
    {
        windowData.windowType = UIWindowType.BackgroundLayer;
        windowData.showMode = UIWindowShowMode.DoNothing;
        windowData.navigationMode = UIWindowNavigationMode.IgnoreNavigation;
        windowData.colliderMode = UIWindowColliderMode.None;
        windowData.closeModel = UIWindowCloseModel.Destory;
        windowData.animationType = UIWindowAnimationType.None;
        windowData.playAnimationModel = UIWindowPlayAnimationModel.Stretching;
    }
    public override void InitWindowOnAwake()
    {

    }


    void Update()
    {

        text1.text = TimeManager.Instance.curYera.ToString();
        text2.text = TimeManager.Instance.curMonth.ToString();
        text3.text = TimeManager.Instance.curDay.ToString();

        if (TimeManager.Instance.curMonth == 2 || TimeManager.Instance.curMonth == 3 || TimeManager.Instance.curMonth == 4)
        {
            text4.text = "春";
        }
        else if (TimeManager.Instance.curMonth == 5 || TimeManager.Instance.curMonth == 6 || TimeManager.Instance.curMonth == 7)
        {
            text4.text = "夏";
        }
        else if (TimeManager.Instance.curMonth == 8 || TimeManager.Instance.curMonth == 9 || TimeManager.Instance.curMonth == 10)
        {
            text4.text = "秋";
        }
        else if (TimeManager.Instance.curMonth == 11 || TimeManager.Instance.curMonth == 12 || TimeManager.Instance.curMonth == 1)
        {
            text4.text = "冬";
        }
    }

}
