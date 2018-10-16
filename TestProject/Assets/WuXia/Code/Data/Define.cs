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
        get { return _value; }
    }

    void Awake()
    {
        _value = this;
        DontDestroyOnLoad(this);
    }
	void Start () {
	}
	

    /// <summary>
    /// 当前游戏状态             -1:发生错误  0:正常界面 1正常战略 2正常战役
    /// </summary>
    public byte GameStatus = 0;

    private LevelData _currentSelectLevelData;

    public LevelData CurrentSelectLevelData
    {
        get
        {
            return _currentSelectLevelData;
        }
        set { _currentSelectLevelData = value; }

    }

    public readonly string TaskSceneName = "Start";
    public readonly string FightingSceneName = "Demo";
    public readonly string ManagerSceneName = "Manager";
    public readonly string LoadingSceneName = "Loading";

    public string CurrentSceneName = "";
    public string NextSceneName = "";

    /// <summary>
    /// 记录当前进入的LivingArea，如果没有则为-1
    /// </summary>
    public int EnterLivingAreaId = -1;

    public List<int> CommanderTurret;
    public List<int> CommanderSkill;
    public GameObject GameTurretManager { get { return Resources.Load<GameObject>("Prefab/TurretManager"); } }
    public GameObject GameSkillControl { get { return Resources.Load<GameObject>("Prefab/SkillElement"); } }
    public GameObject UiTurretTypeItem { get { return Resources.Load<GameObject>("UIPrefab/GenerateItem/TurretTypeItem"); } }
    public GameObject UiTurretItem { get { return Resources.Load<GameObject>("UIPrefab/GenerateItem/TurretItem"); } }
    public GameObject UiTurretDetails { get { return Resources.Load<GameObject>("UIPrefab/GenerateItem/TurretDetails"); } }
    public GameObject UiTaskItem { get { return Resources.Load<GameObject>("UIPrefab/GenerateItem/TaskItem"); } }


    //----->按键
    private KeyCode _cofirmCode = KeyCode.Mouse0;
    private KeyCode _cancelCode = KeyCode.Mouse1;
    private KeyCode _gameRoationLeft = KeyCode.Z;
    private KeyCode _gameRoationRight = KeyCode.X;
    private KeyCode _itemSlot1 = KeyCode.Alpha1;
    private KeyCode _itemSlot2 = KeyCode.Alpha2;
    private KeyCode _itemSlot3 = KeyCode.Alpha3;
    private KeyCode _itemSlot4 = KeyCode.Alpha4;
    private KeyCode _itemSlot5 = KeyCode.Alpha5;
    private KeyCode _itemSlot6 = KeyCode.Alpha6;
    private KeyCode _itemSlot7 = KeyCode.Alpha7;
    private KeyCode _itemSlot8 = KeyCode.Alpha8;
    private KeyCode _commandSkill1 = KeyCode.Q;
    private KeyCode _commandSkill2 = KeyCode.W;
    private KeyCode _commandSkill3 = KeyCode.E;
    private KeyCode _intelligent = KeyCode.LeftShift;
    private KeyCode _levelMap = KeyCode.M;
    public KeyCode ConfirmCode { get { return _cofirmCode; } }
    public KeyCode CancelCode { get { return _cancelCode; } }
    public KeyCode GameRoationLeft { get { return _gameRoationLeft; } }
    public KeyCode GameRoationRight { get { return _gameRoationRight; } }
    public KeyCode ItemSlot1 { get { return _itemSlot1; } }
    public KeyCode ItemSlot2 { get { return _itemSlot2; } }
    public KeyCode ItemSlot3 { get { return _itemSlot3; } }
    public KeyCode ItemSlot4 { get { return _itemSlot4; } }
    public KeyCode ItemSlot5 { get { return _itemSlot5; } }
    public KeyCode ItemSlot6 { get { return _itemSlot6; } }
    public KeyCode ItemSlot7 { get { return _itemSlot7; } }
    public KeyCode ItemSlot8 { get { return _itemSlot8; } }
    public KeyCode CommandSkill1 { get { return _commandSkill1; } }
    public KeyCode CommandSkill2 { get { return _commandSkill2; } }
    public KeyCode CommandSkill3 { get { return _commandSkill3; } }
    public KeyCode Intelligent { get { return _intelligent; } }

    private KeyCode _skillSlot1 = KeyCode.Alpha1;
    private KeyCode _skillSlot2 = KeyCode.Alpha2;
    private KeyCode _skillSlot3 = KeyCode.Alpha3;
    private KeyCode _skillSlot4 = KeyCode.Alpha4;
    private KeyCode _skillSlot5 = KeyCode.Alpha5;
    private KeyCode _skillSlot6 = KeyCode.Alpha6;
    private KeyCode _skillSlot7 = KeyCode.Alpha7;
    private KeyCode _skillSlot8 = KeyCode.Alpha8;

    public bool SkillSlot1 { get { if (Input.GetKey(_skillSlot1) == true) { return true; } return false; } }
    public bool SkillSlot2 { get { if (Input.GetKey(_skillSlot2) == true) { return true; } return false; } }
    public bool SkillSlot3 { get { if (Input.GetKey(_skillSlot3) == true) { return true; } return false; } }
    public bool SkillSlot4 { get { if (Input.GetKey(_skillSlot4) == true) { return true; } return false; } }
    public bool SkillSlot5 { get { if (Input.GetKey(_skillSlot5) == true) { return true; } return false; } }
    public bool SkillSlot6 { get { if (Input.GetKey(_skillSlot6) == true) { return true; } return false; } }
    public bool SkillSlot7 { get { if (Input.GetKey(_skillSlot7) == true) { return true; } return false; } }
    public bool SkillSlot8 { get { if (Input.GetKey(_skillSlot8) == true) { return true; } return false; } }
    public bool LevelMap { get { if (Input.GetKey(_levelMap) == true) { return true; } return false; } }

    /// <summary>
    /// Tips  加载界面上的小知识
    /// </summary>
    public string[] Tips = new[]
    {
        "A ::::::",
        "B ::::::",
        "C ::::::"
    };


    //---->  Main
    public void LoadScene(string nextScene)
    {
        NextSceneName = nextScene;
     //   SceneManager.LoadScene(StaticValue.Instance.LoadingSceneName);
    }
}
