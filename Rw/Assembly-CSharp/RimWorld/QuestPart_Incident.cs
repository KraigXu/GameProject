using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x020009A5 RID: 2469
	public class QuestPart_Incident : QuestPart
	{
		// Token: 0x17000A91 RID: 2705
		// (get) Token: 0x06003AA1 RID: 15009 RVA: 0x0013671A File Offset: 0x0013491A
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in this.<>n__0())
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.mapParent != null)
				{
					yield return this.mapParent;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x06003AA2 RID: 15010 RVA: 0x0013672C File Offset: 0x0013492C
		public override void Notify_QuestSignalReceived(Signal signal)
		{
			base.Notify_QuestSignalReceived(signal);
			if (signal.tag == this.inSignal && this.incidentParms != null)
			{
				if (!this.incidentParms.forced)
				{
					Log.Error("QuestPart incident should always be forced but it's not. incident=" + this.incident, false);
					this.incidentParms.forced = true;
				}
				this.incidentParms.quest = this.quest;
				if (this.mapParent != null)
				{
					if (this.mapParent.HasMap)
					{
						this.incidentParms.target = this.mapParent.Map;
						if (this.incident.Worker.CanFireNow(this.incidentParms, true))
						{
							this.incident.Worker.TryExecute(this.incidentParms);
						}
						this.incidentParms.target = null;
					}
				}
				else if (this.incidentParms.target != null && this.incident.Worker.CanFireNow(this.incidentParms, true))
				{
					this.incident.Worker.TryExecute(this.incidentParms);
				}
				this.incidentParms = null;
			}
		}

		// Token: 0x06003AA3 RID: 15011 RVA: 0x00136854 File Offset: 0x00134A54
		public void SetIncidentParmsAndRemoveTarget(IncidentParms value)
		{
			this.incidentParms = value;
			Map map = this.incidentParms.target as Map;
			if (map != null)
			{
				this.mapParent = map.Parent;
				this.incidentParms.target = null;
				return;
			}
			this.mapParent = null;
		}

		// Token: 0x06003AA4 RID: 15012 RVA: 0x0013689C File Offset: 0x00134A9C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.inSignal, "inSignal", null, false);
			Scribe_Defs.Look<IncidentDef>(ref this.incident, "incident");
			Scribe_Deep.Look<IncidentParms>(ref this.incidentParms, "incidentParms", Array.Empty<object>());
			Scribe_References.Look<MapParent>(ref this.mapParent, "mapParent", false);
		}

		// Token: 0x06003AA5 RID: 15013 RVA: 0x001368F8 File Offset: 0x00134AF8
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.inSignal = "DebugSignal" + Rand.Int;
			if (Find.AnyPlayerHomeMap != null)
			{
				this.incident = IncidentDefOf.RaidEnemy;
				this.SetIncidentParmsAndRemoveTarget(new IncidentParms
				{
					target = Find.RandomPlayerHomeMap,
					points = 500f
				});
			}
		}

		// Token: 0x04002299 RID: 8857
		public string inSignal;

		// Token: 0x0400229A RID: 8858
		public IncidentDef incident;

		// Token: 0x0400229B RID: 8859
		private IncidentParms incidentParms;

		// Token: 0x0400229C RID: 8860
		private MapParent mapParent;
	}
}
