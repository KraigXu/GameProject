using System;
using System.Xml;
using RimWorld;

namespace Verse
{
	// Token: 0x0200041C RID: 1052
	public sealed class ThingDefCountClass : IExposable
	{
		// Token: 0x170005FB RID: 1531
		// (get) Token: 0x06001F7B RID: 8059 RVA: 0x000C1401 File Offset: 0x000BF601
		public string Label
		{
			get
			{
				return GenLabel.ThingLabel(this.thingDef, null, this.count);
			}
		}

		// Token: 0x170005FC RID: 1532
		// (get) Token: 0x06001F7C RID: 8060 RVA: 0x000C1415 File Offset: 0x000BF615
		public string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst(this.thingDef);
			}
		}

		// Token: 0x170005FD RID: 1533
		// (get) Token: 0x06001F7D RID: 8061 RVA: 0x000C1428 File Offset: 0x000BF628
		public string Summary
		{
			get
			{
				return this.count + "x " + ((this.thingDef != null) ? this.thingDef.label : "null");
			}
		}

		// Token: 0x06001F7E RID: 8062 RVA: 0x0000F2A9 File Offset: 0x0000D4A9
		public ThingDefCountClass()
		{
		}

		// Token: 0x06001F7F RID: 8063 RVA: 0x000C145C File Offset: 0x000BF65C
		public ThingDefCountClass(ThingDef thingDef, int count)
		{
			if (count < 0)
			{
				Log.Warning(string.Concat(new object[]
				{
					"Tried to set ThingDefCountClass count to ",
					count,
					". thingDef=",
					thingDef
				}), false);
				count = 0;
			}
			this.thingDef = thingDef;
			this.count = count;
		}

		// Token: 0x06001F80 RID: 8064 RVA: 0x000C14B2 File Offset: 0x000BF6B2
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			Scribe_Values.Look<int>(ref this.count, "count", 1, false);
		}

		// Token: 0x06001F81 RID: 8065 RVA: 0x000C14D8 File Offset: 0x000BF6D8
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			if (xmlRoot.ChildNodes.Count != 1)
			{
				Log.Error("Misconfigured ThingDefCountClass: " + xmlRoot.OuterXml, false);
				return;
			}
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "thingDef", xmlRoot.Name, null, null);
			this.count = ParseHelper.FromString<int>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x06001F82 RID: 8066 RVA: 0x000C1534 File Offset: 0x000BF734
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				"(",
				this.count,
				"x ",
				(this.thingDef != null) ? this.thingDef.defName : "null",
				")"
			});
		}

		// Token: 0x06001F83 RID: 8067 RVA: 0x000C158F File Offset: 0x000BF78F
		public override int GetHashCode()
		{
			return (int)this.thingDef.shortHash + this.count << 16;
		}

		// Token: 0x06001F84 RID: 8068 RVA: 0x000C15A6 File Offset: 0x000BF7A6
		public static implicit operator ThingDefCountClass(ThingDefCount t)
		{
			return new ThingDefCountClass(t.ThingDef, t.Count);
		}

		// Token: 0x0400131E RID: 4894
		public ThingDef thingDef;

		// Token: 0x0400131F RID: 4895
		public int count;
	}
}
