using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;
using UnityEngine.EventSystems;

namespace GameSystem
{
    public class PlayerMapInputSystem : ComponentSystem
    {
        int activeElevation;
        int activeWaterLevel;

        int activeUrbanLevel, activeFarmLevel, activePlantLevel, activeSpecialIndex;

        int activeTerrainTypeIndex;

        int brushSize;

        bool applyElevation = true;
        bool applyWaterLevel = true;

        bool applyUrbanLevel, applyFarmLevel, applyPlantLevel, applySpecialIndex;

        enum OptionalToggle
        {
            Ignore,
            Yes,
            No
        }

        OptionalToggle riverMode, roadMode, walledMode;

        bool isDrag;
        HexDirection dragDirection;
        HexCell previousCell;

        //=========================================

        HexCell currentCell;

        HexUnit selectedUnit;

        public HexCell CurrentCell
        {
            get { return currentCell; }
        }

        public HexUnit CurrentUnit
        {
            get { return CurrentUnit; }
        }


        void DoSelection()
        {
            HexGrid.ClearPath();
            UpdateCurrentCell();
            if (currentCell)
            {
                selectedUnit = currentCell.Unit;
            }
        }

        void DoPathfinding()
        {
            if (UpdateCurrentCell())
            {
                if (currentCell && selectedUnit.IsValidDestination(currentCell))
                {
                    HexGrid.FindPath(selectedUnit.Location, currentCell, selectedUnit);
                }
                else
                {
                    HexGrid.ClearPath();
                }
            }
        }

        void DoMove()
        {
            if (HexGrid.HasPath)
            {
                selectedUnit.Travel(HexGrid.GetPath());
                HexGrid.ClearPath();
            }
        }

        bool UpdateCurrentCell()
        {
            HexCell cell = HexGrid.GetCell(Camera.main.ScreenPointToRay(Input.mousePosition));
            if (cell != currentCell)
            {
                currentCell = cell;
                return true;
            }
            return false;
        }

        //00000000000000000000

        private HexGrid HexGrid
        {
            get { return StrategyScene.Instance.hexGrid;}
        }

        public void SetTerrainTypeIndex(int index)
        {
            activeTerrainTypeIndex = index;
        }

        public void SetApplyElevation(bool toggle)
        {
            applyElevation = toggle;
        }

        public void SetElevation(float elevation)
        {
            activeElevation = (int)elevation;
        }

        public void SetApplyWaterLevel(bool toggle)
        {
            applyWaterLevel = toggle;
        }

        public void SetWaterLevel(float level)
        {
            activeWaterLevel = (int)level;
        }

        public void SetApplyUrbanLevel(bool toggle)
        {
            applyUrbanLevel = toggle;
        }

        public void SetUrbanLevel(float level)
        {
            activeUrbanLevel = (int)level;
        }

        public void SetApplyFarmLevel(bool toggle)
        {
            applyFarmLevel = toggle;
        }

        public void SetFarmLevel(float level)
        {
            activeFarmLevel = (int)level;
        }

        public void SetApplyPlantLevel(bool toggle)
        {
            applyPlantLevel = toggle;
        }

        public void SetPlantLevel(float level)
        {
            activePlantLevel = (int)level;
        }

        public void SetApplySpecialIndex(bool toggle)
        {
            applySpecialIndex = toggle;
        }

        public void SetSpecialIndex(float index)
        {
            activeSpecialIndex = (int)index;
        }

        public void SetBrushSize(float size)
        {
            brushSize = (int)size;
        }

        public void SetRiverMode(int mode)
        {
            riverMode = (OptionalToggle)mode;
        }

        public void SetRoadMode(int mode)
        {
            roadMode = (OptionalToggle)mode;
        }

        public void SetWalledMode(int mode)
        {
            walledMode = (OptionalToggle)mode;
        }

        protected override void OnUpdate()
        {
            return;
            if (!EventSystem.current.IsPointerOverGameObject())
            {
                if (Input.GetMouseButton(0))
                {
                    DoSelection();
                    HandleInput();
                    return;
                }else if (selectedUnit)
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

                //按下U 随机生成角色
                if (Input.GetKeyDown(KeyCode.U))
                {
                    if (Input.GetKey(KeyCode.LeftShift))
                    {
                        DestroyUnit();
                    }
                    else
                    {
                        CreateUnit();
                    }

                    return;
                }
            }

            previousCell = null;
        }


        HexCell GetCellUnderCursor()
        {
            return StrategyScene.Instance.hexGrid.GetCell(Camera.main.ScreenPointToRay(Input.mousePosition));
        }

        void CreateUnit()
        {
            HexCell cell = GetCellUnderCursor();
            if (cell && !cell.Unit)
            {
                HexUnit hexUnit = Object.Instantiate(HexUnit.unitPrefab);
                HexGrid.AddUnit(hexUnit, cell, Random.Range(0f, 360f));
                BiologicalSystem.SpawnRandomBiological(hexUnit.transform);
            }

        }

        void DestroyUnit()
        {
            HexCell cell = GetCellUnderCursor();
            if (cell && cell.Unit)
            {
                HexGrid.RemoveUnit(cell.Unit);
            }
        }

        void HandleInput()
        {
            HexCell currentCell = GetCellUnderCursor();
            if (currentCell)
            {
                if (previousCell && previousCell != currentCell)
                {
                    ValidateDrag(currentCell);
                }
                else
                {
                    isDrag = false;
                }

                EditCells(currentCell);
                previousCell = currentCell;
            }
            else
            {
                previousCell = null;
            }
        }

        void ValidateDrag(HexCell currentCell)
        {
            for (
                dragDirection = HexDirection.NE;
                dragDirection <= HexDirection.NW;
                dragDirection++
            )
            {
                if (previousCell.GetNeighbor(dragDirection) == currentCell)
                {
                    isDrag = true;
                    return;
                }
            }

            isDrag = false;
        }

        void EditCells(HexCell center)
        {
            int centerX = center.coordinates.X;
            int centerZ = center.coordinates.Z;

            for (int r = 0, z = centerZ - brushSize; z <= centerZ; z++, r++)
            {
                for (int x = centerX - r; x <= centerX + brushSize; x++)
                {
                    EditCell(HexGrid.GetCell(new HexCoordinates(x, z)));
                }
            }

            for (int r = 0, z = centerZ + brushSize; z > centerZ; z--, r++)
            {
                for (int x = centerX - brushSize; x <= centerX + r; x++)
                {
                    EditCell(HexGrid.GetCell(new HexCoordinates(x, z)));
                }
            }
        }

        void EditCell(HexCell cell)
        {
            if (cell)
            {
                if (activeTerrainTypeIndex >= 0)
                {
                    cell.TerrainTypeIndex = activeTerrainTypeIndex;
                }

                if (applyElevation)
                {
                    cell.Elevation = activeElevation;
                }

                if (applyWaterLevel)
                {
                    cell.WaterLevel = activeWaterLevel;
                }

                if (applySpecialIndex)
                {
                    cell.SpecialIndex = activeSpecialIndex;
                }

                if (applyUrbanLevel)
                {
                    cell.UrbanLevel = activeUrbanLevel;
                }

                if (applyFarmLevel)
                {
                    cell.FarmLevel = activeFarmLevel;
                }

                if (applyPlantLevel)
                {
                    cell.PlantLevel = activePlantLevel;
                }

                if (riverMode == OptionalToggle.No)
                {
                    cell.RemoveRiver();
                }

                if (roadMode == OptionalToggle.No)
                {
                    cell.RemoveRoads();
                }

                if (walledMode != OptionalToggle.Ignore)
                {
                    cell.Walled = walledMode == OptionalToggle.Yes;
                }

                if (isDrag)
                {
                    HexCell otherCell = cell.GetNeighbor(dragDirection.Opposite());
                    if (otherCell)
                    {
                        if (riverMode == OptionalToggle.Yes)
                        {
                            otherCell.SetOutgoingRiver(dragDirection);
                        }

                        if (roadMode == OptionalToggle.Yes)
                        {
                            otherCell.AddRoad(dragDirection);
                        }
                    }
                }
            }
        }

    }


}