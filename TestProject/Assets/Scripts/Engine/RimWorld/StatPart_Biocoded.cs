using System;

namespace RimWorld
{
	// Token: 0x02000FF7 RID: 4087
	public class StatPart_Biocoded : StatPart
	{
		// Token: 0x060061FB RID: 25083 RVA: 0x0022049B File Offset: 0x0021E69B
		public override void TransformValue(StatRequest req, ref float val)
		{
			if (req.HasThing && EquipmentUtility.IsBiocoded(req.Thing))
			{
				val *= 0f;
			}
		}

		// Token: 0x060061FC RID: 25084 RVA: 0x00019EA1 File Offset: 0x000180A1
		public override string ExplanationPart(StatRequest req)
		{
			return null;
		}
	}
}
