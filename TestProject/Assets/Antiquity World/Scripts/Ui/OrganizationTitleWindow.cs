using System.Collections;
using System.Collections.Generic;
using DataAccessObject;
using GameSystem;
using GameSystem.Ui;
using UnityEngine;

/// <summary>
/// 标题窗口
/// </summary>
public class OrganizationTitleWindow : UIWindowBase
{
    [SerializeField]
    private List<UiOrganizationTitle> _titles = new List<UiOrganizationTitle>();

    [SerializeField]
    private List<Sprite> _types = new List<Sprite>();

    protected override void InitWindowData()
    {
        this.ID = WindowID.OrganizationTitleWindow;

        windowData.windowType = UIWindowType.BackgroundLayer;
        windowData.showMode = UIWindowShowMode.DoNothing;
        windowData.navigationMode = UIWindowNavigationMode.IgnoreNavigation;
        windowData.colliderMode = UIWindowColliderMode.None;
        windowData.closeModel = UIWindowCloseModel.Destory;
    }

    public override void InitWindowOnAwake()
    {
    }


    public void Change(LivingArea livingArea, FactionProperty property, HexCell cell)
    {
        bool flag = false;
        int index;
        for (index = 0; index < _titles.Count; index++)
        {
            if (_titles[index].Id == livingArea.Id)
            {
                flag = true;
                break;
            }
        }

        if (flag)  //说明已有
        {
            var item = _titles[index];
            
        }
        else   //没有
        {
            var data = SQLService.Instance.QueryUnique<LivingAreaData>(" Id=? ", livingArea.Id);
            RectTransform titleRect = WXPoolManager.Pools[Define.GeneratedPool].Spawn(StrategyAssetManager.UiOrganizationTitle, transform);
            titleRect.localScale = Vector3.zero;

            UiOrganizationTitle titleItem = titleRect.gameObject.GetComponent<UiOrganizationTitle>();
            titleItem.Id = livingArea.Id;
            titleItem.TitleTxt.text = data.Name;
            titleItem.TypeImg.sprite = _types[data.LivingAreaType];
            titleItem.Target = cell.transform;
            
            _titles.Add(titleItem);

        }
    }

    void Update()
    {

        for (int i = 0; i < _titles.Count; i++)
        {

            Vector3 point = _titles[i].Target.position + new Vector3(0, 3, 0);
            if (Define.IsAPointInACamera(Camera.main, point))
            {
                Vector2 tempPos = Camera.main.WorldToScreenPoint(point);
                _titles[i].Rect.position = tempPos;
                _titles[i].Rect.localScale = Vector3.one;

            }
            else
            {
                _titles[i].Rect.localScale = Vector3.zero;
            }
        }
    }




}
