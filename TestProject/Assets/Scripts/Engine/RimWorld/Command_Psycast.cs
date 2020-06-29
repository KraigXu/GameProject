using System;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class Command_Psycast : Command_Ability
	{
		
		public Command_Psycast(Psycast ability) : base(ability)
		{
		}

		
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth)
		{
			AbilityDef def = this.ability.def;
			Pawn pawn = this.ability.pawn;
			this.disabled = false;
			if (def.EntropyGain > 1.401298E-45f)
			{
				Hediff hediff = pawn.health.hediffSet.hediffs.FirstOrDefault((Hediff h) => h.def == HediffDefOf.PsychicAmplifier);
				if (hediff == null || hediff.Severity < (float)def.level)
				{
					base.DisableWithReason("CommandPsycastHigherLevelPsylinkRequired".Translate(def.level));
				}
				else if (pawn.psychicEntropy.WouldOverflowEntropy(def.EntropyGain + PsycastUtility.TotalEntropyFromQueuedPsycasts(pawn)))
				{
					base.DisableWithReason("CommandPsycastWouldExceedEntropy".Translate(def.label));
				}
			}
			GizmoResult result = base.GizmoOnGUI(topLeft, maxWidth);
			float num = topLeft.y + 3f;
			float num2 = (float)(((def.EntropyGain > float.Epsilon) ? 15 : 0) + ((def.PsyfocusCost > float.Epsilon) ? 15 : 0));
			if (num2 > 0f)
			{
				GUI.DrawTexture(new Rect(topLeft.x + this.GetWidth(maxWidth) - 38f, num, 43f, num2), TexUI.GrayTextBG);
			}
			Text.Font = GameFont.Tiny;
			if (def.EntropyGain > 1.401298E-45f)
			{
				TaggedString taggedString = "NeuralHeatLetter".Translate() + ": " + def.EntropyGain.ToString();
				float x = Text.CalcSize(taggedString).x;
				Rect rect = new Rect(topLeft.x + this.GetWidth(maxWidth) - x - 2f, num, x, 18f);
				Widgets.Label(rect, taggedString);
				num += rect.height - 4f;
			}
			if (def.PsyfocusCost > 1.401298E-45f)
			{
				TaggedString taggedString2 = "PsyfocusLetter".Translate() + ": " + def.PsyfocusCost.ToStringPercent();
				float x2 = Text.CalcSize(taggedString2).x;
				Widgets.Label(new Rect(topLeft.x + this.GetWidth(maxWidth) - x2 - 2f, num, x2, 18f), taggedString2);
			}
			return result;
		}
	}
}
