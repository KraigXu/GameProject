using System;
using System.Collections.Generic;
using RimWorld.Planet;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class QuestPart_Delay : QuestPartActivable
	{
		
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

		
		// (get) Token: 0x0600382C RID: 14380 RVA: 0x0012D40D File Offset: 0x0012B60D
		public override string ExpiryInfoPartTip
		{
			get
			{
				return this.expiryInfoPartTip.Formatted(GenDate.DateFullStringWithHourAt((long)GenDate.TickGameToAbs(this.enableTick + this.delayTicks), QuestUtility.GetLocForDates()));
			}
		}

		
		// (get) Token: 0x0600382D RID: 14381 RVA: 0x0012D441 File Offset: 0x0012B641
		public override IEnumerable<GlobalTargetInfo> QuestLookTargets
		{
			get
			{
				foreach (GlobalTargetInfo globalTargetInfo in this.n__0())
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

		
		public override void QuestPartTick()
		{
			base.QuestPartTick();
			if (Find.TickManager.TicksGame >= this.enableTick + this.delayTicks)
			{
				this.DelayFinished();
			}
		}

		
		protected virtual void DelayFinished()
		{
			base.Complete();
		}

		
		public override string ExtraInspectString(ISelectable target)
		{
			if (this.inspectStringTargets != null && this.inspectStringTargets.Contains(target))
			{
				return this.inspectString.Formatted(this.TicksLeft.ToStringTicksToPeriod(true, false, true, true));
			}
			return null;
		}

		
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

		
		public override void AssignDebugData()
		{
			base.AssignDebugData();
			this.delayTicks = Rand.RangeInclusive(833, 2500);
		}

		
		public void DebugForceEnd()
		{
			this.DelayFinished();
		}

		
		public int delayTicks;

		
		public string expiryInfoPart;

		
		public string expiryInfoPartTip;

		
		public string inspectString;

		
		public List<ISelectable> inspectStringTargets;

		
		public bool isBad;
	}
}
