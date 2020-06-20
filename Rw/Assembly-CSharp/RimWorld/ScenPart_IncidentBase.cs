using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C1C RID: 3100
	public abstract class ScenPart_IncidentBase : ScenPart
	{
		// Token: 0x17000D0C RID: 3340
		// (get) Token: 0x060049E5 RID: 18917 RVA: 0x00190322 File Offset: 0x0018E522
		public IncidentDef Incident
		{
			get
			{
				return this.incident;
			}
		}

		// Token: 0x17000D0D RID: 3341
		// (get) Token: 0x060049E6 RID: 18918
		protected abstract string IncidentTag { get; }

		// Token: 0x060049E7 RID: 18919 RVA: 0x0019032C File Offset: 0x0018E52C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<IncidentDef>(ref this.incident, "incident");
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.incident == null)
			{
				this.incident = this.RandomizableIncidents().FirstOrDefault<IncidentDef>();
				Log.Error("ScenPart has null incident after loading. Changing to " + this.incident.ToStringSafe<IncidentDef>(), false);
			}
		}

		// Token: 0x060049E8 RID: 18920 RVA: 0x0019038C File Offset: 0x0018E58C
		public override void DoEditInterface(Listing_ScenEdit listing)
		{
			Rect scenPartRect = listing.GetScenPartRect(this, ScenPart.RowHeight);
			this.DoIncidentEditInterface(scenPartRect);
		}

		// Token: 0x060049E9 RID: 18921 RVA: 0x001903B0 File Offset: 0x0018E5B0
		public override string Summary(Scenario scen)
		{
			string key = "ScenPart_" + this.IncidentTag;
			return ScenSummaryList.SummaryWithList(scen, this.IncidentTag, key.Translate());
		}

		// Token: 0x060049EA RID: 18922 RVA: 0x001903E5 File Offset: 0x0018E5E5
		public override IEnumerable<string> GetSummaryListEntries(string tag)
		{
			if (tag == this.IncidentTag)
			{
				yield return this.incident.LabelCap;
			}
			yield break;
		}

		// Token: 0x060049EB RID: 18923 RVA: 0x001903FC File Offset: 0x0018E5FC
		public override void Randomize()
		{
			this.incident = this.RandomizableIncidents().RandomElement<IncidentDef>();
		}

		// Token: 0x060049EC RID: 18924 RVA: 0x00190410 File Offset: 0x0018E610
		public override bool TryMerge(ScenPart other)
		{
			ScenPart_IncidentBase scenPart_IncidentBase = other as ScenPart_IncidentBase;
			return scenPart_IncidentBase != null && scenPart_IncidentBase.Incident == this.incident;
		}

		// Token: 0x060049ED RID: 18925 RVA: 0x00190438 File Offset: 0x0018E638
		public override bool CanCoexistWith(ScenPart other)
		{
			ScenPart_IncidentBase scenPart_IncidentBase = other as ScenPart_IncidentBase;
			return scenPart_IncidentBase == null || scenPart_IncidentBase.Incident != this.incident;
		}

		// Token: 0x060049EE RID: 18926 RVA: 0x00190460 File Offset: 0x0018E660
		protected virtual IEnumerable<IncidentDef> RandomizableIncidents()
		{
			return Enumerable.Empty<IncidentDef>();
		}

		// Token: 0x060049EF RID: 18927 RVA: 0x00190468 File Offset: 0x0018E668
		protected void DoIncidentEditInterface(Rect rect)
		{
			if (Widgets.ButtonText(rect, this.incident.LabelCap, true, true, true))
			{
				FloatMenuUtility.MakeMenu<IncidentDef>(DefDatabase<IncidentDef>.AllDefs, (IncidentDef id) => id.LabelCap, (IncidentDef id) => delegate
				{
					this.incident = id;
				});
			}
		}

		// Token: 0x04002A07 RID: 10759
		protected IncidentDef incident;
	}
}
