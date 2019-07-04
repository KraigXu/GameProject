using System;
using System.Collections;
using System.Collections.Generic;
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
        private List<HexUnit> _personUnits=new List<HexUnit>();
        
        private PlayerMapInputSystem _system;

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
            _system = SystemManager.Get<PlayerMapInputSystem>();
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

        void Update()
        {
            if (_curCell != _unit.Location)
            {
                while (CellFeaturesParent.childCount>0)
                {
                    WXPoolManager.Pools[Define.GeneratedPool].Despawn(CellFeaturesParent.GetChild(0));
                    Debug.Log(">>"+ CellFeaturesParent.GetChild(0).GetInstanceID());
                }

                while (CellPersonsParent.childCount>0)
                {
                    WXPoolManager.Pools[Define.GeneratedPool].Despawn(CellPersonsParent.GetChild(0));
                }

                _personUnits.Clear();

             

                int cellType = 3;

                CellTypeImage.overrideSprite = GameStaticData.CellTypeSprite[cellType];
                CellTypeTxt.text = GameStaticData.CellTypeName[cellType];
                CellUrbanTxt.text = _unit.Location.UrbanLevel.ToString();
                CellFarmTxt.text = _unit.Location.FarmLevel.ToString();
                CellPlantTxt.text = _unit.Location.PlantLevel.ToString();


                if (_unit.Location.SpecialIndex >= 0)
                {
                   
                    switch (_unit.Location.SpecialIndex)
                    {
                        case 1:
                             SystemManager.Get<CitySystem>().InitSpecial(_unit.Location.coordinates.X,_unit.Location.coordinates.Z, CellFeaturesParent);
                            break;
                        case 2:
                            break;
                        case 3:
                            break;
                        default:
                            break;
                    }
                    
                }
                
                SystemManager.Get<BiologicalSystem>().GetPoint(ref _personUnits, _curCell.coordinates.X, _curCell.coordinates.Z);

                for (int i = 0; i < _personUnits.Count; i++)
                {

                    GameObjectEntity personEntity = _personUnits[i].gameObject.GetComponent<GameObjectEntity>();

                    Biological biological = SystemManager.GetProperty<Biological>(personEntity.Entity);

                    RectTransform personRect = WXPoolManager.Pools[Define.GeneratedPool].Spawn(StrategyStyle.UiPersonButton, CellPersonsParent);

                    BiologicalBaseUi bui = personRect.GetComponent<BiologicalBaseUi>();
                    bui.Avatar = GameStaticData.BiologicalAvatar[biological.AvatarId];
                    bui.PersonName = GameStaticData.BiologicalSurnameDic[biological.BiologicalId] + GameStaticData.BiologicalNameDic[biological.BiologicalId];

                }

                _curCell = _unit.Location;
            }

            else
            {

            }
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

