using System;
using System.Collections.Generic;

namespace DataAccessObject
{
    //-------------------------------------------------建表语句--------------------------------Start
    public static class Tables
    {

        /// <summary>
        /// 阵营，不同阵营有不同属性,同时阵营也有 多方混合(如 国家阵营和某些党派阵营的关系向来交好)  帮派  山贼 山庄 党派 国家 
        /// </summary>
        /// <param name="service"></param>
        public static void CreateTable_Faction(SQLService service)
        {
            service.connection.Execute(" CREATE TABLE IF NOT EXISTS FactionData ( " +
                                       " Id INTEGER PRIMARY KEY," +
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
        /// 生活区表 ，设计上生活区的建筑物种类是固有的 但是通过独立的json数据来变更建筑物数据
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
        /// 指定的建筑无
        /// </summary>
        /// <param name="service"></param>
        public static void CreateTable_Building(SQLService service)
        {
            service.connection.Execute(" CREATE TABLE IF NOT EXISTS BuildingData ( " +
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
                                       " Compatibility INTEGER,"+
                                       " LanguageJson TEXT);");
        }



        /// <summary>
        /// 技法
        /// 所有技法均无等级限制 仅为经验的控制  最大经验999  效果为 主参数/（最大经验/1000）
        /// </summary>
        /// <param name="service"></param>
        /// <param name="ifNotExists"></param>
        public static void CreateTable_Techniques(SQLService service )
        {
            service.connection.Execute(" CREATE TABLE IF NOT EXISTS TechniquesData ( " +
                                       " Id INTEGER PRIMARY KEY," +
                                       " Name TEXT," +
                                       " MarkIds TEXT,"+
                                       " Description TEXT, " +
                                       " TechniquesValue INTEGER," +
                                       " Effect TEXT);");
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
        /// 词缀
        /// </summary>
        /// <param name="service"></param>
        public static void CreateTable_Mark(SQLService service)
        {
            service.connection.Execute(" CREATE TABLE IF NOT EXISTS MarkData ( " +
                                       " Id INTEGER PRIMARY KEY," +
                                       " Name TEXT," +
                                       " Description  TEXT," +
                                       " MarkGroup TEXT);");
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

    public class RaceData : BaseData
    {
        public int RaceId { get; set; }
        public int RaceRangeId { get; set; }
        public int RaceType { get; set; }

        public override object[] GetValues()
        {
            throw new NotImplementedException();
        }
    }


    /// <summary>
    /// 生物信息
    /// </summary>
    public class BiologicalData : BaseData
    {
        public int Id { get; set; }                              //ID
        public string Surname { get; set; }                      //姓
        public string Name { get; set; }                         //名 
        public string Title { get; set; }                        //称号
        public string Description { get; set; }                  //说明
        public int RaceType { get; set; }                          //种族类型
        public int Sex { get; set; }                             // 性别
        public int Age { get; set; }                             // 当前年龄
        public int AgeMax { get; set; }                          // 最大年龄
        public int Property1 { get; set; }                       // 主要属性1
        public int Property2 { get; set; }                       //主要属性2
        public int Property3 { get; set; }                       //主要属性3
        public int Property4 { get; set; }                       //主要属性4
        public int Property5 { get; set; }                       //主要属性5
        public int Property6 { get; set; }                       //主要属性6
        public int Prestige { get; set; }                        // 声望
        public int Influence { get; set; }                       //影响力
        public int Disposition { get; set; }                     //性格值  -500到500  
        public DateTime TimeAppearance { get; set; }             //出生时间
        public DateTime TimeEnd { get; set; }                    //死亡时间
        public string FeatureIds { get; set; }                   //特征IDs
        public int IsDebut { get; set; }                         //是否登场
        public string Location { get; set; }                        //所处地方 地区编号
        public string LocationType { get; set; }                    //所处类型 
        public string ArticleJson { get; set; }                  // 物品JSON
        public string EquipmentJson { get; set; }                // 装备JSON
        public string LanguageJson { get; set; }                 // 语言JSON
        public string GongfaJson { get; set; }                   //功法JSON
        public string JifaJson { get; set; }                     //技法JSON
        

        public override object[] GetValues()
        {
            object[] objects = new object[]
                {
                    Id,Surname,Name,Title,Description,RaceType,Sex,Age,AgeMax,Property1,Property2,Property3,Property4,Property5,Property6,
                    Prestige,Influence,Disposition,TimeAppearance,TimeEnd,FeatureIds,IsDebut,Location,LocationType, ArticleJson,EquipmentJson,LanguageJson,
                    GongfaJson,JifaJson
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
            return SQLService.GetInstance("TD.db").QueryUnique<T>(" Id=?", new object[] { id });
        }

        public static List<T> GetAllDatas<T>() where T : BaseData
        {
            return SQLService.GetInstance("TD.db").QueryAll<T>();
        }

        public static List<T> GetWhereDatas<T>(string where, params object[] args) where T : BaseData
        {
            return SQLService.GetInstance("TD.db").SimpleQuery<T>(where, args);
        }

    }

}