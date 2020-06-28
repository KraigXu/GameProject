using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D0A RID: 3338
	[StaticConstructorOnStartup]
	public class CompFireOverlay : ThingComp
	{
		// Token: 0x17000E45 RID: 3653
		// (get) Token: 0x06005135 RID: 20789 RVA: 0x001B3F49 File Offset: 0x001B2149
		public CompProperties_FireOverlay Props
		{
			get
			{
				return (CompProperties_FireOverlay)this.props;
			}
		}

		// Token: 0x06005136 RID: 20790 RVA: 0x001B3F58 File Offset: 0x001B2158
		public override void PostDraw()
		{
			base.PostDraw();
			if (this.refuelableComp != null && !this.refuelableComp.HasFuel)
			{
				return;
			}
			Vector3 drawPos = this.parent.DrawPos;
			drawPos.y += 0.0454545468f;
			CompFireOverlay.FireGraphic.Draw(drawPos, Rot4.North, this.parent, 0f);
		}

		// Token: 0x06005137 RID: 20791 RVA: 0x001B3FB8 File Offset: 0x001B21B8
		public override void PostSpawnSetup(bool respawningAfterLoad)
		{
			base.PostSpawnSetup(respawningAfterLoad);
			this.refuelableComp = this.parent.GetComp<CompRefuelable>();
		}

		// Token: 0x04002CFF RID: 11519
		protected CompRefuelable refuelableComp;

		// Token: 0x04002D00 RID: 11520
		public static readonly Graphic FireGraphic = GraphicDatabase.Get<Graphic_Flicker>("Things/Special/Fire", ShaderDatabase.TransparentPostLight, Vector2.one, Color.white);
	}
}
