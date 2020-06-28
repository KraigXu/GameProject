using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000C47 RID: 3143
	public class MinifiedThing : ThingWithComps, IThingHolder
	{
		// Token: 0x17000D2A RID: 3370
		// (get) Token: 0x06004AED RID: 19181 RVA: 0x00194D0C File Offset: 0x00192F0C
		// (set) Token: 0x06004AEE RID: 19182 RVA: 0x00194D2C File Offset: 0x00192F2C
		public Thing InnerThing
		{
			get
			{
				if (this.innerContainer.Count == 0)
				{
					return null;
				}
				return this.innerContainer[0];
			}
			set
			{
				if (value == this.InnerThing)
				{
					return;
				}
				if (value == null)
				{
					this.innerContainer.Clear();
					return;
				}
				if (this.innerContainer.Count != 0)
				{
					Log.Warning(string.Concat(new string[]
					{
						"Assigned 2 things to the same MinifiedThing ",
						this.ToStringSafe<MinifiedThing>(),
						" (first=",
						this.innerContainer[0].ToStringSafe<Thing>(),
						" second=",
						value.ToStringSafe<Thing>(),
						")"
					}), false);
					this.innerContainer.ClearAndDestroyContents(DestroyMode.Vanish);
				}
				this.innerContainer.TryAdd(value, true);
			}
		}

		// Token: 0x17000D2B RID: 3371
		// (get) Token: 0x06004AEF RID: 19183 RVA: 0x00194DD0 File Offset: 0x00192FD0
		public override Graphic Graphic
		{
			get
			{
				if (this.cachedGraphic == null)
				{
					this.cachedGraphic = this.InnerThing.Graphic.ExtractInnerGraphicFor(this.InnerThing);
					if ((float)this.InnerThing.def.size.x > 1.1f || (float)this.InnerThing.def.size.z > 1.1f)
					{
						Vector2 minifiedDrawSize = this.GetMinifiedDrawSize(this.InnerThing.def.size.ToVector2(), 1.1f);
						Vector2 newDrawSize = new Vector2(minifiedDrawSize.x / (float)this.InnerThing.def.size.x * this.cachedGraphic.drawSize.x, minifiedDrawSize.y / (float)this.InnerThing.def.size.z * this.cachedGraphic.drawSize.y);
						this.cachedGraphic = this.cachedGraphic.GetCopy(newDrawSize);
					}
				}
				return this.cachedGraphic;
			}
		}

		// Token: 0x17000D2C RID: 3372
		// (get) Token: 0x06004AF0 RID: 19184 RVA: 0x00194EDD File Offset: 0x001930DD
		public override string LabelNoCount
		{
			get
			{
				return this.InnerThing.LabelNoCount;
			}
		}

		// Token: 0x17000D2D RID: 3373
		// (get) Token: 0x06004AF1 RID: 19185 RVA: 0x00194EEA File Offset: 0x001930EA
		public override string DescriptionDetailed
		{
			get
			{
				return this.InnerThing.DescriptionDetailed;
			}
		}

		// Token: 0x17000D2E RID: 3374
		// (get) Token: 0x06004AF2 RID: 19186 RVA: 0x00194EF7 File Offset: 0x001930F7
		public override string DescriptionFlavor
		{
			get
			{
				return this.InnerThing.DescriptionFlavor;
			}
		}

		// Token: 0x06004AF3 RID: 19187 RVA: 0x00194F04 File Offset: 0x00193104
		public MinifiedThing()
		{
			this.innerContainer = new ThingOwner<Thing>(this, true, LookMode.Deep);
		}

		// Token: 0x06004AF4 RID: 19188 RVA: 0x00194F1A File Offset: 0x0019311A
		public override void Tick()
		{
			if (this.InnerThing == null)
			{
				Log.Error("MinifiedThing with null InnerThing. Destroying.", false);
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			base.Tick();
			if (this.InnerThing is Building_Battery)
			{
				this.innerContainer.ThingOwnerTick(true);
			}
		}

		// Token: 0x06004AF5 RID: 19189 RVA: 0x00194F56 File Offset: 0x00193156
		public ThingOwner GetDirectlyHeldThings()
		{
			return this.innerContainer;
		}

		// Token: 0x06004AF6 RID: 19190 RVA: 0x00194F5E File Offset: 0x0019315E
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
		}

		// Token: 0x06004AF7 RID: 19191 RVA: 0x00194F6C File Offset: 0x0019316C
		public override Thing SplitOff(int count)
		{
			MinifiedThing minifiedThing = (MinifiedThing)base.SplitOff(count);
			if (minifiedThing != this)
			{
				minifiedThing.InnerThing = ThingMaker.MakeThing(this.InnerThing.def, this.InnerThing.Stuff);
				ThingWithComps thingWithComps = this.InnerThing as ThingWithComps;
				if (thingWithComps != null)
				{
					for (int i = 0; i < thingWithComps.AllComps.Count; i++)
					{
						thingWithComps.AllComps[i].PostSplitOff(minifiedThing.InnerThing);
					}
				}
			}
			return minifiedThing;
		}

		// Token: 0x06004AF8 RID: 19192 RVA: 0x00194FE8 File Offset: 0x001931E8
		public override bool CanStackWith(Thing other)
		{
			MinifiedThing minifiedThing = other as MinifiedThing;
			return minifiedThing != null && base.CanStackWith(other) && this.InnerThing.CanStackWith(minifiedThing.InnerThing);
		}

		// Token: 0x06004AF9 RID: 19193 RVA: 0x0019501D File Offset: 0x0019321D
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Deep.Look<ThingOwner>(ref this.innerContainer, "innerContainer", new object[]
			{
				this
			});
		}

		// Token: 0x06004AFA RID: 19194 RVA: 0x00195040 File Offset: 0x00193240
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			Blueprint_Install blueprint_Install = InstallBlueprintUtility.ExistingBlueprintFor(this);
			if (blueprint_Install != null)
			{
				GenDraw.DrawLineBetween(this.TrueCenter(), blueprint_Install.TrueCenter());
			}
		}

		// Token: 0x06004AFB RID: 19195 RVA: 0x00195070 File Offset: 0x00193270
		public override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			if (this.crateFrontGraphic == null)
			{
				this.crateFrontGraphic = GraphicDatabase.Get<Graphic_Single>("Things/Item/Minified/CrateFront", ShaderDatabase.Cutout, this.GetMinifiedDrawSize(this.InnerThing.def.size.ToVector2(), 1.1f) * 1.16f, Color.white);
			}
			this.crateFrontGraphic.DrawFromDef(drawLoc + Altitudes.AltIncVect * 0.1f, Rot4.North, null, 0f);
			if (this.Graphic is Graphic_Single)
			{
				this.Graphic.Draw(drawLoc, Rot4.North, this, 0f);
				return;
			}
			this.Graphic.Draw(drawLoc, Rot4.South, this, 0f);
		}

		// Token: 0x06004AFC RID: 19196 RVA: 0x00195130 File Offset: 0x00193330
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			bool spawned = base.Spawned;
			Map map = base.Map;
			base.Destroy(mode);
			if (this.InnerThing != null)
			{
				InstallBlueprintUtility.CancelBlueprintsFor(this);
				if (spawned)
				{
					if (mode == DestroyMode.Deconstruct)
					{
						SoundDefOf.Building_Deconstructed.PlayOneShot(new TargetInfo(base.Position, map, false));
						GenLeaving.DoLeavingsFor(this.InnerThing, map, mode, this.OccupiedRect(), null, null);
					}
					else if (mode == DestroyMode.KillFinalize)
					{
						GenLeaving.DoLeavingsFor(this.InnerThing, map, mode, this.OccupiedRect(), null, null);
					}
				}
				if (this.InnerThing is MonumentMarker)
				{
					this.InnerThing.Destroy(DestroyMode.Vanish);
				}
			}
		}

		// Token: 0x06004AFD RID: 19197 RVA: 0x001951CC File Offset: 0x001933CC
		public override void PreTraded(TradeAction action, Pawn playerNegotiator, ITrader trader)
		{
			base.PreTraded(action, playerNegotiator, trader);
			InstallBlueprintUtility.CancelBlueprintsFor(this);
		}

		// Token: 0x06004AFE RID: 19198 RVA: 0x001951DD File Offset: 0x001933DD
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in this.<>n__0())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			yield return InstallationDesignatorDatabase.DesignatorFor(this.def);
			yield break;
			yield break;
		}

		// Token: 0x06004AFF RID: 19199 RVA: 0x001951F0 File Offset: 0x001933F0
		public override string GetInspectString()
		{
			string text = "NotInstalled".Translate();
			string inspectString = this.InnerThing.GetInspectString();
			if (!inspectString.NullOrEmpty())
			{
				text += "\n";
				text += inspectString;
			}
			return text;
		}

		// Token: 0x06004B00 RID: 19200 RVA: 0x00195238 File Offset: 0x00193438
		private Vector2 GetMinifiedDrawSize(Vector2 drawSize, float maxSideLength)
		{
			float num = maxSideLength / Mathf.Max(drawSize.x, drawSize.y);
			if (num >= 1f)
			{
				return drawSize;
			}
			return drawSize * num;
		}

		// Token: 0x04002A79 RID: 10873
		private const float MaxMinifiedGraphicSize = 1.1f;

		// Token: 0x04002A7A RID: 10874
		private const float CrateToGraphicScale = 1.16f;

		// Token: 0x04002A7B RID: 10875
		private ThingOwner innerContainer;

		// Token: 0x04002A7C RID: 10876
		private Graphic cachedGraphic;

		// Token: 0x04002A7D RID: 10877
		private Graphic crateFrontGraphic;
	}
}
