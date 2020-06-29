using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	
	public class DrugPolicyDatabase : IExposable
	{
		
		// (get) Token: 0x0600377F RID: 14207 RVA: 0x00129FF2 File Offset: 0x001281F2
		public List<DrugPolicy> AllPolicies
		{
			get
			{
				return this.policies;
			}
		}

		
		public DrugPolicyDatabase()
		{
			this.GenerateStartingDrugPolicies();
		}

		
		public void ExposeData()
		{
			Scribe_Collections.Look<DrugPolicy>(ref this.policies, "policies", LookMode.Deep, Array.Empty<object>());
		}

		
		public DrugPolicy DefaultDrugPolicy()
		{
			if (this.policies.Count == 0)
			{
				this.MakeNewDrugPolicy();
			}
			return this.policies[0];
		}

		
		public AcceptanceReport TryDelete(DrugPolicy policy)
		{
			foreach (Pawn pawn in PawnsFinder.AllMapsCaravansAndTravelingTransportPods_Alive)
			{
				if (pawn.drugs != null && pawn.drugs.CurrentPolicy == policy)
				{
					return new AcceptanceReport("DrugPolicyInUse".Translate(pawn));
				}
			}
			foreach (Pawn pawn2 in PawnsFinder.AllMapsWorldAndTemporary_AliveOrDead)
			{
				if (pawn2.drugs != null && pawn2.drugs.CurrentPolicy == policy)
				{
					pawn2.drugs.CurrentPolicy = null;
				}
			}
			this.policies.Remove(policy);
			return AcceptanceReport.WasAccepted;
		}

		
		public DrugPolicy MakeNewDrugPolicy()
		{
			int num;
			if (!this.policies.Any<DrugPolicy>())
			{
				num = 1;
			}
			else
			{
				num = this.policies.Max((DrugPolicy o) => o.uniqueId) + 1;
			}
			int uniqueId = num;
			DrugPolicy drugPolicy = new DrugPolicy(uniqueId, "DrugPolicy".Translate() + " " + uniqueId.ToString());
			this.policies.Add(drugPolicy);
			return drugPolicy;
		}

		
		private void GenerateStartingDrugPolicies()
		{
			DrugPolicy drugPolicy = this.MakeNewDrugPolicy();
			drugPolicy.label = "SocialDrugs".Translate();
			drugPolicy[ThingDefOf.Beer].allowedForJoy = true;
			drugPolicy[ThingDefOf.SmokeleafJoint].allowedForJoy = true;
			this.MakeNewDrugPolicy().label = "NoDrugs".Translate();
			DrugPolicy drugPolicy2 = this.MakeNewDrugPolicy();
			drugPolicy2.label = "Unrestricted".Translate();
			for (int i = 0; i < drugPolicy2.Count; i++)
			{
				if (drugPolicy2[i].drug.IsPleasureDrug)
				{
					drugPolicy2[i].allowedForJoy = true;
				}
			}
			DrugPolicy drugPolicy3 = this.MakeNewDrugPolicy();
			drugPolicy3.label = "OneDrinkPerDay".Translate();
			drugPolicy3[ThingDefOf.Beer].allowedForJoy = true;
			drugPolicy3[ThingDefOf.Beer].allowScheduled = true;
			drugPolicy3[ThingDefOf.Beer].takeToInventory = 1;
			drugPolicy3[ThingDefOf.Beer].daysFrequency = 1f;
			drugPolicy3[ThingDefOf.SmokeleafJoint].allowedForJoy = true;
		}

		
		private List<DrugPolicy> policies = new List<DrugPolicy>();
	}
}
