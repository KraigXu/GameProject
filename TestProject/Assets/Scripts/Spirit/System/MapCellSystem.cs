﻿using System.Collections;
using System.Collections.Generic;
using GameSystem;
using Unity.Entities;
using UnityEngine;


namespace AntiquityWorld
{
    /// <summary>
    /// MapCell
    /// </summary>
    public class MapCellSystem : ComponentSystem
    {
        struct Data
        {
            public readonly int Length;
            public CellMap Maps;
            public HexCell Cells;
        }
        
        private Data _data;

        protected override void OnUpdate()
        {
           
        }


        //public void SetupData()
        //{
        //    GameObject hexgrid=GameObject.Find("");
        //}

    }


}

