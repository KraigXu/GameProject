using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


public  class LoadingControl : MonoBehaviour
{

    
    private GameObject _ImageObject = null;

    
    private GameObject _TextObject = null;

    
    public GameObject _FUllMask = null;

    public Text lbPercent;
    public Text lbTips;
    public RawImage sprMask;
    private void Awake()
    {
        this._ImageObject = base.transform.Find("Image").gameObject;
        this._TextObject = base.transform.Find("Text").gameObject;
        this.lbPercent = base.transform.Find("Percentage").gameObject.GetComponent<Text>();
        base.StartCoroutine("rotation");
    }
    public virtual void SetValue(string tip,float value=0f)
    {
        if(_TextObject==null)
            this._TextObject = base.transform.Find("Text").gameObject;

        if (value != -1f)
        {
            lbPercent.text = (value*100).ToString();
        }

        _TextObject.GetComponent<Text>().text = tip;

    }


    
    
    public bool IsNeedFullMask
    {
        set
        {
            this._FUllMask.SetActive(value);
        }
    }

    

    private IEnumerator rotation()
    {
        for (; ; )
        {
            yield return new WaitForEndOfFrame();
            this._ImageObject.transform.Rotate(-Vector3.forward * 5f, Space.World);
        }
        yield break;
    }

    
    private void OnDisable()
    {
        base.StopCoroutine("rotation");
    }

}
