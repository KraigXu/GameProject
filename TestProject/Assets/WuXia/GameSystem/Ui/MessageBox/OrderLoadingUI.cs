using UnityEngine;
using System;
using System.Collections;
using UnityEngine.UI;

public class OrderLoadingUI : LoadingControl
{
    private Scrollbar progress;
    private Text _lbTips;
    private Text _lbValue;
    void Awake()
    {

        _lbTips = transform.Find("Tips").GetComponent<Text>();
        _lbValue = transform.Find("Value").GetComponent<Text>();
        progress = transform.Find("ProgressBar").GetComponent<Scrollbar>();
    }
    void Start()
    {
       // gameObject.SetActive(false);
    }

    public override void SetValue(string tip, float value)
    {
        SetValue(value);
    }

    IEnumerator Hide()
    {
        yield return new WaitForSeconds(1.0f);
        MessageBoxInstance.Instance.DestroyLoading();
    }
    public void SetValue(float value, bool isAlwaysShow = false, string tips = "加载中...")
    {
        if (!gameObject.activeInHierarchy)
        {
            gameObject.SetActive(true);
        }

        _lbTips.text = tips;
        progress.size = value;
        _lbValue.text = string.Format("{0}%", value * 100);
        if (value == 1.0f && !isAlwaysShow)
        {
            StartCoroutine(Hide());
        }
    }
}
