using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BA7 RID: 2983
	public static class ApparelUtility
	{
		// Token: 0x06004602 RID: 17922 RVA: 0x00179F64 File Offset: 0x00178164
		public static bool CanWearTogether(ThingDef A, ThingDef B, BodyDef body)
		{
			bool flag = false;
			for (int i = 0; i < A.apparel.layers.Count; i++)
			{
				for (int j = 0; j < B.apparel.layers.Count; j++)
				{
					if (A.apparel.layers[i] == B.apparel.layers[j])
					{
						flag = true;
					}
					if (flag)
					{
						break;
					}
				}
				if (flag)
				{
					break;
				}
			}
			if (!flag)
			{
				return true;
			}
			List<BodyPartGroupDef> bodyPartGroups = A.apparel.bodyPartGroups;
			List<BodyPartGroupDef> bodyPartGroups2 = B.apparel.bodyPartGroups;
			BodyPartGroupDef[] interferingBodyPartGroups = A.apparel.GetInterferingBodyPartGroups(body);
			BodyPartGroupDef[] interferingBodyPartGroups2 = B.apparel.GetInterferingBodyPartGroups(body);
			for (int k = 0; k < bodyPartGroups.Count; k++)
			{
				if (interferingBodyPartGroups2.Contains(bodyPartGroups[k]))
				{
					return false;
				}
			}
			for (int l = 0; l < bodyPartGroups2.Count; l++)
			{
				if (interferingBodyPartGroups.Contains(bodyPartGroups2[l]))
				{
					return false;
				}
			}
			return true;
		}

		// Token: 0x06004603 RID: 17923 RVA: 0x0017A068 File Offset: 0x00178268
		public static void GenerateLayerGroupPairs(BodyDef body, ThingDef td, Action<ApparelUtility.LayerGroupPair> callback)
		{
			for (int i = 0; i < td.apparel.layers.Count; i++)
			{
				ApparelLayerDef layer = td.apparel.layers[i];
				BodyPartGroupDef[] interferingBodyPartGroups = td.apparel.GetInterferingBodyPartGroups(body);
				for (int j = 0; j < interferingBodyPartGroups.Length; j++)
				{
					callback(new ApparelUtility.LayerGroupPair(layer, interferingBodyPartGroups[j]));
				}
			}
		}

		// Token: 0x06004604 RID: 17924 RVA: 0x0017A0CC File Offset: 0x001782CC
		public static bool HasPartsToWear(Pawn p, ThingDef apparel)
		{
			ApparelUtility.<>c__DisplayClass3_0 <>c__DisplayClass3_ = new ApparelUtility.<>c__DisplayClass3_0();
			List<Hediff> hediffs = p.health.hediffSet.hediffs;
			bool flag = false;
			for (int j = 0; j < hediffs.Count; j++)
			{
				if (hediffs[j] is Hediff_MissingPart)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				return true;
			}
			IEnumerable<BodyPartRecord> notMissingParts = p.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null);
			<>c__DisplayClass3_.groups = apparel.apparel.bodyPartGroups;
			int i;
			int i2;
			for (i = 0; i < <>c__DisplayClass3_.groups.Count; i = i2 + 1)
			{
				if (notMissingParts.Any((BodyPartRecord x) => x.IsInGroup(<>c__DisplayClass3_.groups[i])))
				{
					return true;
				}
				i2 = i;
			}
			return false;
		}

		// Token: 0x02001B13 RID: 6931
		public struct LayerGroupPair : IEquatable<ApparelUtility.LayerGroupPair>
		{
			// Token: 0x06009A18 RID: 39448 RVA: 0x002F1A03 File Offset: 0x002EFC03
			public LayerGroupPair(ApparelLayerDef layer, BodyPartGroupDef group)
			{
				this.layer = layer;
				this.group = group;
			}

			// Token: 0x06009A19 RID: 39449 RVA: 0x002F1A13 File Offset: 0x002EFC13
			public override bool Equals(object rhs)
			{
				return rhs is ApparelUtility.LayerGroupPair && this.Equals((ApparelUtility.LayerGroupPair)rhs);
			}

			// Token: 0x06009A1A RID: 39450 RVA: 0x002F1A2B File Offset: 0x002EFC2B
			public bool Equals(ApparelUtility.LayerGroupPair other)
			{
				return other.layer == this.layer && other.group == this.group;
			}

			// Token: 0x06009A1B RID: 39451 RVA: 0x002F1A4B File Offset: 0x002EFC4B
			public override int GetHashCode()
			{
				return (17 * 23 + this.layer.GetHashCode()) * 23 + this.group.GetHashCode();
			}

			// Token: 0x040066B3 RID: 26291
			private readonly ApparelLayerDef layer;

			// Token: 0x040066B4 RID: 26292
			private readonly BodyPartGroupDef group;
		}
	}
}
