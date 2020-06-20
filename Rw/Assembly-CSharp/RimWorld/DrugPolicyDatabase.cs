using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000921 RID: 2337
	public class DrugPolicyDatabase : IExposable
	{
		// Token: 0x170009ED RID: 2541
		// (get) Token: 0x0600377F RID: 14207 RVA: 0x00129FF2 File Offset: 0x001281F2
		public List<DrugPolicy> AllPolicies
		{
			get
			{
				return this.policies;
			}
		}

		// Token: 0x06003780 RID: 14208 RVA: 0x00129FFA File Offset: 0x001281FA
		public DrugPolicyDatabase()
		{
			this.GenerateStartingDrugPolicies();
		}

		// Token: 0x06003781 RID: 14209 RVA: 0x0012A013 File Offset: 0x00128213
		public void ExposeData()
		{
			Scribe_Collections.Look<DrugPolicy>(ref this.policies, "policies", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x06003782 RID: 14210 RVA: 0x0012A02B File Offset: 0x0012822B
		public DrugPolicy DefaultDrugPolicy()
		{
			if (this.policies.Count == 0)
			{
				this.MakeNewDrugPolicy();
			}
			return this.policies[0];
		}

		// Token: 0x06003783 RID: 14211 RVA: 0x0012A050 File Offset: 0x00128250
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

		// Token: 0x06003784 RID: 14212 RVA: 0x0012A140 File Offset: 0x00128340
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

		// Token: 0x06003785 RID: 14213 RVA: 0x0012A1C4 File Offset: 0x001283C4
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

		// Token: 0x040020E9 RID: 8425
		private List<DrugPolicy> policies = new List<DrugPolicy>();
	}
}
