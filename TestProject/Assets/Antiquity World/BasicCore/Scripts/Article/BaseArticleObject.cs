using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum ENUM_OBJECT_TYPE    //对象类型
{
    OBJECT_UNKNOW = 0x00,          //未知
    OBJECT_ITEM = 0x01,          //道具
    OBJECT_MAIL = 0x02,          //邮件  
};

public enum ENUM_OBJECT_STATE   //对象状态
{
    OBJECT_UNKNOW_STATE = 0x00,            //未知
    OBJECT_DISPLAY_STATE = 0x01,            //可视
    OBJECT_INVALID_STATE = 0x02,            //无效
};


public class BaseArticleObject
{
    private UInt64 m_u8GUID;                    //唯一编号
    private ENUM_OBJECT_TYPE m_emObjectType;   //对象类型
    private ENUM_OBJECT_STATE m_emObjectState; //对象状态
    private UInt16 m_u2Count;                   //当前个数
    private UInt16 m_u2MaxCount;                //最大个数

    public BaseArticleObject()
    {
        m_u8GUID = 0;
        m_emObjectType = ENUM_OBJECT_TYPE.OBJECT_UNKNOW;
        m_emObjectState = ENUM_OBJECT_STATE.OBJECT_UNKNOW_STATE;
        m_u2Count = 1;
        m_u2MaxCount = 1;
    }

    public BaseArticleObject(ENUM_OBJECT_TYPE emObjectType, ENUM_OBJECT_STATE emObjectState)
    {
        m_emObjectType = emObjectType;
        m_emObjectState = emObjectState;
    }



    /// <summary>
    /// @Description: 设置对象的全局ID
    /// </summary>
    /// <param name="u8GUID"></param>
    public void SetGUID(UInt64 u8GUID)
    {
        m_u8GUID = u8GUID;
    }

    /// <summary>
    /// @Description: 获得对象的全局ID
    /// </summary>
    /// <returns></returns>
    public UInt64 GetGUID()
    {
        return m_u8GUID;
    }


    /// <summary>
    /// @Description: 设置对象类型
    /// </summary>
    /// <returns></returns>
    public void SetObjectType(ENUM_OBJECT_TYPE emObjectType)
    {
        m_emObjectType = emObjectType;
    }


    /// <summary>
    /// @Description: 得到对象类型
    /// </summary>
    /// <returns></returns>
    public ENUM_OBJECT_TYPE GetObjectType()
    {
        return m_emObjectType;
    }


    /// <summary>
    /// @Description: 设置对象状态
    /// </summary>
    /// <returns></returns>
    public void SetObjectState(ENUM_OBJECT_STATE emObjectState)
    {
        m_emObjectState = emObjectState;
    }

    /// <summary>
    /// @Description: 设置对象叠加个数
    /// </summary>
    /// <returns></returns>
    public void SetCount(UInt16 u2Count)
    {
        m_u2Count = u2Count;
    }


    /// <summary>
    ///  @Description: 得到对象叠加个数
    /// </summary>
    /// <returns></returns>
    public UInt16 GetCount()
    {
        return m_u2Count;
    }



    /// <summary>
    /// @Description: 设置最大个数
    /// </summary>
    /// <returns></returns>
    public void SetMaxCount(UInt16 u2MaxCount)
    {
        m_u2MaxCount = u2MaxCount;
    }


    /// <summary>
    /// @Description: 获得最大个数
    /// </summary>
    /// <returns></returns>
    public UInt16 GetMaxCount()
    {
        return m_u2MaxCount;
    }



    public bool CHECK_SCOPE(int a, int b)
    {
        if (a < b)
        {
            return false;
        }
        return true;
    }
}


