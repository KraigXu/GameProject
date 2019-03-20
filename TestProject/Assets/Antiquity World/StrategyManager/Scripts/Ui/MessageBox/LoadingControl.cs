using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

// Token: 0x02000002 RID: 2
public  class LoadingControl : MonoBehaviour
{

    // Token: 0x04000001 RID: 1
    private GameObject _ImageObject = null;

    // Token: 0x04000002 RID: 2
    private GameObject _TextObject = null;

    // Token: 0x04000003 RID: 3
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


    // Token: 0x17000002 RID: 2
    // (set) Token: 0x06000003 RID: 3 RVA: 0x000020E9 File Offset: 0x000002E9
    public bool IsNeedFullMask
    {
        set
        {
            this._FUllMask.SetActive(value);
        }
    }

    // Token: 0x06000004 RID: 4 RVA: 0x000020FC File Offset: 0x000002FC

    private IEnumerator rotation()
    {
        for (; ; )
        {
            yield return new WaitForEndOfFrame();
            this._ImageObject.transform.Rotate(-Vector3.forward * 5f, Space.World);
        }
        yield break;
    }

    // Token: 0x06000006 RID: 6 RVA: 0x00002176 File Offset: 0x00000376
    private void OnDisable()
    {
        base.StopCoroutine("rotation");
    }

}
