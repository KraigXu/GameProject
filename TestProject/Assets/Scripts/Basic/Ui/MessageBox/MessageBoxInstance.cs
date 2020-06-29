using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using GameSystem.Ui;



public class MessageBoxInstance:MonoBehaviour
{
    public static MessageBoxInstance Instance
    {
        get { return _instance; }
    }
    public enum LoadingType
    {
        HideLoading, //隐藏窗口的加载界面
        ShowLoading   //显示窗口的加载界面
    }

    private static MessageBoxInstance _instance;
    private GameObject WaringMassageResObject = null;
    private GameObject curWaringMassageInstantiationObject = null;
    public LoadingControl _curLoadingObject = null;
    private GameObject _Canvas = null;
    public int _LoadingObjectReferenceCount = 0;
    private Dictionary<LoadingType, GameObject> _loadingPrefabMap = new Dictionary<LoadingType, GameObject>();
    private LoadingControl _hideLoading;

    void Awake()
    {
        _instance = this;
        this.WaringMassageResObject = (Resources.Load("Prefabs/MessageBoxPrefab/MessageBox") as GameObject);
        _loadingPrefabMap=new Dictionary<LoadingType, GameObject>();
        _loadingPrefabMap.Add(LoadingType.HideLoading, (Resources.Load("Prefabs/MessageBoxPrefab/LoadingObject") as GameObject));
        _loadingPrefabMap.Add(LoadingType.ShowLoading,(Resources.Load("Prefabs/MessageBoxPrefab/OrderLoading") as GameObject));
    }

    
    public MessageBoxEventsUI MessageBoxShow(string Content = "", MessageBoxType ElementType = MessageBoxType.Composite_OkAndCANCEL)
    {
        Debug.Log(Content);
        bool flag = this.curWaringMassageInstantiationObject == null;
        if (flag)
        {
            this._Canvas = GameObject.Find("Canvas");
            bool flag2 = this.WaringMassageResObject == null;
            if (flag2)
            {
                this.WaringMassageResObject = (Resources.Load("Prefabs/MessageBoxPrefab/MessageBox") as GameObject);
            }
            this.curWaringMassageInstantiationObject = this.AddChild(this._Canvas, this.WaringMassageResObject);
            RectTransform component = this.curWaringMassageInstantiationObject.GetComponent<RectTransform>();
            component.offsetMax = Vector2.zero;
            component.offsetMin = Vector2.zero;
            MessageBoxEventsUI component2 = this.curWaringMassageInstantiationObject.GetComponent<MessageBoxEventsUI>();
            this.curWaringMassageInstantiationObject.transform.position = Vector3.zero;
            component2.mMessageBoxType = ElementType;
            bool flag3 = !string.IsNullOrEmpty(Content);
            if (flag3)
            {
                component2._tipstr = Content;
            }
            component2.Show();
            this.WaringMassageResObject = null;
        }
        return this.curWaringMassageInstantiationObject.GetComponent<MessageBoxEventsUI>();
    }

    
    public LoadingControl NewLoadingBox(string tip,LoadingType type=LoadingType.ShowLoading)
    {
        this._LoadingObjectReferenceCount++;
        bool flag = this._curLoadingObject == null;
        if (flag)
        {
            this._Canvas = GameObject.Find("Canvas");
            GameObject gameObject = this.AddChild(this._Canvas,_loadingPrefabMap[type]);
            this.SetRectTransformAnchoredAndOffset(gameObject, Vector2.zero, Vector2.zero, Vector2.zero);
            this._curLoadingObject = gameObject.GetComponent<LoadingControl>();
        }
        this._curLoadingObject.SetValue(tip, 0);
        return this._curLoadingObject;
    }

   /// <summary>
   /// 设置当前Loading显示值,如果没有则不显示数据
   /// </summary>
   /// <param name="tip"></param>
   /// <param name="IsNeedFullScreenMark"></param>
   /// <returns></returns>
    public LoadingControl ChangeLoadingBoxState(string tip, bool IsNeedFullScreenMark = false)
    {
       if (_curLoadingObject == null)
       {
           return _curLoadingObject;
       }
       else
       {
           this._curLoadingObject.SetValue(tip,-1);
           return _curLoadingObject;
       }
    }

    public LoadingControl NewHideLoading(string tips)
    {
        if (_hideLoading == null)
        {
            this._Canvas = GameObject.Find("Canvas");
            GameObject go = UGUITools.AddChild(_Canvas.gameObject, _loadingPrefabMap[LoadingType.HideLoading]);
            _hideLoading = go.GetComponent<LoadingControl>();
        }
        this._hideLoading.SetValue(tips, 0);
        return _hideLoading;
    }

    public void DestroyHideLoading()
    {
        if (_hideLoading != null)
        {
            Destroy(_hideLoading.gameObject);
        }
    }

    public bool IsInstanceLoadingBox()
    {
        return this._curLoadingObject != null;
    }
    public void DestroyLoading()
    {
        this._LoadingObjectReferenceCount--;
        bool flag = this._curLoadingObject != null && this._LoadingObjectReferenceCount <= 0;
        if (flag)
        {
            this._LoadingObjectReferenceCount = 0;
            Destroy(this._curLoadingObject.gameObject);
            this._curLoadingObject = null;
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



    /// <summary>
    /// 隐藏Loading界面
    /// </summary>
    /// <returns></returns>
    IEnumerator HideLoading()
    {
        SetLoadMask();
        yield return new WaitForSeconds(0.5f);
        if (_curLoadingObject != null)
        {
            DestroyLoading();
        }

    }
   

    void SetLoadMask()
    {
        //sprMask.color = new Color(116 / 255f, 116 / 255f, 116 / 255f, 166 / 255f);
    }


    public void ImprovePriority(int Priority)
    {
        this._LoadingObjectReferenceCount += Priority;
    }

    public void LowerPriority(int Priority)
    {
        this._LoadingObjectReferenceCount -= Priority;
    }

    private GameObject AddChild(GameObject parent, GameObject prefab)
    {
        GameObject gameObject = UnityEngine.Object.Instantiate<GameObject>(prefab);
        bool flag = gameObject != null && parent != null;
        if (flag)
        {
            Transform transform = gameObject.transform;
            transform.SetParent(parent.transform);
            transform.localPosition = Vector3.zero;
            transform.localRotation = Quaternion.identity;
            transform.localScale = Vector3.one;
            gameObject.layer = parent.layer;
        }
        return gameObject;
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
