using System;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A77 RID: 2679
	public class CompHibernatable : ThingComp
	{
		// Token: 0x17000B35 RID: 2869
		// (get) Token: 0x06003F2F RID: 16175 RVA: 0x0014FFDB File Offset: 0x0014E1DB
		public CompProperties_Hibernatable Props
		{
			get
			{
				return (CompProperties_Hibernatable)this.props;
			}
		}

		// Token: 0x17000B36 RID: 2870
		// (get) Token: 0x06003F30 RID: 16176 RVA: 0x0014FFE8 File Offset: 0x0014E1E8
		// (set) Token: 0x06003F31 RID: 16177 RVA: 0x0014FFF0 File Offset: 0x0014E1F0
		public HibernatableStateDef State
		{
			get
			{
				return this.state;
			}
			set
			{
				if (this.state == value)
				{
					return;
				}
				this.state = value;
				this.parent.Map.info.parent.Notify_HibernatableChanged();
			}
		}

		// Token: 0x17000B37 RID: 2871
		// (get) Token: 0x06003F32 RID: 16178 RVA: 0x0015001D File Offset: 0x0014E21D
		public bool Running
		{
			get
			{
				return this.State == HibernatableStateDefOf.Running;
			}
		}

		// Token: 0x06003F33 RID: 16179 RVA: 0x0015002C File Offset: 0x0014E22C
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			if (!respawningAfterLoad)
			{
				this.parent.Map.info.parent.Notify_HibernatableChanged();
			}
		}

		// Token: 0x06003F34 RID: 16180 RVA: 0x00150052 File Offset: 0x0014E252
		public override void PostDeSpawn(Map map)
		{
			base.PostDeSpawn(map);
			map.info.parent.Notify_HibernatableChanged();
		}

		// Token: 0x06003F35 RID: 16181 RVA: 0x0015006C File Offset: 0x0014E26C
		public void Startup()
		{
			if (this.State != HibernatableStateDefOf.Hibernating)
			{
				Log.ErrorOnce("Attempted to start a non-hibernating object", 34361223, false);
				return;
			}
			this.State = HibernatableStateDefOf.Starting;
			this.endStartupTick = Mathf.RoundToInt((float)Find.TickManager.TicksGame + this.Props.startupDays * 60000f);
		}

		// Token: 0x06003F36 RID: 16182 RVA: 0x001500CC File Offset: 0x0014E2CC
		public override string CompInspectStringExtra()
		{
			if (this.State == HibernatableStateDefOf.Hibernating)
			{
				return "HibernatableHibernating".Translate();
			}
			if (this.State == HibernatableStateDefOf.Starting)
			{
				return string.Format("{0}: {1}", "HibernatableStartingUp".Translate(), (this.endStartupTick - Find.TickManager.TicksGame).ToStringTicksToPeriod(true, false, true, true));
			}
			return null;
		}

		// Token: 0x06003F37 RID: 16183 RVA: 0x00150138 File Offset: 0x0014E338
		public override void CompTick()
		{
			if (this.State == HibernatableStateDefOf.Starting && Find.TickManager.TicksGame > this.endStartupTick)
			{
				this.State = HibernatableStateDefOf.Running;
				this.endStartupTick = 0;
				string str;
				if (this.parent.Map.Parent.GetComponent<EscapeShipComp>() != null)
				{
					str = "LetterHibernateComplete".Translate();
				}
				else
				{
					str = "LetterHibernateCompleteStandalone".Translate();
				}
				Find.LetterStack.ReceiveLetter("LetterLabelHibernateComplete".Translate(), str, LetterDefOf.PositiveEvent, new GlobalTargetInfo(this.parent), null, null, null, null);
			}
		}

		// Token: 0x06003F38 RID: 16184 RVA: 0x001501E5 File Offset: 0x0014E3E5
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Defs.Look<HibernatableStateDef>(ref this.state, "hibernateState");
			Scribe_Values.Look<int>(ref this.endStartupTick, "hibernateendStartupTick", 0, false);
		}

		// Token: 0x040024C2 RID: 9410
		private HibernatableStateDef state = HibernatableStateDefOf.Hibernating;

		// Token: 0x040024C3 RID: 9411
		private int endStartupTick;
	}
}
