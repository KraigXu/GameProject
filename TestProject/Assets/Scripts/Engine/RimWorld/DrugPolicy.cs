using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B79 RID: 2937
	public class DrugPolicy : IExposable, ILoadReferenceable
	{
		// Token: 0x17000BFE RID: 3070
		// (get) Token: 0x060044C0 RID: 17600 RVA: 0x001736A0 File Offset: 0x001718A0
		public int Count
		{
			get
			{
				return this.entriesInt.Count;
			}
		}

		// Token: 0x17000BFF RID: 3071
		public DrugPolicyEntry this[int index]
		{
			get
			{
				return this.entriesInt[index];
			}
			set
			{
				this.entriesInt[index] = value;
			}
		}

		// Token: 0x17000C00 RID: 3072
		public DrugPolicyEntry this[ThingDef drugDef]
		{
			get
			{
				for (int i = 0; i < this.entriesInt.Count; i++)
				{
					if (this.entriesInt[i].drug == drugDef)
					{
						return this.entriesInt[i];
					}
				}
				throw new ArgumentException();
			}
		}

		// Token: 0x060044C4 RID: 17604 RVA: 0x0000F2A9 File Offset: 0x0000D4A9
		public DrugPolicy()
		{
		}

		// Token: 0x060044C5 RID: 17605 RVA: 0x00173715 File Offset: 0x00171915
		public DrugPolicy(int uniqueId, string label)
		{
			this.uniqueId = uniqueId;
			this.label = label;
			this.InitializeIfNeeded();
		}

		// Token: 0x060044C6 RID: 17606 RVA: 0x00173734 File Offset: 0x00171934
		public void InitializeIfNeeded()
		{
			if (this.entriesInt != null)
			{
				return;
			}
			this.entriesInt = new List<DrugPolicyEntry>();
			List<ThingDef> allDefsListForReading = DefDatabase<ThingDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				if (allDefsListForReading[i].category == ThingCategory.Item && allDefsListForReading[i].IsDrug)
				{
					DrugPolicyEntry drugPolicyEntry = new DrugPolicyEntry();
					drugPolicyEntry.drug = allDefsListForReading[i];
					drugPolicyEntry.allowedForAddiction = true;
					this.entriesInt.Add(drugPolicyEntry);
				}
			}
			this.entriesInt.SortBy((DrugPolicyEntry e) => e.drug.GetCompProperties<CompProperties_Drug>().listOrder);
		}

		// Token: 0x060044C7 RID: 17607 RVA: 0x001737D9 File Offset: 0x001719D9
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.uniqueId, "uniqueId", 0, false);
			Scribe_Values.Look<string>(ref this.label, "label", null, false);
			Scribe_Collections.Look<DrugPolicyEntry>(ref this.entriesInt, "drugs", LookMode.Deep, Array.Empty<object>());
		}

		// Token: 0x060044C8 RID: 17608 RVA: 0x00173815 File Offset: 0x00171A15
		public string GetUniqueLoadID()
		{
			return "DrugPolicy_" + this.label + this.uniqueId.ToString();
		}

		// Token: 0x04002749 RID: 10057
		public int uniqueId;

		// Token: 0x0400274A RID: 10058
		public string label;

		// Token: 0x0400274B RID: 10059
		private List<DrugPolicyEntry> entriesInt;
	}
}
