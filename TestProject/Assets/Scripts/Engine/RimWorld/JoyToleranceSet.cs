using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000B82 RID: 2946
	public class JoyToleranceSet : IExposable
	{
		// Token: 0x17000C10 RID: 3088
		public float this[JoyKindDef d]
		{
			get
			{
				return this.tolerances[d];
			}
		}

		// Token: 0x06004512 RID: 17682 RVA: 0x001757F6 File Offset: 0x001739F6
		public void ExposeData()
		{
			Scribe_Deep.Look<DefMap<JoyKindDef, float>>(ref this.tolerances, "tolerances", Array.Empty<object>());
			Scribe_Deep.Look<DefMap<JoyKindDef, bool>>(ref this.bored, "bored", Array.Empty<object>());
			if (this.bored == null)
			{
				this.bored = new DefMap<JoyKindDef, bool>();
			}
		}

		// Token: 0x06004513 RID: 17683 RVA: 0x00175835 File Offset: 0x00173A35
		public bool BoredOf(JoyKindDef def)
		{
			return this.bored[def];
		}

		// Token: 0x06004514 RID: 17684 RVA: 0x00175844 File Offset: 0x00173A44
		public void Notify_JoyGained(float amount, JoyKindDef joyKind)
		{
			float num = Mathf.Min(this.tolerances[joyKind] + amount * 0.65f, 1f);
			this.tolerances[joyKind] = num;
			if (num > 0.5f)
			{
				this.bored[joyKind] = true;
			}
		}

		// Token: 0x06004515 RID: 17685 RVA: 0x00175892 File Offset: 0x00173A92
		public float JoyFactorFromTolerance(JoyKindDef joyKind)
		{
			return 1f - this.tolerances[joyKind];
		}

		// Token: 0x06004516 RID: 17686 RVA: 0x001758A8 File Offset: 0x00173AA8
		public void NeedInterval(Pawn pawn)
		{
			float num = ExpectationsUtility.CurrentExpectationFor(pawn).joyToleranceDropPerDay * 150f / 60000f;
			for (int i = 0; i < this.tolerances.Count; i++)
			{
				float num2 = this.tolerances[i];
				num2 -= num;
				if (num2 < 0f)
				{
					num2 = 0f;
				}
				this.tolerances[i] = num2;
				if (this.bored[i] && num2 < 0.3f)
				{
					this.bored[i] = false;
				}
			}
		}

		// Token: 0x06004517 RID: 17687 RVA: 0x00175934 File Offset: 0x00173B34
		public string TolerancesString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			List<JoyKindDef> allDefsListForReading = DefDatabase<JoyKindDef>.AllDefsListForReading;
			for (int i = 0; i < allDefsListForReading.Count; i++)
			{
				JoyKindDef joyKindDef = allDefsListForReading[i];
				float num = this.tolerances[joyKindDef];
				if (num > 0.01f)
				{
					if (stringBuilder.Length == 0)
					{
						stringBuilder.AppendLine("JoyTolerances".Translate() + ":");
					}
					string text = "   " + joyKindDef.LabelCap + ": " + num.ToStringPercent();
					if (this.bored[joyKindDef])
					{
						text += " (" + "bored".Translate() + ")";
					}
					stringBuilder.AppendLine(text);
				}
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x06004518 RID: 17688 RVA: 0x00175A2C File Offset: 0x00173C2C
		public bool BoredOfAllAvailableJoyKinds(Pawn pawn)
		{
			List<JoyKindDef> list = JoyUtility.JoyKindsOnMapTempList(pawn.MapHeld);
			bool result = true;
			for (int i = 0; i < list.Count; i++)
			{
				if (!this.bored[list[i]])
				{
					result = false;
					break;
				}
			}
			list.Clear();
			return result;
		}

		// Token: 0x0400277B RID: 10107
		private DefMap<JoyKindDef, float> tolerances = new DefMap<JoyKindDef, float>();

		// Token: 0x0400277C RID: 10108
		private DefMap<JoyKindDef, bool> bored = new DefMap<JoyKindDef, bool>();
	}
}
