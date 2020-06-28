﻿using System;
using System.Xml;
using Verse;

namespace RimWorld
{
	// Token: 0x020008BE RID: 2238
	public class PawnGenOption
	{
		// Token: 0x1700099F RID: 2463
		// (get) Token: 0x060035E8 RID: 13800 RVA: 0x00124DFF File Offset: 0x00122FFF
		public float Cost
		{
			get
			{
				return this.kind.combatPower;
			}
		}

		// Token: 0x060035E9 RID: 13801 RVA: 0x00124E0C File Offset: 0x0012300C
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

		// Token: 0x060035EA RID: 13802 RVA: 0x00124E99 File Offset: 0x00123099
		public void LoadDataFromXmlCustom(XmlNode xmlRoot)
		{
			DirectXmlCrossRefLoader.RegisterObjectWantsCrossRef(this, "kind", xmlRoot.Name, null, null);
			this.selectionWeight = ParseHelper.FromString<float>(xmlRoot.FirstChild.Value);
		}

		// Token: 0x04001DD5 RID: 7637
		public PawnKindDef kind;

		// Token: 0x04001DD6 RID: 7638
		public float selectionWeight;
	}
}
