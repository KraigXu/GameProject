﻿using System;
using System.Collections.Generic;
using UnityEngine;

namespace DataAccessObject
{

    //-------------------------------------------------建表语句--------------------------------Start
    public static class Tables
    {
        /// <summary>
        /// 生活区表
        /// </summary>
        /// <param name="service"></param>
        /// <param name="ifNotExists"></param>
        public static void CreateTable_LivingArea(SQLService service, bool ifNotExists)
        {
            string constraint = ifNotExists ? "IF NOT EXISTS " : "";
            service.connection.Execute("CREATE TABLE " +
                                       constraint +
                                       " LivingAreaModel (" +
                                       " Id INTEGER PRIMARY KEY," +
                                       " Name TEXT," +
                                       " Description TEXT," +
                                       " RegionId INTEGER," +
                                       " PersonNumber INTEGER," +
                                       " LivingAreaLevel INTEGER," +
                                       " LivingAreaType INTEGER," +
                                       " PowerId INTEGER," +
                                       " ThaneId INTEGER," +
                                       " DefenseStrength INTEGER," +
                                       " LivingAreaMoney INTEGER," +
                                       " FoodValue INTEGER," +
                                       " FoodMax INTEGER," +
                                       " MaterialsValue INTEGER," +
                                       " MaterialsMax INTEGER," +
                                       " StableValue INTEGER," +
                                       " BuildingInfoJson TEXT);");
        }

        /// <summary>
        /// 地域类型
        /// </summary>
        /// <param name="service"></param>
        /// <param name="ifNotExists"></param>
        public static void CreateTable_AreaType(SQLService service, bool ifNotExists)
        {
            string constraint = ifNotExists ? "IF NOT EXISTS " : "";
            service.connection.Execute("CREATE TABLE " +
                                       constraint +
                                       " AreaTypeModel (" +
                                       " Id INTEGER PRIMARY KEY," +
                                       " Name TEXT," +
                                       " Description TEXT);");
        }
        /// <summary>
        /// 势力信息
        /// </summary>
        /// <param name="service"></param>
        /// <param name="ifNotExists"></param>
        public static void CreateTable_Power(SQLService service, bool ifNotExists)
        {
            string constraint = ifNotExists ? "IF NOT EXISTS " : "";
            service.connection.Execute("CREATE TABLE " +
                                       constraint +
                                       " PowerModel (" +
                                       " Id INTEGER PRIMARY KEY," +
                                       " Name TEXT," +
                                       " Description TEXT);");
        }
        /// <summary>
        /// 设施功能表
        /// </summary>
        /// <param name="service"></param>
        /// <param name="ifNotExists"></param>
        public static void CreateTable_BuildingFeatures(SQLService service, bool ifNotExists)
        {
            string constraint = ifNotExists ? "IF NOT EXISTS " : "";
            service.connection.Execute("CREATE TABLE " +
                                       constraint +
                                       " BuildingFeatureModel (" +
                                       " Id INTEGER PRIMARY KEY," +
                                       " Name TEXT," +
                                       " Description TEXT," +
                                       " FeaturesEventId INTEGER,"+
                                       " Remarks TEXT);");

        }
        /// <summary>
        /// 时间事件
        /// </summary>
        /// <param name="service"></param>
        /// <param name="ifNotExists"></param>
        public static void CreateTable_TimeEvent(SQLService service, bool ifNotExists)
        {
            string constraint = ifNotExists ? "IF NOT EXISTS " : "";
            service.connection.Execute("CREATE TABLE " +
                                       constraint +
                                       " TimeEventModel (" +
                                       " Id INTEGER PRIMARY KEY," +
                                       " Name TEXT," +
                                       " Description TEXT);");
        }

        /// <summary>
        /// 人物
        /// </summary>
        /// <param name="service"></param>
        /// <param name="ifNotExists"></param>
        public static void CreateTable_Character(SQLService service, bool ifNotExists)
        {
            string constraint = ifNotExists ? "IF NOT EXISTS " : "";
            service.connection.Execute("CREATE TABLE " +
                                       constraint +
                                       " CharacterModel (" +
                                       " Id INTEGER PRIMARY KEY," +
                                       " Name TEXT," +
                                       " Description TEXT, " +
                                       " Gender  INTEGER," +
                                       " Age INTEGER ," +
                                       " AgeMax INTEGER," +
                                       " Description TEXT);");
        }

        /// <summary>
        /// 功法，技法
        /// </summary>
        /// <param name="service"></param>
        /// <param name="ifNotExists"></param>
        public static void CreateTable_Techniques(SQLService service, bool ifNotExists)
        {
            string constraint = ifNotExists ? "IF NOT EXISTS " : "";
            service.connection.Execute("CREATE TABLE " +
                                       constraint +
                                       " TechniquesModel (" +
                                       " Id INTEGER PRIMARY KEY," +
                                       " Name TEXT," +
                                       " Description TEXT, " +
                                       " MoneyValue INTEGER," +
                                       " Level INTEGER," +
                                       " Type INTEGER," +
                                       " ContentJson TEXT);");
        }

        /// <summary>
        /// 书籍
        /// </summary>
        /// <param name="service"></param>
        /// <param name="ifNotExists"></param>
        public static void CreateTable_Books(SQLService service, bool ifNotExists)
        {
            string constraint = ifNotExists ? "IF NOT EXISTS " : "";
            service.connection.Execute("CREATE TABLE " +
                                       constraint +
                                       " BookModel (" +
                                       " Id INTEGER PRIMARY KEY," +
                                       " Name TEXT," +
                                       " Description TEXT, " +
                                       " Type INTEGER," +
                                       " Level INTEGER," +
                                       " ContentJson TEXT);");
        }

        /// <summary>
        /// 任务
        /// </summary>
        /// <param name="service"></param>
        /// <param name="ifNotExists"></param>
        public static void CreateTable_Task(SQLService service, bool ifNotExists)
        {
            string constraint = ifNotExists ? "IF NOT EXISTS " : "";
            service.connection.Execute("CREATE TABLE " +
                                       constraint +
                                       " TaskModel (" +
                                       " Id INTEGER PRIMARY KEY," +
                                       " Name TEXT," +
                                       " Description TEXT, " +
                                       " Type INTEGER," +
                                       " Level INTEGER," +
                                       " ContentJson TEXT);");

        }


        public static void CreateTable_Prestige(SQLService service)
        {
            service.connection.Execute(" CREATE TABLE IF NOT EXISTS PrestigeModel ( " +
                                       " Id INTEGER PRIMARY KEY," +
                                       " Name TEXT," +
                                       " Description TEXT, " +
                                       " Type INTEGER," +
                                       " Level INTEGER," +
                                       " ContentJson TEXT);");

        }


    }

    //-----------------------------------------建表语句 -----------------------------------End




    //----------------------------------------映射数据库----------------------------------Start

    public abstract class BaseModel
    {
        public abstract object[] GetValues();
    }

    public class LivingAreaModel : BaseModel
    {
        public int Id { get; set; }                      //编号 
        public string Name { get; set; }                //名称
        public string Description { get; set; }         //说明
        public int RegionId { get; set; }               //所属地域ID;
        public int PersonNumber { get; set; }           //人口数量
        public int LivingAreaLevel { get; set; }        //生活区等级
        public int LivingAreaType { get; set; }        //生活区类型
        public int PowerId { get; set; }                  //势力ID
        public int ThaneId { get; set; }                 //领主ID
        public int DefenseStrength { get; set; }        //防守强度
        public int LivingAreaMoney { get; set; }             //生活区金钱
        public int FoodValue { get; set; }                 //粮食
        public int FoodMax { get; set; }                    //粮食上限
        public int MaterialsValue { get; set; }             //资材
        public int MaterialsMax { get; set; }              //资材上限
        public int StableValue { get; set; }                  //安定值
        public string BuildingInfoJson { get; set; }        // 建筑Json

        public override object[] GetValues()
        {
            object[] objects = new object[]
                {
                    Id, Name, Description, RegionId,PersonNumber,LivingAreaLevel,LivingAreaType,PowerId,ThaneId,DefenseStrength, LivingAreaMoney, FoodValue, FoodMax, MaterialsValue, StableValue,BuildingInfoJson

                };
            return objects;
        }
    }

    public class AreaTypeModel : BaseModel
    {
        public int Id { get; set; }                      //编号 
        public string Name { get; set; }                //名称
        public string Description { get; set; }         //说明
        public override object[] GetValues()
        {
            object[] objects = new object[]
            {
                Id, Name, Description
            };
            return objects;
        }
    }

    public class PersonModel
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int PowerId { get; set; }
        public string PracticeJson { get; set; }
        public string TechniqueJson { get; set; }

    }

    public class TaskModel : BaseModel
    {
        public int Id { get; set; }
        public string TaskName { get; set; }
        public string Caption { get; set; }
        public string TaskConditionsJson { get; set; }
        public string TaskRewardsJson { get; set; }
        public override object[] GetValues()
        {
            object[] objects = new object[]
                {
                    Id,TaskName,Caption,TaskConditionsJson,TaskRewardsJson
                };
            return objects;
        }
    }


    //----------------------------------------映射数据库----------------------------------End

    //-----------------------------------------取值--------------------------------------Start

    public static class SqlData
    {
        public static T GetModelId<T>(int id) where T : BaseModel
        {
            Debug.Log(id + ">>>");
            return SQLService.GetInstance("TD.db").QueryUnique<T>(" Id=?", new object[] { id });
        }
    }


    //-------------------------------------------取值----------------------------------------End



    [Serializable]
    public class LevelData
    {
        public int LevelId;
        public string LevelCode; //关卡唯一标识
        public string LevelName;   //关卡名字
        public string Description;
        public string LevelInformations;
        public int DifficultyLevel;
        public string LevelTurretTypeAuthority;

        public string[] TurretTypeIds;

        //关卡所需数据
        //public List<TurretDao.TurretModel> UseTurret;  //可用炮塔
        // public FireMapDao.FireMapModel Map;

        //  public Task task;              //功能暂定
        //  public List<SkillDao.SkillModel> UseModels;//当前技术
        //public List<EnemyDao.EnemyModel> UseEnemyModels;  
        public LevelData() { }
        //public LevelData(LevelDao.LevelModel model)
        //{
        //    this.LevelId = model.Id;
        //    this.LevelCode = model.LevelCode;
        //    this.LevelName = model.LevelName;
        //    this.Description = model.Description;
        //    this.DifficultyLevel = model.DifficultyLevel;
        //    this.LevelInformations = model.LevelInformations;
        //    this.DifficultyLevel = model.DifficultyLevel;
        //    this.LevelTurretTypeAuthority = model.LevelTurretTypeAuthority;
        //}
    }



    //---------------------------------------------------------------------------->

    //----------------------------------------标准对象

    ///// <summary>
    ///// 所有对象的父级
    ///// </summary>
    //public abstract class ElementBase
    //{

    //    public abstract void ElementCalculation();

    //} 

    //-------------------------------end
}