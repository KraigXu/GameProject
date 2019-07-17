using System;
using System.Collections;
using System.Collections.Generic;
using AntiquityWorld.StrategyManager;
using DataAccessObject;
using Newtonsoft.Json;
using Unity.Entities;
using UnityEngine;
namespace GameSystem
{
    public class ArticleSystem : ComponentSystem
    {
        struct Data
        {
            public readonly int Length;
            public EntityArray Entitys;
            public ComponentDataArray<ArticleItem> Items;
        }
        [Inject]
        private Data _data;

        private static EntityManager _entityManager;
        private static EntityArchetype _articleArchetype;

        protected override void OnCreateManager()
        {
            _entityManager = World.Active.GetOrCreateManager<EntityManager>();
            _articleArchetype = _entityManager.CreateArchetype(typeof(ArticleItem));
        }

        protected override void OnUpdate()
        {

            for (int i = 0; i < _data.Length; i++)
            {
                var article = _data.Items[i];
                //Check
                if (article.Attribute1 != ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_NONE)
                {
                    ITEM_ATTRIBUTE_CHECK(article.Attribute1, article.AttributeValue1, article.BiologicalEntity);
                }
            }
        }



        /// <summary>
        /// 为一个实体配置物品栏
        /// </summary>
        /// <param name="targetEntity"></param>
        /// <param name="dataid"></param>
        public void SettingArticleFeature(Entity targetEntity, int dataid)
        {

            _entityManager.AddComponentData(targetEntity, new Knapsack
            {
                UpperLimit = 1000000,
                KnapscakCode = 111,
                CurUpper = 10,
            });


            //Dictionary<string, string> values = new Dictionary<string, string>();
            //Dictionary<string, string> vaues1 = new Dictionary<string, string>();


            //List<KeyValuePair<ENUM_ITEM_ATTRIBUTE, string>> valuePairs = new List<KeyValuePair<ENUM_ITEM_ATTRIBUTE, string>>();
            ////valuePairs.Add(new KeyValuePair<ENUM_ITEM_ATTRIBUTE, string>(ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_ATTACKSPEED, "10"));
            ////valuePairs.Add(new KeyValuePair<ENUM_ITEM_ATTRIBUTE, string>(ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_CRIT, "0"));
            ////valuePairs.Add(new KeyValuePair<ENUM_ITEM_ATTRIBUTE, string>(ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_DODGE, "0"));
            ////valuePairs.Add(new KeyValuePair<ENUM_ITEM_ATTRIBUTE, string>(ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_JEWEL_2, "30"));
            ////valuePairs.Add(new KeyValuePair<ENUM_ITEM_ATTRIBUTE, string>(ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_JEWEL_3, "0"));

            //valuePairs.Add(new KeyValuePair<ENUM_ITEM_ATTRIBUTE, string>(ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_MIN_ATTACK, "30"));
            //valuePairs.Add(new KeyValuePair<ENUM_ITEM_ATTRIBUTE, string>(ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_MAX_ATTACK, "35"));
            //valuePairs.Add(new KeyValuePair<ENUM_ITEM_ATTRIBUTE, string>(ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_CRIT, "20"));
            //valuePairs.Add(new KeyValuePair<ENUM_ITEM_ATTRIBUTE, string>(ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_HIT, "90"));
            //valuePairs.Add(new KeyValuePair<ENUM_ITEM_ATTRIBUTE, string>(ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_SKILL_ATTACK, "1"));

            //string json = JsonConvert.SerializeObject(valuePairs);
            //// Debug.Log(JsonConvert.SerializeObject(valuePairs));

            //valuePairs = JsonConvert.DeserializeObject<List<KeyValuePair<ENUM_ITEM_ATTRIBUTE, string>>>(json);


            //Dictionary<int, string> valjson = new Dictionary<int, string>();
            //valjson.Add(1, "100");
            //valjson.Add(2, "200");
            //valjson.Add(3, "300");
            //Debug.Log(JsonConvert.SerializeObject(valjson));

            //string valus = "AAAAA{0},!!!!";

            //Debug.Log(string.Format(valus, 10.333f));

            //  Debug.Log(JsonConvert.SerializeObject(valuePairs));



            List<ArticleData> articleDatas = SQLService.Instance.SimpleQuery<ArticleData>(" Bid=?", dataid);

            for (int i = 0; i < articleDatas.Count; i++)
            {
                var articleData = articleDatas[i];
                Entity entity = _entityManager.CreateEntity(_articleArchetype);
                List<KeyValuePair<ENUM_ITEM_ATTRIBUTE, string>> valuePairs = JsonConvert.DeserializeObject<List<KeyValuePair<ENUM_ITEM_ATTRIBUTE, string>>>(articleData.Value);

                _entityManager.SetComponentData(entity, new ArticleItem
                {
                    BiologicalEntity = targetEntity,
                    GuiId = articleData.Id,
                    Count = articleData.Count,
                    MaxCount = articleData.MaxCount,
                    Weight = articleData.Weight,
                    SpriteId = articleData.AvatarId,
                    ObjectType = ENUM_OBJECT_TYPE.OBJECT_ITEM,
                    ObjectState = ENUM_OBJECT_STATE.OBJECT_INVALID_STATE,

                    Type = (ENUM_ITEM_CLASS)articleData.Type1,
                    Attribute1 = valuePairs.Count >= 1 ? valuePairs[0].Key : ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_NONE,
                    AttributeValue1 = valuePairs.Count >= 1 ? Int32.Parse(valuePairs[0].Value) : 0,
                    Attribute2 = valuePairs.Count >= 2 ? valuePairs[1].Key : ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_NONE,
                    AttributeValue2 = valuePairs.Count >= 2 ? Int32.Parse(valuePairs[1].Value) : 0,
                    Attribute3 = valuePairs.Count >= 3 ? valuePairs[2].Key : ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_NONE,
                    AttributeValue3 = valuePairs.Count >= 3 ? Int32.Parse(valuePairs[2].Value) : 0,
                    Attribute4 = valuePairs.Count >= 4 ? valuePairs[3].Key : ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_NONE,
                    AttributeValue4 = valuePairs.Count >= 4 ? Int32.Parse(valuePairs[3].Value) : 0,
                    Attribute5 = valuePairs.Count >= 5 ? valuePairs[4].Key : ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_NONE,
                    AttributeValue5 = valuePairs.Count >= 5 ? Int32.Parse(valuePairs[4].Value) : 0,
                    Attribute6 = valuePairs.Count >= 6 ? valuePairs[5].Key : ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_NONE,
                    AttributeValue6 = valuePairs.Count >= 6 ? Int32.Parse(valuePairs[5].Value) : 0,
                    Attribute7 = valuePairs.Count >= 7 ? valuePairs[6].Key : ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_NONE,
                    AttributeValue7 = valuePairs.Count >= 7 ? Int32.Parse(valuePairs[6].Value) : 0,
                    Attribute8 = valuePairs.Count >= 8 ? valuePairs[7].Key : ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_NONE,
                    AttributeValue8 = valuePairs.Count >= 8 ? Int32.Parse(valuePairs[7].Value) : 0,
                });

                //赋值实时属性
                GameStaticData.ArticleDictionary.Add(entity, new ArticleItemFixed
                {
                    Desc = articleDatas[i].Text,
                    Name = articleDatas[i].Text,
                    Sprite = null
                });

            }


        }

        /// <summary>
        /// 获取这个实体关联的Article
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public List<Entity> GetEntities(Entity target)
        {
            List<Entity> entities = new List<Entity>();
            for (int i = 0; i < _data.Length; i++)
            {
                if (_data.Items[i].BiologicalEntity == target)
                {
                    entities.Add(_data.Entitys[i]);
                }
            }

            return entities;
        }



        /// <summary>
        /// 属性配置更新
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <param name="entity"></param>
        public static void ITEM_ATTRIBUTE_CHECK(ENUM_ITEM_ATTRIBUTE type, int value, Entity entity)
        {

            switch (type)
            {
                case ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_MIN_ATTACK:
                    {
                        //var fighting = _entityManager.GetComponentData<Fighting>(entity);
                        //fighting.AttackMin += value;
                        //_entityManager.SetComponentData(entity, fighting);
                    }
                    break;
                case ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_MAX_ATTACK:
                    {
                        //var fighting = _entityManager.GetComponentData<Fighting>(entity);
                        //fighting.AttackMax += value;
                        //_entityManager.SetComponentData(entity, fighting);
                    }
                    break;
                case ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_PHYSICS_DEFENCE:
                    {
                        //var fighting = _entityManager.GetComponentData<Fighting>(entity);
                        //fighting.PhysicsDefence += value;
                        //_entityManager.SetComponentData(entity, fighting);
                    }
                    break;
                case ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_MAGIC_DEFENCE:
                    {
                        //var fighting = _entityManager.GetComponentData<Fighting>(entity);
                        //fighting.MagicDefence += value;
                        //_entityManager.SetComponentData(entity, fighting);
                    }
                    break;
                case ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_LIFE:
                    {

                    }
                    break;
                case ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_POWER:
                    {

                    }
                    break;
                case ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_CRIT:
                    {

                    }
                    break;
                case ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_ATTACKSPEED:
                    {

                    }
                    break;
                case ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_HIT:
                    {

                    }
                    break;
                case ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_DODGE:
                    {

                    }
                    break;
                case ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_RECOVER_LIFE:
                    {

                    }
                    break;
                case ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_RECOVER_MAGIC:
                    {

                    }
                    break;
                case ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_SKILL_ATTACK:
                    {

                    }
                    break;
                case ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_SKILL_P_DEFENCE:
                    {

                    }
                    break;
                case ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_SKILL_M_DEFENCE:
                    {

                    }
                    break;
                case ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_SKILL_CRIT:
                    {

                    }
                    break;
                case ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_SKILL_HIT:
                    {

                    }
                    break;
                case ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_SKILL_A_SPEED:
                    {

                    }
                    break;
                case ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_SKILL_DODGE:
                    {

                    }
                    break;
                case ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_POSITION:
                    {

                    }
                    break;
                case ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_CLASS:
                    {

                    }
                    break;
                case ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_PHOTOID:
                    {

                    }
                    break;
                case ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_BASEID:
                    {

                    }
                    break;
                case ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_JEWEL_COUNT:
                    {

                    }
                    break;
                case ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_JEWEL_1:
                    {

                    }
                    break;
                case ENUM_ITEM_ATTRIBUTE.ITEM_ATTRIBUTE_JEWEL_2:
                    {

                    }
                    break;
                default:
                    Debug.Log("未处理");
                    break;
            }
        }




        public static string ArticleType(int type1, int type2, int type3)
        {

            if (type1 == 1)
            {

            }
            else if (type1 == 2)
            {
                if (type2 == 1)
                {
                    return "头盔";
                }else if (type2 == 2)
                {
                    return "护甲";
                }else if (type2 == 3)
                {
                    return "鞋子";
                }else if (type2 == 4)
                {
                    return "腰带";
                }else if (type2 == 5)
                {
                    return "护腕";
                }
                

            }
            else if (type1 == 3)
            {

            }
            else if (type1 == 4)
            {

            }






            return null;
        }
    }

}
