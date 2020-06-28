using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02001020 RID: 4128
	public class StatWorker_PossibleCompOffsets : StatWorker
	{
		// Token: 0x060062DD RID: 25309 RVA: 0x0022558C File Offset: 0x0022378C
		public override float GetValueUnfinalized(StatRequest req, bool applyPostProcess = true)
		{
			float num = base.GetValueUnfinalized(req, applyPostProcess);
			if (req.HasThing)
			{
				CompStatOffsetBase compStatOffsetBase = req.Thing.TryGetComp<CompStatOffsetBase>();
				if (compStatOffsetBase != null && compStatOffsetBase.Props.statDef == this.stat)
				{
					num += compStatOffsetBase.GetStatOffset(req.Pawn);
				}
			}
			return num;
		}

		// Token: 0x060062DE RID: 25310 RVA: 0x002255E0 File Offset: 0x002237E0
		public override string GetExplanationUnfinalized(StatRequest req, ToStringNumberSense numberSense)
		{
			string explanationUnfinalized = base.GetExplanationUnfinalized(req, numberSense);
			StringBuilder stringBuilder = new StringBuilder();
			ThingDef thingDef;
			if (req.Thing != null)
			{
				Thing thing = req.Thing;
				CompStatOffsetBase compStatOffsetBase = thing.TryGetComp<CompStatOffsetBase>();
				List<string> list = new List<string>();
				List<string> list2 = new List<string>();
				if (compStatOffsetBase != null && compStatOffsetBase.Props.statDef == this.stat)
				{
					for (int i = 0; i < compStatOffsetBase.Props.offsets.Count; i++)
					{
						FocusStrengthOffset focusStrengthOffset = compStatOffsetBase.Props.offsets[i];
						if (focusStrengthOffset.CanApply(thing, null))
						{
							list.Add(focusStrengthOffset.GetExplanation(thing));
						}
						else
						{
							list2.Add(focusStrengthOffset.GetExplanationAbstract(thing.def));
						}
					}
					if (list.Count > 0)
					{
						stringBuilder.AppendLine(list.ToLineList(null));
					}
					if (list2.Count > 0)
					{
						if (list.Count > 0)
						{
							stringBuilder.AppendLine();
						}
						stringBuilder.AppendLine("StatReport_PossibleOffsets".Translate() + ":");
						stringBuilder.AppendLine(list2.ToLineList("  - "));
					}
				}
			}
			else if ((thingDef = (req.Def as ThingDef)) != null)
			{
				CompProperties_MeditationFocus compProperties = thingDef.GetCompProperties<CompProperties_MeditationFocus>();
				if (compProperties != null && compProperties.statDef == this.stat)
				{
					stringBuilder.AppendLine();
					stringBuilder.AppendLine("StatReport_PossibleOffsets".Translate() + ":");
					stringBuilder.AppendLine(compProperties.GetExplanationAbstract(thingDef).ToLineList("  - ", false));
				}
			}
			return explanationUnfinalized + stringBuilder;
		}

		// Token: 0x060062DF RID: 25311 RVA: 0x00225788 File Offset: 0x00223988
		public override string GetStatDrawEntryLabel(StatDef stat, float value, ToStringNumberSense numberSense, StatRequest optionalReq, bool finalized = true)
		{
			float num = 0f;
			float num2 = 0f;
			ThingDef thingDef;
			if (optionalReq.Thing != null)
			{
				num2 = optionalReq.Thing.def.GetStatValueAbstract(stat, null);
				CompStatOffsetBase compStatOffsetBase = optionalReq.Thing.TryGetComp<CompStatOffsetBase>();
				if (compStatOffsetBase != null && compStatOffsetBase.Props.statDef == stat)
				{
					num = compStatOffsetBase.Props.GetMaxOffset(false);
				}
			}
			else if ((thingDef = (optionalReq.Def as ThingDef)) != null)
			{
				num2 = thingDef.GetStatValueAbstract(stat, null);
				CompProperties_MeditationFocus compProperties = thingDef.GetCompProperties<CompProperties_MeditationFocus>();
				if (compProperties != null && compProperties.statDef == stat)
				{
					num = compProperties.GetMaxOffset(true);
				}
			}
			if (num != 0f)
			{
				float num3 = num2 + num;
				float f = (num > 0f) ? num2 : num3;
				float val = (num > 0f) ? num3 : num2;
				string str = f.ToStringByStyle(stat.toStringStyle, numberSense);
				string str2 = stat.ValueToString(val, numberSense, finalized);
				return str + " - " + str2;
			}
			return stat.ValueToString(value, numberSense, finalized);
		}
	}
}
