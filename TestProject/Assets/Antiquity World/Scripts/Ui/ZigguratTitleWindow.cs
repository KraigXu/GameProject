using System.Collections;
using System.Collections.Generic;

using GameSystem;
using GameSystem.Ui;
using UnityEngine;

public class ZigguratTitleWindow : UIWindowBase
{
    //Sytle Info Data
    [SerializeField]
    private List<UiCityTitleItem> _titles = new List<UiCityTitleItem>();

    [SerializeField]
    private List<Sprite> _types = new List<Sprite>();

    [SerializeField]
    private List<Sprite> _relations = new List<Sprite>();

    protected override void InitWindowData()
    {
        this.ID = WindowID.ZigguratTitleWindow;

        windowData.windowType = UIWindowType.BackgroundLayer;
        windowData.showMode = UIWindowShowMode.DoNothing;
        windowData.navigationMode = UIWindowNavigationMode.IgnoreNavigation;
        windowData.colliderMode = UIWindowColliderMode.None;
        windowData.closeModel = UIWindowCloseModel.Destory;
    }

    public override void InitWindowOnAwake()
    {
    }


    public void Change(LivingArea livingArea, Ziggurat ziggurat, HexCell cell)
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

            //item.TyepImg.sprite = _types[city.Type];
            //item.RelationImg.sprite = _types[city.Type];
            //item.Id = 10;
            //item.RelationImg.sprite=_types[]
            //item._usedImag.sprite = _types[city.UniqueCode];
            //item
        }
        else   //没有
        {
            var data = SQLService.Instance.QueryUnique<LivingAreaData>(" Id=? ", livingArea.Id);
            RectTransform titleRect = WXPoolManager.Pools[Define.GeneratedPool].Spawn(StrategyAssetManager.UiCityTitle, transform);
            titleRect.localScale = Vector3.zero;

            UiCityTitleItem titleItem = titleRect.gameObject.GetComponent<UiCityTitleItem>();
            titleItem.Id = livingArea.Id;
            titleItem.NameTxt.text = data.Name;
            titleItem.TyepImg.sprite = _types[data.LivingAreaType];
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
                _titles[i].RectTF.position = tempPos;
                _titles[i].RectTF.localScale = Vector3.one;

            }
            else
            {
                _titles[i].RectTF.localScale = Vector3.zero;
            }
        }
    }
}
