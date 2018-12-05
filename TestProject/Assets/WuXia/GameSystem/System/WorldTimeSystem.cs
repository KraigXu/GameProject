using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityStandardAssets.Characters.ThirdPerson;
using GameSystem.Ui;

namespace GameSystem
{

    public enum TimeSatus
    {
        Play,
        Stop,
    }
    /// <summary>
    /// 时间管理  ，包含事件，
    /// </summary>
    public class WorldTimeSystem : ComponentSystem
    {
        public DateTime CurTime;
        public static DateTime CurWorldTime;

        struct TimeDataGroup
        {
            public readonly int Length;
            public ComponentDataArray<TimeData> Data;
            
        }


        struct PlayerData
        {
            public readonly int Length;
            public ComponentDataArray<PlayerInput> Input;
            public ComponentArray<AICharacterControl> Aicontrol;
        }

        [Inject]
        private PlayerData _playerData;
        [Inject]
        private TimeDataGroup _timeData;

        private StrategyWindow _strategyWindow;

        protected override void OnUpdate()
        {

            if (_strategyWindow == null)
            {
                ShowWindowData data = new ShowWindowData();
                data.contextData = new StrategyWindowInData(1, 1);
                _strategyWindow = UICenterMasterManager.Instance.ShowWindow(WindowID.StrategyWindow, data).GetComponent<StrategyWindow>();
                return;
            }

            CurTime = StrategySceneInit.Settings.curTime;

            float dt= Time.deltaTime;
            for (int i = 0; i < _timeData.Length; i++)
            {
                TimeData data = _timeData.Data[i];

                data.Schedule += dt * data.TimeScalar;
                if (data.Schedule >= data.ScheduleCell)
                {
                    data.Schedule = 0;
                    CurTime = CurTime.AddHours(1);
                    data.Year = CurTime.Year;
                    data.Month = CurTime.Month;
                    data.Day = CurTime.Day;
                    data.Hour = CurTime.Hour;
                    data.Shichen = UpdateGuDaiTime(data.Hour);
                    data.Jijie = UpdateTimeJijie(data.Month);

                    _strategyWindow.UpdateTime(data);
                }
                _timeData.Data[i] = data;


            }
            StrategySceneInit.Settings.curTime = CurTime;
        }

        private int UpdateGuDaiTime(int curHour)
        {
            if (curHour == 23 || curHour == 0)
                return 1;
            else if (curHour == 1 || curHour == 2)
                return 2;
            else if (curHour == 3 || curHour == 4)
                return 3;
            else if (curHour == 5 || curHour == 6)
                return 4;
            else if (curHour == 7 || curHour == 8)
                return 5;
            else if (curHour == 9 || curHour == 10)
                return 6;
            else if (curHour == 11 || curHour == 12)
                return 7;
            else if (curHour == 13 || curHour == 14)
                return 8;
            else if (curHour == 15 || curHour == 16)
                return 9;
            else if (curHour == 17 || curHour == 18)
                return 10;
            else if (curHour == 19 || curHour == 20)
                return 11;
            else if (curHour == 21 || curHour == 22)
                return 12;
            else
                return 1;
        }

        private int UpdateTimeJijie(int curMonth)
        {
            if (curMonth == 2 || curMonth == 3 || curMonth == 4)
            {
                return 1;
            }
            else if (curMonth == 5 || curMonth == 6 || curMonth == 7)
            {
                return 2;
            }
            else if (curMonth == 8 || curMonth == 9 || curMonth == 10)
            {
                return 3;
            }
            else if (curMonth == 11 || curMonth == 12 || curMonth == 1)
            {
                return 4;
            }
            else
            {
                return 1;
            }
        }


        public void SetTimeScalar(byte timescalar)
        {
            for (int i = 0; i < _timeData.Length; i++)
            {
                TimeData data = _timeData.Data[i];

                data.TimeScalar = timescalar;
                _timeData.Data[i] = data;
            }
        }
    }


}
