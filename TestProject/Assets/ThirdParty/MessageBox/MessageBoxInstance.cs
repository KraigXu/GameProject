using System;
using UnityEngine;
using WX.Ui;

/// <summary>
/// 消息框显示
/// </summary>
public class MessageBoxInstance
{
    public delegate void MessageBoxSelect(bool flag);

    private static MessageBoxInstance _instance;
    public static MessageBoxInstance Instance
    {
        get
        {
            if (_instance == null)
                _instance = new MessageBoxInstance();
            return _instance;
        }
    }

   // private LoadingControl
    private static MessageBoxInstance instance;
    public LoadingControl _curLoadingObject = null;
    
    private GameObject WaringMassageResObject = null;
    private GameObject curWaringMassageInstantiationObject = null;
    private GameObject LoadingResObject = null;
    private GameObject CacheBoxPrefab;

    private GameObject _canvas = null;
    private GameObject _waringMessageBox;

    private GameObject _currentCacheBox;
    private int _LoadingObjectReferenceCount = 0;

    public bool IsInstanceLoadingBox
    {
        get
        {
            return this._curLoadingObject != null;
        }
    }

    private MessageBoxInstance()
    {
        _waringMessageBox = Resources.Load("UIPrefab/MessageBoxPrefab/MessageBox") as GameObject;
        this.WaringMassageResObject = (Resources.Load("UIPrefab/MessageBoxPrefab/MessageBox") as GameObject);
        this.LoadingResObject = (Resources.Load("UIPrefab/MessageBoxPrefab/LoadingObject") as GameObject);
        this.CacheBoxPrefab = (Resources.Load("UIPrefab/MessageBoxPrefab/CacheBox") as GameObject);
        _canvas = GameObject.Find("Canvas");
    }

    public void MessageBoxShow(string content, MessageBoxType type, MessageBoxSelect selectMain)
    {
        GameObject messageBox= UGUITools.AddChild(_waringMessageBox, _canvas);
        RectTransform rect = messageBox.GetComponent<RectTransform>();
        rect.offsetMax=Vector2.zero;
        rect.offsetMin=Vector2.zero;

        MessageBoxEventsUI eventsUi = messageBox.GetComponent<MessageBoxEventsUI>();
        eventsUi.transform.position=Vector3.zero;
        eventsUi.mMessageBoxType = type;
    }

    /// <summary>
    /// 显示消息框
    /// </summary>
    /// <param name="content"></param>
    /// <param name="type"></param>
    public void MessageBoxShow(string content, MessageBoxType type=MessageBoxType.Simple_OK)
    {
        if (this.curWaringMassageInstantiationObject == null)
        {
            if (this.WaringMassageResObject == null)
            {
                this.WaringMassageResObject = (Resources.Load("UIPrefab/MessageBoxPrefab/MessageBox") as GameObject);
            }

            this.curWaringMassageInstantiationObject = UGUITools.AddChild(WaringMassageResObject,_canvas );
            RectTransform component = this.curWaringMassageInstantiationObject.GetComponent<RectTransform>();
            component.offsetMax = Vector2.zero;
            component.offsetMin = Vector2.zero;
            MessageBoxEventsUI component2 = this.curWaringMassageInstantiationObject.GetComponent<MessageBoxEventsUI>();
            this.curWaringMassageInstantiationObject.transform.position = Vector3.zero;
            component2.mMessageBoxType = type;
            if (!string.IsNullOrEmpty(content))
            {
                component2._tipstr = content;
            }
            component2.Show();
            this.WaringMassageResObject = null;
        }
    }

    /// <summary>
    /// 显示加载框
    /// </summary>
    /// <param name="value"></param>
    /// <param name="type"></param>
    public void NewLoadingBox(float value = 0f, LoadingBoxType type = LoadingBoxType.FullViewLoading)
    {
        if (this._curLoadingObject == null)
        {
            GameObject go = UGUITools.AddChild(_canvas,LoadingResObject );
            SetRectTransformAnchoredAndOffset(go, Vector2.zero, Vector2.zero, Vector2.zero);
            _curLoadingObject = go.GetComponent<LoadingControl>();
        }
        this._curLoadingObject.SetValue(value,type);
        this._curLoadingObject.SetValue(value,type);
    }

    /// <summary>
    /// 更新加载框
    /// </summary>
    /// <param name="value"></param>
    /// <param name="type"></param>
    public void ChangeLoadingBoxState(float value, LoadingBoxType type = LoadingBoxType.FullViewLoading)
    {
        if (this._curLoadingObject == null)
            return;

        this._curLoadingObject.SetValue(value, type);
    }

    /// <summary>
    /// 删除加载框
    /// </summary>
    public void DestroyLoading()
    {
        this._LoadingObjectReferenceCount--;
        bool flag = this._curLoadingObject != null && this._LoadingObjectReferenceCount <= 0;
        if (flag)
        {
            this._LoadingObjectReferenceCount = 0;
            UnityEngine.Object.Destroy(this._curLoadingObject.gameObject);
        }
    }


    public void NewCacheBox()
    {
        if (_currentCacheBox == null)
        {
            _currentCacheBox= UGUITools.AddChild(_canvas, CacheBoxPrefab);
        }

    }

    public void DestoryCacheBox()
    {
        if (_currentCacheBox != null)
        {
            UnityEngine.Object.Destroy(_currentCacheBox);
        }

    }

    public void ImmediatelyDestroyLoading()
    {
        bool flag = this._curLoadingObject != null;
        if (flag)
        {
            this._LoadingObjectReferenceCount = 0;
            UnityEngine.Object.DestroyImmediate(this._curLoadingObject.gameObject);
        }
    }

    public void ImprovePriority(int Priority)
    {
        this._LoadingObjectReferenceCount += Priority;
    }

    public void LowerPriority(int Priority)
    {
        this._LoadingObjectReferenceCount -= Priority;
    }


    private RectTransform SetRectTransformAnchoredAndOffset(GameObject go, Vector2 Anchoredpos, Vector2 min, Vector2 max)
    {
        RectTransform component = go.GetComponent<RectTransform>();
        component.offsetMin = min;
        component.offsetMax = max;
        component.anchoredPosition = Anchoredpos;
        return component;
    }

}
