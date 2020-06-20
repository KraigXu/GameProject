﻿using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000E3B RID: 3643
	public class Designator_Build : Designator_Place
	{
		// Token: 0x17000FC2 RID: 4034
		// (get) Token: 0x0600580C RID: 22540 RVA: 0x001D34A3 File Offset: 0x001D16A3
		public override BuildableDef PlacingDef
		{
			get
			{
				return this.entDef;
			}
		}

		// Token: 0x17000FC3 RID: 4035
		// (get) Token: 0x0600580D RID: 22541 RVA: 0x001D34AC File Offset: 0x001D16AC
		public override string Label
		{
			get
			{
				ThingDef thingDef = this.entDef as ThingDef;
				if (thingDef != null && this.writeStuff)
				{
					return GenLabel.ThingLabel(thingDef, this.stuffDef, 1);
				}
				if (thingDef != null && thingDef.MadeFromStuff)
				{
					return this.entDef.label + "...";
				}
				return this.entDef.label;
			}
		}

		// Token: 0x17000FC4 RID: 4036
		// (get) Token: 0x0600580E RID: 22542 RVA: 0x001D350A File Offset: 0x001D170A
		public override string Desc
		{
			get
			{
				return this.entDef.description;
			}
		}

		// Token: 0x17000FC5 RID: 4037
		// (get) Token: 0x0600580F RID: 22543 RVA: 0x001D3517 File Offset: 0x001D1717
		public override Color IconDrawColor
		{
			get
			{
				if (this.stuffDef != null)
				{
					return this.entDef.GetColorForStuff(this.stuffDef);
				}
				return this.entDef.uiIconColor;
			}
		}

		// Token: 0x17000FC6 RID: 4038
		// (get) Token: 0x06005810 RID: 22544 RVA: 0x001D3540 File Offset: 0x001D1740
		public override bool Visible
		{
			get
			{
				if (DebugSettings.godMode)
				{
					return true;
				}
				if (this.entDef.minTechLevelToBuild != TechLevel.Undefined && Faction.OfPlayer.def.techLevel < this.entDef.minTechLevelToBuild)
				{
					return false;
				}
				if (this.entDef.maxTechLevelToBuild != TechLevel.Undefined && Faction.OfPlayer.def.techLevel > this.entDef.maxTechLevelToBuild)
				{
					return false;
				}
				if (!this.entDef.IsResearchFinished)
				{
					return false;
				}
				if (this.entDef.PlaceWorkers != null)
				{
					using (List<PlaceWorker>.Enumerator enumerator = this.entDef.PlaceWorkers.GetEnumerator())
					{
						while (enumerator.MoveNext())
						{
							if (!enumerator.Current.IsBuildDesignatorVisible(this.entDef))
							{
								return false;
							}
						}
					}
				}
				if (this.entDef.buildingPrerequisites != null)
				{
					for (int i = 0; i < this.entDef.buildingPrerequisites.Count; i++)
					{
						if (!base.Map.listerBuildings.ColonistsHaveBuilding(this.entDef.buildingPrerequisites[i]))
						{
							return false;
						}
					}
				}
				return true;
			}
		}

		// Token: 0x17000FC7 RID: 4039
		// (get) Token: 0x06005811 RID: 22545 RVA: 0x001D366C File Offset: 0x001D186C
		public override int DraggableDimensions
		{
			get
			{
				return this.entDef.placingDraggableDimensions;
			}
		}

		// Token: 0x17000FC8 RID: 4040
		// (get) Token: 0x06005812 RID: 22546 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool DragDrawMeasurements
		{
			get
			{
				return true;
			}
		}

		// Token: 0x17000FC9 RID: 4041
		// (get) Token: 0x06005813 RID: 22547 RVA: 0x001D3679 File Offset: 0x001D1879
		public override float PanelReadoutTitleExtraRightMargin
		{
			get
			{
				return 20f;
			}
		}

		// Token: 0x17000FCA RID: 4042
		// (get) Token: 0x06005814 RID: 22548 RVA: 0x001D3680 File Offset: 0x001D1880
		public override string HighlightTag
		{
			get
			{
				if (this.cachedHighlightTag == null && this.tutorTag != null)
				{
					this.cachedHighlightTag = "Designator-Build-" + this.tutorTag;
				}
				return this.cachedHighlightTag;
			}
		}

		// Token: 0x06005815 RID: 22549 RVA: 0x001D36B0 File Offset: 0x001D18B0
		public Designator_Build(BuildableDef entDef)
		{
			this.entDef = entDef;
			this.icon = entDef.uiIcon;
			this.iconAngle = entDef.uiIconAngle;
			this.iconOffset = entDef.uiIconOffset;
			this.hotKey = entDef.designationHotKey;
			this.tutorTag = entDef.defName;
			this.order = 20f;
			ThingDef thingDef = entDef as ThingDef;
			if (thingDef != null)
			{
				this.iconProportions = thingDef.graphicData.drawSize.RotatedBy(thingDef.defaultPlacingRot);
				this.iconDrawScale = GenUI.IconDrawScale(thingDef);
			}
			else
			{
				this.iconProportions = new Vector2(1f, 1f);
				this.iconDrawScale = 1f;
			}
			if (entDef is TerrainDef)
			{
				this.iconTexCoords = Widgets.CroppedTerrainTextureRect(this.icon);
			}
			this.ResetStuffToDefault();
		}

		// Token: 0x06005816 RID: 22550 RVA: 0x001D3784 File Offset: 0x001D1984
		public override GizmoResult GizmoOnGUI(Vector2 topLeft, float maxWidth)
		{
			GizmoResult result = base.GizmoOnGUI(topLeft, maxWidth);
			ThingDef thingDef = this.entDef as ThingDef;
			if (thingDef != null && thingDef.MadeFromStuff)
			{
				Designator_Dropdown.DrawExtraOptionsIcon(topLeft, this.GetWidth(maxWidth));
			}
			return result;
		}

		// Token: 0x06005817 RID: 22551 RVA: 0x001D37BD File Offset: 0x001D19BD
		protected override void DrawIcon(Rect rect, Material buttonMat = null)
		{
			Widgets.DefIcon(rect, this.PlacingDef, this.stuffDef, 0.85f, false);
		}

		// Token: 0x06005818 RID: 22552 RVA: 0x001D37D8 File Offset: 0x001D19D8
		public Texture2D ResolvedIcon()
		{
			Graphic_Appearances graphic_Appearances;
			if (this.stuffDef != null && (graphic_Appearances = (this.entDef.graphic as Graphic_Appearances)) != null)
			{
				return (Texture2D)graphic_Appearances.SubGraphicFor(this.stuffDef).MatAt(this.entDef.defaultPlacingRot, null).mainTexture;
			}
			return this.icon;
		}

		// Token: 0x06005819 RID: 22553 RVA: 0x001D3830 File Offset: 0x001D1A30
		public void ResetStuffToDefault()
		{
			ThingDef thingDef = this.entDef as ThingDef;
			if (thingDef != null && thingDef.MadeFromStuff)
			{
				this.stuffDef = GenStuff.DefaultStuffFor(thingDef);
			}
		}

		// Token: 0x0600581A RID: 22554 RVA: 0x001D3860 File Offset: 0x001D1A60
		public override void DrawMouseAttachments()
		{
			base.DrawMouseAttachments();
			if (!ArchitectCategoryTab.InfoRect.Contains(UI.MousePositionOnUIInverted))
			{
				DesignationDragger dragger = Find.DesignatorManager.Dragger;
				int num;
				if (dragger.Dragging)
				{
					num = dragger.DragCells.Count<IntVec3>();
				}
				else
				{
					num = 1;
				}
				float num2 = 0f;
				Vector2 vector = Event.current.mousePosition + Designator_Build.DragPriceDrawOffset;
				List<ThingDefCountClass> list = this.entDef.CostListAdjusted(this.stuffDef, true);
				for (int i = 0; i < list.Count; i++)
				{
					ThingDefCountClass thingDefCountClass = list[i];
					float y = vector.y + num2;
					Widgets.ThingIcon(new Rect(vector.x, y, 27f, 27f), thingDefCountClass.thingDef, null, 1f);
					Rect rect = new Rect(vector.x + 29f, y, 999f, 29f);
					int num3 = num * thingDefCountClass.count;
					string text = num3.ToString();
					if (base.Map.resourceCounter.GetCount(thingDefCountClass.thingDef) < num3)
					{
						GUI.color = Color.red;
						text += " (" + "NotEnoughStoredLower".Translate() + ")";
					}
					Text.Font = GameFont.Small;
					Text.Anchor = TextAnchor.MiddleLeft;
					Widgets.Label(rect, text);
					Text.Anchor = TextAnchor.UpperLeft;
					GUI.color = Color.white;
					num2 += 29f;
				}
			}
		}

		// Token: 0x0600581B RID: 22555 RVA: 0x001D39E8 File Offset: 0x001D1BE8
		public override void ProcessInput(Event ev)
		{
			if (!base.CheckCanInteract())
			{
				return;
			}
			ThingDef thingDef = this.entDef as ThingDef;
			if (thingDef == null || !thingDef.MadeFromStuff)
			{
				base.ProcessInput(ev);
				return;
			}
			List<FloatMenuOption> list = new List<FloatMenuOption>();
			foreach (ThingDef thingDef2 in base.Map.resourceCounter.AllCountedAmounts.Keys)
			{
				if (thingDef2.IsStuff && thingDef2.stuffProps.CanMake(thingDef) && (DebugSettings.godMode || base.Map.listerThings.ThingsOfDef(thingDef2).Count > 0))
				{
					ThingDef localStuffDef = thingDef2;
					list.Add(new FloatMenuOption(GenLabel.ThingLabel(this.entDef, localStuffDef, 1).CapitalizeFirst(), delegate
					{
						this.<>n__0(ev);
						Find.DesignatorManager.Select(this);
						this.stuffDef = localStuffDef;
						this.writeStuff = true;
					}, thingDef2, MenuOptionPriority.Default, null, null, 0f, null, null)
					{
						tutorTag = "SelectStuff-" + thingDef.defName + "-" + localStuffDef.defName
					});
				}
			}
			if (list.Count == 0)
			{
				Messages.Message("NoStuffsToBuildWith".Translate(), MessageTypeDefOf.RejectInput, false);
				return;
			}
			FloatMenu floatMenu = new FloatMenu(list);
			floatMenu.vanishIfMouseDistant = true;
			floatMenu.onCloseCallback = delegate
			{
				this.writeStuff = true;
			};
			Find.WindowStack.Add(floatMenu);
			Find.DesignatorManager.Select(this);
		}

		// Token: 0x0600581C RID: 22556 RVA: 0x001D3BB0 File Offset: 0x001D1DB0
		public override AcceptanceReport CanDesignateCell(IntVec3 c)
		{
			return GenConstruct.CanPlaceBlueprintAt(this.entDef, c, this.placingRot, base.Map, DebugSettings.godMode, null, null, this.stuffDef);
		}

		// Token: 0x0600581D RID: 22557 RVA: 0x001D3BD8 File Offset: 0x001D1DD8
		public override void DesignateSingleCell(IntVec3 c)
		{
			if (TutorSystem.TutorialMode && !TutorSystem.AllowAction(new EventPack(base.TutorTagDesignate, c)))
			{
				return;
			}
			if (DebugSettings.godMode || this.entDef.GetStatValueAbstract(StatDefOf.WorkToBuild, this.stuffDef) == 0f)
			{
				if (this.entDef is TerrainDef)
				{
					base.Map.terrainGrid.SetTerrain(c, (TerrainDef)this.entDef);
				}
				else
				{
					Thing thing = ThingMaker.MakeThing((ThingDef)this.entDef, this.stuffDef);
					thing.SetFactionDirect(Faction.OfPlayer);
					GenSpawn.Spawn(thing, c, base.Map, this.placingRot, WipeMode.Vanish, false);
				}
			}
			else
			{
				GenSpawn.WipeExistingThings(c, this.placingRot, this.entDef.blueprintDef, base.Map, DestroyMode.Deconstruct);
				GenConstruct.PlaceBlueprintForBuild(this.entDef, c, base.Map, this.placingRot, Faction.OfPlayer, this.stuffDef);
			}
			MoteMaker.ThrowMetaPuffs(GenAdj.OccupiedRect(c, this.placingRot, this.entDef.Size), base.Map);
			ThingDef thingDef = this.entDef as ThingDef;
			if (thingDef != null && thingDef.IsOrbitalTradeBeacon)
			{
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.BuildOrbitalTradeBeacon, KnowledgeAmount.Total);
			}
			if (TutorSystem.TutorialMode)
			{
				TutorSystem.Notify_Event(new EventPack(base.TutorTagDesignate, c));
			}
			if (this.entDef.PlaceWorkers != null)
			{
				for (int i = 0; i < this.entDef.PlaceWorkers.Count; i++)
				{
					this.entDef.PlaceWorkers[i].PostPlace(base.Map, this.entDef, c, this.placingRot);
				}
			}
		}

		// Token: 0x0600581E RID: 22558 RVA: 0x001D3D76 File Offset: 0x001D1F76
		public override void SelectedUpdate()
		{
			base.SelectedUpdate();
			BuildDesignatorUtility.TryDrawPowerGridAndAnticipatedConnection(this.entDef, this.placingRot);
		}

		// Token: 0x0600581F RID: 22559 RVA: 0x001D3D90 File Offset: 0x001D1F90
		public override void DrawPanelReadout(ref float curY, float width)
		{
			if (this.entDef.costStuffCount <= 0 && this.stuffDef != null)
			{
				this.stuffDef = null;
			}
			ThingDef thingDef = this.entDef as ThingDef;
			if (thingDef != null)
			{
				Widgets.InfoCardButton(width - 24f - 2f, 6f, thingDef, this.stuffDef);
			}
			else
			{
				Widgets.InfoCardButton(width - 24f - 2f, 6f, this.entDef);
			}
			Text.Font = GameFont.Small;
			List<ThingDefCountClass> list = this.entDef.CostListAdjusted(this.stuffDef, false);
			for (int i = 0; i < list.Count; i++)
			{
				ThingDefCountClass thingDefCountClass = list[i];
				Color color = GUI.color;
				Widgets.ThingIcon(new Rect(0f, curY, 20f, 20f), thingDefCountClass.thingDef, null, 1f);
				GUI.color = color;
				if (thingDefCountClass.thingDef != null && thingDefCountClass.thingDef.resourceReadoutPriority != ResourceCountPriority.Uncounted && base.Map.resourceCounter.GetCount(thingDefCountClass.thingDef) < thingDefCountClass.count)
				{
					GUI.color = Color.red;
				}
				Widgets.Label(new Rect(26f, curY + 2f, 50f, 100f), thingDefCountClass.count.ToString());
				GUI.color = Color.white;
				string text;
				if (thingDefCountClass.thingDef == null)
				{
					text = "(" + "UnchosenStuff".Translate() + ")";
				}
				else
				{
					text = thingDefCountClass.thingDef.LabelCap;
				}
				float width2 = width - 60f;
				float num = Text.CalcHeight(text, width2) - 5f;
				Widgets.Label(new Rect(60f, curY + 2f, width2, num + 5f), text);
				curY += num;
			}
			if (this.entDef.constructionSkillPrerequisite > 0)
			{
				this.DrawSkillRequirement(SkillDefOf.Construction, this.entDef.constructionSkillPrerequisite, width, ref curY);
			}
			if (this.entDef.artisticSkillPrerequisite > 0)
			{
				this.DrawSkillRequirement(SkillDefOf.Artistic, this.entDef.artisticSkillPrerequisite, width, ref curY);
			}
			bool flag = false;
			foreach (Pawn pawn in Find.CurrentMap.mapPawns.FreeColonists)
			{
				if (pawn.skills.GetSkill(SkillDefOf.Construction).Level >= this.entDef.constructionSkillPrerequisite && pawn.skills.GetSkill(SkillDefOf.Artistic).Level >= this.entDef.artisticSkillPrerequisite)
				{
					flag = true;
					break;
				}
			}
			if (!flag)
			{
				TaggedString taggedString = "NoColonistWithAllSkillsForConstructing".Translate(Faction.OfPlayer.def.pawnsPlural);
				Rect rect = new Rect(0f, curY + 2f, width, Text.CalcHeight(taggedString, width));
				GUI.color = Color.red;
				Widgets.Label(rect, taggedString);
				GUI.color = Color.white;
				curY += rect.height;
			}
			curY += 4f;
		}

		// Token: 0x06005820 RID: 22560 RVA: 0x001D40CC File Offset: 0x001D22CC
		private bool AnyColonistWithSkill(int skill, SkillDef skillDef, bool careIfDisabled)
		{
			foreach (Pawn pawn in Find.CurrentMap.mapPawns.FreeColonists)
			{
				if (pawn.skills.GetSkill(skillDef).Level >= skill && (!careIfDisabled || pawn.workSettings.WorkIsActive(WorkTypeDefOf.Construction)))
				{
					return true;
				}
			}
			return false;
		}

		// Token: 0x06005821 RID: 22561 RVA: 0x001D4154 File Offset: 0x001D2354
		private void DrawSkillRequirement(SkillDef skillDef, int requirement, float width, ref float curY)
		{
			Rect rect = new Rect(0f, curY + 2f, width, 24f);
			if (!this.AnyColonistWithSkill(requirement, skillDef, false))
			{
				GUI.color = Color.red;
				TooltipHandler.TipRegionByKey(rect, "NoColonistWithSkillTip", Faction.OfPlayer.def.pawnsPlural);
			}
			else if (!this.AnyColonistWithSkill(requirement, skillDef, true))
			{
				GUI.color = Color.yellow;
				TooltipHandler.TipRegionByKey(rect, "AllColonistsWithSkillHaveDisabledConstructingTip", Faction.OfPlayer.def.pawnsPlural, WorkTypeDefOf.Construction.gerundLabel);
			}
			else
			{
				GUI.color = new Color(0.72f, 0.87f, 0.72f);
			}
			Widgets.Label(rect, string.Format("{0}: {1}", "SkillNeededForConstructing".Translate(skillDef.LabelCap), requirement));
			GUI.color = Color.white;
			curY += 18f;
		}

		// Token: 0x06005822 RID: 22562 RVA: 0x001D4256 File Offset: 0x001D2456
		public void SetStuffDef(ThingDef stuffDef)
		{
			this.stuffDef = stuffDef;
		}

		// Token: 0x06005823 RID: 22563 RVA: 0x001CFE8D File Offset: 0x001CE08D
		public override void RenderHighlight(List<IntVec3> dragCells)
		{
			DesignatorUtility.RenderHighlightOverSelectableCells(this, dragCells);
		}

		// Token: 0x04002FA7 RID: 12199
		protected BuildableDef entDef;

		// Token: 0x04002FA8 RID: 12200
		private ThingDef stuffDef;

		// Token: 0x04002FA9 RID: 12201
		private bool writeStuff;

		// Token: 0x04002FAA RID: 12202
		private static readonly Vector2 DragPriceDrawOffset = new Vector2(19f, 17f);

		// Token: 0x04002FAB RID: 12203
		private const float DragPriceDrawNumberX = 29f;
	}
}
