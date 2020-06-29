using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;


public class MessageBoxEventsUI : MonoBehaviour
{
    
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

    
    private void Awake()
    {
        this._recttransform = base.transform.GetComponent<RectTransform>();
        this._SureButton = base.transform.Find("Image/SureButton").gameObject;
        this._CancelButton = base.transform.Find("Image/CancelButton").gameObject;
        this._SureButton.GetComponent<Button>().onClick.AddListener(new UnityAction(this.OnSureClick));
        this._CancelButton.GetComponent<Button>().onClick.AddListener(new UnityAction(this.OnCancelClick));
    }

    
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

    
    private void OnSureClick()
    {
        bool flag = this._SureClick != null;
        if (flag)
        {
            this._SureClick();
        }
        UnityEngine.Object.Destroy(base.gameObject);
    }

    
    private void OnCancelClick()
    {
        bool flag = this._CancelClick != null;
        if (flag)
        {
            this._CancelClick();
        }
        UnityEngine.Object.Destroy(base.gameObject);
    }

    
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

    
    public MessageBoxEventsUI.MessageBoxSimpleClick _SureClick = null;

    
    public MessageBoxEventsUI.MessageBoxSimpleClick _CancelClick = null;

    
    private GameObject _TipTextObject = null;

    
    private GameObject _SureButton = null;

    
    private GameObject _CancelButton = null;

    
    private RectTransform _recttransform;

    
    public MessageBoxType mMessageBoxType = MessageBoxType.Composite_OkAndCANCEL;

    
    // (Invoke) Token: 0x06000022 RID: 34
    public delegate void MessageBoxSimpleClick();
}
