using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	
	public class PawnGenOption
	{
		
		
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
