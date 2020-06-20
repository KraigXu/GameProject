using GameSystem;
using Unity.Collections;
using Unity.Entities;
using Unity.Jobs;

public class EquipmentHelmetSystem : JobComponentSystem {

    struct Data
    {
        public readonly int Length;
        public ExternalProperty ExternalPropertys;
        public EquipmentHelmet EquipmentHelmets;
    }

         private Data _data;

    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        return inputDeps;
    }


    //struct EquipmentHelmetUpdate : IJobProcessComponentData<ExternalProperty, EquipmentHelmet>
    //{
    //    public void Execute(ref ExternalProperty externalProperty, [ReadOnly] ref EquipmentHelmet equipmentHelmet)
    //    {
    //        externalProperty.HelmetProperty1 = equipmentHelmet.Property1;
    //        externalProperty.HelmetProperty2 = equipmentHelmet.Property2;
    //        externalProperty.HelmetProperty3 = equipmentHelmet.Property3;
    //    }

    //}

    //protected override JobHandle OnUpdate(JobHandle inputDeps)
    //{
    //    var job = new EquipmentHelmetUpdate();
    //    return job.Schedule(this,inputDeps);
    //}
}
