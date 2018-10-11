using System;
using System.Collections.Generic;
using UnityEngine;

namespace DataAccessObject
{

    //-------------------------------------------------建表语句--------------------------------Start
    public static class Tables
    {
        public static void CreateTable_LivingArea(SQLService service, bool ifNotExists)
        {
            string constraint = ifNotExists ? "IF NOT EXISTS " : "";
            service.connection.Execute("CREATE TABLE " +
                                       constraint +
                                       " LivingAreaModel (" +
                                       " Id INTEGER PRIMARY KEY," +
                                       " Name TEXT," +
                                       " Description TEXT," +
                                       " AreaId INTEGER," +
                                       " PowerId INTEGER," +
                                       " HaveId INTEGER," +
                                       " TypeId INTEGER," +
                                       " BuildingLevel INTEGER," +
                                       " PersonNumber INTEGER," +
                                       " LivingAreaMoney INTEGER," +
                                       " StableValue INTEGER,"+
                                       " BuildingInfoJson TEXT);");
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
        public int Id { get; set; }                           //编号 
        public string Name { get; set; }                //名称
        public string Description { get; set; }         //说明
        public int AreaId { get; set; }                   //地域ID
        public int PowerId { get; set; }                  //势力ID
        public int HaveId { get; set; }                    //拥有者ID
        public int TypeId { get; set; }                    //类型ID
        public int BuildingLevel { get; set; }           //等级
        public int PersonNumber { get; set; }           //人口数量
        public int LivingAreaMoney { get; set; }             //生活区金钱
        public int StableValue { get; set; }                  //安定值
        public string BuildingInfoJson { get; set; }        // 建筑Json
        
        public override object[] GetValues()
        {
            object[] objects = new object[]
                {
                    Id, Name, Description, PowerId, HaveId, TypeId, BuildingLevel, PersonNumber, LivingAreaMoney,StableValue,BuildingInfoJson
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


    
  


    //----------------------------------------映射数据库----------------------------------End

    //-----------------------------------------取值--------------------------------------Start

    public static class SqlData
    {
        public static T GetModelId<T>(int id) where T : BaseModel
        {
            Debug.Log(id+">>>");
            return SQLService.GetInstance("TD.db").QueryUnique<T>(" Id=?", new object[] {id});
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

}