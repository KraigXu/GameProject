using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 时间管理  ，包含事件，
/// </summary>
public class TimeManager : MonoBehaviour
{

    private static TimeManager _instance;
    public static TimeManager Instance { get { return _instance; } }

    public DateTime CurTime;
    public int YearTime=194;            //年
    public int Month=1;                   //月
    public int DayTime=1;              //日

    public int curYera = 194;
    public int curMonth=1;
    public int curDay=1;

    public TimeSatus curStatus;
    public TimeSpeed curSpeed;

    public float TimeScalar=3;         //时间标量，以秒为单位
    public float curSchedule=0;        //当前进度，当进度>=时间标量时则计算为一天  ，时间速率分别为  {正常=时间标量*1,  慢速=时间标量*2,快速=时间标量*0.5}
    public float curSpeedValue=1;

    public bool IsResetTime = true;

    public StringBuilder curYerastr ;
    public StringBuilder curMonthstr;
    public StringBuilder curDaystr  ;
    public StringBuilder curHourstr;
    ///// <summary>
    ///// 时间实体
    ///// </summary>
    //public class TimeEntity
    //{
    //    public 

    //}

    public int GetYear
    {
        get { return CurTime.Year; }
    }

    public int GetMonth
    {
        get { return CurTime.Month; }
    }
    public int GetDay
    {
        get { return CurTime.Day; }
    }

    public int GetHour
    {
        get { return CurTime.Hour; }
    }

    //public string GetShiChen
    //{
    //    get
    //    {
    //        return;


    //        子时 丑时  寅时 卯时  辰时 巳时
    //        23:00 - 00:59 01:00 - 02:59 03:00 - 04:59 05:00 - 06:59 07:00 - 08:59 09:00 - 10:59
    //        午时 未时  申时 酉时  戊时 亥时
    //        11:00 - 12:59 13:00 - 14:59 15:00 - 16:59 17:00 - 18:59 19:00 - 20:59 21:00 - 22:59

    //    }
    //}


    public enum TimeSpeed
    {
        Normal=1,  //正常
        Slow=2,   //慢速
        Extreme=3  //快速
    }

    public enum TimeSatus
    {
        Play,
        Stop,
    }

    void Awake()
    {
        _instance = this;
    }

    void Start()
    {
        if (IsResetTime)  //重置为当前系统时间 转化为古代时间
        {
            CurTime =DateTime.Now;
            curYerastr=new StringBuilder(CurTime.Day+'年');
            curMonthstr=new StringBuilder(CurTime.Month+'月');
            curDaystr = new StringBuilder(CurTime.Day + '日');

            //curYerastr =
            //    curMonthstr
            //curDaystr =
            //    curHourstr

        }
        else            //取得剧本时间
        {

        }

    }

    public void InitTime()
    {
        
    }

	void Update () {
	    if (curStatus == TimeSatus.Play)
	    {
	        curSchedule += Time.deltaTime;
            if (curSchedule >= (TimeScalar * curSpeedValue))
	        {
                TimeAddDay();
	            curSchedule = 0;
                
	        }
	    }
	    else if(curStatus==TimeSatus.Stop)
	    {
	    }
    }

    void TimeAddDay()
    {
        if (curDay + 1 > 30)
        {
            curDay = 1;
            TimeAddMonth();
        }
        else
        {
            curDay++;
        }
    }

    void TimeAddMonth()
    {  

        if (curMonth + 1 > 12)
        {
            curMonth = 1;     
            TimeAddYear();
        }
        else
        {
            curMonth++;
        }

       
    }

    void TimeAddYear()
    {
        curYera++;
    }

    //===============时间履历

    //===============时间事件

    /// <summary>
    /// 新增时间
    /// </summary>
    public void AddTimeEvent()
    {
        
        

    }
    /// <summary>
    /// 
    /// </summary>
    /// <param name="startYear"></param>
    /// <param name="startMonth"></param>
    /// <param name="startDay"></param>
    /// <param name="endYear"></param>
    /// <param name="endMonth"></param>
    /// <param name="endDay"></param>
    /// <param name="target"></param>
    public void AddTimeEvent(int startYear,int startMonth,int startDay,int endYear,int endMonth,int endDay, params object[] target)
    {
        


    }


    public void AddTimeEvent(int startYear, int startMonth, int startDay, int endYear, int endMonth, int endDay,int eventId)
    {



    }

    /// <summary>
    /// 删除时间
    /// </summary>
    public void RemoveTimeEvent()
    {

    }


}
