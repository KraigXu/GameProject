using System;
using System.Collections.Generic;
using GameSystem;
using UnityEngine;

/// <summary>
/// 时间管理
/// </summary>
public class WorldTime : MonoBehaviour
{
    private static WorldTime _instance;

    public static WorldTime Instance
    {
        get { return _instance; }
    }

    public DateTime CurTime = Convert.ToDateTime("1000-01-01 00:00:00");
    public int Shichen = 1;
    [SerializeField]
    private byte _timeScalar = 10;           //时间的放大比 如果是0 则是暂停  //10  //50  //100
    private byte _timeAdd=1;
    public byte ScheduleCell = 5;         //时间节点的大小
    public float Schedule = 0;             //一个时间节点的进度

    public List<WorldTimerNode> WorldTimerNodes = new List<WorldTimerNode>();

    private List<int> _removalPending = new List<int>();
    private int _idCounter;

    public int AddTime = 0;
    public float delay = 0;

    TimeSatus CurTimeSatus;
    public DateTime Times;


    public static int Year
    {
        get { return Instance.CurTime.Year; }
    }

    public static int Month
    {
        get { return Instance.CurTime.Month; }
    }

    public static int Day
    {
        get { return Instance.CurTime.Day; }
    }

    public static int ShiChen
    {
        get { return Instance.UpdateGuDaiTime(Instance.CurTime.Hour); }
    }

    void Awake()
    {
        _instance = this;

        CurTime = Convert.ToDateTime("1000-01-01 00:00:00");
        Shichen = UpdateGuDaiTime(CurTime.Hour);

        CurTimeSatus = TimeSatus.Play;

    }

  
    void Update()
    {

        if (CurTimeSatus == TimeSatus.Play)
        {
            Schedule += Time.deltaTime * _timeScalar;
            if (Schedule >= ScheduleCell)   //表示一个时间节点完成
            {
                Schedule = 0;
                CurTime = CurTime.AddHours(2);
            }
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


    public static void AddDay(int number)
    {
        Instance.AddTime += number;
    }

    public static float RunTime
    {
        get
        {
            if (Instance.CurTimeSatus == TimeSatus.Play)
            {
                return Time.deltaTime* Instance._timeAdd;
            }
            return 0;
        }
    }

    public static void DefaultSpeed()
    {
        Instance._timeAdd = 1;
        Instance._timeScalar = 10;
    }

    public static void SpeedMultiplier2()
    {
        Instance._timeAdd = 2;
        Instance._timeScalar = 20;
    }

    public static void SpeedMultiplier3()
    {
        Instance._timeAdd = 3;
        Instance._timeScalar = 30;
    }
}
