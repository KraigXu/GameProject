using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// Token: 0x02000003 RID: 3
public class MessageBoxEventsUI : MonoBehaviour
{
    // Token: 0x17000003 RID: 3
    // (set) Token: 0x06000008 RID: 8 RVA: 0x000021A3 File Offset: 0x000003A3
    [HideInInspector]
    public string _tipstr
    {
        set
        {
            this._TipTextObject = base.transform.Find("Image/TipText").gameObject;
            this._TipTextObject.GetComponent<Text>().text = value;
        }
    }

    // Token: 0x06000009 RID: 9 RVA: 0x000021D4 File Offset: 0x000003D4
    private void Awake()
    {
        this._recttransform = base.transform.GetComponent<RectTransform>();
        this._SureButton = base.transform.Find("Image/SureButton").gameObject;
        this._CancelButton = base.transform.Find("Image/CancelButton").gameObject;
        this._SureButton.GetComponent<Button>().onClick.AddListener(new UnityAction(this.OnSureClick));
        this._CancelButton.GetComponent<Button>().onClick.AddListener(new UnityAction(this.OnCancelClick));
    }

    // Token: 0x0600000A RID: 10 RVA: 0x00002270 File Offset: 0x00000470
    public MessageBoxEventsUI Show()
    {
        switch (this.mMessageBoxType)
        {
            case MessageBoxType.Simple_OK:
                {
                    RectTransform component = this._SureButton.GetComponent<RectTransform>();
                    component.anchoredPosition = new Vector2(0f, component.anchoredPosition.y);
                    this._CancelButton.SetActive(false);
                    break;
                }
            case MessageBoxType.Simple_CANCEL:
                {
                    RectTransform component2 = this._CancelButton.GetComponent<RectTransform>();
                    component2.anchoredPosition = new Vector2(0f, component2.anchoredPosition.y);
                    this._SureButton.SetActive(false);
                    break;
                }
            case MessageBoxType.Composite_OkAndCANCEL:
                this._SureButton.SetActive(true);
                this._CancelButton.SetActive(true);
                break;
        }
        return this;
    }

    // Token: 0x0600000B RID: 11 RVA: 0x00002334 File Offset: 0x00000534
    private void OnSureClick()
    {
        bool flag = this._SureClick != null;
        if (flag)
        {
            this._SureClick();
        }
        UnityEngine.Object.Destroy(base.gameObject);
    }

    // Token: 0x0600000C RID: 12 RVA: 0x0000236C File Offset: 0x0000056C
    private void OnCancelClick()
    {
        bool flag = this._CancelClick != null;
        if (flag)
        {
            this._CancelClick();
        }
        UnityEngine.Object.Destroy(base.gameObject);
    }

    // Token: 0x0600000D RID: 13 RVA: 0x000023A4 File Offset: 0x000005A4
    private void Update()
    {
        Vector2 offsetMin = this._recttransform.offsetMin;
        Vector2 offsetMax = this._recttransform.offsetMax;
        bool flag = offsetMax != Vector2.zero || offsetMin != Vector2.zero;
        if (flag)
        {
            this._recttransform.offsetMin = Vector2.zero;
            this._recttransform.offsetMax = Vector2.zero;
        }
    }

    // Token: 0x04000004 RID: 4
    public MessageBoxEventsUI.MessageBoxSimpleClick _SureClick = null;

    // Token: 0x04000005 RID: 5
    public MessageBoxEventsUI.MessageBoxSimpleClick _CancelClick = null;

    // Token: 0x04000006 RID: 6
    private GameObject _TipTextObject = null;

    // Token: 0x04000007 RID: 7
    private GameObject _SureButton = null;

    // Token: 0x04000008 RID: 8
    private GameObject _CancelButton = null;

    // Token: 0x04000009 RID: 9
    private RectTransform _recttransform;

    // Token: 0x0400000A RID: 10
    public MessageBoxType mMessageBoxType = MessageBoxType.Composite_OkAndCANCEL;

    // Token: 0x02000007 RID: 7
    // (Invoke) Token: 0x06000022 RID: 34
    public delegate void MessageBoxSimpleClick();
}
