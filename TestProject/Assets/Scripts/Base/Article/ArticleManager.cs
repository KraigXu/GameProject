using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AntiquityWorld.StrategyManager
{
    public class ArticleManager
    {

        public Dictionary<int, AricleItem> MapItemUse;


        //typedef map<uint64, CItem*> mapItemUsed;
        //typedef vector<CItem*> vecItemPool;

        //mapItemUsed m_mapItemUsed;        //记载和GUID的映射关系
        //vecItemPool m_vecItemUnUsed;      //空闲的道具

        //Dictionary<int,> m_pItemDic;   //物品字典，用于翻阅
        //int m_u4MaxCount; //最大生成物品个数（包含目前已经有的）

        ////生成GUID相关参数
        //int m_u4TimeSeed;       //时间种子
        //int m_u4TimeSeedCount;  //当前种子个数

        ////道具监控者
        //CItemMonitor m_objItemMonitor;   //监控道具的类


        ///*
        //*    @Description: 创建一个GUID
        //*/
        //int CreateGUID()
        //{

        //}

        /// <summary>
        /// @Description: 初始化道具管理器
        /// </summary>
        public bool Init(Dictionary<int, AricleItem> itemDic, int maxCount)
        {
            MapItemUse = itemDic;

            


            return true;
        }

        ///*
        //*    
        //*/
        //bool Init(CItemDictionary* pItemDic, int u4ItemMaxCount)
        //{

        //}

        ///*
        //*    @Description: 清除道具管理器
        //*/
        //void Close()
        //{

        //}

        ///*
        //*    @Description: 从道具管理器中获得一个空闲的CItem并返回
        //*/
        //CItem* CreateItem()
        //{

        //}

        ///*
        //*    @Description: 从道具管理器中回收一个道具(这个道具还未生效)
        //*/
        //ENUM_ITEM_MANAGER_ERROR DeleteItemUnRegedit(CItem*& pItem)
        //{

        //}

        ///*
        //*    @Description: 从道具管理器中回收一个道具
        //*/
        //ENUM_ITEM_MANAGER_ERROR DeleteItem(CItem*& pItem)
        //{

        //}

        ///*
        //*    @Description: 从道具管理器中回收一个道具
        //*/
        //ENUM_ITEM_MANAGER_ERROR DeleteItem(uint64 u6GUID)
        //{

        //}

        ///*
        //*    @Description: 建立物品和GUID之间的对应关系
        //*/
        //ENUM_ITEM_MANAGER_ERROR RegeditItem(uint64 u8GUID, CItem* pItem)
        //{

        //}

        ///*
        //*    @Description: 得到空余的道具数量
        //*/
        //uint32 GetFreeItemCount()
        //{

        //}

        /*
        *    @Description: 保存监控日志
        */
        void SaveMonitor()
        {

        }

    }

}