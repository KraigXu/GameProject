using System.Collections;
using System.Collections.Generic;
using DataAccessObject;
using DG.Tweening;
using GameSystem.Ui;
using UnityEngine;
using UnityEngine.UI;

public class LoadingView : MonoBehaviour
{

    public delegate void OnLoadingOverCallback();

    public OnLoadingOverCallback Callback;
    private GameObject _fullMask;
    private GameObject _icon;
    private Sequence _sequence;
    private Slider _slider;

    //Tips
    private Text _tipsTitle;
    private Text _tipsContent;
    private int _currentPage = 0;
    private int _maxPage = 0;

    public Text loadingText;

    private float loadingSpeed = 1;

    private float targetValue;

    private AsyncOperation operation;

    private List<TipsData> _tipsDatas;
    [Range(0.00f, 1.00f)]
    public float value;


    void Awake()
    {
        this._fullMask = transform.Find("FullMake").gameObject;

        this._icon = transform.Find("Icon").gameObject;
        _sequence = DOTween.Sequence();
        _sequence.Append(_icon.transform.DORotate(new Vector3(0, 0, 180), 2));
        _sequence.Append(_icon.transform.DOScale(new Vector3(0.5f, 0.5f, 1f), 1));
        _sequence.Append(_icon.transform.DORotate(new Vector3(0, 0, 360), 2));
        _sequence.Append(_icon.transform.DOScale(new Vector3(1f, 1f, 1f), 1));
        _sequence.SetLoops(-1);

        _slider = transform.Find("Slider").GetComponent<Slider>();
        _tipsTitle = transform.Find("TipsTitle").GetComponent<Text>();
        _tipsContent = transform.Find("TipsContent").GetComponent<Text>();
        transform.Find("TipsUp").GetComponent<Button>().onClick.AddListener(ButtonUp);
        transform.Find("TipsDown").GetComponent<Button>().onClick.AddListener(ButtonDown);

        //取值
        _tipsDatas = SQLService.Instance.SimpleQuery<TipsData>(" TipsType=? ", new object[] { "1" });
        _maxPage = _tipsDatas.Count;
        _tipsTitle.text = _currentPage + "/" + _maxPage;

        _tipsContent.text = _tipsDatas[_currentPage].Content;
        _maxPage = _tipsDatas.Count;
        _tipsTitle.text = _tipsDatas[_currentPage].ContentTitle;
        _tipsContent.text = _tipsDatas[_currentPage].Content;

    }


    void Update()
    {
        Define.LoadingValue = value;
    }

    public void Open()
    {
        gameObject.SetActive(true);
    }

    public void Close()
    {
        gameObject.SetActive(false);
    }


    void LateUpdate()
    {
        _slider.value = Define.LoadingValue;

        if (_slider.value >= 1)
        {
            gameObject.SetActive(false);

            if (Callback != null)
            {
                Callback();
            }

           GameObject.Destroy(this.gameObject);

        }
    }

    public float Schedule
    {
        get { return _slider.value; }
        set
        {
            if (!_slider)
                _slider = transform.Find("Slider").GetComponent<Slider>();

            _slider.value = value;
        }
    }
    /// <summary>
    /// 翻页
    /// </summary>
    private void ButtonUp()
    {
        _currentPage = _currentPage == 0 ? _maxPage - 1 : _currentPage - 1;
        ChangeTips();
    }

    /// <summary>
    /// 下页
    /// </summary>
    private void ButtonDown()
    {
        _currentPage = _currentPage == _maxPage - 1 ? 0 : _currentPage + 1;
        ChangeTips();
    }

    private void ChangeTips()
    {
        _tipsTitle.text = _tipsDatas[_currentPage].ContentTitle;
        _tipsContent.text = _tipsDatas[_currentPage].Content;
    }
    public bool IsNeedFullMask
    {
        set
        {
            this._fullMask.SetActive(value);
        }
    }
    private void OnDisable()
    {
        base.StopCoroutine("rotation");
    }

    void OnDestroy()
    {
        _sequence.Kill();
    }

    //public bool IsInstanceLoadingBox
    //{
    //    get
    //    {
    //     //   return this._curLoadingObject != null;
    //    }
    //}

    //private MessageBoxInstance()
    //{
    //    _waringMessageBox = Resources.Load("UIPrefab/MessageBoxPrefab/MessageBox") as GameObject;
    //    this.WaringMassageResObject = (Resources.Load("UIPrefab/MessageBoxPrefab/MessageBox") as GameObject);
    //    this.LoadingResObject = (Resources.Load("UIPrefab/MessageBoxPrefab/LoadingObject") as GameObject);
    //    this.CacheBoxPrefab = (Resources.Load("UIPrefab/MessageBoxPrefab/CacheBox") as GameObject);
    //    _canvas = GameObject.Find("Canvas");
    //}

    //public void MessageBoxShow(string content, MessageBoxType type, MessageBoxSelect selectMain)
    //{
    //    GameObject messageBox = UGUITools.AddChild(_waringMessageBox, _canvas);
    //    RectTransform rect = messageBox.GetComponent<RectTransform>();
    //    rect.offsetMax = Vector2.zero;
    //    rect.offsetMin = Vector2.zero;

    //    MessageBoxEventsUI eventsUi = messageBox.GetComponent<MessageBoxEventsUI>();
    //    eventsUi.transform.position = Vector3.zero;
    //    eventsUi.mMessageBoxType = type;
    //}

    ///// <summary>
    ///// 显示消息框
    ///// </summary>
    ///// <param name="content"></param>
    ///// <param name="type"></param>
    //public void MessageBoxShow(string content, MessageBoxType type = MessageBoxType.Simple_OK)
    //{
    //    if (this.curWaringMassageInstantiationObject == null)
    //    {
    //        if (this.WaringMassageResObject == null)
    //        {
    //            this.WaringMassageResObject = (Resources.Load("UIPrefab/MessageBoxPrefab/MessageBox") as GameObject);
    //        }

    //        this.curWaringMassageInstantiationObject = UGUITools.AddChild(WaringMassageResObject, _canvas);
    //        RectTransform component = this.curWaringMassageInstantiationObject.GetComponent<RectTransform>();
    //        component.offsetMax = Vector2.zero;
    //        component.offsetMin = Vector2.zero;
    //        MessageBoxEventsUI component2 = this.curWaringMassageInstantiationObject.GetComponent<MessageBoxEventsUI>();
    //        this.curWaringMassageInstantiationObject.transform.position = Vector3.zero;
    //        component2.mMessageBoxType = type;
    //        if (!string.IsNullOrEmpty(content))
    //        {
    //            component2._tipstr = content;
    //        }
    //        component2.Show();
    //        this.WaringMassageResObject = null;
    //    }
    //}



    ///// <summary>
    ///// 删除加载框
    ///// </summary>
    //public void DestroyLoading()
    //{
    //    this._LoadingObjectReferenceCount--;
    //    bool flag = this._curLoadingObject != null && this._LoadingObjectReferenceCount <= 0;
    //    if (flag)
    //    {
    //        this._LoadingObjectReferenceCount = 0;
    //        UnityEngine.Object.Destroy(this._curLoadingObject.gameObject);
    //    }
    //}


    //public void NewCacheBox()
    //{
    //    if (_currentCacheBox == null)
    //    {
    //        _currentCacheBox = UGUITools.AddChild(_canvas, CacheBoxPrefab);
    //    }

    //}

    //public void DestoryCacheBox()
    //{
    //    if (_currentCacheBox != null)
    //    {
    //        UnityEngine.Object.Destroy(_currentCacheBox);
    //    }

    //}

    //public void ImmediatelyDestroyLoading()
    //{
    //    bool flag = this._curLoadingObject != null;
    //    if (flag)
    //    {
    //        this._LoadingObjectReferenceCount = 0;
    //        UnityEngine.Object.DestroyImmediate(this._curLoadingObject.gameObject);
    //    }
    //}

    //public void ImprovePriority(int Priority)
    //{
    //    this._LoadingObjectReferenceCount += Priority;
    //}

    //public void LowerPriority(int Priority)
    //{
    //    this._LoadingObjectReferenceCount -= Priority;
    //}


    //private RectTransform SetRectTransformAnchoredAndOffset(GameObject go, Vector2 Anchoredpos, Vector2 min, Vector2 max)
    //{
    //    RectTransform component = go.GetComponent<RectTransform>();
    //    component.offsetMin = min;
    //    component.offsetMax = max;
    //    component.anchoredPosition = Anchoredpos;
    //    return component;
    //}

}


//public class MessageBoxEventsUI : MonoBehaviour
//{
//    // Token: 0x04000004 RID: 4
//    public MessageBoxEventsUI.MessageBoxSimpleClick _SureClick = null;

//    // Token: 0x04000005 RID: 5
//    public MessageBoxEventsUI.MessageBoxSimpleClick _CancelClick = null;

//    // Token: 0x04000006 RID: 6
//    private GameObject _TipTextObject = null;

//    // Token: 0x04000007 RID: 7
//    private GameObject _SureButton = null;

//    // Token: 0x04000008 RID: 8
//    private GameObject _CancelButton = null;

//    // Token: 0x04000009 RID: 9
//    private RectTransform _recttransform;

//    // Token: 0x0400000A RID: 10
//    public MessageBoxType mMessageBoxType = MessageBoxType.Composite_OkAndCANCEL;

//    // Token: 0x02000007 RID: 7
//    // (Invoke) Token: 0x06000022 RID: 34
//    public delegate void MessageBoxSimpleClick();

//    // Token: 0x17000003 RID: 3
//    // (set) Token: 0x06000008 RID: 8 RVA: 0x000021A3 File Offset: 0x000003A3
//    [HideInInspector]
//    public string _tipstr
//    {
//        set
//        {
//            this._TipTextObject = base.transform.Find("Image/TipText").gameObject;
//            this._TipTextObject.GetComponent<Text>().text = value;
//        }
//    }

//    // Token: 0x06000009 RID: 9 RVA: 0x000021D4 File Offset: 0x000003D4
//    private void Awake()
//    {
//        this._recttransform = base.transform.GetComponent<RectTransform>();
//        this._SureButton = base.transform.Find("Image/SureButton").gameObject;
//        this._CancelButton = base.transform.Find("Image/CancelButton").gameObject;
//        this._SureButton.GetComponent<Button>().onClick.AddListener(new UnityAction(this.OnSureClick));
//        this._CancelButton.GetComponent<Button>().onClick.AddListener(new UnityAction(this.OnCancelClick));
//    }

//    // Token: 0x0600000A RID: 10 RVA: 0x00002270 File Offset: 0x00000470
//    public MessageBoxEventsUI Show()
//    {
//        switch (this.mMessageBoxType)
//        {
//            case MessageBoxType.Simple_OK:
//                {
//                    RectTransform component = this._SureButton.GetComponent<RectTransform>();
//                    component.anchoredPosition = new Vector2(0f, component.anchoredPosition.y);
//                    this._CancelButton.SetActive(false);
//                    break;
//                }
//            case MessageBoxType.Simple_CANCEL:
//                {
//                    RectTransform component2 = this._CancelButton.GetComponent<RectTransform>();
//                    component2.anchoredPosition = new Vector2(0f, component2.anchoredPosition.y);
//                    this._SureButton.SetActive(false);
//                    break;
//                }
//            case MessageBoxType.Composite_OkAndCANCEL:
//                this._SureButton.SetActive(true);
//                this._CancelButton.SetActive(true);
//                break;
//        }
//        return this;
//    }

//    // Token: 0x0600000B RID: 11 RVA: 0x00002334 File Offset: 0x00000534
//    private void OnSureClick()
//    {
//        bool flag = this._SureClick != null;
//        if (flag)
//        {
//            this._SureClick();
//        }
//        UnityEngine.Object.Destroy(base.gameObject);
//    }

//    // Token: 0x0600000C RID: 12 RVA: 0x0000236C File Offset: 0x0000056C
//    private void OnCancelClick()
//    {
//        bool flag = this._CancelClick != null;
//        if (flag)
//        {
//            this._CancelClick();
//        }
//        UnityEngine.Object.Destroy(base.gameObject);
//    }

//    // Token: 0x0600000D RID: 13 RVA: 0x000023A4 File Offset: 0x000005A4
//    private void Update()
//    {
//        Vector2 offsetMin = this._recttransform.offsetMin;
//        Vector2 offsetMax = this._recttransform.offsetMax;
//        bool flag = offsetMax != Vector2.zero || offsetMin != Vector2.zero;
//        if (flag)
//        {
//            this._recttransform.offsetMin = Vector2.zero;
//            this._recttransform.offsetMax = Vector2.zero;
//        }
//    }


//}
