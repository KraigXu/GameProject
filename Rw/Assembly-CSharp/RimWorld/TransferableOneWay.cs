using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E7C RID: 3708
	public class TransferableOneWay : Transferable
	{
		// Token: 0x17001034 RID: 4148
		// (get) Token: 0x06005A1C RID: 23068 RVA: 0x001E6E62 File Offset: 0x001E5062
		public override Thing AnyThing
		{
			get
			{
				if (!this.HasAnyThing)
				{
					return null;
				}
				return this.things[0];
			}
		}

		// Token: 0x17001035 RID: 4149
		// (get) Token: 0x06005A1D RID: 23069 RVA: 0x001C5D00 File Offset: 0x001C3F00
		public override ThingDef ThingDef
		{
			get
			{
				if (!this.HasAnyThing)
				{
					return null;
				}
				return this.AnyThing.def;
			}
		}

		// Token: 0x17001036 RID: 4150
		// (get) Token: 0x06005A1E RID: 23070 RVA: 0x001E6E7A File Offset: 0x001E507A
		public override bool HasAnyThing
		{
			get
			{
				return this.things.Count != 0;
			}
		}

		// Token: 0x17001037 RID: 4151
		// (get) Token: 0x06005A1F RID: 23071 RVA: 0x001C5C54 File Offset: 0x001C3E54
		public override string Label
		{
			get
			{
				return this.AnyThing.LabelNoCount;
			}
		}

		// Token: 0x17001038 RID: 4152
		// (get) Token: 0x06005A20 RID: 23072 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool Interactive
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17001039 RID: 4153
		// (get) Token: 0x06005A21 RID: 23073 RVA: 0x0001028D File Offset: 0x0000E48D
		public override TransferablePositiveCountDirection PositiveCountDirection
		{
			get
			{
				return TransferablePositiveCountDirection.Destination;
			}
		}

		// Token: 0x1700103A RID: 4154
		// (get) Token: 0x06005A22 RID: 23074 RVA: 0x001C5D2E File Offset: 0x001C3F2E
		public override string TipDescription
		{
			get
			{
				if (!this.HasAnyThing)
				{
					return "";
				}
				return this.AnyThing.DescriptionDetailed;
			}
		}

		// Token: 0x1700103B RID: 4155
		// (get) Token: 0x06005A23 RID: 23075 RVA: 0x001E6E8A File Offset: 0x001E508A
		// (set) Token: 0x06005A24 RID: 23076 RVA: 0x001E6E92 File Offset: 0x001E5092
		public override int CountToTransfer
		{
			get
			{
				return this.countToTransfer;
			}
			protected set
			{
				this.countToTransfer = value;
				base.EditBuffer = value.ToStringCached();
			}
		}

		// Token: 0x1700103C RID: 4156
		// (get) Token: 0x06005A25 RID: 23077 RVA: 0x001E6EA8 File Offset: 0x001E50A8
		public int MaxCount
		{
			get
			{
				int num = 0;
				for (int i = 0; i < this.things.Count; i++)
				{
					num += this.things[i].stackCount;
				}
				return num;
			}
		}

		// Token: 0x06005A26 RID: 23078 RVA: 0x00010306 File Offset: 0x0000E506
		public override int GetMinimumToTransfer()
		{
			return 0;
		}

		// Token: 0x06005A27 RID: 23079 RVA: 0x001E6EE2 File Offset: 0x001E50E2
		public override int GetMaximumToTransfer()
		{
			return this.MaxCount;
		}

		// Token: 0x06005A28 RID: 23080 RVA: 0x001E6EEA File Offset: 0x001E50EA
		public override AcceptanceReport OverflowReport()
		{
			return new AcceptanceReport("ColonyHasNoMore".Translate());
		}

		// Token: 0x06005A29 RID: 23081 RVA: 0x001E6F00 File Offset: 0x001E5100
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.things.RemoveAll((Thing x) => x.Destroyed);
			}
			Scribe_Values.Look<int>(ref this.countToTransfer, "countToTransfer", 0, false);
			Scribe_Collections.Look<Thing>(ref this.things, "things", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				base.EditBuffer = this.countToTransfer.ToStringCached();
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.things.RemoveAll((Thing x) => x == null) != 0)
				{
					Log.Warning("Some of the things were null after loading.", false);
				}
			}
		}

		// Token: 0x040030E4 RID: 12516
		public List<Thing> things = new List<Thing>();

		// Token: 0x040030E5 RID: 12517
		private int countToTransfer;
	}
}
