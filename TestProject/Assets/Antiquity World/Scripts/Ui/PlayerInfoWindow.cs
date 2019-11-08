using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Unity.Entities;
using UnityEngine;
using UnityEngine.UI;
using GameSystem;
using UnityEngine.EventSystems;

namespace GameSystem.Ui
{
    public class PlayerInfoWindow : UIWindowBase
    {


        public UiBiologicalAvatarItem[] Items;
        public int PlayNumber;


        public Image AvateImage;
        public Text NameText;

        public RectTransform RestBtns;
        public RectTransform TeamBtns;
        public RectTransform PersonBtns;
        public RectTransform LogBtns;
        public RectTransform IntelligenceBtns;
        public RectTransform OptionBtns;

        public HexGrid grid;
        public RectTransform mouseview;
        public RectTransform PrefabItem;

        HexCell currentCell;

        HexUnit unit;

        [HideInInspector]
        public bool Isflag=true;
        protected override void InitWindowData()
        {
            this.ID = WindowID.PlayerInfoWindow;

            windowData.windowType = UIWindowType.NormalLayer;
            windowData.showMode = UIWindowShowMode.DoNothing;
            windowData.navigationMode = UIWindowNavigationMode.IgnoreNavigation;
            windowData.colliderMode = UIWindowColliderMode.None;
            windowData.closeModel = UIWindowCloseModel.Hide;
        }

        public override void InitWindowOnAwake()
        {
            //AvateImage.overrideSprite = StrategyPlayer.AvatarSprite;
            //NameText.text = StrategyPlayer.SurName + StrategyPlayer.Name;
            //grid = StrategyScene.Instance.hexGrid;
            //unit = StrategyPlayer.Unit;
            transform.Find("Relation").GetComponent<Button>().onClick.AddListener(OpenRelation);
        }

        public void OpenRelation()
        {
            //UICenterMasterManager.Instance.ShowWindow(WindowID.)
        }

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

        

        


        public void SetRestTog(bool toggle)
        {
            RestBtns.gameObject.SetActive(toggle);
        }

        public void SetTeamTog(bool toggle)
        {
            TeamBtns.gameObject.SetActive(toggle);

        }

        public void SetPersonTog(bool toggle)
        {
            PersonBtns.gameObject.SetActive(toggle);
        }

        public void SetLogTog(bool toggle)
        {
            LogBtns.gameObject.SetActive(toggle);
        }

        public void SetIntelligenceTog(bool toggle)
        {
            IntelligenceBtns.gameObject.SetActive(toggle);
        }

        public void SetOptionTog(bool toggle)
        {
            OptionBtns.gameObject.SetActive(toggle);
        }
        public void SetEditMode(bool toggle)
        {
            enabled = !toggle;
            grid.ShowUI(!toggle);
            grid.ClearPath();
            if (toggle)
            {
                Shader.EnableKeyword("HEX_MAP_EDIT_MODE");
            }
            else
            {
                Shader.DisableKeyword("HEX_MAP_EDIT_MODE");
            }
        }

        void Update()
        {
            if (!Isflag) return;

            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (Input.GetMouseButtonDown(0))
                {
                    DoSelection();
                }
                else if (unit)
                {
                    if (Input.GetMouseButtonDown(1))
                    {
                        DoMove();
                    }
                    else
                    {
                        DoPathfinding();
                    }
                }
            }
        }

        void DoSelection()
        {
            grid.ClearPath();
            UpdateCurrentCell();
            if (currentCell)
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
                if (currentCell && unit.IsValidDestination(currentCell))
                {
                    grid.FindPath(unit.Location, currentCell, unit);

                    
                }
                else
                {
                    grid.ClearPath();
                }
            }
        }

        void DoMove()
        {
            if (grid.HasPath)
            {
                unit.Travel(grid.GetPath());
                grid.ClearPath();
            }
        }

        bool UpdateCurrentCell()
        {
            HexCell cell = grid.GetCell(Camera.main.ScreenPointToRay(Input.mousePosition));
            if (cell != currentCell)
            {
                currentCell = cell;
                return true;
            }
            return false;
        }


    }
}