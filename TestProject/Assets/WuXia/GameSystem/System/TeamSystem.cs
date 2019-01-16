using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using Unity.Transforms;
using UnityEngine;

namespace GameSystem
{
    /// <summary>
    /// 队伍
    /// </summary>
    public class TeamSystem : ComponentSystem
    {
        struct TeamGroup
        {
            public readonly int Length;
            public ComponentDataArray<BiologicalStatus> Status;
            public ComponentDataArray<Biological> Biological;
            public ComponentDataArray<Team> Team;
        }

        [Inject] private TeamGroup _data;

        protected override void OnUpdate()
        {
            for (int i = 0; i < _data.Length; i++)
            {
                var team = _data.Team[i];
                var status = _data.Status[i];
                var biological = _data.Biological[i];
                if (team.TeamBossId == biological.BiologicalId)
                {
                    switch (status.LocationType)
                    {
                        case LocationType.None:
                            ModelManager.Instance.ModelStatus(team.RunModelCode, false);
                            break;
                        case LocationType.City:
                            if (status.TargetLocationType == LocationType.Field)
                            {
                                status.LocationType = status.TargetLocationType;
                            }
                            ModelManager.Instance.ModelStatus(team.RunModelCode, false);
                            break;
                        case LocationType.Field:
                            ModelManager.Instance.ModelStatus(team.RunModelCode, true);
                            ModelManager.Instance.ModelTarget(team.RunModelCode, status.TargetPosition);
                            break;
                    }
                }
                else
                {

                }

                _data.Biological[i] = biological;
                _data.Status[i] = status;
            }
        }

        public List<Biological> TeamIdRetrunBiological(int biologicalId)
        {
            List<Biological> biologicals = new List<Biological>();

            for (int i = 0; i < _data.Length; i++)
            {
                if (_data.Team[i].TeamBossId == biologicalId)
                {
                    biologicals.Add(_data.Biological[i]);
                }
            }
            return biologicals;
        }
    }

}
