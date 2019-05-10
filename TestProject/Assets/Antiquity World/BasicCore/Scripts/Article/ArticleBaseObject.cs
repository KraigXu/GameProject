using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AntiquityWorld.StrategyManager
{
    public enum ENUM_OBJECT_TYPE    //对象类型
    {
        OBJECT_UNKNOW = 0x00,          //未知
        OBJECT_ITEM = 0x01,          //道具
        OBJECT_MAIL = 0x02,          //邮件  
    };

    public enum ENUM_OBJECT_STATE   //对象状态
    {
        OBJECT_UNKNOW_STATE = 0x00,            //未知
        OBJECT_DISPLAY_STATE = 0x01,            //可视
        OBJECT_INVALID_STATE = 0x02,            //无效
    };
    public enum ENUM_ITEM_ATTRIBUTE : int
    {
        ITEM_ATTRIBUTE_NONE=-1,
        ITEM_ATTRIBUTE_MIN_ATTACK = 0x00,    //最小攻击
        ITEM_ATTRIBUTE_MAX_ATTACK = 0x01,    //最大攻击
        ITEM_ATTRIBUTE_PHYSICS_DEFENCE = 0x02,    //物理防御 
        ITEM_ATTRIBUTE_MAGIC_DEFENCE = 0x03,    //魔法防御 
        ITEM_ATTRIBUTE_LIFE = 0x04,    //生命
        ITEM_ATTRIBUTE_MAGIC = 0x05,    //魔法
        ITEM_ATTRIBUTE_POWER = 0x06,    //精力 
        ITEM_ATTRIBUTE_CRIT = 0x07,    //暴击 
        ITEM_ATTRIBUTE_ATTACKSPEED = 0x08,    //攻速 
        ITEM_ATTRIBUTE_HIT = 0x09,    //命中 
        ITEM_ATTRIBUTE_DODGE = 0x0A,    //躲闪 
        ITEM_ATTRIBUTE_RECOVER_LIFE = 0x1F,    //生命回复 
        ITEM_ATTRIBUTE_RECOVER_MAGIC = 0x0C,    //魔法回复 
        ITEM_ATTRIBUTE_SKILL_ATTACK = 0x0D,    //攻击技能 
        ITEM_ATTRIBUTE_SKILL_P_DEFENCE = 0x0E,    //物防技能
        ITEM_ATTRIBUTE_SKILL_M_DEFENCE = 0x0F,    //魔防技能
        ITEM_ATTRIBUTE_SKILL_CRIT = 0x10,    //暴击技能
        ITEM_ATTRIBUTE_SKILL_HIT = 0x11,    //命中技能  
        ITEM_ATTRIBUTE_SKILL_A_SPEED = 0x12,    //攻速技能
        ITEM_ATTRIBUTE_SKILL_DODGE = 0x13,    //躲闪技能
        ITEM_ATTRIBUTE_POSITION = 0x14,    //位置
        ITEM_ATTRIBUTE_CLASS = 0x15,    //物品类型
        ITEM_ATTRIBUTE_PHOTOID = 0x16,    //图片ID
        ITEM_ATTRIBUTE_BASEID = 0x17,    //物品基础ID
        ITEM_ATTRIBUTE_JEWEL_COUNT = 0x18,    //孔槽个数
        ITEM_ATTRIBUTE_JEWEL_1 = 0x19,    //孔槽1
        ITEM_ATTRIBUTE_JEWEL_2 = 0x1A,    //孔槽2
        ITEM_ATTRIBUTE_JEWEL_3 = 0x1B,    //孔槽3
        ITEM_ATTRIBUTE_JEWEL_4 = 0x1C,    //孔槽4
        ITEM_ATTRIBUTE_JEWEL_5 = 0x1D,    //孔槽5
        ITEM_ATTRIBUTE_VERSION = 0x1E,    //道具版本号
        ITEM_ATTRIBUTE_USE_LEVEL = 0x1F,    //使用等级
        ITEM_ATTRIBUTE_LEVEL = 0x20,    //道具等级
        ITEM_ATTRIBUTE_SYNTHETIC_COUNT = 0x21,    //合成所需道具数量（图纸专属）
        ITEM_ATTRIBUTE_SYN_BASEID_1 = 0x22,    //合成道具1的BaseID（图纸专属）
        ITEM_ATTRIBUTE_SYN_BASEID_2 = 0x23,    //合成道具2的BaseID（图纸专属）
        ITEM_ATTRIBUTE_SYN_BASEID_3 = 0x24,    //合成道具3的BaseID（图纸专属）
        ITEM_ATTRIBUTE_SYN_BASEID_4 = 0x25,    //合成道具4的BaseID（图纸专属）
        ITEM_ATTRIBUTE_SYN_BASEID_5 = 0x26,    //合成道具5的BaseID（图纸专属）
        ITEM_ATTRIBUTE_SYN_BID_1_COUNT = 0x27,    //合成道具1的BID数量（图纸专属）
        ITEM_ATTRIBUTE_SYN_BID_2_COUNT = 0x28,    //合成道具2的BID数量（图纸专属）
        ITEM_ATTRIBUTE_SYN_BID_3_COUNT = 0x29,    //合成道具3的BID数量（图纸专属）
        ITEM_ATTRIBUTE_SYN_BID_4_COUNT = 0x2A,    //合成道具4的BID数量（图纸专属）
        ITEM_ATTRIBUTE_SYN_BID_5_COUNT = 0x2B,    //合成道具5的BID数量（图纸专属）
        ITEM_ATTRIBUTE_SYN_NEWID_COUNT = 0x2C,    //合成新道具BaseID的数量
        ITEM_ATTRIBUTE_SYN_NEWID_1 = 0x2D,    //新BaseID1
        ITEM_ATTRIBUTE_SYN_NEWID_2 = 0x2E,    //新BaseID2
        ITEM_ATTRIBUTE_SYN_NEWID_3 = 0x2F,    //新BaseID3
        ITEM_ATTRIBUTE_SYN_NEWID_C_1 = 0x30,    //新BaseID1数量
        ITEM_ATTRIBUTE_SYN_NEWID_C_2 = 0x31,    //新BaseID2数量
        ITEM_ATTRIBUTE_SYN_NEWID_C_3 = 0x32,    //新BaseID3数量
    };

    public enum ENUM_ITEM_CLASS : int
    {
        ITEM_CLASS_SKILL_BOOK = 0x00,    //技能书
        ITEM_CLASS_BOX = 0x01,    //盒子(容器)
        ITEM_CLASS_EQUIPMENT = 0x02,    //装备
        ITEM_CLASS_RESOURCE = 0x03,    //材料
        ITEM_CLASS_JEWEL = 0x04,    //宝石
        ITEM_CLASS_RUNE = 0x05,    //符文 
        ITEM_CLASS_STONE = 0x06,    //石头
        ITEM_CLASS_TASK = 0x07,    //任务道具
        ITEM_CLASS_DRAW = 0x08,    //图纸
    };

    public class ArticleBaseObject
    {
        private int _guiId; //唯一编号
        private ENUM_OBJECT_TYPE _emObjectType;   //对象类型
        private ENUM_OBJECT_STATE _emObjectState;  //对象状态
        private int _count;    //当前个数
        private int _maxCount;  //最大个数


        public ArticleBaseObject()
        {
            _guiId = 0;
            _emObjectType = ENUM_OBJECT_TYPE.OBJECT_UNKNOW;
            _emObjectState = ENUM_OBJECT_STATE.OBJECT_UNKNOW_STATE;
            _count = 1;
            _maxCount = 1;

        }


        public ArticleBaseObject(ENUM_OBJECT_TYPE emObjectType, ENUM_OBJECT_STATE emObjectState)
        {
            _emObjectType = emObjectType;
            _emObjectState = emObjectState;
        }

        /*
        *    @Description: 设置对象的全局ID
        */
        void SetGUID(int u8GUID)
        {
            _guiId = u8GUID;
        }

        /*
        *    @Description: 获得对象的全局ID
        */
        int GetGUID() { return _guiId; }

        /*
        *    @Description: 设置对象类型
        */
        void SetObjectType(ENUM_OBJECT_TYPE emObjectType)
        {
            _emObjectType = emObjectType;
        }

        /*
        *    @Description: 得到对象类型
        */
        ENUM_OBJECT_TYPE GetObjectType()
        {
            return _emObjectType;
        }

        /*
        *    @Description: 设置对象状态
        */
        void SetObjectState(ENUM_OBJECT_STATE emObjectState)
        {
            _emObjectState = emObjectState;
        }

        /*
        *    @Description: 得到对象状态
        */
        ENUM_OBJECT_STATE GetObjectState()
        {
            return _emObjectState;
        }

        /*
        *    @Description: 设置对象叠加个数
        */
        void SetCount(int u2Count)
        {
            _count = u2Count;
        }

        /*
        *    @Description: 得到对象叠加个数
        */
        int GetCount()
        {
            return _count;
        }

        /*
        *    @Description: 设置最大个数
        */
        void SetMaxCount(int u2MaxCount)
        {
            _maxCount = u2MaxCount;
        }

        /*
        *    @Description: 获得最大个数
        */
        int GetMaxCount()
        {
            return _maxCount;
        }


    }
}
