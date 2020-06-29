using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class Area_Allowed : Area
	{
		
		// (get) Token: 0x06003D9F RID: 15775 RVA: 0x00145C86 File Offset: 0x00143E86
		public override string Label
		{
			get
			{
				return this.labelInt;
			}
		}

		
		// (get) Token: 0x06003DA0 RID: 15776 RVA: 0x00145C8E File Offset: 0x00143E8E
		public override Color Color
		{
			get
			{
				return this.colorInt;
			}
		}

		
		// (get) Token: 0x06003DA1 RID: 15777 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool Mutable
		{
			get
			{
				return true;
			}
		}

		
		// (get) Token: 0x06003DA2 RID: 15778 RVA: 0x00145C96 File Offset: 0x00143E96
		public override int ListPriority
		{
			get
			{
				return 500;
			}
		}

		
		public Area_Allowed()
		{
		}

		
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

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.labelInt, "label", null, false);
			Scribe_Values.Look<Color>(ref this.colorInt, "color", default(Color), false);
		}

		
		public override bool AssignableAsAllowed()
		{
			return true;
		}

		
		public override void SetLabel(string label)
		{
			this.labelInt = label;
		}

		
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

		
		public override string ToString()
		{
			return this.labelInt;
		}

		
		private string labelInt;

		
		private Color colorInt = Color.red;
	}
}
