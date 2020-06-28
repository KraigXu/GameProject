using System;
using System.Collections;
using System.Collections.Generic;
using AntiquityWorld.StrategyManager;
using UnityEngine;
using UnityEngine.UI;
using GameSystem;
using Unity.Entities;

namespace GameSystem.Ui
{

    /// <summary>
    /// 显示信息面板
    /// </summary>
    public class WXCharacterPanelWidow : UIWindowBase
    {
        [Header("Introduction")]
        public Text NameTxt;
        public Text SurnameTxt;
        public Text AgeTxt;
        public Text SexTxt;
        public Text PrestigeTxt;
        public Text FamilyTxt;
        public Text FactionTxt;


        [Header("Property")]
        public Text Tizhitxt;
        public Text Lidaotxt;
        public Text Jingshentxt;
        public Text Lingdongtxt;
        public Text Wuxingtxt;
        public Text Jingtxt;
        public Text Qitxt;
        public Text Shentxt;
        public Text Neigongtxt;
        public Text Waigongtxt;

        public Text ThoughTxt;
        public Text NeckTxt;
        public Text HeartTxt;
        public Text EyeTxt;
        public Text EarTxt;
        public Text LegLeftTxt;
        public Text LegRightTxt;
        public Text HandRightTxt;
        public Text HandLeftTxt;
        public Text FertilityTxt;
        public Text AppearanceTxt;
        public Text DressTxt;
        public Text SkinTxt;
        public Text BlodTxt;
        public Text JingLuoTxt;

        public UiEquipmentBox HelmetBox;
        public UiEquipmentBox ClothesBox;
        public UiEquipmentBox BeltBox;
        public UiEquipmentBox HandGuradBox;
        public UiEquipmentBox PantsBox;
        public UiEquipmentBox ShoeaBox;
        public UiEquipmentBox WeaponFirstBox;
        public UiEquipmentBox WeaponSecondaryBox;

        [Header("Speciality")]
        public RectTransform SpecialityContentParent;

        [Header("Article")]
        private ArticleManager _articleManager = new ArticleManager();
        [SerializeField]
        private RectTransform _articleParent;

        [Header("Info")]

        [SerializeField] private Button _exitBtn;
        //--------------------------------运行时属性
        private Entity _curEntity;
        private EntityManager _entityManager;


        private Dictionary<GameObject,Entity> _spawnArticleDic=new Dictionary<GameObject, Entity>();


        protected override void InitWindowData()
        {
            this.ID = WindowID.WXCharacterPanelWindow;

            windowData.windowType = UIWindowType.ForegroundLayer;
            windowData.showMode = UIWindowShowMode.DoNothing;
            windowData.navigationMode = UIWindowNavigationMode.IgnoreNavigation;
            windowData.colliderMode = UIWindowColliderMode.None;
            windowData.closeModel = UIWindowCloseModel.Destory;
        }

        //public override void InitWindowOnAwake()
        //{
        //    _entityManager = World.Active.GetOrCreateManager<EntityManager>();

        //    _exitBtn.onClick.AddListener(ExitButtonClick);
        //}

        protected override void BeforeShowWindow(BaseWindowContextData contextData = null)
        {
            if (contextData == null)
            {
                //_curEntity = StrategyPlayer.Entity;
            }
            else
            {
                BiologicalUiInData uidata = (BiologicalUiInData)contextData;
                _curEntity = uidata.Entities[0];
            }

            Biological biological = _entityManager.GetComponentData<Biological>(_curEntity);

            BiologicalFixed biologicalFixed = BiologicalSystem.GetBiologicalFixedByKey(_curEntity);

            NameTxt.text = biologicalFixed.Name;
            SurnameTxt.text = biologicalFixed.Surname;
            AgeTxt.text = biological.Age.ToString();
            SexTxt.text = biologicalFixed.Sex;
            PrestigeTxt.text = "10000";

            //FamilyTxt.text = GameStaticData.FamilyName[biological.FamilyId];
            //FactionTxt.text = GameStaticData.FactionName[biological.FactionId];

            //Show body Info
            BodyProperty bodyPropert = _entityManager.GetComponentData<BodyProperty>(_curEntity);

            Tizhitxt.text = biological.Tizhi.ToString();
            Lidaotxt.text = biological.Lidao.ToString();
            Jingshentxt.text = biological.Jingshen.ToString();
            Lingdongtxt.text = biological.Lingdong.ToString();
            Wuxingtxt.text = biological.Wuxing.ToString();
            Jingtxt.text = biological.Jing.ToString();
            Qitxt.text = biological.Qi.ToString();
            Shentxt.text = biological.Shen.ToString();
            Neigongtxt.text = (biological.Lidao * 0.5f + 1).ToString();
            Waigongtxt.text = (biological.Lidao * 0.3f + 1).ToString();

            //ThoughTxt.text = bodyPropert.Thought.ToString();
            //NeckTxt.text = bodyPropert.Neck.ToString();
            //HeartTxt.text = bodyPropert.Heart.ToString();
            //EyeTxt.text = bodyPropert.Eye.ToString();
            //EarTxt.text = bodyPropert.Ear.ToString();
            //LegLeftTxt.text = bodyPropert.LeftLeg.ToString();
            //LegRightTxt.text = bodyPropert.LeftLeg.ToString();
            //HandRightTxt.text = bodyPropert.RightHand.ToString();
            //HandLeftTxt.text = bodyPropert.LeftHand.ToString();
            //FertilityTxt.text = bodyPropert.Fertility.ToString();
            //AppearanceTxt.text = bodyPropert.Appearance.ToString();
            //DressTxt.text = bodyPropert.Dress.ToString();
            //SkinTxt.text = bodyPropert.Skin.ToString();
            //BlodTxt.text = bodyPropert.Blod.ToString();
            //JingLuoTxt.text = bodyPropert.JingLuo.ToString();
            //解析 PS暂留 

            Equipment equipment = _entityManager.GetComponentData<Equipment>(_curEntity);

            if (equipment.HelmetE != Entity.Null)
            {
                HelmetBox.Entity = equipment.HelmetE;
                //ArticleItem articleItem = _entityManager.GetComponentData<ArticleItem>(_curEntity);
            }
            if (equipment.ClothesE != Entity.Null)
            {
                ClothesBox.Entity = equipment.ClothesE;
            }
            if (equipment.BeltE != Entity.Null)
            {
                BeltBox.Entity = equipment.BeltE;
            }
            if (equipment.HandGuardE != Entity.Null)
            {
                HandGuradBox.Entity = equipment.HandGuardE;
            }
            if (equipment.PantsE != Entity.Null)
            {
                PantsBox.Entity = equipment.PantsE;
            }
            if (equipment.ShoesE != Entity.Null)
            {
                ShoeaBox.Entity = equipment.ShoesE;
            }
            if (equipment.WeaponFirstE != Entity.Null)
            {
                WeaponFirstBox.Entity = equipment.WeaponFirstE;
            }
            if (equipment.WeaponSecondaryE != Entity.Null)
            {
                WeaponSecondaryBox.Entity = equipment.WeaponSecondaryE;
            }
            
            //TeamSystem

            //Show knpasck Info  PS ？？
            Knapsack knapsack = _entityManager.GetComponentData<Knapsack>(_curEntity);
            RectTransform itemView = WXPoolManager.Pools[Define.GeneratedPool].Spawn(StrategyAssetManager.UiArticleView, _articleParent);
            UiArticleView articleView = itemView.gameObject.GetComponent<UiArticleView>();
            articleView.Text.text = knapsack.CurUpper + "/" + knapsack.UpperLimit;
            List<Entity> entities = SystemManager.Get<ArticleSystem>().GetEntities(_curEntity);
            for (int j = 0; j < entities.Count; j++)
            {
                ArticleItem articleItem = _entityManager.GetComponentData<ArticleItem>(entities[j]);

                RectTransform box = WXPoolManager.Pools[Define.GeneratedPool].Spawn(StrategyAssetManager.UiArticleBox, articleView.ContentRect);
                UiArticleBox aiArticleBox = box.gameObject.GetComponent<UiArticleBox>();
                aiArticleBox.NumberText.text = GameStaticData.ArticleDictionary[entities[j]].Name;
                aiArticleBox.Entity = entities[j];
                aiArticleBox.image.sprite = StrategyAssetManager.GetArticleSprites(articleItem.SpriteId);

                _spawnArticleDic.Add(box.gameObject,entities[j]);
            }

            //Show
            List<Entity> specialityEntitys = new List<Entity>();

            for (int i = 0; i < specialityEntitys.Count; i++)
            {
                RectTransform specialityRect = WXPoolManager.Pools[Define.GeneratedPool].Spawn(StrategyAssetManager.UiSpeciality, SpecialityContentParent);
                
            }

        }



        public void ExitButtonClick()
        {

            UICenterMasterManager.Instance.CloseWindow(this.ID);
           // UICenterMasterManager.Instance.ShowWindow(WindowID)


        }

        void OnGUI()
        {
            var point = Input.mousePosition;
            GUISkin skin = StrategyAssetManager.UiArticleSkin;
            GUI.Window(0, new Rect(300, 300, 345, 461), OnWindowNew, "", skin.GetStyle("window"));

        }

        void OnWindowNew(GUISkin skin)
        {

            GUI.Label(new Rect(0, 0, 461, 72), "摇篮", skin.GetStyle("label"));
            GUI.Label(new Rect(0, 72, 461, 16), "护符", skin.GetStyle("normallable"));
            GUI.Label(new Rect(0, 88, 461, 42), "物理防御999", skin.GetStyle("lableMax"));


           // GUI.Label(new Rect(10, 130, 26, 26), Texture);

            GUI.Label(new Rect(42, 130, 461, 26), "+2 智力", skin.GetStyle("lableblue"));
            GUI.Label(new Rect(42, 156, 461, 26), "+1 烈火学派", skin.GetStyle("lableblue"));
            GUI.Label(new Rect(42, 182, 461, 26), "+2 大气学派", skin.GetStyle("lableblue"));
            GUI.Label(new Rect(42, 208, 461, 26), "+1 领袖", skin.GetStyle("lableblue"));
            GUI.Label(new Rect(42, 234, 461, 26), "+1 坚毅", skin.GetStyle("lableblue"));

          //  GUI.Label(new Rect(10, 260, 26, 26),);
            GUI.Label(new Rect(42, 260, 461, 26), "等级21", skin.GetStyle("lableh"));

           // GUI.Label(new Rect(10, 286, 26, 26), Texture);
            GUI.Label(new Rect(42, 286, 461, 26), "巨型火焰威能符文", skin.GetStyle("lable1"));
            GUI.Label(new Rect(42, 312, 461, 26), "智力 + 3", skin.GetStyle("lable1"));
            GUI.Label(new Rect(42, 338, 461, 26), "暴击率 +12%", skin.GetStyle("lable1"));

            //  GUI.Label(new Rect(20,), );

            GUI.Label(new Rect(20, 411, 345, 50), "神圣", skin.GetStyle("lablem"));
            GUI.Label(new Rect(280, 411, 65, 50), "9999999Y", skin.GetStyle("lablem"));

        }


        public void OnWindowNew(int id)
        {

        }

        public override void InitWindowOnAwake()
        {
            throw new NotImplementedException();
        }

        ///// <summary>
        ///// 切换人物信息
        ///// </summary>
        ///// <param name="key"></param>
        //private void BiologicalChange(Entity entity, int key)
        //{
        //    _curEntity = entity;

        //    Biological biological = _entityManager.GetComponentData<Biological>(_curEntity);
        //    _name.text = GameStaticData.BiologicalNameDic[biological.BiologicalId];
        //    _surname.text = GameStaticData.BiologicalSurnameDic[biological.BiologicalId];

        //    PropertyTogChange(_propertyTog.isOn);
        //    CombatTogChange(_combatTog.isOn);
        //    JiyiTogChange(_jiyiTog.isOn);
        //    TagTogChange(_tagTog.isOn);
        //    ChangeEquipment();
        //}

        //private void PropertyTogChange(bool flag)
        //{
        //    _propertyGo.gameObject.SetActive(flag);
        //    if (flag == true)
        //    {
        //        //_tizhitxt.text = _curBiological.Tizhi.ToString();
        //        //_lidaotxt.text = _curBiological.AgeMax.ToString();
        //        //_tizhitxt.text = _curBiological.Tizhi.ToString();
        //        //_lidaotxt.text = _curBiological.Lidao.ToString();
        //        //_jingshentxt.text = _curBiological.Jingshen.ToString();
        //        //_lingdongtxt.text = _curBiological.Lingdong.ToString();
        //        //_wuxingtxt.text = _curBiological.Wuxing.ToString();
        //        //_jingtxt.text = _curBiological.Jing.ToString();
        //        //_qitxt.text = _curBiological.Qi.ToString();
        //        //_shentxt.text = _curBiological.Shen.ToString();
        //    }
        //    else
        //    {
        //    }
        //}

        //private void CombatTogChange(bool flag)
        //{
        //    _combatGo.gameObject.SetActive(flag);
        //    if (flag == true)
        //    {

        //    }
        //    else
        //    {

        //    }
        //}
        ///// <summary>
        ///// Techniques面板打开 更新和清除
        ///// </summary>
        ///// <param name="flag"></param>
        //private void JiyiTogChange(bool flag)
        //{
        //    _jiyiGo.gameObject.SetActive(flag);

        //    if (flag == true)
        //    {
        //        //List<KeyValuePair<int, int>> content = TechniquesSystem.GetTechnique(_curBiological.TechniquesId).Content;

        //        //for (int i = 0; i < content.Count; i++)
        //        //{
        //        //    RectTransform rectGo = WXPoolManager.Pools[Define.GeneratedPool].Spawn(_techniquesPrefab, _jiyiContent);
        //        //    rectGo.GetChild(0).GetComponent<Text>().text = GameStaticData.TechniquesName[content[i].Key];
        //        //    rectGo.GetChild(1).GetComponent<Text>().text = content[i].Value.ToString();
        //        //}
        //    }
        //    else
        //    {
        //        for (int i = 0; i < _techniquesItems.Count; i++)
        //        {
        //            WXPoolManager.Pools[Define.GeneratedPool].Despawn(_techniquesItems[i].transform);
        //        }
        //        _techniquesItems.Clear();
        //    }
        //}
        //private void TagTogChange(bool flag)
        //{
        //    _tagGo.gameObject.SetActive(flag);
        //    if (flag == true)
        //    {

        //    }
        //    else
        //    {

        //    }
        //}

        ///// <summary>
        ///// 更新装备界面
        ///// </summary>
        //private void ChangeEquipment()
        //{
        //    //EquipmentCoat equipment = SystemManager.GetProperty<EquipmentCoat>(_curEntity);

        //    //Entity biologicalEntity = SystemManager.Get<BiologicalSystem>().GetBiologicalEntity(_curShowId);
        //    //_showTransform = WXPoolManager.Pools[Define.GeneratedPool].Spawn(StrategySceneInit.Settings.ArticleInfoPerfab.transform, transform);

        //    //UiEquipmentItem equipment = _showTransform.gameObject.GetComponent<UiEquipmentItem>();

        //    //string[] nameValue = go.name.Split('_');  //type_Id

        //    //switch ((ArticleType)int.Parse(nameValue[0]))
        //    //{
        //    //    case ArticleType.Coat:
        //    //        {
        //    //            if (SystemManager.Contains<EquipmentCoat>(biologicalEntity))
        //    //            {
        //    //                UiEquipmentStyle style = new UiEquipmentStyle();
        //    //                EquipmentCoat equipmentCoat = SystemManager.GetProperty<EquipmentCoat>(biologicalEntity);

        //    //                style.Title = "equipmentCoat";
        //    //                style.Level = (int)EquipLevel.General;
        //    //                style.conents = new Dictionary<string, List<string>>();
        //    //                style.Values = new Dictionary<string, string>();
        //    //                style.Values.Add("钝器防御:", equipmentCoat.BluntDefense.ToString());
        //    //                style.Values.Add("利器防御:", equipmentCoat.SharpDefense.ToString());
        //    //                style.Values.Add("操作性:", equipmentCoat.Operational.ToString());

        //    //                style.Values.Add("重量:", equipmentCoat.Weight.ToString());
        //    //                style.Values.Add("价格:", equipmentCoat.Price.ToString());
        //    //                style.Values.Add("耐久:", equipmentCoat.Durable.ToString());
        //    //                style.BackgroundId = equipmentCoat.SpriteId;


        //    //                //equipmentCoat
        //    //            }
        //    //            else
        //    //            {
        //    //            }

        //    //        }
        //    //        break;
        //    //    case ArticleType.Gloves:
        //    //        break;
        //    //    case ArticleType.Pants:
        //    //        break;
        //    //}

        //    //UICenterMasterManager.Instance.ShowWindow(id:)
        //}
    }
}