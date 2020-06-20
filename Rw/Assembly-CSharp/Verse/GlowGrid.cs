using System;
using System.Collections.Generic;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000166 RID: 358
	public sealed class GlowGrid
	{
		// Token: 0x060009F3 RID: 2547 RVA: 0x00036618 File Offset: 0x00034818
		public GlowGrid(Map map)
		{
			this.map = map;
			this.glowGrid = new Color32[map.cellIndices.NumGridCells];
			this.glowGridNoCavePlants = new Color32[map.cellIndices.NumGridCells];
		}

		// Token: 0x060009F4 RID: 2548 RVA: 0x00036674 File Offset: 0x00034874
		public Color32 VisualGlowAt(IntVec3 c)
		{
			return this.glowGrid[this.map.cellIndices.CellToIndex(c)];
		}

		// Token: 0x060009F5 RID: 2549 RVA: 0x00036694 File Offset: 0x00034894
		public float GameGlowAt(IntVec3 c, bool ignoreCavePlants = false)
		{
			float num = 0f;
			if (!this.map.roofGrid.Roofed(c))
			{
				num = this.map.skyManager.CurSkyGlow;
				if (num == 1f)
				{
					return num;
				}
			}
			Color32 color = (ignoreCavePlants ? this.glowGridNoCavePlants : this.glowGrid)[this.map.cellIndices.CellToIndex(c)];
			if (color.a == 1)
			{
				return 1f;
			}
			float b = (float)(color.r + color.g + color.b) / 3f / 255f * 3.6f;
			b = Mathf.Min(0.5f, b);
			return Mathf.Max(num, b);
		}

		// Token: 0x060009F6 RID: 2550 RVA: 0x00036747 File Offset: 0x00034947
		public PsychGlow PsychGlowAt(IntVec3 c)
		{
			return GlowGrid.PsychGlowAtGlow(this.GameGlowAt(c, false));
		}

		// Token: 0x060009F7 RID: 2551 RVA: 0x00036756 File Offset: 0x00034956
		public static PsychGlow PsychGlowAtGlow(float glow)
		{
			if (glow > 0.9f)
			{
				return PsychGlow.Overlit;
			}
			if (glow > 0.3f)
			{
				return PsychGlow.Lit;
			}
			return PsychGlow.Dark;
		}

		// Token: 0x060009F8 RID: 2552 RVA: 0x0003676D File Offset: 0x0003496D
		public void RegisterGlower(CompGlower newGlow)
		{
			this.litGlowers.Add(newGlow);
			this.MarkGlowGridDirty(newGlow.parent.Position);
			if (Current.ProgramState != ProgramState.Playing)
			{
				this.initialGlowerLocs.Add(newGlow.parent.Position);
			}
		}

		// Token: 0x060009F9 RID: 2553 RVA: 0x000367AB File Offset: 0x000349AB
		public void DeRegisterGlower(CompGlower oldGlow)
		{
			this.litGlowers.Remove(oldGlow);
			this.MarkGlowGridDirty(oldGlow.parent.Position);
		}

		// Token: 0x060009FA RID: 2554 RVA: 0x000367CB File Offset: 0x000349CB
		public void MarkGlowGridDirty(IntVec3 loc)
		{
			this.glowGridDirty = true;
			this.map.mapDrawer.MapMeshDirty(loc, MapMeshFlag.GroundGlow);
		}

		// Token: 0x060009FB RID: 2555 RVA: 0x000367E6 File Offset: 0x000349E6
		public void GlowGridUpdate_First()
		{
			if (this.glowGridDirty)
			{
				this.RecalculateAllGlow();
				this.glowGridDirty = false;
			}
		}

		// Token: 0x060009FC RID: 2556 RVA: 0x00036800 File Offset: 0x00034A00
		private void RecalculateAllGlow()
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return;
			}
			if (this.initialGlowerLocs != null)
			{
				foreach (IntVec3 loc in this.initialGlowerLocs)
				{
					this.MarkGlowGridDirty(loc);
				}
				this.initialGlowerLocs = null;
			}
			int numGridCells = this.map.cellIndices.NumGridCells;
			for (int i = 0; i < numGridCells; i++)
			{
				this.glowGrid[i] = new Color32(0, 0, 0, 0);
				this.glowGridNoCavePlants[i] = new Color32(0, 0, 0, 0);
			}
			foreach (CompGlower compGlower in this.litGlowers)
			{
				this.map.glowFlooder.AddFloodGlowFor(compGlower, this.glowGrid);
				if (compGlower.parent.def.category != ThingCategory.Plant || !compGlower.parent.def.plant.cavePlant)
				{
					this.map.glowFlooder.AddFloodGlowFor(compGlower, this.glowGridNoCavePlants);
				}
			}
		}

		// Token: 0x0400081C RID: 2076
		private Map map;

		// Token: 0x0400081D RID: 2077
		public Color32[] glowGrid;

		// Token: 0x0400081E RID: 2078
		public Color32[] glowGridNoCavePlants;

		// Token: 0x0400081F RID: 2079
		private bool glowGridDirty;

		// Token: 0x04000820 RID: 2080
		private HashSet<CompGlower> litGlowers = new HashSet<CompGlower>();

		// Token: 0x04000821 RID: 2081
		private List<IntVec3> initialGlowerLocs = new List<IntVec3>();

		// Token: 0x04000822 RID: 2082
		public const int AlphaOfNotOverlit = 0;

		// Token: 0x04000823 RID: 2083
		public const int AlphaOfOverlit = 1;

		// Token: 0x04000824 RID: 2084
		private const float GameGlowLitThreshold = 0.3f;

		// Token: 0x04000825 RID: 2085
		private const float GameGlowOverlitThreshold = 0.9f;

		// Token: 0x04000826 RID: 2086
		private const float GroundGameGlowFactor = 3.6f;

		// Token: 0x04000827 RID: 2087
		private const float MaxGameGlowFromNonOverlitGroundLights = 0.5f;
	}
}
