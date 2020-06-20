using System;
using RimWorld;

namespace Verse.AI
{
	// Token: 0x02000581 RID: 1409
	public class PawnDuty : IExposable
	{
		// Token: 0x06002819 RID: 10265 RVA: 0x000ECF00 File Offset: 0x000EB100
		public PawnDuty()
		{
		}

		// Token: 0x0600281A RID: 10266 RVA: 0x000ECF38 File Offset: 0x000EB138
		public PawnDuty(DutyDef def)
		{
			this.def = def;
		}

		// Token: 0x0600281B RID: 10267 RVA: 0x000ECF77 File Offset: 0x000EB177
		public PawnDuty(DutyDef def, LocalTargetInfo focus, float radius = -1f) : this(def)
		{
			this.focus = focus;
			this.radius = radius;
		}

		// Token: 0x0600281C RID: 10268 RVA: 0x000ECF8E File Offset: 0x000EB18E
		public PawnDuty(DutyDef def, LocalTargetInfo focus, LocalTargetInfo focusSecond, float radius = -1f) : this(def, focus, radius)
		{
			this.focusSecond = focusSecond;
		}

		// Token: 0x0600281D RID: 10269 RVA: 0x000ECFA4 File Offset: 0x000EB1A4
		public void ExposeData()
		{
			Scribe_Defs.Look<DutyDef>(ref this.def, "def");
			Scribe_TargetInfo.Look(ref this.focus, "focus", LocalTargetInfo.Invalid);
			Scribe_TargetInfo.Look(ref this.focusSecond, "focusSecond", LocalTargetInfo.Invalid);
			Scribe_Values.Look<float>(ref this.radius, "radius", -1f, false);
			Scribe_Values.Look<LocomotionUrgency>(ref this.locomotion, "locomotion", LocomotionUrgency.None, false);
			Scribe_Values.Look<Danger>(ref this.maxDanger, "maxDanger", Danger.Unspecified, false);
			Scribe_Values.Look<CellRect>(ref this.spectateRect, "spectateRect", default(CellRect), false);
			Scribe_Values.Look<SpectateRectSide>(ref this.spectateRectAllowedSides, "spectateRectAllowedSides", SpectateRectSide.All, false);
			Scribe_Values.Look<bool>(ref this.canDig, "canDig", false, false);
			Scribe_Values.Look<PawnsToGather>(ref this.pawnsToGather, "pawnsToGather", PawnsToGather.None, false);
			Scribe_Values.Look<int>(ref this.transportersGroup, "transportersGroup", -1, false);
			Scribe_Values.Look<bool>(ref this.attackDownedIfStarving, "attackDownedIfStarving", false, false);
		}

		// Token: 0x0600281E RID: 10270 RVA: 0x000ED09C File Offset: 0x000EB29C
		public override string ToString()
		{
			string text = this.focus.IsValid ? this.focus.ToString() : "";
			string text2 = this.focusSecond.IsValid ? (", second=" + this.focusSecond.ToString()) : "";
			string text3 = (this.radius > 0f) ? (", rad=" + this.radius.ToString("F2")) : "";
			return string.Concat(new object[]
			{
				"(",
				this.def,
				" ",
				text,
				text2,
				text3,
				")"
			});
		}

		// Token: 0x0600281F RID: 10271 RVA: 0x000ED168 File Offset: 0x000EB368
		internal void DrawDebug(Pawn pawn)
		{
			if (this.focus.IsValid)
			{
				GenDraw.DrawLineBetween(pawn.DrawPos, this.focus.Cell.ToVector3Shifted());
				if (this.radius > 0f)
				{
					GenDraw.DrawRadiusRing(this.focus.Cell, this.radius);
				}
			}
		}

		// Token: 0x04001827 RID: 6183
		public DutyDef def;

		// Token: 0x04001828 RID: 6184
		public LocalTargetInfo focus = LocalTargetInfo.Invalid;

		// Token: 0x04001829 RID: 6185
		public LocalTargetInfo focusSecond = LocalTargetInfo.Invalid;

		// Token: 0x0400182A RID: 6186
		public float radius = -1f;

		// Token: 0x0400182B RID: 6187
		public LocomotionUrgency locomotion;

		// Token: 0x0400182C RID: 6188
		public Danger maxDanger;

		// Token: 0x0400182D RID: 6189
		public CellRect spectateRect;

		// Token: 0x0400182E RID: 6190
		public SpectateRectSide spectateRectAllowedSides = SpectateRectSide.All;

		// Token: 0x0400182F RID: 6191
		public SpectateRectSide spectateRectPreferredSide;

		// Token: 0x04001830 RID: 6192
		public bool canDig;

		// Token: 0x04001831 RID: 6193
		public PawnsToGather pawnsToGather;

		// Token: 0x04001832 RID: 6194
		public int transportersGroup = -1;

		// Token: 0x04001833 RID: 6195
		public bool attackDownedIfStarving;
	}
}
