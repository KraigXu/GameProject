using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000F2B RID: 3883
	public class TutorialState : IExposable
	{
		// Token: 0x06005F19 RID: 24345 RVA: 0x0020D0CC File Offset: 0x0020B2CC
		public void ExposeData()
		{
			if (Scribe.mode == LoadSaveMode.Saving && this.startingItems != null)
			{
				this.startingItems.RemoveAll((Thing it) => it == null || it.Destroyed || (it.Map == null && it.MapHeld == null));
			}
			Scribe_Collections.Look<Thing>(ref this.startingItems, "startingItems", LookMode.Reference, Array.Empty<object>());
			Scribe_Values.Look<CellRect>(ref this.roomRect, "roomRect", default(CellRect), false);
			Scribe_Values.Look<CellRect>(ref this.sandbagsRect, "sandbagsRect", default(CellRect), false);
			Scribe_Values.Look<int>(ref this.endTick, "endTick", -1, false);
			Scribe_Values.Look<bool>(ref this.introDone, "introDone", false, false);
			if (this.startingItems != null)
			{
				this.startingItems.RemoveAll((Thing it) => it == null);
			}
		}

		// Token: 0x06005F1A RID: 24346 RVA: 0x0020D1B5 File Offset: 0x0020B3B5
		public void Notify_TutorialEnding()
		{
			this.startingItems.Clear();
			this.roomRect = default(CellRect);
			this.sandbagsRect = default(CellRect);
			this.endTick = Find.TickManager.TicksGame;
		}

		// Token: 0x06005F1B RID: 24347 RVA: 0x0020D1EA File Offset: 0x0020B3EA
		public void AddStartingItem(Thing t)
		{
			if (this.startingItems.Contains(t))
			{
				return;
			}
			this.startingItems.Add(t);
		}

		// Token: 0x04003393 RID: 13203
		public List<Thing> startingItems = new List<Thing>();

		// Token: 0x04003394 RID: 13204
		public CellRect roomRect;

		// Token: 0x04003395 RID: 13205
		public CellRect sandbagsRect;

		// Token: 0x04003396 RID: 13206
		public int endTick = -1;

		// Token: 0x04003397 RID: 13207
		public bool introDone;
	}
}
