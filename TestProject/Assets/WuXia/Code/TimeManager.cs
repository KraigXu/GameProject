using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


/// <summary>
/// 时间管理  ，包含事件，
/// </summary>
public class TimeManager : MonoBehaviour
{
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

    public Text text1;
    public Text text2;
    public Text text3;
    public Text text4;
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

	void Start () {
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

	    text1.text = curYera.ToString();
	    text2.text = curMonth.ToString();
	    text3.text = curDay.ToString();

        if (curMonth == 2 || curMonth == 3 || curMonth == 4)
        {
            text4.text = "春";
        }
        else if (curMonth == 5 || curMonth == 6 || curMonth == 7)
        {
            text4.text = "夏";
        }
        else if (curMonth == 8 || curMonth == 9 || curMonth == 10)
        {
            text4.text = "秋";
        }
        else if (curMonth == 11 || curMonth == 12 || curMonth == 1)
        {
            text4.text = "冬";
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
          //春夏秋冬
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
