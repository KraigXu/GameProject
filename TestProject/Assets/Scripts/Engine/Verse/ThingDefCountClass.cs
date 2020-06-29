using System;
using System.Xml;
using RimWorld;

namespace Verse
{
	
	public sealed class ThingDefCountClass : IExposable
	{
		
		// (get) Token: 0x06001F7B RID: 8059 RVA: 0x000C1401 File Offset: 0x000BF601
		public string Label
		{
			get
			{
				return GenLabel.ThingLabel(this.thingDef, null, this.count);
			}
		}

		
		// (get) Token: 0x06001F7C RID: 8060 RVA: 0x000C1415 File Offset: 0x000BF615
		public string LabelCap
		{
			get
			{
				return this.Label.CapitalizeFirst(this.thingDef);
			}
		}

		
		// (get) Token: 0x06001F7D RID: 8061 RVA: 0x000C1428 File Offset: 0x000BF628
		public string Summary
		{
			get
			{
				return this.count + "x " + ((this.thingDef != null) ? this.thingDef.label : "null");
			}
		}

		
		public ThingDefCountClass()
		{
		}

		
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

		
		public void ExposeData()
		{
			Scribe_Defs.Look<ThingDef>(ref this.thingDef, "thingDef");
			Scribe_Values.Look<int>(ref this.count, "count", 1, false);
		}

		
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

		
		public override int GetHashCode()
		{
			return (int)this.thingDef.shortHash + this.count << 16;
		}

		
		public static implicit operator ThingDefCountClass(ThingDefCount t)
		{
			return new ThingDefCountClass(t.ThingDef, t.Count);
		}

		
		public ThingDef thingDef;

		
		public int count;
	}
}
