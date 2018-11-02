using System.Collections;
using System.Collections.Generic;
using DataAccessObject;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// 记录整个程序运行中所有的确定 和流动数据，不应该被删除
/// </summary>
public class Define : MonoBehaviour
{
    private static Define _value;

    public static Define Value
    {
        get
        {
            if (_value == null)
            {
                GameObject go = GameObject.Find("Define");
                if (go == null)
                {
                    go=new GameObject("Define");
                    _value=go.AddComponent<Define>();
                }
            }
            return _value;
        }
    }

    public readonly string TaskSceneName = "Start";
    public readonly string FightingSceneName = "Demo";
    public readonly string ManagerSceneName = "Manager";
    public readonly string LoadingSceneName = "Loading";



    /// <summary>
    /// 当前游戏状态             -1:发生错误  0:正常界面 1正常战略 2正常战役
    /// </summary>
    public byte GameStatus = 0;

    public int PlayerId = 1;

    public string CurrentSceneName = "";
    public string NextSceneName = "";


    //public  string AvatarPath = "Atlas/";
    //public TextAsset AvatarText { get { return Resources.Load<TextAsset>("Text/AvatarCodeMap"); } }

    //public string BiologicalPath = "Model/";
    //public TextAsset BiologicalText { get { return Resources.Load<TextAsset>("Text/BiologicalModelText"); } }


    public GameObject GameTurretManager { get { return Resources.Load<GameObject>("Prefab/TurretManager"); } }
    public GameObject GameSkillControl { get { return Resources.Load<GameObject>("Prefab/SkillElement"); } }
    public GameObject UiTurretTypeItem { get { return Resources.Load<GameObject>("UIPrefab/GenerateItem/TurretTypeItem"); } }
    public GameObject UiTurretItem { get { return Resources.Load<GameObject>("UIPrefab/GenerateItem/TurretItem"); } }
    public GameObject UiTurretDetails { get { return Resources.Load<GameObject>("UIPrefab/GenerateItem/TurretDetails"); } }
    public GameObject UiTaskItem { get { return Resources.Load<GameObject>("UIPrefab/GenerateItem/TaskItem"); } }

    public GameObject UiLivingAreaBuff { get { return Resources.Load<GameObject>("UIPrefab/ThumbnailsInfo"); } }
    public GameObject UiLivingAreaBuilding { get { return Resources.Load<GameObject>("UIPrefab/LivingArea/LivingAreaBuilding"); } }

    //----->按键
    public KeyCode CofirmCode = KeyCode.Mouse0;
    public KeyCode CancelCode = KeyCode.Mouse1;
    public KeyCode GameRoationLeft = KeyCode.Z;
    public KeyCode GameRoationRight = KeyCode.X;
    public KeyCode ItemSlot1 = KeyCode.Alpha1;
    public KeyCode ItemSlot2 = KeyCode.Alpha2;
    public KeyCode ItemSlot3 = KeyCode.Alpha3;
    public KeyCode ItemSlot4 = KeyCode.Alpha4;
    public KeyCode ItemSlot5 = KeyCode.Alpha5;
    public KeyCode ItemSlot6 = KeyCode.Alpha6;
    public KeyCode ItemSlot7 = KeyCode.Alpha7;
    public KeyCode ItemSlot8 = KeyCode.Alpha8;
    public KeyCode CommandSkill1 = KeyCode.Q;
    public KeyCode CommandSkill2 = KeyCode.W;
    public KeyCode CommandSkill3 = KeyCode.E;
    public KeyCode Intelligent = KeyCode.LeftShift;
    public KeyCode LevelMap = KeyCode.M;
    public KeyCode SkillSlot1 = KeyCode.Alpha1;
    public KeyCode SkillSlot2 = KeyCode.Alpha2;
    public KeyCode SkillSlot3 = KeyCode.Alpha3;
    public KeyCode SkillSlot4 = KeyCode.Alpha4;
    public KeyCode SkillSlot5 = KeyCode.Alpha5;
    public KeyCode SkillSlot6 = KeyCode.Alpha6;
    public KeyCode SkillSlot7 = KeyCode.Alpha7;
    public KeyCode SkillSlot8 = KeyCode.Alpha8;

    void Awake()
    {
        _value = this;
        DontDestroyOnLoad(this);
      
    }
	void Start () {}
    //---->  Main
    public void LoadScene(string nextScene)
    {
        NextSceneName = nextScene;
         SceneManager.LoadScene(LoadingSceneName);
    }


    //----> Algorithm
    public Vector2 s()
    {
        return new Vector2();
    }

}
