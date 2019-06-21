using System;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace DataAccessObject
{
    //-------------------------------------------------建表语句--------------------------------Start
    public static class Tables
    {

        /// <summary>
        /// 剧本
        /// </summary>
        /// <param name="service"></param>
        public static void CreateTable_PlayProject(SQLService service)
        {
            service.connection.Execute(" CREATE TABLE IF NOT EXISTS PlayProjectData ( " +
                                       " Id INTEGER PRIMARY KEY," +
                                       " PlayName TEXT," +
                                       " CreateDataTime TEXT," +
                                       " StartDataTime TEXT," +
                                       " EndDataTime TEXT," +
                                       " Description TEXT);");
        }

        /// <summary>
        /// 区
        /// </summary>
        /// <param name="service"></param>
        public static void CreateTable_District(SQLService service)
        {
            service.connection.Execute(" CREATE TABLE IF NOT EXISTS DistrictData ( " +
                                       " Id INTEGER PRIMARY KEY," +
                                       " Name TEXT," +
                                       " Description TEXT," +
                                       " Model INTEGER," +
                                       " Type INTEGER," +
                                       " ProsperityLevel INTEGER," +
                                       " TrafficLevel INTEGER," +
                                       " GrowingModulus INTEGER," +
                                       " SecurityModulus INTEGER," +
                                       " X INTEGER," +
                                       " Y INTEGER," +
                                       " Z INTEGER);");
        }

        /// <summary>
        /// 生活区表 ，设计上生活区的建筑物种类是固有的 但是通过独立的json数据来变更建筑物数据
        /// </summary>
        /// <param name="service"></param>
        /// <param name="ifNotExists"></param>
        public static void CreateTable_LivingArea(SQLService service)
        {
            service.connection.Execute("CREATE TABLE IF NOT EXISTS LivingAreaData (" +
                                       " Id INTEGER PRIMARY KEY," +
                                       " DistrictId INTEGER," +
                                       " Name TEXT," +
                                       " Description TEXT," +
                                       " ModelBaseId INTEGER," +
                                       " PersonNumber INTEGER," +
                                       " Money INTEGER," +
                                       " MoneyMax INTEGER," +
                                       " Iron INTEGER," +
                                       " IronMax INTEGER," +
                                       " Wood INTEGER," +
                                       " WoodMax INTEGER," +
                                       " Food INTEGER," +
                                       " FoodMax INTEGER," +
                                       " LivingAreaLevel INTEGER," +
                                       " LivingAreaMaxLevel INTEGER," +
                                       " LivingAreaType INTEGER," +
                                       " DefenseStrength INTEGER," +
                                       " StableValue INTEGER," +
                                       " BuildingInfoJson TEXT);");
        }

        /// <summary>
        /// 建筑表
        /// </summary>
        /// <param name="service"></param>
        public static void CreateTable_BuildingData(SQLService service)
        {
            service.connection.Execute(" CREATE TABLE IF NOT EXISTS BuildingData ( " +
                                       " Id INTEGER PRIMARY KEY," +
                                       " Name TEXT," +
                                       " Description TEXT," +
                                       " BuildingModelId INTEGER," +
                                       " Behavior INTEGER," +
                                       " ParentId INTEGER," +
                                       " BuildingLevel TEXT," +
                                       " Status INTEGER," +
                                       " Type INTEGER," +
                                       " DurableValue INTEGER," +
                                       " OwnId INTEGER," +
                                       " ImageId INTEGER," +
                                       " X INTEGER," +
                                       " Y INTEGER," +
                                       " Z INTEGER," +
                                       " Property1 TEXT," +
                                       " Property2 TEXT," +
                                       " Property3 TEXT," +
                                       " Property4 TEXT);");
        }


        /// <summary>
        /// 生物属性
        /// </summary>
        /// <param name="service"></param>
        public static void CreateTable_Biological(SQLService service)
        {
            service.connection.Execute(" CREATE TABLE IF NOT EXISTS BiologicalData ( " +
                                       " Id INTEGER PRIMARY KEY," +
                                       " Surname TEXT," +
                                       " Name TEXT," +
                                       " AvatarId INTEGER," +
                                       " ModelId INTEGER," +
                                       " PrestigeId INTEGER," +
                                       " FamilyId INTEGER," +
                                       " FactionId INTEGER," +
                                       " TitleId INTEGER," +
                                       " Description TEXT," +
                                       " Sex INTEGER," +
                                       " Age INTEGER," +
                                       " AgeMax INTEGER," +
                                       " TimeAppearance TEXT," +
                                       " TimeEnd TEXT," +
                                       " FeatureIds TEXT," +
                                       " IsDebut INTEGER," +
                                       " Location TEXT," +
                                       " LocationType INTEGER," +
                                       " NeutralValue INTEGER," +
                                       " X INTEGER," +
                                       " Y INTEGER," +
                                       " Z INTEGER," +
                                       " Tizhi INTEGER," +
                                       " Lidao INTEGER," +
                                       " Jingshen INTEGER," +
                                       " Lingdong INTEGER," +
                                       " Wuxing INTEGER, " +
                                       " ArticleJson INTEGER," +
                                       " EquipmentJson INTEGER," +
                                       " GongfaJson INTEGER," +
                                       " JifaJson INTEGER," +
                                       " LanguageJson TEXT);");
        }

        /// <summary>
        /// 阵营,派系，不同阵营有不同属性,同时阵营也有 多方混合(如 国家阵营和某些党派阵营的关系向来交好)  帮派  山贼 山庄 党派 国家 
        /// </summary>
        /// <param name="service"></param>
        public static void CreateTable_FactionData(SQLService service)
        {
            service.connection.Execute(" CREATE TABLE IF NOT EXISTS FactionData ( " +
                                       " Id INTEGER PRIMARY KEY," +
                                       " Name TEXT," +
                                       " Description TEXT, " +
                                       " BossId INTEGER," +
                                       " Money INTEGER," +
                                       " Iron INTEGER," +
                                       " Wood INTEGER," +
                                       " Food INTEGER," +
                                       " Population TEXT," +
                                       " FactionType INTEGER);");
        }

        /// <summary>
        /// 家族
        /// </summary>
        public static void CreateTable_FamilyData(SQLService service)
        {
            service.connection.Execute(" CREATE TABLE IF NOT EXISTS FamilyData ( " +
                                       " Id INTEGER PRIMARY KEY," +
                                       " Name TEXT," +
                                       " Description TEXT, " +
                                       " BossId INTEGER," +
                                       " ChildIds INTEGER);");
        }


        /// <summary>
        /// 关系
        /// </summary>
        /// <param name="service"></param>
        public static void CreateTable_RelationData(SQLService service)
        {
            service.connection.Execute(" CREATE TABLE IF NOT EXISTS RelationData ( " +
                                       " MainId INTEGER ," +
                                       " AimsId INTEGER," +
                                       " RelationshipValue INTEGER," +
                                       " RalationType INTEGER);");
        }

        /// <summary>
        /// 声望
        /// </summary>
        /// <param name="service"></param>
        public static void CreateTable_Prestige(SQLService service)
        {
            service.connection.Execute(" CREATE TABLE IF NOT EXISTS PrestigeData ( " +
                                       " Id INTEGER PRIMARY KEY," +
                                       " Type INTEGER," +
                                       " Title TEXT," +
                                       " ValueMin INTEGER," +
                                       " ValueMax INTEGER);");
        }

        /// <summary>
        /// 技法
        /// 所有技法均无等级限制 仅为经验的控制  最大经验999  效果为 主参数/（最大经验/1000）
        /// </summary>
        /// <param name="service"></param>
        /// <param name="ifNotExists"></param>
        public static void CreateTable_Techniques(SQLService service)
        {
            service.connection.Execute(" CREATE TABLE IF NOT EXISTS TechniquesData ( " +
                                       " Id INTEGER PRIMARY KEY," +
                                       " ParentId INTEGER, " +
                                       " BiologicalId INTEGER ," +
                                       " Name TEXT," +
                                       " MarkIds TEXT," +
                                       " Description TEXT, " +
                                       " TechniquesValue INTEGER," +
                                       " Effect TEXT);");
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
                                       " TipsType TEXT," +
                                       " Content TEXT," +
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



        /// <summary>
        /// 头像数据
        /// </summary>
        /// <param name="service"></param>
        public static void CreateTable_Avatar(SQLService service)
        {
            service.connection.Execute(" CREATE TABLE IF NOT EXISTS AvatarData ( " +
                                       " Id INTEGER PRIMARY KEY," +
                                       " Code TEXT," +
                                       " Type INTEGER," +
                                       " Path TEXT);");
        }

        /// <summary>
        /// 生物头像数据
        /// </summary>
        /// <param name="service"></param>
        public static void CreateTable_BiologicalAvatarData(SQLService service)
        {
            service.connection.Execute(" CREATE TABLE IF NOT EXISTS BiologicalAvatarData ( " +
                                       " Id INTEGER PRIMARY KEY," +
                                       " Code TEXT," +
                                       " Path TEXT);");
        }





        /// <summary>
        /// 模型地图
        /// </summary>
        /// <param name="service"></param>
        public static void CreateTable_ModelMap(SQLService service)
        {
            service.connection.Execute(" CREATE TABLE IF NOT EXISTS ModelMapData ( " +
                                       " Id INTEGER PRIMARY KEY," +
                                       " Code TEXT," +
                                       " Type TEXT," +
                                       " Path TEXT);");
        }

        /// <summary>
        /// 话语数据
        /// </summary>
        /// <param name="service"></param>
        public static void CreateTable_SocialDialog(SQLService service)
        {
            service.connection.Execute(" CREATE TABLE IF NOT EXISTS SocialDialogData ( " +
                                       " Id INTEGER PRIMARY KEY," +
                                       " Type INTEGER," +
                                       " Content TEXT);");
        }


        public static void CreateTable_Skill(SQLService service)
        {
            service.connection.Execute(" CREATE TABLE IF NOT EXISTS SkillData ( " +
                                       " Id INTEGER PRIMARY KEY," +
                                       " Name TEXT," +
                                       " Description TEXT," +
                                       " IconPath TEXT," +
                                       " SkillType INTEGER," +
                                       " DifficultLevel INTEGER," +
                                       " Wisdom INTEGER," +
                                       " Strength INTEGER," +
                                       " Effect INTEGER," +
                                       " Consumption INTEGER," +
                                       " AvatarId INTEGER);");
        }

        public static void CreateTable_NounsData(SQLService service)
        {
            service.connection.Execute(" CREATE TABLE IF NOT EXISTS NounsData ( " +
                                       " NId INTEGER PRIMARY KEY," +
                                       " Name TEXT," +
                                       " Note TEXT," +
                                       " Content1 TEXT," +
                                       " Content2 TEXT);");
        }


        public static void CreateTable_ArticleData(SQLService service)
        {
            service.connection.Execute(" CREATE TABLE IF NOT EXISTS ArticleData ( " +
                                       " Id INTEGER PRIMARY KEY," +
                                       " AvatarId INTEGER," +
                                       " Type1 INTEGER," +
                                       " Type2 INTEGER," +
                                       " Type3 INTEGER," +
                                       " Text TEXT," +
                                       " Value TEXT," +
                                       " Count INTEGER," +
                                       " MaxCount INTEGER," +
                                       " Effect TEXT," +
                                       " Weight INTEGER," +
                                       " Bid INTEGER);");
        }

        public static void CreateTable_ArticleTypeData(SQLService service)
        {
            service.connection.Execute(" CREATE TABLE IF NOT EXISTS ArticleTypeData ( " +
                                       " Id INTEGER PRIMARY KEY," +
                                       " PId INTEGER," +
                                       " Text TEXT);");

        }

        public static void CreateTable_ArticleRecordData(SQLService service)
        {
            service.connection.Execute(" CREATE TABLE IF NOT EXISTS ArticleRecordData ( " +
                                       " Bid INTEGER," +
                                       " ArticleId INTEGER," +
                                       " Count INTEGER," +
                                       " MaxCount INTEGER," +
                                       " Time TEXT);");
        }


        /// <summary>
        /// 模型
        /// </summary>
        /// <param name="service"></param>
        public static void CreateTable_BiologicalModelData(SQLService service)
        {
            service.connection.Execute(" CREATE TABLE IF NOT EXISTS BiologicalModelData ( " +
                                       " Id INTEGER PRIMARY KEY," +
                                       " Code TEXT," +
                                       " Type INTEGER," +
                                       " Path TEXT);");
        }

        /// <summary>
        /// 模型
        /// </summary>
        /// <param name="service"></param>
        public static void CreateTable_LivingAreaModelData(SQLService service)
        {
            service.connection.Execute(" CREATE TABLE IF NOT EXISTS LivingAreaModelData ( " +
                                       " Id INTEGER PRIMARY KEY," +
                                       " Code TEXT," +
                                       " Type INTEGER," +
                                       " Path TEXT);");
        }

    }
    //-----------------------------------------建表语句 -----------------------------------End


    //----------------------------------------映射数据库----------------------------------Start

    public abstract class BaseData
    {
        public abstract object[] GetValues();
    }

    /// <summary>
    /// 省份 
    /// </summary>
    public class DistrictData : BaseData
    {
        public int Id { get; set; }                          // ID
        public string Name { get; set; }                     // 名称
        public string Description { get; set; }              // 说明
        public int Model { get; set; }
        public int Type { get; set; }
        public int ProsperityLevel { get; set; }
        public int TrafficLevel { get; set; }              //交通系数
        public int GrowingModulus { get; set; }              // 发展系数
        public int SecurityModulus { get; set; }             // 安全系数
        public string LivinfAreasIds { get; set; }           //生活区ID
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }


        public override object[] GetValues()
        {
            object[] objects = new object[]
            {
                Id, Name, Description,Model,Type,ProsperityLevel,TrafficLevel,GrowingModulus,SecurityModulus,LivinfAreasIds,X,Y,Z
            };
            return objects;
        }
    }
    /// <summary>
    /// 居住地
    /// </summary>
    public class LivingAreaData : BaseData
    {
        public int Id { get; set; }                      //编号 
        public int PowerId { get; set; }
        public string Name { get; set; }                //名称
        public string Description { get; set; }         //说明
        public string ModelBase { get; set; }
        public int ModelBaseId { get; set; }
        public int ModelMain { get; set; }
        public int PersonNumber { get; set; }           //人口数量
        public int Money { get; set; }
        public int MoneyMax { get; set; }
        public int Iron { get; set; }
        public int IronMax { get; set; }
        public int Wood { get; set; }
        public int WoodMax { get; set; }
        public int Food { get; set; }
        public int FoodMax { get; set; }
        public int LivingAreaLevel { get; set; }            //生活区等级
        public int LivingAreaMaxLevel { get; set; }        //生活区最大等级
        public int LivingAreaType { get; set; }             //生活区类型
        public int DefenseStrength { get; set; }            //防守强度
        public int StableValue { get; set; }                  //安定值
        public string BuildingInfoJson { get; set; }        // 建筑Json
        public int PositionX { get; set; }
        public int PositionY { get; set; }
        public int PositionZ { get; set; }

        public byte IsBazaar { get; set; }
        public byte IsBlacksmith { get; set; }



        public override object[] GetValues()
        {
            object[] objects = new object[]
            {
                Id,PowerId, Name, Description,ModelBase,ModelBaseId,ModelMain, PersonNumber,Money,MoneyMax,Iron,IronMax,
                Wood, WoodMax, Food, FoodMax, LivingAreaLevel,LivingAreaMaxLevel,LivingAreaType,DefenseStrength,StableValue,BuildingInfoJson,PositionX,PositionY,PositionZ
            };
            return objects;
        }
    }
    /// <summary>
    /// 建筑
    /// </summary>
    public class BuildingData : BaseData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int BuildingModelId { get; set; }
        public string Behavior { get; set; }
        public int ParentId { get; set; }
        public int BuildingLevel { get; set; }
        public int Status { get; set; }
        public int Type { get; set; }
        public int DurableValue { get; set; }
        public int OwnId { get; set; }
        public int ImageId { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public string Property1 { get; set; }
        public string Property2 { get; set; }
        public string Property3 { get; set; }
        public string Property4 { get; set; }

        public override object[] GetValues()
        {
            object[] objects = new object[]
            {
                Id,Name,Description,BuildingModelId,Behavior,ParentId,BuildingLevel,Status,Type,DurableValue,OwnId,ImageId,X,Y,Z,Property1,Property2,Property3,Property4
            };

            return objects;
        }
    }

    /// <summary>
    /// 生物信息
    /// </summary>
    public class BiologicalData : BaseData
    {
        public int Id { get; set; }                              //ID
        public int Identity { get; set; }

        public string Surname { get; set; }                      //姓
        public string Name { get; set; }                         //名 

        public int AvatarId { get; set; }           //头像ID
        public int ModelId { get; set; }            //模型ID

        public int FamilyId { get; set; }               //家族ID
        public int FactionId { get; set; }              //派系ID
        public int TitleId { get; set; }                //称号ID
        public int TeamId { get; set; }

        public string Description { get; set; }                  //说明
        public int Sex { get; set; }                             // 性别
        public int Age { get; set; }                             // 当前年龄
        public int AgeMax { get; set; }                          // 最大年龄
        public DateTime TimeAppearance { get; set; }             //出生时间
        public DateTime TimeEnd { get; set; }                    //死亡时间
        public string FeatureIds { get; set; }                   //特征ID
        public int IsDebut { get; set; }                         //是否登场
        public string Location { get; set; }                        //所处地方 地区编号
        public int LocationType { get; set; }                       //所处类型 
        public int X { get; set; }
        public int Y { get; set; }
        public int Z { get; set; }

        public int Tizhi { get; set; }
        public int Lidao { get; set; }
        public int Jingshen { get; set; }
        public int Lingdong { get; set; }
        public int Wuxing { get; set; }

        public string ArticleJson { get; set; }                  // 物品JSON
        public string EquipmentJson { get; set; }                // 装备JSON
        public string GongfaJson { get; set; }                   //功法JSON
        public string JifaJson { get; set; }                     //技法JSON
        public string LanguageJson { get; set; }                 // 语言JSON

        public int Disposition { get; set; }                    //性格
        public int NeutralValue { get; set; }

        public int PrestigeValue { get; set; }

        public override object[] GetValues()
        {
            object[] objects = new object[]
                {
                    Id,Identity,Surname,Name,AvatarId,ModelId,FamilyId,FactionId,TitleId,TeamId,Description,Sex,Age,AgeMax,TimeAppearance,TimeEnd,FeatureIds,IsDebut,Location,
                    LocationType,X,Y,Z,Tizhi,Lidao,Jingshen,Lingdong,Wuxing,ArticleJson,EquipmentJson, GongfaJson,JifaJson,LanguageJson,Disposition,NeutralValue,PrestigeValue
                };
            return objects;
        }
    }
    /// <summary>
    /// 派系
    /// </summary>
    public class FactionData : BaseData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int FactionLevel { get; set; }
        public int FactionType { get; set; }
        public int Money { get; set; }
        public int MoneyMax { get; set; }
        public int Iron { get; set; }
        public int IronMax { get; set; }
        public int Wood { get; set; }
        public int WoodMax { get; set; }
        public int Food { get; set; }
        public int FoodMax { get; set; }

        public override object[] GetValues()
        {
            object[] objects = new object[]
            {
                Id,Name,Description,FactionLevel,FactionType,Money,Iron,Wood,Food
            };

            return objects;
        }
    }
    /// <summary>
    /// 家族
    /// </summary>
    public class FamilyData : BaseData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int BossId { get; set; }
        public string ChildIds { get; set; }

        public override object[] GetValues()
        {
            object[] objects = new object[]
            {
                Id,Name,Description,BossId,ChildIds
            };

            return objects;
        }
    }
    /// <summary>
    /// 声望
    /// </summary>
    public class PrestigeData : BaseData
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public string Title { get; set; }
        public int ValueMin { get; set; }
        public int ValueMax { get; set; }

        public override object[] GetValues()
        {
            object[] objects = new object[]
            {
                Id,Type, ValueMin, ValueMax,Title
            };
            return objects;
        }
    }

    /// <summary>
    /// 生物关系
    /// </summary>
    public class RelationData : BaseData
    {

        public int ObjectAid { get; set; }
        public int ObjectBid { get; set; }
        public int Value { get; set; }

        public override object[] GetValues()
        {
            object[] objects = new object[]
            {
                ObjectAid,ObjectBid,Value
            };
            return objects;
        }
    }

    public class TechniquesData : BaseData
    {
        public int Id { get; set; }
        public string AvatarPath { get; set; }
        public int ParentId { get; set; }
        public string Name { get; set; }
        public string MarkIds { get; set; }
        public string Description { get; set; }
        public int TechniquesValue { get; set; }
        public string Effect { get; set; }


        public override object[] GetValues()
        {
            object[] objects = new object[]
            {
                Id,AvatarPath,ParentId,Name,MarkIds,Description,TechniquesValue,Effect
            };
            return objects;
        }
    }

    public class EquipmentData : BaseData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Type { get; set; }
        public string AvatarPath { get; set; }

        public override object[] GetValues()
        {
            object[] objects = new object[]
            {
                Id,Name,Description,Type,AvatarPath
            };
            return objects;
        }
    }

    public class OrganizationData : BaseData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string AvaterPath { get; set; }
        public int Level { get; set; }
        public int Permanence { get; set; }
        public int ResearchValue { get; set; }
        public int FoodValue { get; set; }
        public int HandworkValue { get; set; }


        public override object[] GetValues()
        {
            object[] objects = new object[]
            {
                Id, Name, Description, AvaterPath,Level,Permanence,ResearchValue,FoodValue,HandworkValue
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

    /// <summary>
    /// 模型信息
    /// </summary>
    public class BiologicalModelData : BaseData
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int Type { get; set; }
        public string Path { get; set; }

        public override object[] GetValues()
        {
            object[] objects = new object[]
            {
                Id,Code,Type,Path
            };
            return objects;
        }
    }
    /// <summary>
    /// 模型信息
    /// </summary>
    public class LivingAreaModelData : BaseData
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int Type { get; set; }
        public string Path { get; set; }

        public override object[] GetValues()
        {
            object[] objects = new object[]
            {
                Id,Code,Type,Path
            };
            return objects;
        }
    }


    /// <summary>
    /// 头像数据
    /// </summary>
    public class AvatarData : BaseData
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public int Type { get; set; }
        public string Path { get; set; }

        public override object[] GetValues()
        {
            object[] objects = new object[]
            {
                Id,Code,Type,Path
            };
            return objects;
        }
    }

    public class BiologicalAvatarData : BaseData
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Path { get; set; }

        public override object[] GetValues()
        {
            object[] objects = new object[]
            {
                Id,Code,Path
            };
            return objects;
        }
    }

    public class SkillData : BaseData
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string IconPath { get; set; }
        public int SkillType { get; set; }
        public int DifficultLevel { get; set; }
        public int Wisdom { get; set; }
        public int Strength { get; set; }
        public int Effect { get; set; }
        public int Consumption { get; set; }
        public int AvatarId { get; set; }
        public override object[] GetValues()
        {
            object[] objects = new object[]
            {
                Id,Name,Description,IconPath,SkillType,DifficultLevel,Wisdom,Strength,Effect,Consumption,AvatarId,
            };
            return objects;
        }

    }

    /// <summary>
    /// SocialInfoData
    /// </summary>
    public class SocialDialogData : BaseData
    {
        public int Id { get; set; }
        public int Type { get; set; }
        public string Content { get; set; }
        public string ParentId { get; set; }
        public string ChildId { get; set; }

        public override object[] GetValues()
        {
            object[] objects = new object[]
            {
                Id,Type,Content,ParentId,ChildId
            };
            return objects;
        }
    }

    public class PlayProjectData : BaseData
    {
        public int Id { get; set; }
        public string PlayName { get; set; }
        public string Description { get; set; }

        public override object[] GetValues()
        {
            object[] objects = new object[]
            {
               Id,PlayName,Description
            };
            return objects;
        }
    }





    /// <summary>
    /// 名词注解表
    /// </summary>
    public class NounsData : BaseData
    {
        public int NId { get; set; }
        public string Name { get; set; }
        public string Note { get; set; }
        public string Content1 { get; set; }
        public string Content2 { get; set; }

        public override object[] GetValues()
        {
            object[] objects = { NId, Name, Note, Content1, Content2 };
            return objects;
        }
    }


    public class ArticleRecordingData : BaseData
    {
        public ArticleRecordingData()
        {

        }

        public ArticleRecordingData(int id, string name, string attribute, string itemdesc, int guiid, int type, int state, int count, int maxCount)
        {
            this.Id = id;
            this.Name = name;
            this.Attribute = attribute;
            this.ItemDesc = itemdesc;
            this.GuiId = guiid;
            this.Type = type;
            this.State = state;
            this.Count = count;
            this.MaxCount = count;

        }
        public int Id { get; set; }
        public string Name { get; set; }
        public string Attribute { get; set; }
        public string ItemDesc { get; set; }
        public int GuiId { get; set; }
        public int Type { get; set; }
        public int State { get; set; }
        public int Count { get; set; }
        public int MaxCount { get; set; }


        public override object[] GetValues()
        {
            object[] objects = { Id };
            return objects;
        }

    }



    public class ArticleData : BaseData
    {

        public int Id { get; set; }
        public int AvatarId { get; set; }
        public byte Type1 { get; set; }
        public byte Type2 { get; set; }
        public byte Type3 { get; set; }
        public string Text { get; set; }
        public string Value { get; set; }
        public int Count { get; set; }
        public int MaxCount { get; set; }
        public string Effect { get; set; }
        public int Weight { get; set; }
        public int Bid { get; set; }

        public override object[] GetValues()
        {
            object[] objects = { Id, AvatarId, Type1, Type2, Type3, Text, Value, Count, MaxCount, Weight, Bid };
            return objects;
        }

    }

    public class ArticleTypeData : BaseData
    {
        public int Id { get; set; }
        public int PId { get; set; }
        public string Text { get; set; }


        public override object[] GetValues()
        {
            object[] objects = { Id, PId, Text };
            return objects;
        }

    }


    public class ArticleRecordData : BaseData
    {
        public int Bid { get; set; }
        public int ArticleId { get; set; }
        public int Count { get; set; }
        public int MaxCount { get; set; }
        public string Time { get; set; }

        public override object[] GetValues()
        {
            object[] objects = { Bid, ArticleId, Count, MaxCount, Time };
            return objects;
        }
    }



    //----------------------------------------映射数据库----------------------------------End

    //-----------------------------------------取值--------------------------------------Start

    //public static class SqlData
    //{
    //    public static T GetDataId<T>(int id) where T : BaseData
    //    {
    //        return SQLService.GetInstance("TD.db").QueryUnique<T>(" Id=? ", new object[] { id });
    //    }

    //    public static List<T> GetAllDatas<T>() where T : BaseData
    //    {
    //        return SQLService.GetInstance("TD.db").QueryAll<T>();
    //    }

    //    public static List<T> GetWhereDatas<T>(string where, params object[] args) where T : BaseData
    //    {
    //        return SQLService.GetInstance("TD.db").SimpleQuery<T>(where, args);
    //    }

    //    public static T GetDataWhereOnly<T>(string where, params object[] args) where T : BaseData
    //    {

    //        return SQLService.GetInstance("TD.db").QueryUnique<T>(where, args);
    //    }

    //}


}