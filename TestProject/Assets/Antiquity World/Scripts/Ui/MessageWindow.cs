using System;
using System.Collections;
using System.Collections.Generic;
using AntiquityWorld;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Unity.Entities;

namespace GameSystem.Ui
{
    /// <summary>
    /// 消息界面 显示提示消息的界面
    /// </summary>
    public class MessageWindow : UIWindowBase
    {

        public RectTransform ContentParent;
        public GameObject Prefab;
        public List<Text> Texts;

        public Dictionary<Text, Sequence> TextSequences = new Dictionary<Text, Sequence>();

        public Queue<RectTransform> overflows = new Queue<RectTransform>();

        public int MaxNumber;
        public float Time = 5f;

        [Header("UI")]
        public Image CellTypeImage;
        public Text CellTypeTxt;
        public Text CellUrbanTxt;
        public Text CellFarmTxt;
        public Text CellPlantTxt;

        public RectTransform CellFeaturesParent;

        public RectTransform CellPersonsParent;
        private List<HexUnit> _personUnits = new List<HexUnit>();

        private StrategyScene _strategyScene;
        private StrategyPlayer _player;
        private HexCell _curCell;
        private HexCell _beforeCell;
        private HexUnit _unit;

        protected override void InitWindowData()
        {
            this.ID = WindowID.MessageWindow;

            windowData.windowType = UIWindowType.NormalLayer;
            windowData.showMode = UIWindowShowMode.DoNothing;
            windowData.navigationMode = UIWindowNavigationMode.IgnoreNavigation;
            windowData.colliderMode = UIWindowColliderMode.None;
            windowData.closeModel = UIWindowCloseModel.Destory;
            windowData.animationType = UIWindowAnimationType.None;
        }

        public override void InitWindowOnAwake()
        {
            _strategyScene = StrategyScene.Instance;
            _player = StrategyScene.Instance.Player;
            _unit = _player.Unit;
        }

        void TextAnimation(Text text)
        {
            text.gameObject.SetActive(false);
            text.color = Color.black;
            Debug.Log(text.gameObject.name);
        }
        void DestoryText()
        {
            GameObject.Destroy(overflows.Dequeue().gameObject);
        }

        public void FeaturesTog(bool flag)
        {
            CellFeaturesParent.gameObject.SetActive(flag);
        }

        public void PersonsTog(bool flag)
        {
            CellPersonsParent.gameObject.SetActive(flag);
        }
        void LateUpdate()
        {
            if (_curCell != _unit.Location)
            {
                _curCell = _unit.Location;
                while (CellFeaturesParent.childCount > 0)
                {
                    WXPoolManager.Pools[Define.GeneratedPool].Despawn(CellFeaturesParent.GetChild(0));
                }

                while (CellPersonsParent.childCount > 0)
                {
                    WXPoolManager.Pools[Define.GeneratedPool].Despawn(CellPersonsParent.GetChild(0));
                }

                _personUnits.Clear();


                //根据cell显示内容，解析cell



                int cellType = 3;

                CellTypeImage.overrideSprite = StrategyAssetManager.GetCellTypeSprites(cellType);
                CellTypeTxt.text = "";
                CellUrbanTxt.text = _unit.Location.UrbanLevel.ToString();
                CellFarmTxt.text = _unit.Location.FarmLevel.ToString();

                switch (_unit.Location.FarmLevel)
                {
                    case 0:
                        CellFarmTxt.text = "沙漠";
                        break;
                    case 1:
                        CellFarmTxt.text = "平地";
                        break;
                    case 2:
                        CellFarmTxt.text = "草原";
                        break;
                    default:
                        CellFarmTxt.text = "无";
                        break;
                }


                switch (_unit.Location.PlantLevel)
                {
                    case 0:
                        CellPlantTxt.text = "树木稀少";
                        break;
                    case 1:
                        CellPlantTxt.text = "草原";
                        break;
                    case 2:
                        CellPlantTxt.text = "树林";
                        break;
                    case 3:
                        CellPlantTxt.text = "森林";
                        break;
                    case 4:
                        CellPlantTxt.text = "丛林";
                        break;
                    default:
                        break;
                }

                CellPlantTxt.text = _unit.Location.PlantLevel.ToString();


                if (_unit.Location.SpecialIndex >= 0)
                {
                    switch (_unit.Location.SpecialIndex)
                    {
                        case 1:  //城市
                            {
                                Entity entity = SystemManager.Get<MapCellSystem>().GetEntity(_unit.Location.coordinates.X, _unit.Location.coordinates.Z);
                                LivingArea livingArea = SystemManager.GetProperty<LivingArea>(entity);

                                UiCellFeature featureUi = WXPoolManager.Pools[Define.GeneratedPool].Spawn(StrategyAssetManager.UiCellFeature, CellFeaturesParent).GetComponent<UiCellFeature>();
                                featureUi.Init(GameStaticData.CityRunDataDic[livingArea.Id].Name, StrategyAssetManager.GetCellFeatureSpt(livingArea.ModelId), entity, CityOpenEntity);
                            }
                            break;
                        case 2: //帮派
                            {
                                Entity entity = SystemManager.Get<MapCellSystem>().GetEntity(_unit.Location.coordinates.X, _unit.Location.coordinates.Z);

                                Collective collective = SystemManager.GetProperty<Collective>(entity);

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

                SystemManager.Get<BiologicalSystem>().GetPoint(ref _personUnits, _curCell.coordinates.X, _curCell.coordinates.Z);

                for (int i = 0; i < _personUnits.Count; i++)
                {
                    if (_personUnits[i] == StrategyScene.Instance.Player.Unit)
                        continue;

                    GameObjectEntity personEntity = _personUnits[i].gameObject.GetComponent<GameObjectEntity>();

                    Biological biological = SystemManager.GetProperty<Biological>(personEntity.Entity);

                    BiologicalFixed biologicalFixed = GameStaticData.BiologicalDictionary[personEntity.Entity];

                    RectTransform personRect = WXPoolManager.Pools[Define.GeneratedPool].Spawn(StrategyAssetManager.UiPersonButton, CellPersonsParent);

                    BiologicalBaseUi bui = personRect.GetComponent<BiologicalBaseUi>();
                    bui.Avatar = StrategyAssetManager.GetBiologicalAvatar(biological.AvatarId);

                    bui.PersonName = biologicalFixed.Surname + biologicalFixed.Name;
                    bui.Entity = personEntity.Entity;
                }

            }
            else
            {

            }
        }

        /// <summary>
        /// 打开City实体 ,并且切换场景显示
        /// </summary>
        /// <param name="target"></param>
        public void CityOpenEntity(Entity target)
        {
            StrategyScene.Instance.ExitMapModel();
            StrategyScene.Instance.EnterBuildModel();

            ShowWindowData cityWindowData = new ShowWindowData();

            LivingAreaWindowCD livingAreaWindowCd = new LivingAreaWindowCD();
            livingAreaWindowCd.LivingAreaEntity = target;
            cityWindowData.contextData = livingAreaWindowCd;

            UICenterMasterManager.Instance.ShowWindow(WindowID.CityWindow, cityWindowData);


        }

        public void Log(string value)
        {
            Text current = null;
            for (int i = 0; i < Texts.Count; i++)
            {
                if (Texts[i].gameObject.activeSelf == false)
                {
                    current = Texts[i];
                    break;
                }
            }

            if (current != null)
            {
                current.text = value;
                current.gameObject.SetActive(true);
                current.transform.SetAsLastSibling();
                current.DOFade(0, Time).OnComplete(() => TextAnimation(current));
            }
            else
            {
                GameObject go = GameObject.Instantiate(Prefab) as GameObject;
                RectTransform rect = go.GetComponent<RectTransform>();
                rect.SetParent(ContentParent);
                overflows.Enqueue(rect);
                Text text = go.GetComponent<Text>();
                text.text = value;
                text.color = Color.black;

                text.DOFade(0, Time).OnComplete((() => DestoryText()));
                go.SetActive(true);
            }
        }


    }

}

