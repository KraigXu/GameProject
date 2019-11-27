using System.Collections;
using System.Collections.Generic;
using System.Text;
using DataAccessObject;
using Unity.Entities;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace GameSystem.Ui
{
    /// <summary>
    /// 显示鼠标提示信息
    /// </summary>
    public class TipsWindow : UIWindowBase
    {
        [SerializeField]
        private RectTransform _textPrefab;
        [SerializeField]
        private RectTransform _pointTf;
        [SerializeField]
        private RectTransform _contentParent;

        [SerializeField]
        private RectTransform _mouseView;
        [SerializeField]
        private Text _contenText;

        



        private Vector3 _wordpos = Vector3.zero;
        private Camera _camera3D;
        private Camera _camera2D;
        private TipsInfoWindowData _infodata;
        private List<RectTransform> _initItem = new List<RectTransform>();
        private int _curId;

        public UiBiologicalAvatarItem[] Items;

        public RectTransform PrefabItem;

        [SerializeField]
        private HexGrid _grid;
        [SerializeField]
        private HexCell _currentCell;
        [SerializeField]
        private HexUnit _currentUnit;
        [SerializeField]
        private Vector2 _guiSize = new Vector2(150, 200);

        private RectTransform _canvasRect;
        private StringBuilder _contentstr=new StringBuilder();

        protected override void InitWindowData()
        {
            this.ID = WindowID.TipsWindow;

            windowData.windowType = UIWindowType.NormalLayer;
            windowData.showMode = UIWindowShowMode.DoNothing;
            windowData.navigationMode = UIWindowNavigationMode.IgnoreNavigation;
            windowData.closeModel = UIWindowCloseModel.Destory;
        }


        public override void InitWindowOnAwake()
        {
            _camera3D = Camera.main;
            _camera2D = UICenterMasterManager.Instance._Camera;
            //_grid = StrategyScene.MainGrid;
            _canvasRect= UICenterMasterManager.Instance._Canvas.GetComponent<RectTransform>();
        }

        protected override void BeforeShowWindow(BaseWindowContextData contextData = null)
        {
            if (contextData == null)
                return;
            _infodata = (TipsInfoWindowData)contextData;
            List<TipsInfoItemData> infoItemDatas = _infodata.InfoItemDatas;

            if (_initItem.Count >= infoItemDatas.Count)
            {
                for (int i = 0; i < _initItem.Count; i++)
                {
                    if (i < infoItemDatas.Count)
                    {
                        TipsInfoItemData data = infoItemDatas[i];
                        RectTransform item = _initItem[i];
                        item.GetComponent<Text>().text = data.Title + data.Content;
                    }
                    else
                    {
                        WXPoolManager.Pools[Define.GeneratedPool].Despawn(_initItem[i]);
                    }
                }
                _initItem.RemoveRange(infoItemDatas.Count,_initItem.Count-infoItemDatas.Count);
            }
            else
            {
                for (int i = 0; i < _initItem.Count; i++)
                {
                    WXPoolManager.Pools[Define.GeneratedPool].Despawn(_initItem[i]);
                }
                _initItem.Clear();
                for (int i = 0; i < infoItemDatas.Count; i++)
                {
                    TipsInfoItemData data = infoItemDatas[i];
                    RectTransform item = WXPoolManager.Pools[Define.GeneratedPool].Spawn(_textPrefab, _contentParent);
                    item.GetComponent<Text>().text = data.Title + data.Content;
                    _initItem.Add(item);
                }
                _curId = _infodata.Id;
            }
        }


        void Update()
        {
            if (_grid == null)
                return;

            if (EventSystem.current.IsPointerOverGameObject() == true)
                return;

            if (UpdateCurrentCell() == true)
            {
                if (_currentCell == null)
                    return;

                _contentstr.Clear();



                switch (_currentCell.PlantLevel)
                {
                    case 0:
                        _contentstr.AppendFormat("<Color=red>{0}</color>\n\n", "树木稀少");
                        break;
                    case 1:
                        _contentstr.AppendFormat("<Color=red>{0}</color>\n\n", "草原");
                        break;
                    case 2:
                        _contentstr.AppendFormat("<Color=red>{0}</color>\n\n", "树林");
                        break;
                    case 3:
                        _contentstr.AppendFormat("<Color=red>{0}</color>\n\n", "森林");
                        break;
                    case 4:
                        _contentstr.AppendFormat("<Color=red>{0}</color>\n\n", "丛林");
                        break;
                    default:
                        break;
                }

                if (_currentCell.SpecialIndex >= 0)
                {
                    switch (_currentCell.SpecialIndex)
                    {
                        case 1:  //城市
                            {
                                LivingArea livingArea = SystemManager.GetProperty<LivingArea>(_currentCell.Entity);
                                _contentstr.AppendFormat("城市:<Color=blue>{0}</color>\n", GameStaticData.CityRunDataDic[_currentCell.Entity].Name);
                                // UiCellFeature featureUi = WXPoolManager.Pools[Define.GeneratedPool].Spawn(StrategyAssetManager.UiCellFeature, CellFeaturesParent).GetComponent<UiCellFeature>();
                                // featureUi.Init(GameStaticData.CityRunDataDic[livingArea.Id].Name, StrategyAssetManager.GetCellFeatureSpt(livingArea.ModelId), _unit.Location.Entity, CityOpenEntity);
                            }
                            break;
                        case 2: //帮派
                            {
                               // Collective collective = SystemManager.GetProperty<Collective>(_unit.Location.Entity);
                              //  UiCellFeature featureUi = WXPoolManager.Pools[Define.GeneratedPool].Spawn(StrategyAssetManager.UiCellFeature, CellFeaturesParent).GetComponent<UiCellFeature>();
                                //featureUi.Init(GameStaticData.CityRunDataDic);
                                //UiCellFeature featureUi=WXPoolManager.Pools[Define.GeneratedPool].Spawn(StrategyAssetManager.UiCellFeature,CellPersonsParent).GetComponent<UiCellFeature>();
                                //featureUi.Init(GameStaticData.);
                            }
                            break;
                        case 3:  //遗迹
                            {

                            }
                            break;
                        case 4: //事件
                            {

                            }
                            break;
                        case 5: //奖励
                            {

                            }
                            break;
                        case 6: //修炼地
                            {
                            }
                            break;
                    }

                }

                //SystemManager.Get<BiologicalSystem>().GetPoint(ref _personUnits, _curCell.coordinates.X, _curCell.coordinates.Z);

                //for (int i = 0; i < _personUnits.Count; i++)
                //{
                //    if (_personUnits[i] == Define.Player.Unit)
                //        continue;

                //    Entity entity = _personUnits[i].Entity;

                //    if (SystemManager.Contains<Biological>(entity))
                //    {
                //        Biological biological = SystemManager.GetProperty<Biological>(entity);
                //        BiologicalFixed biologicalFixed = GameStaticData.BiologicalDictionary[entity];


                //        RectTransform personRect = WXPoolManager.Pools[Define.GeneratedPool].Spawn(StrategyAssetManager.UiPersonButton, CellPersonsParent);
                //        BiologicalBaseUi bui = personRect.GetComponent<BiologicalBaseUi>();
                //        bui.Avatar = StrategyAssetManager.GetBiologicalAvatar(biological.AvatarId);

                //        bui.PersonName = biologicalFixed.Surname + biologicalFixed.Name;
                //        bui.Entity = entity;
                //    }

                //}






                // _contentstr.AppendFormat()




                // Debug.Log(_currentCell.Position);
                _contenText.text = _contentstr.ToString();
            }
            else
            {
            }

            Vector2 outVec;
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(_canvasRect, Input.mousePosition, null, out outVec))
            {
                Debug.Log("Setting anchored positiont to: " + outVec);
                _mouseView.anchoredPosition = outVec;
            }

            //if (Input.GetMouseButtonDown(0))
            //{
            //    DoSelection();
            //}
            //else if (_currentUnit)
            //{
            //    if (Input.GetMouseButtonDown(1))
            //    {
            //        DoMove();
            //    }
            //    else
            //    {
            //        DoPathfinding();
            //    }
            //}
        }


        void LateUpdate()
        {
            //if (_infodata.IsShow)
            //{
            //    _pointTf.gameObject.SetActive(true);
            //}
            //else
            //{
            //    _pointTf.gameObject.SetActive(false);
            //    return;
            //}

            //_wordpos = _infodata.Point;
            //if (Define.IsAPointInACamera(_camera3D, _wordpos))
            //{
            //    _pointTf.localScale = Vector3.one;
            //    Vector2 tempPos = _camera3D.WorldToScreenPoint(_wordpos);
            //    Vector3 temppos = _camera2D.ScreenToWorldPoint(tempPos);
            //    temppos.z = 0f;
            //    _pointTf.position = temppos;
            //}
            //else
            //{
            //    _pointTf.localScale = Vector3.zero;
            //}
        }

        public void ShowTips()
        {

        }


        //public void SetBiologicalTip(Vector3 point,int id)
        //{
        //    _curdelaytime = 0.3f;
        //    _pointTf.gameObject.SetActive(true);
        //    if (CheckId(id * 2)==false)
        //    {
        //        UiTitleitem control= _pointTf.gameObject.GetComponent<UiTitleitem>();
        //        control.Init(Camera.main,UICenterMasterManager.Instance._Camera,point);
        //        //_text.text = GameStaticData.BiologicalSurnameDic[id] + GameStaticData.BiologicalNameDic[id];
        //        _curId = id * 2;
        //    }
        //}

        //public void Hide()
        //{
        //    _pointTf.gameObject.SetActive(false);
        //}

        public void ChangePlayer(Entity entity)
        {
            int index = -1;
            for (int i = 0; i < Items.Length; i++)
            {
                if (Items[i].Entity == entity)
                {

                    index = i;
                }
            }

            if (index == -1)
            {
                for (int i = 0; i < Items.Length; i++)
                {
                    if (Items[i].IsEnable == false)
                    {
                        Items[i].SetupData(entity);
                    }

                    index = i;
                }
            }
            Items[index].Change();
        }

        public void SetEditMode(bool toggle)
        {
            enabled = !toggle;
            _grid.ShowUI(!toggle);
            _grid.ClearPath();
            if (toggle)
            {
                Shader.EnableKeyword("HEX_MAP_EDIT_MODE");
            }
            else
            {
                Shader.DisableKeyword("HEX_MAP_EDIT_MODE");
            }
        }


        void DoSelection()
        {
            _grid.ClearPath();
            UpdateCurrentCell();
            if (_currentCell)
            {
                //显示地形信息//解析地形信息 //todo:增加地区事件 描述

                //for (int i = 0; i < currentCell.; i++)
                //{

                //}
                //if (currentCell.Unit == null)
                //{
                //   


                //}
                //else
                //{
                //    //如果当前选中了目标
                //    if (unit == StrategyPlayer.Unit)
                //    {

                //    }
                //    else //r
                //    {

                //    }

                //}
                //unit = currentCell.Unit;
            }
        }

        void DoPathfinding()
        {
            if (UpdateCurrentCell())
            {
                if (_currentCell && _currentUnit.IsValidDestination(_currentCell))
                {
                    _grid.FindPath(_currentUnit.Location, _currentCell, _currentUnit);


                }
                else
                {
                    _grid.ClearPath();
                }
            }
        }

        void DoMove()
        {
            if (_grid.HasPath)
            {
                _currentUnit.Travel(_grid.GetPath());
                _grid.ClearPath();
            }
        }

        bool UpdateCurrentCell()
        {
            HexCell cell = _grid.GetCell(Camera.main.ScreenPointToRay(Input.mousePosition));
            if (cell != _currentCell)
            {
                _currentCell = cell;
                return true;
            }
            return false;
        }



    }




}