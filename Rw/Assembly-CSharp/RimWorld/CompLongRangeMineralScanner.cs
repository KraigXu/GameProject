using System;
using System.Collections.Generic;
using RimWorld.QuestGen;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000D85 RID: 3461
	public class CompLongRangeMineralScanner : CompScanner
	{
		// Token: 0x17000EFB RID: 3835
		// (get) Token: 0x06005457 RID: 21591 RVA: 0x001C2857 File Offset: 0x001C0A57
		public new CompProperties_LongRangeMineralScanner Props
		{
			get
			{
				return this.props as CompProperties_LongRangeMineralScanner;
			}
		}

		// Token: 0x06005458 RID: 21592 RVA: 0x001C2864 File Offset: 0x001C0A64
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.targetMineable, "targetMineable");
			if (Scribe.mode == LoadSaveMode.PostLoadInit && this.targetMineable == null)
			{
				this.SetDefaultTargetMineral();
			}
		}

		// Token: 0x06005459 RID: 21593 RVA: 0x001C2892 File Offset: 0x001C0A92
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.SetDefaultTargetMineral();
		}

		// Token: 0x0600545A RID: 21594 RVA: 0x001C28A1 File Offset: 0x001C0AA1
		private void SetDefaultTargetMineral()
		{
			this.targetMineable = ThingDefOf.MineableGold;
		}

		// Token: 0x0600545B RID: 21595 RVA: 0x001C28B0 File Offset: 0x001C0AB0
		protected override void DoFind(Pawn worker)
		{
			Slate slate = new Slate();
			slate.Set<Map>("map", this.parent.Map, false);
			slate.Set<ThingDef>("targetMineable", this.targetMineable, false);
			slate.Set<Pawn>("worker", worker, false);
			if (!QuestScriptDefOf.LongRangeMineralScannerLump.CanRun(slate))
			{
				return;
			}
			Quest quest = QuestUtility.GenerateQuestAndMakeAvailable(QuestScriptDefOf.LongRangeMineralScannerLump, slate);
			Find.LetterStack.ReceiveLetter(quest.name, quest.description, LetterDefOf.PositiveEvent, null, null, quest, null, null);
		}

		// Token: 0x0600545C RID: 21596 RVA: 0x001C2938 File Offset: 0x001C0B38
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			foreach (Gizmo gizmo in this.<>n__0())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (this.parent.Faction == Faction.OfPlayer)
			{
				ThingDef mineableThing = this.targetMineable.building.mineableThing;
				Command_Action command_Action = new Command_Action();
				command_Action.defaultLabel = "CommandSelectMineralToScanFor".Translate() + ": " + mineableThing.LabelCap;
				command_Action.icon = mineableThing.uiIcon;
				command_Action.iconAngle = mineableThing.uiIconAngle;
				command_Action.iconOffset = mineableThing.uiIconOffset;
				command_Action.action = delegate
				{
					List<ThingDef> mineables = ((GenStep_PreciousLump)GenStepDefOf.PreciousLump.genStep).mineables;
					List<FloatMenuOption> list = new List<FloatMenuOption>();
					foreach (ThingDef localD2 in mineables)
					{
						ThingDef localD = localD2;
						FloatMenuOption item = new FloatMenuOption(localD.building.mineableThing.LabelCap, delegate
						{
							foreach (object obj in Find.Selector.SelectedObjects)
							{
								Thing thing = obj as Thing;
								if (thing != null)
								{
									CompLongRangeMineralScanner compLongRangeMineralScanner = thing.TryGetComp<CompLongRangeMineralScanner>();
									if (compLongRangeMineralScanner != null)
									{
										compLongRangeMineralScanner.targetMineable = localD;
									}
								}
							}
						}, MenuOptionPriority.Default, null, null, 29f, (Rect rect) => Widgets.InfoCardButton(rect.x + 5f, rect.y + (rect.height - 24f) / 2f, localD.building.mineableThing), null);
						list.Add(item);
					}
					Find.WindowStack.Add(new FloatMenu(list));
				};
				yield return command_Action;
			}
			yield break;
			yield break;
		}

		// Token: 0x04002E73 RID: 11891
		private ThingDef targetMineable;
	}
}
