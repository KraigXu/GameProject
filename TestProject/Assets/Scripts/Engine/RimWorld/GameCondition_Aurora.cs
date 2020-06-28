using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x020009B5 RID: 2485
	public class GameCondition_Aurora : GameCondition
	{
		// Token: 0x17000AA8 RID: 2728
		// (get) Token: 0x06003B3E RID: 15166 RVA: 0x001395CD File Offset: 0x001377CD
		public Color CurrentColor
		{
			get
			{
				return Color.Lerp(GameCondition_Aurora.Colors[this.prevColorIndex], GameCondition_Aurora.Colors[this.curColorIndex], this.curColorTransition);
			}
		}

		// Token: 0x17000AA9 RID: 2729
		// (get) Token: 0x06003B3F RID: 15167 RVA: 0x001395FA File Offset: 0x001377FA
		private int TransitionDurationTicks
		{
			get
			{
				if (!base.Permanent)
				{
					return 280;
				}
				return 3750;
			}
		}

		// Token: 0x17000AAA RID: 2730
		// (get) Token: 0x06003B40 RID: 15168 RVA: 0x00139610 File Offset: 0x00137810
		private bool BrightInAllMaps
		{
			get
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					if (GenCelestial.CurCelestialSunGlow(maps[i]) <= 0.5f)
					{
						return false;
					}
				}
				return true;
			}
		}

		// Token: 0x17000AAB RID: 2731
		// (get) Token: 0x06003B41 RID: 15169 RVA: 0x000FAF75 File Offset: 0x000F9175
		public override int TransitionTicks
		{
			get
			{
				return 200;
			}
		}

		// Token: 0x06003B42 RID: 15170 RVA: 0x0013964C File Offset: 0x0013784C
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<int>(ref this.curColorIndex, "curColorIndex", 0, false);
			Scribe_Values.Look<int>(ref this.prevColorIndex, "prevColorIndex", 0, false);
			Scribe_Values.Look<float>(ref this.curColorTransition, "curColorTransition", 0f, false);
		}

		// Token: 0x06003B43 RID: 15171 RVA: 0x00139699 File Offset: 0x00137899
		public override void Init()
		{
			base.Init();
			this.curColorIndex = Rand.Range(0, GameCondition_Aurora.Colors.Length);
			this.prevColorIndex = this.curColorIndex;
			this.curColorTransition = 1f;
		}

		// Token: 0x06003B44 RID: 15172 RVA: 0x000B6834 File Offset: 0x000B4A34
		public override float SkyGazeChanceFactor(Map map)
		{
			return 8f;
		}

		// Token: 0x06003B45 RID: 15173 RVA: 0x001396CB File Offset: 0x001378CB
		public override float SkyGazeJoyGainFactor(Map map)
		{
			return 5f;
		}

		// Token: 0x06003B46 RID: 15174 RVA: 0x001396D2 File Offset: 0x001378D2
		public override float SkyTargetLerpFactor(Map map)
		{
			return GameConditionUtility.LerpInOutValue(this, (float)this.TransitionTicks, 1f);
		}

		// Token: 0x06003B47 RID: 15175 RVA: 0x001396E8 File Offset: 0x001378E8
		public override SkyTarget? SkyTarget(Map map)
		{
			Color currentColor = this.CurrentColor;
			SkyColorSet colorSet = new SkyColorSet(Color.Lerp(Color.white, currentColor, 0.075f) * this.Brightness(map), new Color(0.92f, 0.92f, 0.92f), Color.Lerp(Color.white, currentColor, 0.025f) * this.Brightness(map), 1f);
			return new SkyTarget?(new SkyTarget(Mathf.Max(GenCelestial.CurCelestialSunGlow(map), 0.25f), colorSet, 1f, 1f));
		}

		// Token: 0x06003B48 RID: 15176 RVA: 0x00139779 File Offset: 0x00137979
		private float Brightness(Map map)
		{
			return Mathf.Max(0.73f, GenCelestial.CurCelestialSunGlow(map));
		}

		// Token: 0x06003B49 RID: 15177 RVA: 0x0013978C File Offset: 0x0013798C
		public override void GameConditionTick()
		{
			this.curColorTransition += 1f / (float)this.TransitionDurationTicks;
			if (this.curColorTransition >= 1f)
			{
				this.prevColorIndex = this.curColorIndex;
				this.curColorIndex = this.GetNewColorIndex();
				this.curColorTransition = 0f;
			}
			if (!base.Permanent && base.TicksLeft > this.TransitionTicks && this.BrightInAllMaps)
			{
				base.TicksLeft = this.TransitionTicks;
			}
		}

		// Token: 0x06003B4A RID: 15178 RVA: 0x0013980D File Offset: 0x00137A0D
		private int GetNewColorIndex()
		{
			return (from x in Enumerable.Range(0, GameCondition_Aurora.Colors.Length)
			where x != this.curColorIndex
			select x).RandomElement<int>();
		}

		// Token: 0x04002306 RID: 8966
		private int curColorIndex = -1;

		// Token: 0x04002307 RID: 8967
		private int prevColorIndex = -1;

		// Token: 0x04002308 RID: 8968
		private float curColorTransition;

		// Token: 0x04002309 RID: 8969
		public const float MaxSunGlow = 0.5f;

		// Token: 0x0400230A RID: 8970
		private const float Glow = 0.25f;

		// Token: 0x0400230B RID: 8971
		private const float SkyColorStrength = 0.075f;

		// Token: 0x0400230C RID: 8972
		private const float OverlayColorStrength = 0.025f;

		// Token: 0x0400230D RID: 8973
		private const float BaseBrightness = 0.73f;

		// Token: 0x0400230E RID: 8974
		private const int TransitionDurationTicks_NotPermanent = 280;

		// Token: 0x0400230F RID: 8975
		private const int TransitionDurationTicks_Permanent = 3750;

		// Token: 0x04002310 RID: 8976
		private static readonly Color[] Colors = new Color[]
		{
			new Color(0f, 1f, 0f),
			new Color(0.3f, 1f, 0f),
			new Color(0f, 1f, 0.7f),
			new Color(0.3f, 1f, 0.7f),
			new Color(0f, 0.5f, 1f),
			new Color(0f, 0f, 1f),
			new Color(0.87f, 0f, 1f),
			new Color(0.75f, 0f, 1f)
		};
	}
}
