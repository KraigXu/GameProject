using TinyFrameWork;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// 信息UI
/// </summary>
public class StrategyMessageWindow : UIWindowBase
{
    [SerializeField]
    private Text _content;

    protected override void SetWindowId()
    {
        this.ID = WindowID.StrategyMessageWindow;
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


    private void Update()
    {

    }


    public void ShowValue(string[] values)
    {
      //  _content
    }

}