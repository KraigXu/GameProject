using System;
using System.Collections.Generic;
using UnityEngine;

namespace DataAccessObject
{

    //-------------------------------------------------建表语句--------------------------------Start
    public static class Tables
    {

        /// <summary>
        /// 区
        /// </summary>
        /// <param name="service"></param>
        public static void CreateTable_District(SQLService service)
        {
            service.connection.Execute(" CREATE TABLE IF NOT EXISTS DistrictData ( " +
                                       " Id INTEGER PRIMARY KEY," +
                                       " RaceId INTEGER," +
                                       " Name TEXT," +
                                       " Description TEXT, " +
                                       " Sex INTEGER," +
                                       " Age INTEGER," +
                                       " AgeMax INTEGER," +
                                       " Life INTEGER," +
                                       " LifeMax INTEGER," +
                                       " Prestige INTEGER," +
                                       " TimeAppearance TEXT," +
                                       " TimeEnd TEXT," +
                                       " IsDebut TEXT," +
                                       " Location INTEGER," +
                                       " LocationType INTEGER," +
                                       " ArticleJson TEXT," +
                                       " EquipmentJson TEXT," +
                                       " LanguageJson TEXT);");
        }

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
                                       " LivingAreaData (" +
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
                                       " AreaTypeData (" +
                                       " Id INTEGER PRIMARY KEY," +
                                       " Name TEXT," +
                                       " Description TEXT);");
        }

        /// <summary>
        /// 生物属性
        /// </summary>
        /// <param name="service"></param>
        public static void CreateTable_Biological(SQLService service)
        {
            service.connection.Execute(" CREATE TABLE IF NOT EXISTS BiologicalData ( " +
                                       " Id INTEGER PRIMARY KEY," +
                                       " RaceId INTEGER," +
                                       " Name TEXT," +
                                       " Description TEXT, " +
                                       " Sex INTEGER," +
                                       " Age INTEGER," +
                                       " AgeMax INTEGER," +
                                       " Life INTEGER," +
                                       " LifeMax INTEGER," +
                                       " Prestige INTEGER," +
                                       " TimeAppearance TEXT," +
                                       " TimeEnd TEXT," +
                                       " IsDebut TEXT," +
                                       " Location INTEGER," +
                                       " LocationType INTEGER," +
                                       " ArticleJson TEXT," +
                                       " EquipmentJson TEXT," +
                                       " LanguageJson TEXT);");
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
                                       " PowerData (" +
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
                                       " BuildingFeatureData (" +
                                       " Id INTEGER PRIMARY KEY," +
                                       " Name TEXT," +
                                       " Description TEXT," +
                                       " FeaturesEventId INTEGER," +
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
                                       " TimeEventData (" +
                                       " Id INTEGER PRIMARY KEY," +
                                       " Name TEXT," +
                                       " Description TEXT);");
        }



        /// <summary>
        /// 技法
        /// </summary>
        /// <param name="service"></param>
        /// <param name="ifNotExists"></param>
        public static void CreateTable_Techniques(SQLService service, bool ifNotExists)
        {
            string constraint = ifNotExists ? "IF NOT EXISTS " : "";
            service.connection.Execute("CREATE TABLE " +
                                       constraint +
                                       " TechniquesData (" +
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
                                       " BookData (" +
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
                                       " TaskData (" +
                                       " Id INTEGER PRIMARY KEY," +
                                       " Name TEXT," +
                                       " Description TEXT, " +
                                       " Type INTEGER," +
                                       " Level INTEGER," +
                                       " ContentJson TEXT);");

        }

        /// <summary>
        /// 声望
        /// </summary>
        /// <param name="service"></param>
        public static void CreateTable_Prestige(SQLService service)
        {
            service.connection.Execute(" CREATE TABLE IF NOT EXISTS PrestigeData ( " +
                                       " Id INTEGER PRIMARY KEY," +
                                       " Name TEXT," +
                                       " Description TEXT, " +
                                       " Type INTEGER," +
                                       " Level INTEGER," +
                                       " ContentJson TEXT);");

        }

        /// <summary>
        /// 对话
        /// </summary>
        /// <param name="service"></param>
        public static void CreateTable_Dialog(SQLService service)
        {
            service.connection.Execute(" CREATE TABLE IF NOT EXISTS DialogData ( " +
                                       " Id INTEGER PRIMARY KEY," +
                                       " PlotCoding TEXT," +
                                       " IndexId INTEGER, " +
                                       " StatementType INTEGER, " +
                                       " Content string," +
                                       " ReplyIndex INTEGER);");

        }

        /// <summary>
        /// 小知识
        /// </summary>
        /// <param name="service"></param>
        public static void CreateTable_Tips(SQLService service)
        {
            service.connection.Execute(" CREATE TABLE IF NOT EXISTS TipsData ( " +
                                       " Id INTEGER PRIMARY KEY," +
                                       " TipsType TEXT,"+
                                       " Content TEXT," +
                                       " ContentTitle TEXT);");
        }

        /// <summary>
        /// 功法
        /// </summary>
        /// <param name="service"></param>
        public static void CreateTable_Gongfa(SQLService service)
        {
            service.connection.Execute(" CREATE TABLE IF NOT EXISTS GongfaData ( " +
                                       " Id INTEGER PRIMARY KEY," +
                                       " Name TEXT," +
                                       " Description  TEXT," +
                                       " ContentTitle TEXT);");
        }
        /// <summary>
        /// 心法
        /// </summary>
        /// <param name="service"></param>
        public static void CreateTable_Xinfa(SQLService service)
        {
            service.connection.Execute(" CREATE TABLE IF NOT EXISTS XinfaData ( " +
                                       " Id INTEGER PRIMARY KEY," +
                                       " Name TEXT," +
                                       " Description  TEXT," +
                                       " XinfaType TEXT,"+

                                       " ContentTitle TEXT);");
        }

        /// <summary>
        /// 关系
        /// </summary>
        /// <param name="service"></param>
        public static void CreateTable_Relation(SQLService service)
        {
            service.connection.Execute(" CREATE TABLE IF NOT EXISTS RelationData ( " +
                                       " Id INTEGER PRIMARY KEY," +
                                       " Name TEXT," +
                                       " Description  TEXT," +
                                       " XinfaType TEXT," +

                                       " ContentTitle TEXT);");
        }

    }

    //-----------------------------------------建表语句 -----------------------------------End




    //----------------------------------------映射数据库----------------------------------Start

    public abstract class BaseData
    {
        public abstract object[] GetValues();
    }

    public class LivingAreaData : BaseData
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

    public class AreaTypeData : BaseData
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

    //service.connection.Execute(" CREATE TABLE IF NOT EXISTS BiologicalData ( " +
    //                                       " Id INTEGER PRIMARY KEY," +
    //                                       " RaceId INTEGER," +
    //                                       " Name TEXT," +
    //                                       " Description TEXT, " +
    //                                       " Sex INTEGER," +
    //                                       " Age INTEGER," +
    //                                       " AgeMax INTEGER," +
    //                                       " Prestige INTEGER," +
    //                                       " ArticleJson TEXT," +
    //                                       " EquipmentJson TEXT," +
    //                                       " LanguageJson TEXT);");
    //        }

    //        public static void CreateTable_Race(SQLService service)
    //        {
    //            service.connection.Execute(" CREATE TABLE IF NOT EXISTS RaceData ( " +
    //                                      " Id INTEGER PRIMARY KEY," +
    //                                      " Name TEXT," +
    //                                      " Description TEXT);");
    //        }
    public class BiologicalData : BaseData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int RaceType { get; set; }
        public int Sex { get; set; }
        public int Age { get; set; }

        public int Property1 { get; set; }
        public int Property2 { get; set; }
        public int Property3 { get; set; }
        public int Property4 { get; set; }
        public int Property5 { get; set; }

        public int RaceId { get; set; }
        public int RaceRangeId { get; set; }
        
       
        public int AgeMax { get; set; }
        public int Life { get; set; }
        public int LifeMax { get; set; }
        public int Prestige { get; set; }
        public DateTime TimeAppearance { get; set; }
        public DateTime TimeEnd { get; set; }
        public int IsDebut { get; set; }
        public int Location { get; set; }
        public int LocationType { get; set; }
        public string ArticleJson { get; set; }
        public string EquipmentJson { get; set; }
        public string LanguageJson { get; set; }

        public override object[] GetValues()
        {
            object[] objects = new object[]
                {
                    Id,RaceType,Name,Description,Sex,Age,AgeMax,Life,LifeMax,Prestige,TimeAppearance,TimeEnd,IsDebut,Location,LocationType, ArticleJson,EquipmentJson,LanguageJson
                };
            return objects;
        }
    }

    public class DistrictData : BaseData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int PowerId { get; set; }

        public override object[] GetValues()
        {
            object[] objects = new object[]
            {
                Id, Name, Description, PowerId
            };
            return objects;
        }
    }

    public class DialogData : BaseData
    {
        public int Id { get; set; }
        public string PlotCoding { get; set; }
        public int IndexId { get; set; }
        public int StatementType { get; set; }
        public string Content { get; set; }
        public int ReplyIndex { get; set; }
        


        public override object[] GetValues()
        {
            object[] objects = new object[]
                {
                    Id,PlotCoding,IndexId,StatementType,Content,ReplyIndex
                };
            return objects;
        }
    }


    public class ScenarioData : BaseData
    {
        public int Id { get; set; }

        public override object[] GetValues()
        {
            throw new NotImplementedException();
        }
    }


    public class TaskData : BaseData
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

    public class TipsData : BaseData
    {
        public int Id { get; set; }
        public string TipsType { get; set; }
        public string ContentTitle { get; set; }
        public string Content { get; set; }
        public override object[] GetValues()
        {
            object[] objects = new object[]
            {
                Id,TipsType,ContentTitle,Content
            };
            return objects;
        }
    }


    //----------------------------------------映射数据库----------------------------------End

    //-----------------------------------------取值--------------------------------------Start

    public static class SqlData
    {
        public static T GetDataId<T>(int id) where T : BaseData
        {
            return SQLService.GetInstance("Data.db").QueryUnique<T>(" Id=?", new object[] { id });
        }

        public static List<T> GetAllDatas<T>() where T : BaseData
        {
            return SQLService.GetInstance("Data.db").QueryAll<T>();
        }

        public static List<T> GetWhereDatas<T>(string where, params object[] args) where T : BaseData
        {
            return SQLService.GetInstance("Data.db").SimpleQuery<T>(where, args);
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
        //public List<TurretDao.TurretData> UseTurret;  //可用炮塔
        // public FireMapDao.FireMapData Map;

        //  public Task task;              //功能暂定
        //  public List<SkillDao.SkillData> UseDatas;//当前技术
        //public List<EnemyDao.EnemyData> UseEnemyDatas;  
        public LevelData() { }
        //public LevelData(LevelDao.LevelData Data)
        //{
        //    this.LevelId = Data.Id;
        //    this.LevelCode = Data.LevelCode;
        //    this.LevelName = Data.LevelName;
        //    this.Description = Data.Description;
        //    this.DifficultyLevel = Data.DifficultyLevel;
        //    this.LevelInformations = Data.LevelInformations;
        //    this.DifficultyLevel = Data.DifficultyLevel;
        //    this.LevelTurretTypeAuthority = Data.LevelTurretTypeAuthority;
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