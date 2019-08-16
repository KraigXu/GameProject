using System;
using System.Collections;
using System.Collections.Generic;
using GameSystem;
using GameSystem.Ui;
using Unity.Entities;
using UnityEngine;

public class WorldTimeSystem : ComponentSystem
{
    struct Data
    {
        public readonly int Length;
        public EntityArray Entity;
        public ComponentDataArray<Timer> Timers;
        
    }
    [Inject]
    private Data _data;
    private EntityManager _entityManager;

    public DateTime CurTime = Convert.ToDateTime("1000-01-01 00:00:00");
    public int Shichen = 1;
    [SerializeField]
    private byte _timeScalar = 10;           //时间的放大比 如果是0 则是暂停  //10  //50  //100
    private byte _timeAdd = 1;
    public byte ScheduleCell = 5;         //时间节点的大小
    public float Schedule = 0;             //一个时间节点的进度

    public List<WorldTimerNode> WorldTimerNodes = new List<WorldTimerNode>();

    private List<int> _removalPending = new List<int>();
    private int _idCounter;

    public int AddTime = 0;
    public float delay = 0;

    TimeSatus CurTimeSatus;
    public DateTime Times;

    private WorldTimeWindow _worldTimeWindow;

    public void SetupValue(bool isShowUi)
    {

        CurTime = Convert.ToDateTime("1000-01-01 00:00:00");
        Shichen = UpdateGuDaiTime(CurTime.Hour);
        TargetTimes = CurTime.AddDays(30);
        CurTimeSatus = TimeSatus.Play;
        if (isShowUi)
        {
            _worldTimeWindow = (WorldTimeWindow)UICenterMasterManager.Instance.ShowWindow(WindowID.WorldTimeWindow);
            _worldTimeWindow.AddFloat(CurTime.Year, CurTime.Month, CurTime.Hour, CurTime.Day / (float)DateTime.DaysInMonth(CurTime.Year,CurTime.Month));
        }
       
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

    /// <summary>
    /// 创建新的时间序列
    /// </summary>
    /// <param name="startTime"></param>
    /// <param name="startAction"></param>
    /// <param name="endTime"></param>
    /// <param name="endAction"></param>
    /// <returns></returns>
    public int AddTimerNode(DateTime startTime, Action startAction, DateTime endTime, Action endAction)
    {
        WorldTimerNode node = new WorldTimerNode(++_idCounter, startTime, startAction, endTime, endAction);
        WorldTimerNodes.Add(node);
        return node.Id;
    }


    /// <summary>
    /// 删除时间序列
    /// </summary>
    /// <param name="timerId">时间ID</param>
    public void RemoveTimer(int timerId)
    {
        _removalPending.Add(timerId);
    }

    /// <summary>
    /// 定时器删除队列处理程序
    /// </summary>
    private void Remove()
    {
        if (_removalPending.Count > 0)
        {
            foreach (int id in _removalPending)
            {
                for (int i = 0; i < WorldTimerNodes.Count; i++)
                {
                    if (WorldTimerNodes[i].Id == id)
                    {
                        WorldTimerNodes.RemoveAt(i);
                        break;
                    }
                }
            }
            _removalPending.Clear();
        }
    }


    public void AddTimeRange(int time, int id)
    {
        Debug.Log(">>增加时间");



    }

    public DateTime TargetTimes;
    public float Timeer;

    public List<TimeEvent> Events;

    public class TimeEvent
    {
        public DateTime Time;

        public Action Action;
    }

    public void NextTime(int day)
    {
        TargetTimes = CurTime.AddDays(day);

    }


    public void CheckEvent()
    {
        for (int i = 0; i < Events.Count; i++)
        {
            if (Events[i].Time == CurTime)
            {
                Events[i].Action();
            }
            else
            {

            }
        }
    }

    protected override void OnUpdate()
    {
        bool isAdd = false;

        if (CurTime != TargetTimes)
        {
            if (Timeer > 5)
            {
                Timeer = 0;
                CurTime = CurTime.AddDays(1);
              //  CheckEvent();
                isAdd = true;
            }
            else
            {
                Timeer += Time.deltaTime;
            }

            for (int i = 0; i < _data.Length; i++)
            {
                var timer = _data.Timers[i];

                if (isAdd == true)
                {
                    timer.TimeAdd += 1;
                }

                if (timer.TimeType == 1)
                {
                }
                else if (timer.TimeType == 2)
                {
                }
                _data.Timers[i] = timer;

            }
            

            if (_worldTimeWindow)
            {
                _worldTimeWindow.AddFloat(CurTime.Year,CurTime.Month,CurTime.Hour, CurTime.Day/(float)DateTime.DaysInMonth(CurTime.Year, CurTime.Month));
              //  _worldTimeWindow.ad
            }
           

           // _worldTimeWindow.Year.text=Ye

            //_cd += Time.deltaTime;
            //if (_cd > 1)
            //{
            //    Year.text = WorldTime.Year.ToString();
            //    Month.text = WorldTime.Month.ToString();
            //    Day.text = WorldTime.Day.ToString();
            //    ShiChen.text = TimeShichen[WorldTime.ShiChen];
            //    //Season.text = _timeManager.Season;
            //    _cd = 0;
            //}
        }
        else
        {
            //处于时间线停留状态


        }


        
    }


}
