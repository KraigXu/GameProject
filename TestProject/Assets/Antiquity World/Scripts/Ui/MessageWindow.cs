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

        public Text CellTerrainTxt;
        public Text CellTypeTxt;
        public Text CellUrbanTxt;
        public Text CellFarmTxt;
        public Text CellPlantTxt;
        public Text CellPlauTxt;

        public RectTransform CellFeaturesParent;

        public RectTransform CellPersonsParent;
        private List<HexUnit> _personUnits = new List<HexUnit>();

        private HexCell _curCell;
        private HexCell _beforeCell;

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

        /// <summary>
        /// 清空未更新的内容
        /// </summary>
        public void Clear()
        {
            while (CellFeaturesParent.childCount > 0)
            {
                WXPoolManager.Pools[Define.GeneratedPool].Despawn(CellFeaturesParent.GetChild(0));
            }

            while (CellPersonsParent.childCount > 0)
            {
                WXPoolManager.Pools[Define.GeneratedPool].Despawn(CellPersonsParent.GetChild(0));
            }

            _personUnits.Clear();
        }


        void LateUpdate()
        {
            if (StrategyPlayer.Unit == null)
            {
                return;
            }

            if (StrategyPlayer.Unit.Location == null)
            {
                return;
            }
            if (_curCell != StrategyPlayer.Unit.Location)
            {
                Clear();
                _curCell = StrategyPlayer.Unit.Location;

                //Debug.Log(_curCell.Elevation);

                //Debug.Log(_curCell.FarmLevel);

                //Debug.Log(_curCell.SpecialIndex);
                //Debug.Log(_curCell.UrbanLevel);

                int cellType = 3;

                CellTypeImage.overrideSprite = StrategyAssetManager.GetCellTypeSprites(cellType);
                CellTypeTxt.text = "";
                CellUrbanTxt.text = _curCell.UrbanLevel.ToString();
                CellFarmTxt.text = _curCell.FarmLevel.ToString();

                switch (_curCell.FarmLevel)
                {
                    case 0:

                        break;
                }


                switch (_curCell.PlantLevel)
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

                CellPlantTxt.text = _curCell.PlantLevel.ToString();


                if (_curCell.SpecialIndex >= 0)
                {
                    switch (_curCell.SpecialIndex)
                    {
                        case 1:  //城市
                            {

                                LivingArea livingArea = SystemManager.GetProperty<LivingArea>(_curCell.Entity);
                                UiCellFeature featureUi = WXPoolManager.Pools[Define.GeneratedPool].Spawn(StrategyAssetManager.UiCellFeature, CellFeaturesParent).GetComponent<UiCellFeature>();
                                featureUi.Init(GameStaticData.CityRunDataDic[livingArea.Id].Name, StrategyAssetManager.GetCellFeatureSpt(livingArea.ModelId), _curCell.Entity, CityOpenEntity);


                            }
                            break;
                        case 2: //帮派
                            {

                                Collective collective = SystemManager.GetProperty<Collective>(_curCell.Entity);

                                UiCellFeature featureUi = WXPoolManager.Pools[Define.GeneratedPool].Spawn(StrategyAssetManager.UiCellFeature, CellFeaturesParent).GetComponent<UiCellFeature>();
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

                SystemManager.Get<BiologicalSystem>().GetPoint(ref _personUnits, _curCell.coordinates.X, _curCell.coordinates.Z);

                for (int i = 0; i < _personUnits.Count; i++)
                {
                    if (_personUnits[i] == StrategyPlayer.Unit)
                        continue;

                    Entity entity = _personUnits[i].Entity;

                    if (SystemManager.Contains<Biological>(entity))
                    {
                        Biological biological = SystemManager.GetProperty<Biological>(entity);
                        BiologicalFixed biologicalFixed = GameStaticData.BiologicalDictionary[entity];

                        RectTransform personRect = WXPoolManager.Pools[Define.GeneratedPool].Spawn(StrategyAssetManager.UiPersonButton, CellPersonsParent);
                        BiologicalBaseUi bui = personRect.GetComponent<BiologicalBaseUi>();
                        bui.Avatar = StrategyAssetManager.GetBiologicalAvatar(biological.AvatarId);
                        bui.OnClickEvent += BiologicalEvent;
                        bui.PersonName = biologicalFixed.Surname + biologicalFixed.Name;
                        bui.Entity = entity;

                    }

                }
            }
            else
            {

            }
        }


        /// <summary>
        /// 打开City实体 ,并且切换场景显示 ---City
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

            SystemManager.Get<CitySystem>().CityMass(target, StrategyPlayer.Entity);

        }

        public void BiologicalEvent(Entity entity)
        {

            SocialDialogWindowData socialDialogWindowData = new SocialDialogWindowData();
            socialDialogWindowData.SceneId = 1;
            socialDialogWindowData.Aside = "流窜的山贼用贪婪的眼神看着你，威胁你交出身上{0}金钱";
            socialDialogWindowData.OnSelfEntity = StrategyPlayer.Entity;
            socialDialogWindowData.OnSelf = new List<DialogNode>();
            DialogNode onselfs1 = new DialogNode();
            onselfs1.Id = 1;
            onselfs1.Content = "攻击对方";
            onselfs1.EventCode = 99;
            socialDialogWindowData.OnSelf.Add(onselfs1);

            DialogNode onselfs2 = new DialogNode();
            onselfs2.Id = 2;
            onselfs2.Content = "偷窃";
            onselfs2.EventCode = 98;
            socialDialogWindowData.OnSelf.Add(onselfs2);

            DialogNode onselfs3 = new DialogNode();
            onselfs3.Id = 3;
            onselfs3.Content = "交流";
            onselfs3.EventCode = 97;
            socialDialogWindowData.OnSelf.Add(onselfs3);

            socialDialogWindowData.OtherEntity = entity;
            socialDialogWindowData.Other = new List<DialogNode>();
            DialogNode dialogOther1 = new DialogNode();
            dialogOther1.Id = 4;
            dialogOther1.Content = "交出{0}金钱。";
            dialogOther1.EventCode = 80;
            socialDialogWindowData.Other.Add(dialogOther1);

            DialogNode dialogOther2 = new DialogNode();
            dialogOther2.Id = 5;
            dialogOther2.Content = "恐吓对方";
            dialogOther2.EventCode = 70;
            socialDialogWindowData.Other.Add(dialogOther2);

            DialogNode dialogOther4 = new DialogNode();
            dialogOther4.Id = 5;
            dialogOther4.Content = "[口才>10]说服对方";
            dialogOther4.EventCode = 60;
            socialDialogWindowData.Other.Add(dialogOther4);


            ShowWindowData showWindowData = new ShowWindowData();
            showWindowData.contextData = socialDialogWindowData;

            UICenterMasterManager.Instance.ShowWindow(WindowID.SocialDialogWindow, showWindowData);
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

