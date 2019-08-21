using System.Collections;
using System.Collections.Generic;
using GameSystem;
using GameSystem.Ui;
using Unity.Entities;
using UnityEngine;

public class PlayerCityUiSystem : ComponentSystem
{

    struct Data
    {
        public readonly int Length;
        public ComponentDataArray<PlayerInput> Input;
        public ComponentDataArray<City> City;
    }

    private MainAssetWindow _mainAsset;
    public void SetupGameObjects()
    {
        _mainAsset = (MainAssetWindow) UICenterMasterManager.Instance.ShowWindow(WindowID.MainAssetWindow);

    }
    protected override void OnUpdate()
    {
        
    }




}
