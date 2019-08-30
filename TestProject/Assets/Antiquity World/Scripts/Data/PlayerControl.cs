using System.Collections;
using System.Collections.Generic;
using GameSystem;
using GameSystem.Ui;
using Unity.Entities;
using UnityEngine;

/// <summary>
/// 玩家控制器
/// </summary>
public class PlayerControl : MonoBehaviour
{
    public bool IsEdit = false;


    public PlayerInfoWindow PlayerInfoWin;

    private EntityManager _entityManager;
    void Start()
    {
        
        _entityManager = World.Active.GetOrCreateManager<EntityManager>();
        if (StrategyPlayer.PlayerId != 0)
        {
            PlayerInfoWin = UICenterMasterManager.Instance.ShowWindow(WindowID.PlayerInfoWindow) as PlayerInfoWindow; ;
        }

    }


    void Update()
    {
        if (Input.GetKeyUp(KeyCode.K))
        {

            _entityManager.AddComponentData(StrategyPlayer.Entity,new Timer()
            {
                //StartTime = SystemManager.Get<WorldTimeSystem>().CurTime,
                //OverTime = SystemManager.Get<WorldTimeSystem>().GetDayExpend(3),
                DayEnd = 0,
                ExpendDay =3,
            });

        }

        
    }

    //------------------------------------PlayerEvent

    /// <summary>
    /// 打开城市窗口
    /// </summary>
    public void OpenCityWindow()
    {
        if(StrategyPlayer.Unit.Location==null)
            return;

        CitySystem.ShowCityWindow(StrategyPlayer.Unit.Location.Entity, StrategyPlayer.Entity);
    }
}


