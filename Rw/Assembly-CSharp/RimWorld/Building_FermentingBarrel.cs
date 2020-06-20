using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C7B RID: 3195
	[StaticConstructorOnStartup]
	public class Building_FermentingBarrel : Building
	{
		// Token: 0x17000D8E RID: 3470
		// (get) Token: 0x06004CBC RID: 19644 RVA: 0x0019BD6A File Offset: 0x00199F6A
		// (set) Token: 0x06004CBD RID: 19645 RVA: 0x0019BD72 File Offset: 0x00199F72
		public float Progress
		{
			get
			{
				return this.progressInt;
			}
			set
			{
				if (value == this.progressInt)
				{
					return;
				}
				this.progressInt = value;
				this.barFilledCachedMat = null;
			}
		}

		// Token: 0x17000D8F RID: 3471
		// (get) Token: 0x06004CBE RID: 19646 RVA: 0x0019BD8C File Offset: 0x00199F8C
		private Material BarFilledMat
		{
			get
			{
				if (this.barFilledCachedMat == null)
				{
					this.barFilledCachedMat = SolidColorMaterials.SimpleSolidColorMaterial(Color.Lerp(Building_FermentingBarrel.BarZeroProgressColor, Building_FermentingBarrel.BarFermentedColor, this.Progress), false);
				}
				return this.barFilledCachedMat;
			}
		}

		// Token: 0x17000D90 RID: 3472
		// (get) Token: 0x06004CBF RID: 19647 RVA: 0x0019BDC3 File Offset: 0x00199FC3
		public int SpaceLeftForWort
		{
			get
			{
				if (this.Fermented)
				{
					return 0;
				}
				return 25 - this.wortCount;
			}
		}

		// Token: 0x17000D91 RID: 3473
		// (get) Token: 0x06004CC0 RID: 19648 RVA: 0x0019BDD8 File Offset: 0x00199FD8
		private bool Empty
		{
			get
			{
				return this.wortCount <= 0;
			}
		}

		// Token: 0x17000D92 RID: 3474
		// (get) Token: 0x06004CC1 RID: 19649 RVA: 0x0019BDE6 File Offset: 0x00199FE6
		public bool Fermented
		{
			get
			{
				return !this.Empty && this.Progress >= 1f;
			}
		}

		// Token: 0x17000D93 RID: 3475
		// (get) Token: 0x06004CC2 RID: 19650 RVA: 0x0019BE04 File Offset: 0x0019A004
		private float CurrentTempProgressSpeedFactor
		{
			get
			{
				CompProperties_TemperatureRuinable compProperties = this.def.GetCompProperties<CompProperties_TemperatureRuinable>();
				float ambientTemperature = base.AmbientTemperature;
				if (ambientTemperature < compProperties.minSafeTemperature)
				{
					return 0.1f;
				}
				if (ambientTemperature < 7f)
				{
					return GenMath.LerpDouble(compProperties.minSafeTemperature, 7f, 0.1f, 1f, ambientTemperature);
				}
				return 1f;
			}
		}

		// Token: 0x17000D94 RID: 3476
		// (get) Token: 0x06004CC3 RID: 19651 RVA: 0x0019BE5C File Offset: 0x0019A05C
		private float ProgressPerTickAtCurrentTemp
		{
			get
			{
				return 2.77777781E-06f * this.CurrentTempProgressSpeedFactor;
			}
		}

		// Token: 0x17000D95 RID: 3477
		// (get) Token: 0x06004CC4 RID: 19652 RVA: 0x0019BE6A File Offset: 0x0019A06A
		private int EstimatedTicksLeft
		{
			get
			{
				return Mathf.Max(Mathf.RoundToInt((1f - this.Progress) / this.ProgressPerTickAtCurrentTemp), 0);
			}
		}

		// Token: 0x06004CC5 RID: 19653 RVA: 0x0019BE8A File Offset: 0x0019A08A
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.wortCount, "wortCount", 0, false);
			Scribe_Values.Look<float>(ref this.progressInt, "progress", 0f, false);
		}

		// Token: 0x06004CC6 RID: 19654 RVA: 0x0019BEBA File Offset: 0x0019A0BA
		public override void TickRare()
		{
			base.TickRare();
			if (!this.Empty)
			{
				this.Progress = Mathf.Min(this.Progress + 250f * this.ProgressPerTickAtCurrentTemp, 1f);
			}
		}

		// Token: 0x06004CC7 RID: 19655 RVA: 0x0019BEF0 File Offset: 0x0019A0F0
		public void AddWort(int count)
		{
			base.GetComp<CompTemperatureRuinable>().Reset();
			if (this.Fermented)
			{
				Log.Warning("Tried to add wort to a barrel full of beer. Colonists should take the beer first.", false);
				return;
			}
			int num = Mathf.Min(count, 25 - this.wortCount);
			if (num <= 0)
			{
				return;
			}
			this.Progress = GenMath.WeightedAverage(0f, (float)num, this.Progress, (float)this.wortCount);
			this.wortCount += num;
		}

		// Token: 0x06004CC8 RID: 19656 RVA: 0x0019BF5E File Offset: 0x0019A15E
		protected override void ReceiveCompSignal(string signal)
		{
			if (signal == "RuinedByTemperature")
			{
				this.Reset();
			}
		}

		// Token: 0x06004CC9 RID: 19657 RVA: 0x0019BF73 File Offset: 0x0019A173
		private void Reset()
		{
			this.wortCount = 0;
			this.Progress = 0f;
		}

		// Token: 0x06004CCA RID: 19658 RVA: 0x0019BF88 File Offset: 0x0019A188
		public void AddWort(Thing wort)
		{
			int num = Mathf.Min(wort.stackCount, 25 - this.wortCount);
			if (num > 0)
			{
				this.AddWort(num);
				wort.SplitOff(num).Destroy(DestroyMode.Vanish);
			}
		}

		// Token: 0x06004CCB RID: 19659 RVA: 0x0019BFC4 File Offset: 0x0019A1C4
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			if (stringBuilder.Length != 0)
			{
				stringBuilder.AppendLine();
			}
			CompTemperatureRuinable comp = base.GetComp<CompTemperatureRuinable>();
			if (!this.Empty && !comp.Ruined)
			{
				if (this.Fermented)
				{
					stringBuilder.AppendLine("ContainsBeer".Translate(this.wortCount, 25));
				}
				else
				{
					stringBuilder.AppendLine("ContainsWort".Translate(this.wortCount, 25));
				}
			}
			if (!this.Empty)
			{
				if (this.Fermented)
				{
					stringBuilder.AppendLine("Fermented".Translate());
				}
				else
				{
					stringBuilder.AppendLine("FermentationProgress".Translate(this.Progress.ToStringPercent(), this.EstimatedTicksLeft.ToStringTicksToPeriod(true, false, true, true)));
					if (this.CurrentTempProgressSpeedFactor != 1f)
					{
						stringBuilder.AppendLine("FermentationBarrelOutOfIdealTemperature".Translate(this.CurrentTempProgressSpeedFactor.ToStringPercent()));
					}
				}
			}
			stringBuilder.AppendLine("Temperature".Translate() + ": " + base.AmbientTemperature.ToStringTemperature("F0"));
			stringBuilder.AppendLine("IdealFermentingTemperature".Translate() + ": " + 7f.ToStringTemperature("F0") + " ~ " + comp.Props.maxSafeTemperature.ToStringTemperature("F0"));
			return stringBuilder.ToString().TrimEndNewlines();
		}

		// Token: 0x06004CCC RID: 19660 RVA: 0x0019C194 File Offset: 0x0019A394
		public Thing TakeOutBeer()
		{
			if (!this.Fermented)
			{
				Log.Warning("Tried to get beer but it's not yet fermented.", false);
				return null;
			}
			Thing thing = ThingMaker.MakeThing(ThingDefOf.Beer, null);
			thing.stackCount = this.wortCount;
			this.Reset();
			return thing;
		}

		// Token: 0x06004CCD RID: 19661 RVA: 0x0019C1C8 File Offset: 0x0019A3C8
		public override void Draw()
		{
			base.Draw();
			if (!this.Empty)
			{
				Vector3 drawPos = this.DrawPos;
				drawPos.y += 0.0454545468f;
				drawPos.z += 0.25f;
				GenDraw.DrawFillableBar(new GenDraw.FillableBarRequest
				{
					center = drawPos,
					size = Building_FermentingBarrel.BarSize,
					fillPercent = (float)this.wortCount / 25f,
					filledMat = this.BarFilledMat,
					unfilledMat = Building_FermentingBarrel.BarUnfilledMat,
					margin = 0.1f,
					rotation = Rot4.North
				});
			}
		}

		// Token: 0x06004CCE RID: 19662 RVA: 0x0019C274 File Offset: 0x0019A474
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in this.<>n__0())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (Prefs.DevMode && !this.Empty)
			{
				yield return new Command_Action
				{
					defaultLabel = "Debug: Set progress to 1",
					action = delegate
					{
						this.Progress = 1f;
					}
				};
			}
			yield break;
			yield break;
		}

		// Token: 0x04002B13 RID: 11027
		private int wortCount;

		// Token: 0x04002B14 RID: 11028
		private float progressInt;

		// Token: 0x04002B15 RID: 11029
		private Material barFilledCachedMat;

		// Token: 0x04002B16 RID: 11030
		public const int MaxCapacity = 25;

		// Token: 0x04002B17 RID: 11031
		private const int BaseFermentationDuration = 360000;

		// Token: 0x04002B18 RID: 11032
		public const float MinIdealTemperature = 7f;

		// Token: 0x04002B19 RID: 11033
		private static readonly Vector2 BarSize = new Vector2(0.55f, 0.1f);

		// Token: 0x04002B1A RID: 11034
		private static readonly Color BarZeroProgressColor = new Color(0.4f, 0.27f, 0.22f);

		// Token: 0x04002B1B RID: 11035
		private static readonly Color BarFermentedColor = new Color(0.9f, 0.85f, 0.2f);

		// Token: 0x04002B1C RID: 11036
		private static readonly Material BarUnfilledMat = SolidColorMaterials.SimpleSolidColorMaterial(new Color(0.3f, 0.3f, 0.3f), false);
	}
}
