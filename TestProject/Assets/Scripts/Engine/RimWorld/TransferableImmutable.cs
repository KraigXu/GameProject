using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E7B RID: 3707
	public class TransferableImmutable : Transferable
	{
		// Token: 0x17001029 RID: 4137
		// (get) Token: 0x06005A0B RID: 23051 RVA: 0x001E6CF7 File Offset: 0x001E4EF7
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

		// Token: 0x1700102A RID: 4138
		// (get) Token: 0x06005A0C RID: 23052 RVA: 0x001C5D00 File Offset: 0x001C3F00
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

		// Token: 0x1700102B RID: 4139
		// (get) Token: 0x06005A0D RID: 23053 RVA: 0x001E6D0F File Offset: 0x001E4F0F
		public override bool HasAnyThing
		{
			get
			{
				return this.things.Count != 0;
			}
		}

		// Token: 0x1700102C RID: 4140
		// (get) Token: 0x06005A0E RID: 23054 RVA: 0x001C5C54 File Offset: 0x001C3E54
		public override string Label
		{
			get
			{
				return this.AnyThing.LabelNoCount;
			}
		}

		// Token: 0x1700102D RID: 4141
		// (get) Token: 0x06005A0F RID: 23055 RVA: 0x00010306 File Offset: 0x0000E506
		public override bool Interactive
		{
			get
			{
				return false;
			}
		}

		// Token: 0x1700102E RID: 4142
		// (get) Token: 0x06005A10 RID: 23056 RVA: 0x0001028D File Offset: 0x0000E48D
		public override TransferablePositiveCountDirection PositiveCountDirection
		{
			get
			{
				return TransferablePositiveCountDirection.Destination;
			}
		}

		// Token: 0x1700102F RID: 4143
		// (get) Token: 0x06005A11 RID: 23057 RVA: 0x001C5D2E File Offset: 0x001C3F2E
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

		// Token: 0x17001030 RID: 4144
		// (get) Token: 0x06005A12 RID: 23058 RVA: 0x00010306 File Offset: 0x0000E506
		// (set) Token: 0x06005A13 RID: 23059 RVA: 0x001E6D1F File Offset: 0x001E4F1F
		public override int CountToTransfer
		{
			get
			{
				return 0;
			}
			protected set
			{
				if (value != 0)
				{
					throw new InvalidOperationException("immutable transferable");
				}
			}
		}

		// Token: 0x17001031 RID: 4145
		// (get) Token: 0x06005A14 RID: 23060 RVA: 0x001E6D30 File Offset: 0x001E4F30
		public string LabelWithTotalStackCount
		{
			get
			{
				string text = this.Label;
				int totalStackCount = this.TotalStackCount;
				if (totalStackCount != 1)
				{
					text = text + " x" + totalStackCount.ToStringCached();
				}
				return text;
			}
		}

		// Token: 0x17001032 RID: 4146
		// (get) Token: 0x06005A15 RID: 23061 RVA: 0x001E6D62 File Offset: 0x001E4F62
		public string LabelCapWithTotalStackCount
		{
			get
			{
				return this.LabelWithTotalStackCount.CapitalizeFirst(this.ThingDef);
			}
		}

		// Token: 0x17001033 RID: 4147
		// (get) Token: 0x06005A16 RID: 23062 RVA: 0x001E6D78 File Offset: 0x001E4F78
		public int TotalStackCount
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

		// Token: 0x06005A17 RID: 23063 RVA: 0x00010306 File Offset: 0x0000E506
		public override int GetMinimumToTransfer()
		{
			return 0;
		}

		// Token: 0x06005A18 RID: 23064 RVA: 0x00010306 File Offset: 0x0000E506
		public override int GetMaximumToTransfer()
		{
			return 0;
		}

		// Token: 0x06005A19 RID: 23065 RVA: 0x001E6CDC File Offset: 0x001E4EDC
		public override AcceptanceReport OverflowReport()
		{
			return false;
		}

		// Token: 0x06005A1A RID: 23066 RVA: 0x001E6DB4 File Offset: 0x001E4FB4
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				this.things.RemoveAll((Thing x) => x.Destroyed);
			}
			Scribe_Collections.Look<Thing>(ref this.things, "things", LookMode.Reference, Array.Empty<object>());
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.things.RemoveAll((Thing x) => x == null) != 0)
				{
					Log.Warning("Some of the things were null after loading.", false);
				}
			}
		}

		// Token: 0x040030E3 RID: 12515
		public List<Thing> things = new List<Thing>();
	}
}
