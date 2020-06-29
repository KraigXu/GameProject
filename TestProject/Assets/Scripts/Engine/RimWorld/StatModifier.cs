using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	
	public class StatModifier
	{
		
		// (get) Token: 0x06003587 RID: 13703 RVA: 0x00123BFC File Offset: 0x00121DFC
		public string ValueToStringAsOffset
		{
			get
			{
				return this.stat.Worker.ValueToString(this.value, false, ToStringNumberSense.Offset);
			}
		}

		
		// (get) Token: 0x06003588 RID: 13704 RVA: 0x00123C16 File Offset: 0x00121E16
		public string ToStringAsFactor
		{
			get
			{
				return this.stat.Worker.ValueToString(this.value, false, ToStringNumberSense.Factor);
			}
		}

		
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "stat", xmlRoot.Name, null, null);
			this.value = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		
		public override string ToString()
		{
			if (this.stat == null)
			{
				return "(null stat)";
			}
			return this.stat.defName + "-" + this.value.ToString();
		}

		
		public StatDef stat;

		
		public float value;
	}
}
