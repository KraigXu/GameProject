using System;
using GameSystem;
using UnityEngine;
using UnityEngine.EventSystems;

public delegate void TouchInputEventHandler<T, U>(T sender, U e);
public class TouchInputEventArgs : EventArgs
{
    public int mouseButton = 0;
    public Vector3 touchPoint = Vector3.zero;

}

public class KeyInputEventArgs : EventArgs
{
    public KeyCode keyCode = 0;
    public Vector3 touchPoint = Vector3.zero;
}

public class StrategyMouseInfo : MonoBehaviour
{
    public event TouchInputEventHandler<StrategyMouseInfo, TouchInputEventArgs> DoubleLeftClick;
    /// <summary>
    /// 单点开始
    /// </summary>
    public event TouchInputEventHandler<StrategyMouseInfo, TouchInputEventArgs> SingleStart;
    /// <summary>
    /// 单点结束
    /// </summary>
    public event TouchInputEventHandler<StrategyMouseInfo, TouchInputEventArgs> SingleEnded;
    /// <summary>
    /// 单点旋转开始
    /// </summary>
    public event TouchInputEventHandler<StrategyMouseInfo, TouchInputEventArgs> SinglePanStart;
    /// <summary>
    /// 单点旋转
    /// </summary>
    public event TouchInputEventHandler<StrategyMouseInfo, TouchInputEventArgs> SinglePan;
    /// <summary>
    /// 单点按住
    /// </summary>
    public event TouchInputEventHandler<StrategyMouseInfo, TouchInputEventArgs> SingleHold;
    /// <summary>
    /// 两点同时按住
    /// </summary>
    public event TouchInputEventHandler<StrategyMouseInfo, TouchInputEventArgs> DoubleStart;
    /// <summary>
    /// 两点平移
    /// </summary>
    public event TouchInputEventHandler<StrategyMouseInfo, TouchInputEventArgs> DoublePan;
    /// <summary>
    /// 两点缩放
    /// </summary>
    public event TouchInputEventHandler<StrategyMouseInfo, TouchInputEventArgs> DoubleScale;
    /// <summary>
    /// 两点同时离开
    /// </summary>
    public event TouchInputEventHandler<StrategyMouseInfo, TouchInputEventArgs> DoubleEnd;
    /// <summary>
    /// 双指变单指
    /// </summary>
    public event TouchInputEventHandler<StrategyMouseInfo, TouchInputEventArgs> Double2Single;
    /// <summary>
    /// 没有手指在屏幕上
    /// </summary>
    public event TouchInputEventHandler<StrategyMouseInfo, TouchInputEventArgs> NoTouchSinger;
    /// <summary>
    /// 鼠标左键按下
    /// </summary>
    public event TouchInputEventHandler<StrategyMouseInfo, TouchInputEventArgs> LeftMouseMove;
    /// <summary>
    /// 鼠标右键弹起
    /// </summary>
    public event TouchInputEventHandler<StrategyMouseInfo, TouchInputEventArgs> RightMouseUp;
    /// <summary>
    /// 鼠标右键按下
    /// </summary>
    public event TouchInputEventHandler<StrategyMouseInfo, TouchInputEventArgs> RightMouseDown;
    /// <summary>
    /// 发射测点射线
    /// </summary>
    public event TouchInputEventHandler<StrategyMouseInfo, TouchInputEventArgs> RayMonitorCast;
    /// <summary>
    /// 键盘键按下
    /// </summary>
    public event TouchInputEventHandler<StrategyMouseInfo, KeyCode> KeyDown;
    /// <summary>
    /// 键盘键弹起
    /// </summary>
    public event TouchInputEventHandler<StrategyMouseInfo, KeyCode> KeyUp;


    private TouchInputEventArgs _curArgs = new TouchInputEventArgs();

    public static StrategyMouseInfo Instance
    {
        get { return _instance; }
    }
    private static StrategyMouseInfo _instance;


    public GameObject CurObject
    {
        get { return _curObject; }
    }

    [SerializeField]
    private GameObject _curObject;

    [SerializeField]
    private EventSystem _eventSystem;

    public bool IsShowArticleUi;

    private string _target;





    [SerializeField]
    private GUISkin Skin;
    [SerializeField]
    private Texture Texture;

    public EventSystem EventSystem
    {
        get { return _eventSystem; }
    }
    void Awake()
    {
        _instance = this;
    }



    public void InObject(GameObject go)
    {

        if (_curObject == null)    //当前对象为空时
        {
            _curObject = go;
        }else if (go.GetInstanceID() != _curObject.GetInstanceID())   //当前对象!=时
        {
            _curObject = go;
        }
        else
        {
            return;
        }


        if (go.GetComponent<UiArticleBox>() != null)
        {
            IsShowArticleUi = true;
            _target = "ArticleInfo";
        }
    }




    public void PutObject(GameObject go)
    {
        if (_curObject.GetInstanceID() == go.GetInstanceID())
        {
            _curObject = null;
            IsShowArticleUi = false;
            _target = string.Empty;
        }

    }

	void Start () {
	}
    void Update()
    {


        if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.S) || Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.D))
        {
            if (KeyUp != null)
            {
                KeyUp(this, KeyCode.None);
            }
        }
        //if (IsCrossUI)
        //{
        //    Cursor.visible = true;
        //    lastMousePosition = Vector3.zero;
        //    return;
        //}
        //if (UICamera.isOverUI)
        //{
        //    Cursor.visible = true;
        //    return;
        //}
        //if (isJudgeDoubleClick)
        //{
        //    doubleClickIntervalTime += Time.deltaTime;
        //}
        //鼠标左键按下时
        if (Input.GetMouseButtonDown(0) && !Input.GetMouseButtonDown(1))
        {
            _curArgs.touchPoint = Input.mousePosition;
            _curArgs.mouseButton = 0;

            if (SingleStart != null)
            {
                SingleStart(this, _curArgs);
            }

            //if (isJudgeDoubleClick)
            //{
            //    //间隔时间超过0.3s，视为第二次单击
            //    if (doubleClickIntervalTime > 0.3f)
            //    {

            //        isJudgeDoubleClick = false;
            //        doubleClickIntervalTime = 0f;
            //    }
            //    else
            //    {
            //        //未超过0.3s，视为双击
                   
            //        if (DoubleLeftClick != null)
            //        {
            //            DoubleLeftClick(this, args);
            //        }
            //        isJudgeDoubleClick = false;
            //        doubleClickIntervalTime = 0f;
            //    }
            //    return;
            //}
            //else
            //{
            //    if (SingleStart != null)
            //    {
            //        SingleStart(this, args);
            //    }
            //}
        }
        ////鼠标左键弹起
        //if (Input.GetMouseButtonUp(0))
        //{
        //    Cursor.visible = true;
        //    _curArgs.touchPoint = Input.mousePosition;
        //    _curArgs.mouseButton = 0;
        //    if (SingleEnded != null)
        //    {
        //        SingleEnded(this, _curArgs);
        //    }
        //    isJudgeDoubleClick = true;
        //    doubleClickIntervalTime = 0f;
        //    return;
        //}

        ////右键按住旋转
        //if (Input.GetMouseButton(1))
        //{
        //    //pressedMouseButton = 1;
        //    if (holdTime > 0.15f)
        //    {
        //        this.holding = true;
        //        Cursor.visible = false;
        //        if (lastMousePosition != Vector3.zero)
        //        {
        //            //TouchInputEventArgs args = new TouchInputEventArgs();
        //            args.touchPoint = Input.mousePosition;
        //            args.mouseButton = 1;
        //            if (SinglePan != null)
        //            {
        //                this.deltaPan = Input.mousePosition - lastMousePosition;
        //                SinglePan(this, args);
        //            }
        //        }
        //        lastMousePosition = Input.mousePosition;
        //    }
        //    else
        //    {
        //        holdTime += Time.deltaTime;
        //    }
        //    return;
        //}
        ////右键弹起
        //if (Input.GetMouseButtonUp(1))
        //{
        //    Cursor.visible = true;
        //    //TouchInputEventArgs args = new TouchInputEventArgs();
        //    args.touchPoint = Input.mousePosition;
        //    args.mouseButton = 1;
        //    if (!holding)
        //    {
        //        if (RightMouseUp != null)
        //        {
        //            RightMouseUp(this, args);
        //        }
        //    }
        //    holding = false;
        //    holdTime = 0f;
        //    lastMousePosition = Vector3.zero;
        //    return;
        //}
        ////移动
        //if (Input.GetMouseButton(2))
        //{
        //    //TouchInputEventArgs args = new TouchInputEventArgs();
        //    args.touchPoint = Input.mousePosition;
        //    args.mouseButton = 2;
        //    if (lastMousePosition != Vector3.zero)
        //    {
        //        this.deltaPan = Input.mousePosition - lastMousePosition;
        //        if (DoublePan != null)
        //        {
        //            DoublePan(this, args);
        //        }
        //    }
        //    lastMousePosition = Input.mousePosition;
        //    return;
        //}

        ////缩放
        //if (Input.GetAxis("Mouse ScrollWheel") != 0)
        //{
        //    //TouchInputEventArgs args = new TouchInputEventArgs();
        //    args.touchPoint = Input.mousePosition;
        //    args.mouseButton = -1;
        //    this.deltaScale = Input.GetAxis("Mouse ScrollWheel") * 500;

        //    if (DoubleScale != null)
        //    {
        //        DoubleScale(this, args);
        //    }
        //    return;
        //}
        //if (Input.GetMouseButtonUp(1) || Input.GetMouseButtonUp(2))
        //{
        //    Cursor.visible = true;
        //    //TouchInputEventArgs args = new TouchInputEventArgs();
        //    args.touchPoint = Input.mousePosition;
        //    args.mouseButton = -1;
        //    this.holding = false;
        //    lastMousePosition = Vector3.zero;
        //    return;
        //}

        ////鼠标实时监测 是否有触碰到测点构件
        ////TouchInputEventArgs _Touch = new TouchInputEventArgs();
        //if (RayMonitorCast != null)
        //{
        //    args.touchPoint = Input.mousePosition;
        //    args.mouseButton = 0;
        //    RayMonitorCast(this, args);
        //}
    }


    void OnGUI()
    {
        if (IsShowArticleUi)
        {
            var point = Input.mousePosition;
            GUI.Window(0, new Rect(point.x,Screen.height-point.y, 345, 461), ArticleInfoUi, "", Skin.GetStyle("window"));
        }

    }

    private void ArticleInfoUi (int id)
    {
        if (_curObject != null)
        {
            UiArticleBox articleBox = _curObject.GetComponent<UiArticleBox>();
        }
           

        GUI.Label(new Rect(0, 0, 461, 72), "摇篮", Skin.GetStyle("label"));
        GUI.Label(new Rect(0, 72, 461, 16), "护符", Skin.GetStyle("normallable"));
        GUI.Label(new Rect(0, 88, 461, 42), "物理防御999", Skin.GetStyle("lableMax"));


        GUI.Label(new Rect(10, 130, 26, 26), Texture);

        GUI.Label(new Rect(42, 130, 461, 26), "+2 智力", Skin.GetStyle("lableblue"));
        GUI.Label(new Rect(42, 156, 461, 26), "+1 烈火学派", Skin.GetStyle("lableblue"));
        GUI.Label(new Rect(42, 182, 461, 26), "+2 大气学派", Skin.GetStyle("lableblue"));
        GUI.Label(new Rect(42, 208, 461, 26), "+1 领袖", Skin.GetStyle("lableblue"));
        GUI.Label(new Rect(42, 234, 461, 26), "+1 坚毅", Skin.GetStyle("lableblue"));

        GUI.Label(new Rect(10, 260, 26, 26), Texture);
        GUI.Label(new Rect(42, 260, 461, 26), "等级21", Skin.GetStyle("lableh"));

        GUI.Label(new Rect(10, 286, 26, 26), Texture);
        GUI.Label(new Rect(42, 286, 461, 26), "巨型火焰威能符文", Skin.GetStyle("lable1"));
        GUI.Label(new Rect(42, 312, 461, 26), "智力 + 3", Skin.GetStyle("lable1"));
        GUI.Label(new Rect(42, 338, 461, 26), "暴击率 +12%", Skin.GetStyle("lable1"));


        GUI.Label(new Rect(20, 411, 345, 50), "神圣", Skin.GetStyle("lablem"));
        GUI.Label(new Rect(280, 411, 65, 50), "9999999Y", Skin.GetStyle("lablem"));


    }


    
}
