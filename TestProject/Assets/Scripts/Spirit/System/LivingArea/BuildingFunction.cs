using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

public interface BuildingFunction
{


    bool IsBuilding(Entity entity);

    UiBuildingItem GetBuildingItem(Entity entity);

    void OpenUi(Entity entity);

    void AnalysisDataSet(Entity entity, string[] values);
}
