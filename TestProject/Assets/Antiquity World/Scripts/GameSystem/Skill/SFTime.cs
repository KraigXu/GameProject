using System;
using System.Collections.Generic;
using UnityEngine;

public class SFTime : MonoBehaviour
{
    private static SFTime _time;
    public static SFTime Instance {get { return _time; }}

    private List<Timer> _timers;
    private List<int> _removalPending;
    private int _idCounter;

    private class Timer
    {
        public int Id;
        public bool IsActive;
        public float Rate;
        public int Ticks;
        public int TicksElapsed;
        public float Last;
        public Action CallBack;

        public Timer(int id, float rate,int ticks,Action callback)
        {
            this.Id = id;
            this.Rate = rate < 0 ? 0 : rate;
            this.Ticks = ticks < 0 ? 0 : ticks;
            this.CallBack = callback;
            this.Last = 0;
            this.TicksElapsed = 0;
            this.IsActive = true;
        }

        public void Tick()
        {
            Last += UnityEngine.Time.deltaTime;
            if (IsActive && Last >= Rate)
            {
                Last = 0;
                TicksElapsed++;
                CallBack.Invoke();
                if (Ticks > 0 && Ticks == TicksElapsed)
                {
                    IsActive = false;
                    Instance.RemoveTimer(Id);
                }
            }
        }

    }

    /// <summary>
    /// 初始化，单例
    /// </summary>
    void Awake()
    {
        _time = this;
        _timers=new List<Timer>();
        _removalPending=new List<int>();
    }

    /// <summary>
    /// 创建新的时间序列
    /// </summary>
    /// <param name="rate">速率</param>
    /// <param name="callBack">回调</param>
    /// <returns>时间序列ID</returns>
    public int AddTimer(float rate, Action callBack)
    {
        return AddTimer(rate, 0, callBack);
    }

    /// <summary>
    /// 创建新的时间序列
    /// </summary>
    /// <param name="rate">速率</param>
    /// <param name="ticks">次数</param>
    /// <param name="callBack">回调</param>
    /// <returns>时间序列ID</returns>
    public int AddTimer(float rate, int ticks, Action callBack)
    {
        Timer newTimer = new Timer(++_idCounter, rate, ticks, callBack);
        _timers.Add(newTimer);
        return newTimer.Id;
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
                for (int i = 0; i < _timers.Count; i++)
                {
                    if (_timers[i].Id == id)
                    {
                        _timers.RemoveAt(i);
                        break;
                    }
                }
            }
            _removalPending.Clear();
        }
    }

    /// <summary>
    /// 更新时间
    /// </summary>
    void Tick()
    {
        for (int i = 0; i < _timers.Count; i++)
        {
            _timers[i].Tick();
        }
    }
	
	void Update () {
		Remove();
        Tick();
	}
}
