using System.Collections;
using GameSystem;
using GameSystem.Ui;
using Unity.Entities;
using UnityEngine;

public enum StrategySceneModel
{
    Map,
    LivingArea,
    Fighting
}

/// <summary>
/// StrategyScene主脚本，控制整个场景的的生命周期
/// </summary>
public class StrategyScene : MonoBehaviour
{
    private static StrategyScene _instance;
    public static StrategyScene Instance
    {
        get { return _instance; }
    }

    public IEnumeratorLoad IeEnumeratorLoad;
    public StrategySceneModel SceneModel = StrategySceneModel.Map;

    void Awake()
    {
        _instance = this;

        SignalCenter.GameDataLoadOver.AddListener(UiInit);
    }

    void Start()
    {
        gameObject.GetComponent<DataLoadEcs>().StartLoad();
    }

    void OnDestroy()
    {
        SignalCenter.GameDataLoadOver.RemoveListener(UiInit);
    }

    private void UiInit(DataLoadEcs ecs)
    {
        UICenterMasterManager.Instance.ShowWindow(WindowID.TipsWindow);
        UICenterMasterManager.Instance.ShowWindow(WindowID.MenuWindow);
    }

   
    /// <summary>
    /// 退出地图模式
    /// </summary>
    public void ExitMapModel()
    {
        UICenterMasterManager.Instance.CloseWindow(WindowID.MessageWindow);
    }


    /// <summary>
    /// 进入地图模式
    /// </summary>
    public void EnterMapModel()
    {
        UICenterMasterManager.Instance.ShowWindow(WindowID.MessageWindow);

        //  UICenterMasterManager.Instance.ShowWindow(WindowID.PlayerInfoWindow);

        //Instance.MainCamera.enabled = true;

        //  PlayerInfoView.Isflag = true;
    }



    /// <summary>
    /// 切换模式
    /// </summary>
    public void ChangeModel(StrategySceneModel model)
    {
        if (SceneModel == model)
        {
            return;
        }

        switch (SceneModel)
        {
            case StrategySceneModel.Map:
                // UICenterMasterManager.Instance.CloseWindow(WindowID.CityTitleWindow);
                // UICenterMasterManager.Instance.Cl
                break;
            case StrategySceneModel.Fighting:
                break;
            case StrategySceneModel.LivingArea:
                break;
        }

        switch (model)
        {
            case StrategySceneModel.Fighting:
                break;
            case StrategySceneModel.LivingArea:
                break;
            case StrategySceneModel.Map:
                break;
        }

    }



    public void EnterBuildModel()
    {
    }

    public void ExitBuildModel()
    {
    }

}
