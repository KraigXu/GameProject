using System.Collections;
using System.Collections.Generic;
using GameSystem.Ui;
using Manager;
using UnityEngine;

public class StrategyScene : MonoBehaviour
{

    private static StrategyScene _instance;

    public static StrategyScene Instance
    {
        get { return _instance; }
    }

    void Awake()
    {
        _instance = this;
    }

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// 生成开局UI
    /// </summary>
    public void InitStartUi()
    {
        UICenterMasterManager.Instance.ShowWindow(WindowID.WorldTimeWindow);

        UICenterMasterManager.Instance.ShowWindow(WindowID.MenuWindow);

        UICenterMasterManager.Instance.ShowWindow(WindowID.PlayerInfoWindow);

        UICenterMasterManager.Instance.ShowWindow(WindowID.MessageWindow);

        UICenterMasterManager.Instance.ShowWindow(WindowID.MapWindow);

        UICenterMasterManager.Instance.ShowWindow(WindowID.LivingAreaTitleWindow);


    }

    /// <summary>
    /// 删除开局UI
    /// </summary>
    public void RemoveStartUi()
    {

    }
}
