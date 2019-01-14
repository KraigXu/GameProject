using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using SQLite4Unity3d;
using System.IO;
using DataAccessObject;
using System;
public class SQLService
{
    public SQLiteConnection connection;
    private static SQLService sqlService;
    private int currentVersion = 1;
    private string databaseName;
    private string versionPath = Application.dataPath + "//version.txt";

    public static SQLService GetInstance(string databaseName)
    {
        if (sqlService == null)
        {
            sqlService = new SQLService(databaseName);
        }
        else
        {
            if (!sqlService.databaseName.Equals(databaseName))
            {
                sqlService.connection.Close();
                sqlService = new SQLService(databaseName);
            }
        }
        return sqlService;
    }

    public static void CloseDB()
    {
        if (sqlService != null)
        {
            sqlService.connection.Close();
            sqlService = null;
        }
    }

    public SQLService(string databaseName)
    {
        bool createTable = false;
        this.databaseName = databaseName;

#if UNITY_EDITOR
        var dbPath = string.Format(@"Assets/StreamingAssets/{0}", databaseName);
        //FIXME:判断是否存在
        createTable = true;
#else
        // check if file exists in Application.persistentDataPath
        var filepath = string.Format("{0}/{1}", Application.persistentDataPath, databaseName);

        //如果不存在就将数据库复制到指定路径
        if (!File.Exists(filepath))
        {
            createTable = true;
            Debug.Log("Database not in Persistent path");
#if UNITY_ANDROID
            File.Create(filepath);
#elif UNITY_IOS
            File.Create(filepath);
#endif
            Debug.Log("Database written");
        }
        var dbPath = filepath;
#endif
        connection = new SQLiteConnection(dbPath, SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create);

        if (createTable)
        {
            CreateDB();
        }

    }

    public void CreateDB()
    {

        Tables.CreateTable_PlayProject(this);
        Tables.CreateTable_District(this);
        Tables.CreateTable_LivingArea(this);
        Tables.CreateTable_BuildingData(this);

        Tables.CreateTable_Biological(this);
        Tables.CreateTable_FactionData(this);
        Tables.CreateTable_FamilyData(this);
        Tables.CreateTable_RelationData(this);
        Tables.CreateTable_Prestige(this);
        Tables.CreateTable_Techniques(this);
        Tables.CreateTable_Gongfa(this);

        Tables.CreateTable_Avatar(this);
        Tables.CreateTable_BiologicalAvatarData(this);
        Tables.CreateTable_ModelData(this);
        Tables.CreateTable_ModelMap(this);
        Tables.CreateTable_SocialDialog(this);
    }


    public long InsertOrReplace(BaseData baseModel)
    {
        return connection.InsertOrReplace(baseModel);
    }

    public void InsertOrReplaceAll<T>(ICollection<T> baseModels) where T : BaseData
    {
        connection.RunInTransaction(() =>
        {
            foreach (var baseModel in baseModels)
            {
                InsertOrReplace(baseModel);
            }
        });
    }

    public void DropTable<T>() where T : BaseData
    {
        var query = string.Format("drop table if exists \"{0}\"", typeof(T).Name);
        connection.ExecuteNonQuery(query);
    }

    public List<T> Query<T>(string query, params object[] args) where T : BaseData
    {
        var cmd = connection.CreateCommand(query, args);
        return cmd.ExecuteQuery<T>();
    }

    public List<T> QueryAll<T>() where T : BaseData
    {
        string query = "select * from " + typeof(T).Name.Split('+')[0].ToString();
        return Query<T>(query, new object[] { });
    }



    public T QueryUniqueThrowException<T>(string where, params object[] args) where T : BaseData
    {
        string query = "select * from " + typeof(T).Name.Split('+')[0].ToString();
        List<T> list = Query<T>(query, args);

        if (list.Count == 1)
        {
            return list[0];
        }
        else
        {
            if (list.Count > 1)
                throw new Exception("Not Unique");
            else
                return null;
        }
    }

    public T QueryUnique<T>(string where, params object[] args) where T : BaseData
    {
        string query = "select * from " + typeof(T).Name.Split('+')[0] + " where " + where;
        List<T> list = Query<T>(query, args);

        if (list.Count > 0)
        {
            return list[0];
        }
        else
        {
            return null;
        }
    }

    public List<T> SimpleQuery<T>(string where, params object[] args) where T : BaseData
    {
        string query = "select * from " + typeof(T).Name.Split('+')[0].ToString() + " where " + where;
        return Query<T>(query, args);
    }

    public int SimpleDelete<T>(string where, params object[] args) where T : BaseData
    {
        string query = "delete from " + typeof(T).Name.Split('+')[0].ToString() + " where " + where;
        var cmd = connection.CreateCommand(query, args);
        return cmd.ExecuteNonQuery();
    }



    public static string SQLHolder(int size)
    {
        List<string> paras = new List<string>();
        for (int i = 0; i < size; i++)
            paras.Add("?");
        return string.Join(",", paras.ToArray());
    }
}
