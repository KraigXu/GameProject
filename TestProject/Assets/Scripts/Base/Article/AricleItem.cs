using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AntiquityWorld.StrategyManager
{

    public class AricleItem : ArticleBaseObject
    {

        private int[] m_ayAttribute = new int[51];
        private char[] m_szItemName = new char[200];
        private char[] m_szItemDesc = new char[1024];


        /*
        *    @Description: 添加相关物品属性
        */
        void AddItemAttribute(ENUM_ITEM_ATTRIBUTE emAttrName, int u2Value)
        {

        }

        /// <summary>
        /// 显示所有该物品属性
        /// </summary>
        public void DisPlay()
        {

        }

        /// <summary>
        /// @Description: 创建物品相关唯一ID，物品名称，物品描述
        /// </summary>
        public void Create(char[] name, int nItemNameSize,char[] pItemDesc, int nItemDesc)
        {

        }

        




        //        /*
        //        *    @Description: 得到物品属性数组
        //        */
        //        int* GetItemAttributeList()
        //        {

        //        }

        //        /*
        //        *    @Description: 设置物品属性数组
        //        */
        //        void SetItemAttributeList(int* ayAttribute)
        //        {

        //        }

        //        //物品使用相关方法
        //        /*
        //        *    @Description: 得到指定的物品属性
        //        */
        //        int GetItemAttribute(ENUM_ITEM_ATTRIBUTE emAttrName)
        //        {

        //        }

        //        /*
        //        *    @Description: 得到物品名称
        //        */
        //        const char* GetItemName()
        //        {

        //        }

        //        /*
        //        *    @Description: 得到物品描述
        //        */
        //        const char* GetItemDesc()
        //        {

        //        }

        //        /*
        //        *    @Description: 清除所有属性
        //        */
        //        void ClearAttribute()
        //        {

        //        }

        //        //物品存取相关方法
        //        /*
        //        *    @Description: 从数据流还原Item对象
        //        */
        //        public bool ReadFromBuffer(char* pBuffer, int& nSize)
        //        {

        //        }

        //*
        //        *    @Description: 序列化入数据流
        //        */
        //        public bool WriteToBuffer(char* pBuffer, int& nSize)
        //        {
        //            return true;

        //        }

    }
}