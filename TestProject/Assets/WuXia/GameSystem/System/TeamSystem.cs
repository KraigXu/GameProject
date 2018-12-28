using System.Collections;
using System.Collections.Generic;
using Unity.Entities;
using UnityEngine;

namespace GameSystem
{
    public class TeamSystem : ComponentSystem
    {

        struct TeamGroup
        {
            public readonly int Length;
            public ComponentDataArray<Biological> Biological;
            public ComponentDataArray<Team> Team;
        }

        [Inject] private TeamGroup _teamGroup;

        protected override void OnUpdate()
        {
        }

        public List<Biological> TeamIdRetrunBiological(int biologicalId)
        {
            List<Biological> biologicals=new List<Biological>();

            for (int i = 0; i < _teamGroup.Length; i++)
            {
                if (_teamGroup.Team[i].TeamBossId == biologicalId)
                {
                    biologicals.Add(_teamGroup.Biological[i]);
                }
            }
            return biologicals;
        }
    }

}
