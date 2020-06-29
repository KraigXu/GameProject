using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	
	public class PawnGenOption
	{
		
		// (get) Token: 0x060035E8 RID: 13800 RVA: 0x00124DFF File Offset: 0x00122FFF
		public float Cost
		{
			get
			{
				return this.kind.combatPower;
			}
		}

		
		public override string ToString()
		{
			return string.Concat(new string[]
			{
				"(",
				(this.kind != null) ? this.kind.ToString() : "null",
				" w=",
				this.selectionWeight.ToString("F2"),
				" c=",
				(this.kind != null) ? this.Cost.ToString("F2") : "null",
				")"
			});
		}

		
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "kind", xmlRoot.Name, null, null);
			this.selectionWeight = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		
		public PawnKindDef kind;

		
		public float selectionWeight;
	}
}
