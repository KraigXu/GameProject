using System;
using System.Collections.Generic;
using System.Linq;
using RimWorld.Planet;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000EA9 RID: 3753
	public class ITab_Pawn_Gear : ITab
	{
		// Token: 0x1700107C RID: 4220
		// (get) Token: 0x06005BA6 RID: 23462 RVA: 0x001F9474 File Offset: 0x001F7674
		public override bool IsVisible
		{
			get
			{
				Pawn selPawnForGear = this.SelPawnForGear;
				return this.ShouldShowInventory(selPawnForGear) || this.ShouldShowApparel(selPawnForGear) || this.ShouldShowEquipment(selPawnForGear);
			}
		}

		// Token: 0x1700107D RID: 4221
		// (get) Token: 0x06005BA7 RID: 23463 RVA: 0x001F94A4 File Offset: 0x001F76A4
		private bool CanControl
		{
			get
			{
				Pawn selPawnForGear = this.SelPawnForGear;
				return !selPawnForGear.Downed && !selPawnForGear.InMentalState && (selPawnForGear.Faction == Faction.OfPlayer || selPawnForGear.IsPrisonerOfColony) && (!selPawnForGear.IsPrisonerOfColony || !selPawnForGear.Spawned || selPawnForGear.Map.mapPawns.AnyFreeColonistSpawned) && (!selPawnForGear.IsPrisonerOfColony || (!PrisonBreakUtility.IsPrisonBreaking(selPawnForGear) && (selPawnForGear.CurJob == null || !selPawnForGear.CurJob.exitMapOnArrival)));
			}
		}

		// Token: 0x1700107E RID: 4222
		// (get) Token: 0x06005BA8 RID: 23464 RVA: 0x001F952D File Offset: 0x001F772D
		private bool CanControlColonist
		{
			get
			{
				return this.CanControl && this.SelPawnForGear.IsColonistPlayerControlled;
			}
		}

		// Token: 0x1700107F RID: 4223
		// (get) Token: 0x06005BA9 RID: 23465 RVA: 0x001F9544 File Offset: 0x001F7744
		private Pawn SelPawnForGear
		{
			get
			{
				if (base.SelPawn != null)
				{
					return base.SelPawn;
				}
				Corpse corpse = base.SelThing as Corpse;
				if (corpse != null)
				{
					return corpse.InnerPawn;
				}
				throw new InvalidOperationException("Gear tab on non-pawn non-corpse " + base.SelThing);
			}
		}

		// Token: 0x06005BAA RID: 23466 RVA: 0x001F958B File Offset: 0x001F778B
		public ITab_Pawn_Gear()
		{
			this.size = new Vector2(460f, 450f);
			this.labelKey = "TabGear";
			this.tutorTag = "Gear";
		}

		// Token: 0x06005BAB RID: 23467 RVA: 0x001F95CC File Offset: 0x001F77CC
		protected override void FillTab()
		{
			Text.Font = GameFont.Small;
			Rect rect = new Rect(0f, 20f, this.size.x, this.size.y - 20f).ContractedBy(10f);
			Rect position = new Rect(rect.x, rect.y, rect.width, rect.height);
			GUI.BeginGroup(position);
			Text.Font = GameFont.Small;
			GUI.color = Color.white;
			Rect outRect = new Rect(0f, 0f, position.width, position.height);
			Rect viewRect = new Rect(0f, 0f, position.width - 16f, this.scrollViewHeight);
			Widgets.BeginScrollView(outRect, ref this.scrollPosition, viewRect, true);
			float num = 0f;
			this.TryDrawMassInfo(ref num, viewRect.width);
			this.TryDrawComfyTemperatureRange(ref num, viewRect.width);
			if (this.ShouldShowOverallArmor(this.SelPawnForGear))
			{
				Widgets.ListSeparator(ref num, viewRect.width, "OverallArmor".Translate());
				this.TryDrawOverallArmor(ref num, viewRect.width, StatDefOf.ArmorRating_Sharp, "ArmorSharp".Translate());
				this.TryDrawOverallArmor(ref num, viewRect.width, StatDefOf.ArmorRating_Blunt, "ArmorBlunt".Translate());
				this.TryDrawOverallArmor(ref num, viewRect.width, StatDefOf.ArmorRating_Heat, "ArmorHeat".Translate());
			}
			if (this.ShouldShowEquipment(this.SelPawnForGear))
			{
				Widgets.ListSeparator(ref num, viewRect.width, "Equipment".Translate());
				foreach (ThingWithComps thing in this.SelPawnForGear.equipment.AllEquipmentListForReading)
				{
					this.DrawThingRow(ref num, viewRect.width, thing, false);
				}
			}
			if (this.ShouldShowApparel(this.SelPawnForGear))
			{
				Widgets.ListSeparator(ref num, viewRect.width, "Apparel".Translate());
				foreach (Apparel thing2 in from ap in this.SelPawnForGear.apparel.WornApparel
				orderby ap.def.apparel.bodyPartGroups[0].listOrder descending
				select ap)
				{
					this.DrawThingRow(ref num, viewRect.width, thing2, false);
				}
			}
			if (this.ShouldShowInventory(this.SelPawnForGear))
			{
				Widgets.ListSeparator(ref num, viewRect.width, "Inventory".Translate());
				ITab_Pawn_Gear.workingInvList.Clear();
				ITab_Pawn_Gear.workingInvList.AddRange(this.SelPawnForGear.inventory.innerContainer);
				for (int i = 0; i < ITab_Pawn_Gear.workingInvList.Count; i++)
				{
					this.DrawThingRow(ref num, viewRect.width, ITab_Pawn_Gear.workingInvList[i], true);
				}
				ITab_Pawn_Gear.workingInvList.Clear();
			}
			if (Event.current.type == EventType.Layout)
			{
				this.scrollViewHeight = num + 30f;
			}
			Widgets.EndScrollView();
			GUI.EndGroup();
			GUI.color = Color.white;
			Text.Anchor = TextAnchor.UpperLeft;
		}

		// Token: 0x06005BAC RID: 23468 RVA: 0x001F9950 File Offset: 0x001F7B50
		private void DrawThingRow(ref float y, float width, Thing thing, bool inventory = false)
		{
			Rect rect = new Rect(0f, y, width, 28f);
			Widgets.InfoCardButton(rect.width - 24f, y, thing);
			rect.width -= 24f;
			bool flag = false;
			if (this.CanControl && (inventory || this.CanControlColonist || (this.SelPawnForGear.Spawned && !this.SelPawnForGear.Map.IsPlayerHome)))
			{
				Rect rect2 = new Rect(rect.width - 24f, y, 24f, 24f);
				bool flag2;
				if (this.SelPawnForGear.IsQuestLodger())
				{
					if (inventory)
					{
						flag2 = true;
					}
					else
					{
						CompBiocodable compBiocodable = thing.TryGetComp<CompBiocodable>();
						if (compBiocodable != null && compBiocodable.Biocoded)
						{
							flag2 = true;
						}
						else
						{
							CompBladelinkWeapon compBladelinkWeapon = thing.TryGetComp<CompBladelinkWeapon>();
							flag2 = (compBladelinkWeapon != null && compBladelinkWeapon.bondedPawn == this.SelPawnForGear);
						}
					}
				}
				else
				{
					flag2 = false;
				}
				Apparel apparel;
				bool flag3 = (apparel = (thing as Apparel)) != null && this.SelPawnForGear.apparel != null && this.SelPawnForGear.apparel.IsLocked(apparel);
				flag = (flag2 || flag3);
				if (Mouse.IsOver(rect2))
				{
					if (flag3)
					{
						TooltipHandler.TipRegion(rect2, "DropThingLocked".Translate());
					}
					else if (flag2)
					{
						TooltipHandler.TipRegion(rect2, "DropThingLodger".Translate());
					}
					else
					{
						TooltipHandler.TipRegion(rect2, "DropThing".Translate());
					}
				}
				Color color = flag ? Color.grey : Color.white;
				Color mouseoverColor = flag ? color : GenUI.MouseoverColor;
				if (Widgets.ButtonImage(rect2, TexButton.Drop, color, mouseoverColor, !flag) && !flag)
				{
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					this.InterfaceDrop(thing);
				}
				rect.width -= 24f;
			}
			if (this.CanControlColonist)
			{
				if ((thing.def.IsNutritionGivingIngestible || thing.def.IsNonMedicalDrug) && thing.IngestibleNow && base.SelPawn.WillEat(thing, null, true))
				{
					Rect rect3 = new Rect(rect.width - 24f, y, 24f, 24f);
					TooltipHandler.TipRegionByKey(rect3, "ConsumeThing", thing.LabelNoCount, thing);
					if (Widgets.ButtonImage(rect3, TexButton.Ingest, true))
					{
						SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
						this.InterfaceIngest(thing);
					}
				}
				rect.width -= 24f;
			}
			Rect rect4 = rect;
			rect4.xMin = rect4.xMax - 60f;
			CaravanThingsTabUtility.DrawMass(thing, rect4);
			rect.width -= 60f;
			if (Mouse.IsOver(rect))
			{
				GUI.color = ITab_Pawn_Gear.HighlightColor;
				GUI.DrawTexture(rect, TexUI.HighlightTex);
			}
			if (thing.def.DrawMatSingle != null && thing.def.DrawMatSingle.mainTexture != null)
			{
				Widgets.ThingIcon(new Rect(4f, y, 28f, 28f), thing, 1f);
			}
			Text.Anchor = TextAnchor.MiddleLeft;
			GUI.color = ITab_Pawn_Gear.ThingLabelColor;
			Rect rect5 = new Rect(36f, y, rect.width - 36f, rect.height);
			string text = thing.LabelCap;
			Apparel apparel2 = thing as Apparel;
			if (apparel2 != null && this.SelPawnForGear.outfits != null && this.SelPawnForGear.outfits.forcedHandler.IsForced(apparel2))
			{
				text += ", " + "ApparelForcedLower".Translate();
			}
			if (flag)
			{
				text += " (" + "ApparelLockedLower".Translate() + ")";
			}
			Text.WordWrap = false;
			Widgets.Label(rect5, text.Truncate(rect5.width, null));
			Text.WordWrap = true;
			if (Mouse.IsOver(rect))
			{
				string text2 = thing.DescriptionDetailed;
				if (thing.def.useHitPoints)
				{
					text2 = string.Concat(new object[]
					{
						text2,
						"\n",
						thing.HitPoints,
						" / ",
						thing.MaxHitPoints
					});
				}
				TooltipHandler.TipRegion(rect, text2);
			}
			y += 28f;
		}

		// Token: 0x06005BAD RID: 23469 RVA: 0x001F9DC4 File Offset: 0x001F7FC4
		private void TryDrawOverallArmor(ref float curY, float width, StatDef stat, string label)
		{
			float num = 0f;
			float num2 = Mathf.Clamp01(this.SelPawnForGear.GetStatValue(stat, true) / 2f);
			List<BodyPartRecord> allParts = this.SelPawnForGear.RaceProps.body.AllParts;
			List<Apparel> list = (this.SelPawnForGear.apparel != null) ? this.SelPawnForGear.apparel.WornApparel : null;
			for (int i = 0; i < allParts.Count; i++)
			{
				float num3 = 1f - num2;
				if (list != null)
				{
					for (int j = 0; j < list.Count; j++)
					{
						if (list[j].def.apparel.CoversBodyPart(allParts[i]))
						{
							float num4 = Mathf.Clamp01(list[j].GetStatValue(stat, true) / 2f);
							num3 *= 1f - num4;
						}
					}
				}
				num += allParts[i].coverageAbs * (1f - num3);
			}
			num = Mathf.Clamp(num * 2f, 0f, 2f);
			Rect rect = new Rect(0f, curY, width, 100f);
			Widgets.Label(rect, label.Truncate(120f, null));
			rect.xMin += 120f;
			Widgets.Label(rect, num.ToStringPercent());
			curY += 22f;
		}

		// Token: 0x06005BAE RID: 23470 RVA: 0x001F9F30 File Offset: 0x001F8130
		private void TryDrawMassInfo(ref float curY, float width)
		{
			if (this.SelPawnForGear.Dead || !this.ShouldShowInventory(this.SelPawnForGear))
			{
				return;
			}
			Rect rect = new Rect(0f, curY, width, 22f);
			float num = MassUtility.GearAndInventoryMass(this.SelPawnForGear);
			float num2 = MassUtility.Capacity(this.SelPawnForGear, null);
			Widgets.Label(rect, "MassCarried".Translate(num.ToString("0.##"), num2.ToString("0.##")));
			curY += 22f;
		}

		// Token: 0x06005BAF RID: 23471 RVA: 0x001F9FC0 File Offset: 0x001F81C0
		private void TryDrawComfyTemperatureRange(ref float curY, float width)
		{
			if (this.SelPawnForGear.Dead)
			{
				return;
			}
			Rect rect = new Rect(0f, curY, width, 22f);
			float statValue = this.SelPawnForGear.GetStatValue(StatDefOf.ComfyTemperatureMin, true);
			float statValue2 = this.SelPawnForGear.GetStatValue(StatDefOf.ComfyTemperatureMax, true);
			Widgets.Label(rect, "ComfyTemperatureRange".Translate() + ": " + statValue.ToStringTemperature("F0") + " ~ " + statValue2.ToStringTemperature("F0"));
			curY += 22f;
		}

		// Token: 0x06005BB0 RID: 23472 RVA: 0x001FA060 File Offset: 0x001F8260
		private void InterfaceDrop(Thing t)
		{
			ThingWithComps thingWithComps = t as ThingWithComps;
			Apparel apparel = t as Apparel;
			if (apparel != null && this.SelPawnForGear.apparel != null && this.SelPawnForGear.apparel.WornApparel.Contains(apparel))
			{
				this.SelPawnForGear.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.RemoveApparel, apparel), JobTag.Misc);
				return;
			}
			if (thingWithComps != null && this.SelPawnForGear.equipment != null && this.SelPawnForGear.equipment.AllEquipmentListForReading.Contains(thingWithComps))
			{
				this.SelPawnForGear.jobs.TryTakeOrderedJob(JobMaker.MakeJob(JobDefOf.DropEquipment, thingWithComps), JobTag.Misc);
				return;
			}
			if (!t.def.destroyOnDrop)
			{
				Thing thing;
				this.SelPawnForGear.inventory.innerContainer.TryDrop(t, this.SelPawnForGear.Position, this.SelPawnForGear.Map, ThingPlaceMode.Near, out thing, null, null);
			}
		}

		// Token: 0x06005BB1 RID: 23473 RVA: 0x001FA150 File Offset: 0x001F8350
		private void InterfaceIngest(Thing t)
		{
			Job job = JobMaker.MakeJob(JobDefOf.Ingest, t);
			job.count = Mathf.Min(t.stackCount, t.def.ingestible.maxNumToIngestAtOnce);
			job.count = Mathf.Min(job.count, FoodUtility.WillIngestStackCountOf(this.SelPawnForGear, t.def, t.GetStatValue(StatDefOf.Nutrition, true)));
			this.SelPawnForGear.jobs.TryTakeOrderedJob(job, JobTag.Misc);
		}

		// Token: 0x06005BB2 RID: 23474 RVA: 0x001FA1D0 File Offset: 0x001F83D0
		private bool ShouldShowInventory(Pawn p)
		{
			return p.RaceProps.Humanlike || p.inventory.innerContainer.Any;
		}

		// Token: 0x06005BB3 RID: 23475 RVA: 0x001FA1F1 File Offset: 0x001F83F1
		private bool ShouldShowApparel(Pawn p)
		{
			return p.apparel != null && (p.RaceProps.Humanlike || p.apparel.WornApparel.Any<Apparel>());
		}

		// Token: 0x06005BB4 RID: 23476 RVA: 0x001FA21C File Offset: 0x001F841C
		private bool ShouldShowEquipment(Pawn p)
		{
			return p.equipment != null;
		}

		// Token: 0x06005BB5 RID: 23477 RVA: 0x001FA228 File Offset: 0x001F8428
		private bool ShouldShowOverallArmor(Pawn p)
		{
			return p.RaceProps.Humanlike || this.ShouldShowApparel(p) || p.GetStatValue(StatDefOf.ArmorRating_Sharp, true) > 0f || p.GetStatValue(StatDefOf.ArmorRating_Blunt, true) > 0f || p.GetStatValue(StatDefOf.ArmorRating_Heat, true) > 0f;
		}

		// Token: 0x04003208 RID: 12808
		private Vector2 scrollPosition = Vector2.zero;

		// Token: 0x04003209 RID: 12809
		private float scrollViewHeight;

		// Token: 0x0400320A RID: 12810
		private const float TopPadding = 20f;

		// Token: 0x0400320B RID: 12811
		public static readonly Color ThingLabelColor = new Color(0.9f, 0.9f, 0.9f, 1f);

		// Token: 0x0400320C RID: 12812
		public static readonly Color HighlightColor = new Color(0.5f, 0.5f, 0.5f, 1f);

		// Token: 0x0400320D RID: 12813
		private const float ThingIconSize = 28f;

		// Token: 0x0400320E RID: 12814
		private const float ThingRowHeight = 28f;

		// Token: 0x0400320F RID: 12815
		private const float ThingLeftX = 36f;

		// Token: 0x04003210 RID: 12816
		private const float StandardLineHeight = 22f;

		// Token: 0x04003211 RID: 12817
		private static List<Thing> workingInvList = new List<Thing>();
	}
}
