using System.Collections;
using System.Collections.Generic;
using DataAccessObject;
using TinyFrameWork;
using UnityEngine;

namespace LivingArea
{

    /// <summary>
    /// 居住地节点 ，核心逻辑解析解析建筑物
    /// </summary>
    public class LivingAreaNode : MonoBehaviour
    {

        public int Id;         //这个Id需要手动输入， 映射到数据库中的Id        
        public LivingAreaData Value;
        public BuildingObject[] BuildingObjects;
        
        public int Renown;      
       
        public GameObject LivingAreaM;

        public Renderer LivingAreaRender;
        public Collider LivingAreaCollider;

        public bool IsOpen =false;  //是否打开

        public LivingAreaState[] Groups;
        void Start()
        {
        }
        void Update()
        {

            if (IsOpen == true)
            {
                for (int i = 0; i < Groups.Length; i++)
                {
                    Groups[i].UpdateBehaviors(this);
                }
            }
            else
            {
                
            }
        }
    }
}
