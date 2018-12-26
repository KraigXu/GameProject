using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;
using DataAccessObject;

/// <summary>
/// 附带内容的加载界面
/// </summary>
public class LoadingControl : MonoBehaviour
{
    private GameObject _fullMask;
    private GameObject _icon;
    private Sequence _sequence;
    private Slider _slider;
    //Tips
    private Text _tipsTitle;
    private Text _tipsContent;
    [SerializeField]
    private int _currentPage = 0;
    [SerializeField] private int _maxPage = 0;
    private string _tips;
    private List<TipsData> _tipsLoading;

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

    }

    void Start()
    {
        _tipsLoading = SqlData.GetWhereDatas<TipsData>(" TipsType=? ", new object[] {"Loading"});
        _maxPage = _tipsLoading.Count;
        _tipsTitle.text = _tipsLoading[_currentPage].ContentTitle;
        _tipsContent.text = _tipsLoading[_currentPage].Content;
    }


    public void SetValue(float value, LoadingBoxType type)
    {
        switch (type)
        {
            case LoadingBoxType.FullViewLoading:
                break;
            case LoadingBoxType.LocalLoading:
                break;
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
        _currentPage = _currentPage == 0 ? _maxPage - 1 : _currentPage-1;
        ChangeTips();
    }

    /// <summary>
    /// 下页
    /// </summary>
    private void ButtonDown()
    {
        _currentPage = _currentPage == _maxPage - 1 ? 0 : _currentPage+1;
        ChangeTips();
    }

    private void ChangeTips()
    {
        _tipsTitle.text = _tipsLoading[_currentPage].ContentTitle;
        _tipsContent.text = _tipsLoading[_currentPage].Content;
    }

    //public string Tipstr
    //{
    //    get
    //    {
    //        bool flag = this._TextObject == null;
    //        if (flag)
    //        {
    //            this._TextObject = base.transform.Find("Text").gameObject;
    //        }
    //        return this._TextObject.GetComponent<Text>().text;
    //    }
    //    set
    //    {
    //        bool flag = this._TextObject == null;
    //        if (flag)
    //        {
    //            this._TextObject = base.transform.Find("Text").gameObject;
    //        }
    //        this._TextObject.GetComponent<Text>().text = value;
    //    }
    //}

    public string Tipstr
    {
        get { return null; }
        set { }
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


}
