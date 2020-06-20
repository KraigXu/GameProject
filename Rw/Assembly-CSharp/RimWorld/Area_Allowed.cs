using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000A2F RID: 2607
	public class Area_Allowed : Area
	{
		// Token: 0x17000AF1 RID: 2801
		// (get) Token: 0x06003D9F RID: 15775 RVA: 0x00145C86 File Offset: 0x00143E86
		public override string Label
		{
			get
			{
				return this.labelInt;
			}
		}

		// Token: 0x17000AF2 RID: 2802
		// (get) Token: 0x06003DA0 RID: 15776 RVA: 0x00145C8E File Offset: 0x00143E8E
		public override Color Color
		{
			get
			{
				return this.colorInt;
			}
		}

		// Token: 0x17000AF3 RID: 2803
		// (get) Token: 0x06003DA1 RID: 15777 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool Mutable
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000AF4 RID: 2804
		// (get) Token: 0x06003DA2 RID: 15778 RVA: 0x00145C96 File Offset: 0x00143E96
		public override int ListPriority
		{
			get
			{
				return 500;
			}
		}

		// Token: 0x06003DA3 RID: 15779 RVA: 0x00145C9D File Offset: 0x00143E9D
		public Area_Allowed()
		{
		}

		// Token: 0x06003DA4 RID: 15780 RVA: 0x00145CB0 File Offset: 0x00143EB0
		public Area_Allowed(AreaManager areaManager, string label = null) : base(areaManager)
		{
			this.areaManager = areaManager;
			if (!label.NullOrEmpty())
			{
				this.labelInt = label;
			}
			else
			{
				int num = 1;
				for (;;)
				{
					this.labelInt = "AreaDefaultLabel".Translate(num);
					if (areaManager.GetLabeled(this.labelInt) == null)
					{
						break;
					}
					num++;
				}
			}
			this.colorInt = new Color(Rand.Value, Rand.Value, Rand.Value);
			this.colorInt = Color.Lerp(this.colorInt, Color.gray, 0.5f);
		}

		// Token: 0x06003DA5 RID: 15781 RVA: 0x00145D50 File Offset: 0x00143F50
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.labelInt, "label", null, false);
			Scribe_Values.Look<Color>(ref this.colorInt, "color", default(Color), false);
		}

		// Token: 0x06003DA6 RID: 15782 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool AssignableAsAllowed()
		{
			return true;
		}

		// Token: 0x06003DA7 RID: 15783 RVA: 0x00145D8F File Offset: 0x00143F8F
		public override void SetLabel(string label)
		{
			this.labelInt = label;
		}

		// Token: 0x06003DA8 RID: 15784 RVA: 0x00145D98 File Offset: 0x00143F98
		public override string GetUniqueLoadID()
		{
			return string.Concat(new object[]
			{
				"Area_",
				this.ID,
				"_Named_",
				this.labelInt
			});
		}

		// Token: 0x06003DA9 RID: 15785 RVA: 0x00145C86 File Offset: 0x00143E86
		public override string ToString()
		{
			return this.labelInt;
		}

		// Token: 0x04002406 RID: 9222
		private string labelInt;

		// Token: 0x04002407 RID: 9223
		private Color colorInt = Color.red;
	}
}
