using System;
using UnityEngine;
using System.Reflection;
using DataAccessObject;
using GameSystem;
using Random = UnityEngine.Random;


public class GameRuntimeEdit : MonoBehaviour
{


    public float f_UpdateInterval = 0.5F;
    private float f_LastInterval;
    private int i_Frames = 0;
    private float f_Fps;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        f_LastInterval = Time.realtimeSinceStartup;

        i_Frames = 0;
    }

    void Update()
    {

        if (Input.GetKey(KeyCode.BackQuote) && Input.GetKeyUp(KeyCode.K))
        {

            Define.IsEdit = !Define.IsEdit;
        }
        if (Define.IsEdit == false)
            return;


        ++i_Frames;

        if (Time.realtimeSinceStartup > f_LastInterval + f_UpdateInterval)
        {
            f_Fps = i_Frames / (Time.realtimeSinceStartup - f_LastInterval);

            i_Frames = 0;

            f_LastInterval = Time.realtimeSinceStartup;
        }


    }

    void OnGUI()
    {
        if (Define.IsEdit == true)
        {

            float windth = Screen.width * 0.4f;
            float height = Screen.height * 0.3f;
            GUI.Window(0, new Rect(Screen.width - windth, height, windth, height), OnDebugWindow, "Debug");
        }
    }


    //  public GUISkin GuiSkin = Resources.Load<GUISkin>("GUISkin");
    public GUISkin GuiSkin;
    public bool mainsceneIsflag = false;
    public bool correlationIsflag = false;
    public bool assListSceneIsflag = false;
    public bool inputIsflag = false;
    public string inputValue;


    public bool articleIsflag = false;
    public int articleId;
    public ArticleData ArticleData;
 
    public string log;



    public void OnDebugWindow(int id)
    {

        //  GUI.skin.label.font = font;
        GUILayout.BeginVertical();

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("MainScene"))
        {
            mainsceneIsflag = !mainsceneIsflag;
            if (mainsceneIsflag == true)
            {
                correlationIsflag = false;
                assListSceneIsflag = false;
                inputIsflag = false;
                articleIsflag = false;
            }
        }

        if (GUILayout.Button("CorrelationComScene"))
        {
            correlationIsflag = !correlationIsflag;
            if (correlationIsflag == true)
            {
                mainsceneIsflag = false;
                assListSceneIsflag = false;
                inputIsflag = false;
                articleIsflag = false;
            }
        }

        if (GUILayout.Button("Input"))
        {
            inputIsflag = !inputIsflag;
            if (inputIsflag)
            {
                mainsceneIsflag = false;
                correlationIsflag = false;
                assListSceneIsflag = false;
                articleIsflag = false;
            }

        }
        if (GUILayout.Button("AssetListScene"))
        {
            assListSceneIsflag = !assListSceneIsflag;
            if (assListSceneIsflag == true)
            {
                mainsceneIsflag = false;
                correlationIsflag = false;
                inputIsflag = false;
                articleIsflag = false;
            }
        }

        if (GUILayout.Button("Article"))
        {
            articleIsflag = !articleIsflag;

            if (articleIsflag)
            {
                mainsceneIsflag = false;
                correlationIsflag = false;
                inputIsflag = false;
                assListSceneIsflag = false;
            }
        }

        GUILayout.EndHorizontal();


        //----mainscene

        if (mainsceneIsflag == true)
        {
            GUILayout.BeginHorizontal();
            GUILayout.Label("Select Button");
            GUILayout.EndHorizontal();


            GUILayout.BeginHorizontal();
            GUILayout.Button("Enter");
            GUILayout.Button("Exit");
            GUILayout.EndHorizontal();
        }

        if (correlationIsflag == true)
        {
            GUILayout.BeginScrollView(Vector2.zero, GUIStyle.none);

            GUILayout.Button("Test");
            GUILayout.Button("Test12");
            GUILayout.Button("Test3");
            GUILayout.Button("Test4");

            GUILayout.EndScrollView();
        }

        if (inputIsflag)
        {

            GUILayout.Label("string type not '' ");

            inputValue = GUILayout.TextField(inputValue);
            if (GUILayout.Button("Confirm"))
            {

                string classname = inputValue.Substring(0, inputValue.IndexOf('.'));
                Debug.Log(classname);

                string mainname = inputValue.Substring(inputValue.IndexOf('.') + 1, inputValue.IndexOf('(') - inputValue.IndexOf('.') - 1);
                Debug.Log(mainname);

                string values = inputValue.Substring(inputValue.IndexOf('(') + 1, inputValue.IndexOf(')') - inputValue.IndexOf('(') - 1);
                Debug.Log(values);

                //获取类型信息
                Type t = Type.GetType(classname);
                //构造器的参数
                object[] constuctParms = values.Split(',');
                //根据类型创建对象
                object dObj = Activator.CreateInstance(t);
                //获取方法的信息
                MethodInfo method = t.GetMethod(mainname);
                //调用方法的一些标志位，这里的含义是Public并且是实例方法，这也是默认的值
                BindingFlags flag = BindingFlags.Public | BindingFlags.Static;

                //调用方法，用一个object接收返回值
                method.Invoke(dObj, flag, Type.DefaultBinder, constuctParms, null);
            }
        }

        if (articleIsflag)
        {
            articleId = int.Parse(GUILayout.TextField(articleId.ToString()));
            GUILayout.BeginHorizontal();


            if (GUILayout.Button("Init Article to Id"))
            {

                ArticleData = SQLService.Instance.QueryUnique<ArticleData>(" Id=?", articleId);

                if (ArticleData != null)
                {


                }
                else
                {
                    log += "\n Not this Id :" + articleId;
                }


            }
            if (GUILayout.Button("Random Article"))
            {
                articleId = Random.Range(0, 10);
                ArticleData = SQLService.Instance.QueryUnique<ArticleData>(" Id=?", articleId);

                if (ArticleData != null)
                {
                }
                else
                {
                    log += "\n Not this Id :" + articleId;
                }
            }

            GUILayout.EndHorizontal();



            if (ArticleData != null)
            {

                //GUI.DrawTexture(new Rect(0,0,461,200), ArticleTexture);

                //GUI.Label(new Rect(0, 0, 461, 72), ArticleData.Name, GuiSkin.GetStyle("label"));
                //GUI.Label(new Rect(0, 72, 461, 16), ArticleSystem.ArticleType(ArticleData.Type1, ArticleData.Type2, ArticleData.Type3), GuiSkin.GetStyle("normallable"));


                if (ArticleData.Type1 == 1)
                {
                    GUILayout.BeginVertical();

                    GUILayout.Label("---------------------------------");
                    GUILayout.Label("名字" + ArticleData.Name);
                    GUILayout.Label("类型" + ArticleSystem.ArticleType(ArticleData.Type1, ArticleData.Type2, ArticleData.Type3));

                    GUILayout.Label(ArticleData.Effect);
                    GUILayout.Label("Bumber:" + ArticleData.Count);
                    GUILayout.Label(ArticleData.Text);

                    GUILayout.Label("Price:" + ArticleData.Value);
                    GUILayout.EndVertical();




                }
                else if (ArticleData.Type1 == 2)
                {
                    GUI.Label(new Rect(0, 88, 461, 42), "物理防御999", GuiSkin.GetStyle("lableMax"));


                    // GUI.Label(new Rect(10, 130, 26, 26), Texture);

                    GUI.Label(new Rect(42, 130, 461, 26), "+2 智力", GuiSkin.GetStyle("lableblue"));
                    GUI.Label(new Rect(42, 156, 461, 26), "+1 烈火学派", GuiSkin.GetStyle("lableblue"));
                    GUI.Label(new Rect(42, 182, 461, 26), "+2 大气学派", GuiSkin.GetStyle("lableblue"));
                    GUI.Label(new Rect(42, 208, 461, 26), "+1 领袖", GuiSkin.GetStyle("lableblue"));
                    GUI.Label(new Rect(42, 234, 461, 26), "+1 坚毅", GuiSkin.GetStyle("lableblue"));

                    //  GUI.Label(new Rect(10, 260, 26, 26),);
                    GUI.Label(new Rect(42, 260, 461, 26), "等级21", GuiSkin.GetStyle("lableh"));

                    // GUI.Label(new Rect(10, 286, 26, 26), Texture);
                    GUI.Label(new Rect(42, 286, 461, 26), "巨型火焰威能符文", GuiSkin.GetStyle("lable1"));
                    GUI.Label(new Rect(42, 312, 461, 26), "智力 + 3", GuiSkin.GetStyle("lable1"));
                    GUI.Label(new Rect(42, 338, 461, 26), "暴击率 +12%", GuiSkin.GetStyle("lable1"));

                    //  GUI.Label(new Rect(20,), );

                    GUI.Label(new Rect(20, 411, 345, 50), "神圣", GuiSkin.GetStyle("lablem"));
                    GUI.Label(new Rect(280, 411, 65, 50), "9999999Y", GuiSkin.GetStyle("lablem"));
                }
            }


        }

        if (GUILayout.Button("Clear Log"))
        {
            log = "";
        }
        GUILayout.Label(log);

        GUILayout.EndVertical();


    }



}
