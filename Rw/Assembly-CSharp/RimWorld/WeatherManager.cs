using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000AB2 RID: 2738
	public sealed class WeatherManager : IExposable
	{
		// Token: 0x17000B79 RID: 2937
		// (get) Token: 0x060040DA RID: 16602 RVA: 0x0015B650 File Offset: 0x00159850
		public float TransitionLerpFactor
		{
			get
			{
				float num = (float)this.curWeatherAge / 4000f;
				if (num > 1f)
				{
					num = 1f;
				}
				return num;
			}
		}

		// Token: 0x17000B7A RID: 2938
		// (get) Token: 0x060040DB RID: 16603 RVA: 0x0015B67A File Offset: 0x0015987A
		public float RainRate
		{
			get
			{
				return Mathf.Lerp(this.lastWeather.rainRate, this.curWeather.rainRate, this.TransitionLerpFactor);
			}
		}

		// Token: 0x17000B7B RID: 2939
		// (get) Token: 0x060040DC RID: 16604 RVA: 0x0015B69D File Offset: 0x0015989D
		public float SnowRate
		{
			get
			{
				return Mathf.Lerp(this.lastWeather.snowRate, this.curWeather.snowRate, this.TransitionLerpFactor);
			}
		}

		// Token: 0x17000B7C RID: 2940
		// (get) Token: 0x060040DD RID: 16605 RVA: 0x0015B6C0 File Offset: 0x001598C0
		public float CurWindSpeedFactor
		{
			get
			{
				return Mathf.Lerp(this.lastWeather.windSpeedFactor, this.curWeather.windSpeedFactor, this.TransitionLerpFactor);
			}
		}

		// Token: 0x17000B7D RID: 2941
		// (get) Token: 0x060040DE RID: 16606 RVA: 0x0015B6E3 File Offset: 0x001598E3
		public float CurWindSpeedOffset
		{
			get
			{
				return Mathf.Lerp(this.lastWeather.windSpeedOffset, this.curWeather.windSpeedOffset, this.TransitionLerpFactor);
			}
		}

		// Token: 0x17000B7E RID: 2942
		// (get) Token: 0x060040DF RID: 16607 RVA: 0x0015B706 File Offset: 0x00159906
		public float CurMoveSpeedMultiplier
		{
			get
			{
				return Mathf.Lerp(this.lastWeather.moveSpeedMultiplier, this.curWeather.moveSpeedMultiplier, this.TransitionLerpFactor);
			}
		}

		// Token: 0x17000B7F RID: 2943
		// (get) Token: 0x060040E0 RID: 16608 RVA: 0x0015B729 File Offset: 0x00159929
		public float CurWeatherAccuracyMultiplier
		{
			get
			{
				return Mathf.Lerp(this.lastWeather.accuracyMultiplier, this.curWeather.accuracyMultiplier, this.TransitionLerpFactor);
			}
		}

		// Token: 0x17000B80 RID: 2944
		// (get) Token: 0x060040E1 RID: 16609 RVA: 0x0015B74C File Offset: 0x0015994C
		public WeatherDef CurWeatherPerceived
		{
			get
			{
				if (this.curWeather == null)
				{
					return this.lastWeather;
				}
				if (this.lastWeather == null)
				{
					return this.curWeather;
				}
				float num;
				if (this.curWeather.perceivePriority > this.lastWeather.perceivePriority)
				{
					num = 0.18f;
				}
				else if (this.lastWeather.perceivePriority > this.curWeather.perceivePriority)
				{
					num = 0.82f;
				}
				else
				{
					num = 0.5f;
				}
				if (this.TransitionLerpFactor >= num)
				{
					return this.curWeather;
				}
				return this.lastWeather;
			}
		}

		// Token: 0x17000B81 RID: 2945
		// (get) Token: 0x060040E2 RID: 16610 RVA: 0x0015B7D9 File Offset: 0x001599D9
		public WeatherDef CurWeatherLerped
		{
			get
			{
				if (this.curWeather == null)
				{
					return this.lastWeather;
				}
				if (this.lastWeather == null)
				{
					return this.curWeather;
				}
				if (this.TransitionLerpFactor >= 0.5f)
				{
					return this.curWeather;
				}
				return this.lastWeather;
			}
		}

		// Token: 0x060040E3 RID: 16611 RVA: 0x0015B814 File Offset: 0x00159A14
		public WeatherManager(Map map)
		{
			this.map = map;
			this.growthSeasonMemory = new TemperatureMemory(map);
		}

		// Token: 0x060040E4 RID: 16612 RVA: 0x0015B868 File Offset: 0x00159A68
		public void ExposeData()
		{
			Scribe_Defs.Look<WeatherDef>(ref this.curWeather, "curWeather");
			Scribe_Defs.Look<WeatherDef>(ref this.lastWeather, "lastWeather");
			Scribe_Values.Look<int>(ref this.curWeatherAge, "curWeatherAge", 0, true);
			Scribe_Deep.Look<TemperatureMemory>(ref this.growthSeasonMemory, "growthSeasonMemory", new object[]
			{
				this.map
			});
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				this.ambienceSustainers.Clear();
			}
		}

		// Token: 0x060040E5 RID: 16613 RVA: 0x0015B8D9 File Offset: 0x00159AD9
		public void TransitionTo(WeatherDef newWeather)
		{
			this.lastWeather = this.curWeather;
			this.curWeather = newWeather;
			this.curWeatherAge = 0;
		}

		// Token: 0x060040E6 RID: 16614 RVA: 0x0015B8F8 File Offset: 0x00159AF8
		public void DoWeatherGUI(Rect rect)
		{
			WeatherDef curWeatherPerceived = this.CurWeatherPerceived;
			Text.Anchor = TextAnchor.MiddleRight;
			Rect rect2 = new Rect(rect);
			rect2.width -= 15f;
			Text.Font = GameFont.Small;
			Widgets.Label(rect2, curWeatherPerceived.LabelCap);
			if (!curWeatherPerceived.description.NullOrEmpty())
			{
				TooltipHandler.TipRegion(rect, curWeatherPerceived.description);
			}
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x060040E7 RID: 16615 RVA: 0x0015B964 File Offset: 0x00159B64
		public void WeatherManagerTick()
		{
			this.eventHandler.WeatherEventHandlerTick();
			this.curWeatherAge++;
			this.curWeather.Worker.WeatherTick(this.map, this.TransitionLerpFactor);
			this.lastWeather.Worker.WeatherTick(this.map, 1f - this.TransitionLerpFactor);
			this.growthSeasonMemory.GrowthSeasonMemoryTick();
			for (int i = 0; i < this.curWeather.ambientSounds.Count; i++)
			{
				bool flag = false;
				for (int j = this.ambienceSustainers.Count - 1; j >= 0; j--)
				{
					if (this.ambienceSustainers[j].def == this.curWeather.ambientSounds[i])
					{
						flag = true;
						break;
					}
				}
				if (!flag && this.VolumeOfAmbientSound(this.curWeather.ambientSounds[i]) > 0.0001f)
				{
					SoundInfo info = SoundInfo.OnCamera(MaintenanceType.None);
					Sustainer sustainer = this.curWeather.ambientSounds[i].TrySpawnSustainer(info);
					if (sustainer != null)
					{
						this.ambienceSustainers.Add(sustainer);
					}
				}
			}
		}

		// Token: 0x060040E8 RID: 16616 RVA: 0x0015BA88 File Offset: 0x00159C88
		public void WeatherManagerUpdate()
		{
			this.SetAmbienceSustainersVolume();
		}

		// Token: 0x060040E9 RID: 16617 RVA: 0x0015BA90 File Offset: 0x00159C90
		public void EndAllSustainers()
		{
			for (int i = 0; i < this.ambienceSustainers.Count; i++)
			{
				this.ambienceSustainers[i].End();
			}
			this.ambienceSustainers.Clear();
		}

		// Token: 0x060040EA RID: 16618 RVA: 0x0015BAD0 File Offset: 0x00159CD0
		private void SetAmbienceSustainersVolume()
		{
			for (int i = this.ambienceSustainers.Count - 1; i >= 0; i--)
			{
				float num = this.VolumeOfAmbientSound(this.ambienceSustainers[i].def);
				if (num > 0.0001f)
				{
					this.ambienceSustainers[i].externalParams["LerpFactor"] = num;
				}
				else
				{
					this.ambienceSustainers[i].End();
					this.ambienceSustainers.RemoveAt(i);
				}
			}
		}

		// Token: 0x060040EB RID: 16619 RVA: 0x0015BB50 File Offset: 0x00159D50
		private float VolumeOfAmbientSound(SoundDef soundDef)
		{
			if (this.map != Find.CurrentMap)
			{
				return 0f;
			}
			for (int i = 0; i < Find.WindowStack.Count; i++)
			{
				if (Find.WindowStack[i].silenceAmbientSound)
				{
					return 0f;
				}
			}
			float num = 0f;
			for (int j = 0; j < this.lastWeather.ambientSounds.Count; j++)
			{
				if (this.lastWeather.ambientSounds[j] == soundDef)
				{
					num += 1f - this.TransitionLerpFactor;
				}
			}
			for (int k = 0; k < this.curWeather.ambientSounds.Count; k++)
			{
				if (this.curWeather.ambientSounds[k] == soundDef)
				{
					num += this.TransitionLerpFactor;
				}
			}
			return num;
		}

		// Token: 0x060040EC RID: 16620 RVA: 0x0015BC1A File Offset: 0x00159E1A
		public void DrawAllWeather()
		{
			this.eventHandler.WeatherEventsDraw();
			this.lastWeather.Worker.DrawWeather(this.map);
			this.curWeather.Worker.DrawWeather(this.map);
		}

		// Token: 0x040025AA RID: 9642
		public Map map;

		// Token: 0x040025AB RID: 9643
		public WeatherEventHandler eventHandler = new WeatherEventHandler();

		// Token: 0x040025AC RID: 9644
		public WeatherDef curWeather = WeatherDefOf.Clear;

		// Token: 0x040025AD RID: 9645
		public WeatherDef lastWeather = WeatherDefOf.Clear;

		// Token: 0x040025AE RID: 9646
		public int curWeatherAge;

		// Token: 0x040025AF RID: 9647
		private List<Sustainer> ambienceSustainers = new List<Sustainer>();

		// Token: 0x040025B0 RID: 9648
		public TemperatureMemory growthSeasonMemory;

		// Token: 0x040025B1 RID: 9649
		public const float TransitionTicks = 4000f;
	}
}
