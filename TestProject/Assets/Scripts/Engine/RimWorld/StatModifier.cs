using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x0200089F RID: 2207
	public class StatModifier
	{
		// Token: 0x17000987 RID: 2439
		// (get) Token: 0x06003587 RID: 13703 RVA: 0x00123BFC File Offset: 0x00121DFC
		public string ValueToStringAsOffset
		{
			get
			{
				return this.stat.Worker.ValueToString(this.value, false, ToStringNumberSense.Offset);
			}
		}

		// Token: 0x17000988 RID: 2440
		// (get) Token: 0x06003588 RID: 13704 RVA: 0x00123C16 File Offset: 0x00121E16
		public string ToStringAsFactor
		{
			get
			{
				return this.stat.Worker.ValueToString(this.value, false, ToStringNumberSense.Factor);
			}
		}

		// Token: 0x06003589 RID: 13705 RVA: 0x00123C30 File Offset: 0x00121E30
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "stat", xmlRoot.Name, null, null);
			this.value = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x0600358A RID: 13706 RVA: 0x00123C5B File Offset: 0x00121E5B
		public override string ToString()
		{
			if (this.stat == null)
			{
				return "(null stat)";
			}
			return this.stat.defName + "-" + this.value.ToString();
		}

		// Token: 0x04001D4F RID: 7503
		public StatDef stat;

		// Token: 0x04001D50 RID: 7504
		public float value;
	}
}
