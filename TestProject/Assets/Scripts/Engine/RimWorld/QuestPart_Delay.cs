using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000940 RID: 2368
	public class QuestPart_Delay : QuestPartActivable
	{
		// Token: 0x17000A13 RID: 2579
		// (get) Token: 0x0600382A RID: 14378 RVA: 0x0012D3B3 File Offset: 0x0012B5B3
		public int TicksLeft
		{
			get
			{
				if (base.State != QuestPartState.Enabled)
				{
					return 0;
				}
				return this.enableTick + this.delayTicks - Find.TickManager.TicksGame;
			}
		}

		// Token: 0x17000A14 RID: 2580
		// (get) Token: 0x0600382B RID: 14379 RVA: 0x0012D3D8 File Offset: 0x0012B5D8
		public override string ExpiryInfoPart
		{
			get
			{
				if (this.quest.Historical)
				{
					return null;
				}
				return this.expiryInfoPart.Formatted(this.TicksLeft.ToStringTicksToPeriod(true, false, true, true));
			}
		}

		// Token: 0x17000A15 RID: 2581
		// (get) Token: 0x0600382C RID: 14380 RVA: 0x0012D40D File Offset: 0x0012B60D
		public override string ExpiryInfoPartTip
		{
			get
			{
				return this.expiryInfoPartTip.Formatted(GenDate.DateFullStringWithHourAt((long)GenDate.TickGameToAbs(this.enableTick + this.delayTicks), QuestUtility.GetLocForDates()));
			}
		}

		// Token: 0x17000A16 RID: 2582
		// (get) Token: 0x0600382D RID: 14381 RVA: 0x0012D441 File Offset: 0x0012B641
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in this.<>n__0())
				{
					yield return globalTargetInfo;
				}
				IEnumerator<GlobalTargetInfo> enumerator = null;
				if (this.inspectStringTargets != null)
				{
					int num;
					for (int i = 0; i < this.inspectStringTargets.Count; i = num + 1)
					{
						ISelectable selectable = this.inspectStringTargets[i];
						if (selectable is Thing)
						{
							yield return (Thing)selectable;
						}
						else if (selectable is WorldObject)
						{
							yield return (WorldObject)selectable;
						}
						num = i;
					}
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x0600382E RID: 14382 RVA: 0x0012D451 File Offset: 0x0012B651
		public override void QuestPartTick()
		{
			base.QuestPartTick();
			if (Find.TickManager.TicksGame >= this.enableTick + this.delayTicks)
			{
				this.DelayFinished();
			}
		}

		// Token: 0x0600382F RID: 14383 RVA: 0x0012D478 File Offset: 0x0012B678
		protected virtual void DelayFinished()
		{
			base.Complete();
		}

		// Token: 0x06003830 RID: 14384 RVA: 0x0012D480 File Offset: 0x0012B680
		public override string ExtraInspectString(ISelectable target)
		{
			if (this.inspectStringTargets != null && this.inspectStringTargets.Contains(target))
			{
				return this.inspectString.Formatted(this.TicksLeft.ToStringTicksToPeriod(true, false, true, true));
			}
			return null;
		}

		// Token: 0x06003831 RID: 14385 RVA: 0x0012D4C0 File Offset: 0x0012B6C0
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.delayTicks, "delayTicks", 0, false);
			Scribe_Values.Look<string>(ref this.expiryInfoPart, "expiryInfoPart", null, false);
			Scribe_Values.Look<string>(ref this.expiryInfoPartTip, "expiryInfoPartTip", null, false);
			Scribe_Values.Look<string>(ref this.inspectString, "inspectString", null, false);
			Scribe_Collections.Look<ISelectable>(ref this.inspectStringTargets, "inspectStringTargets", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.isBad, "isBad", false, false);
		}

		// Token: 0x06003832 RID: 14386 RVA: 0x0012D544 File Offset: 0x0012B744
		public override void DoDebugWindowContents(Rect innerRect, ref float curY)
		{
			if (base.State != QuestPartState.Enabled)
			{
				return;
			}
			Rect rect = new Rect(innerRect.x, curY, 500f, 25f);
			if (Widgets.ButtonText(rect, "End " + this.ToString(), true, true, true))
			{
				this.DelayFinished();
			}
			curY += rect.height + 4f;
		}

		// Token: 0x06003833 RID: 14387 RVA: 0x0012D5A8 File Offset: 0x0012B7A8
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.delayTicks = Rand.RangeInclusive(833, 2500);
		}

		// Token: 0x06003834 RID: 14388 RVA: 0x0012D5C5 File Offset: 0x0012B7C5
		public void DebugForceEnd()
		{
			this.DelayFinished();
		}

		// Token: 0x0400213B RID: 8507
		public int delayTicks;

		// Token: 0x0400213C RID: 8508
		public string expiryInfoPart;

		// Token: 0x0400213D RID: 8509
		public string expiryInfoPartTip;

		// Token: 0x0400213E RID: 8510
		public string inspectString;

		// Token: 0x0400213F RID: 8511
		public List<ISelectable> inspectStringTargets;

		// Token: 0x04002140 RID: 8512
		public bool isBad;
	}
}
