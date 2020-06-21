using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using Verse;

namespace RimWorld.Planet
{
	// Token: 0x0200125D RID: 4701
	[StaticConstructorOnStartup]
	public class Site : MapParent
	{
		// Token: 0x1700126B RID: 4715
		// (get) Token: 0x06006DEB RID: 28139 RVA: 0x00266808 File Offset: 0x00264A08
		public override string Label
		{
			get
			{
				if (!this.customLabel.NullOrEmpty())
				{
					return this.customLabel;
				}
				if (this.MainSitePartDef == SitePartDefOf.PreciousLump && this.MainSitePart.parms.preciousLumpResources != null)
				{
					return "PreciousLumpLabel".Translate(this.MainSitePart.parms.preciousLumpResources.label);
				}
				return this.MainSitePartDef.label;
			}
		}

		// Token: 0x1700126C RID: 4716
		// (get) Token: 0x06006DEC RID: 28140 RVA: 0x0026687D File Offset: 0x00264A7D
		public override Texture2D ExpandingIcon
		{
			get
			{
				return this.MainSitePartDef.ExpandingIconTexture;
			}
		}

		// Token: 0x1700126D RID: 4717
		// (get) Token: 0x06006DED RID: 28141 RVA: 0x0001028D File Offset: 0x0000E48D
		public override bool HandlesConditionCausers
		{
			get
			{
				return true;
			}
		}

		// Token: 0x1700126E RID: 4718
		// (get) Token: 0x06006DEE RID: 28142 RVA: 0x0026688C File Offset: 0x00264A8C
		public override Material Material
		{
			get
			{
				if (this.cachedMat == null)
				{
					Color color;
					if (this.MainSitePartDef.applyFactionColorToSiteTexture && base.Faction != null)
					{
						color = base.Faction.Color;
					}
					else
					{
						color = Color.white;
					}
					this.cachedMat = MaterialPool.MatFrom(this.MainSitePartDef.siteTexture, ShaderDatabase.WorldOverlayTransparentLit, color, WorldMaterials.WorldObjectRenderQueue);
				}
				return this.cachedMat;
			}
		}

		// Token: 0x1700126F RID: 4719
		// (get) Token: 0x06006DEF RID: 28143 RVA: 0x002668F7 File Offset: 0x00264AF7
		public override bool AppendFactionToInspectString
		{
			get
			{
				return this.MainSitePartDef.applyFactionColorToSiteTexture || this.MainSitePartDef.showFactionInInspectString;
			}
		}

		// Token: 0x17001270 RID: 4720
		// (get) Token: 0x06006DF0 RID: 28144 RVA: 0x00266914 File Offset: 0x00264B14
		private SitePart MainSitePart
		{
			get
			{
				if (!this.parts.Any<SitePart>())
				{
					Log.ErrorOnce("Site without any SitePart at " + base.Tile, this.ID ^ 93890909, false);
					return null;
				}
				if (this.parts[0].hidden)
				{
					Log.ErrorOnce("Site with first SitePart hidden at " + base.Tile, this.ID ^ 48471239, false);
					return this.parts[0];
				}
				return this.parts[0];
			}
		}

		// Token: 0x17001271 RID: 4721
		// (get) Token: 0x06006DF1 RID: 28145 RVA: 0x002669AA File Offset: 0x00264BAA
		private SitePartDef MainSitePartDef
		{
			get
			{
				return this.MainSitePart.def;
			}
		}

		// Token: 0x17001272 RID: 4722
		// (get) Token: 0x06006DF2 RID: 28146 RVA: 0x002669B7 File Offset: 0x00264BB7
		public override IEnumerable<GenStepWithParams> ExtraGenStepDefs
		{
			get
			{
				foreach (GenStepWithParams genStepWithParams in this.<>n__0())
				{
					yield return genStepWithParams;
				}
				IEnumerator<GenStepWithParams> enumerator = null;
				int num;
				for (int i = 0; i < this.parts.Count; i = num + 1)
				{
					GenStepParams partGenStepParams = default(GenStepParams);
					partGenStepParams.sitePart = this.parts[i];
					List<GenStepDef> partGenStepDefs = this.parts[i].def.ExtraGenSteps;
					for (int j = 0; j < partGenStepDefs.Count; j = num + 1)
					{
						yield return new GenStepWithParams(partGenStepDefs[j], partGenStepParams);
						num = j;
					}
					partGenStepParams = default(GenStepParams);
					partGenStepDefs = null;
					num = i;
				}
				yield break;
				yield break;
			}
		}

		// Token: 0x17001273 RID: 4723
		// (get) Token: 0x06006DF3 RID: 28147 RVA: 0x002669C8 File Offset: 0x00264BC8
		public string ApproachOrderString
		{
			get
			{
				return this.MainSitePartDef.approachOrderString.NullOrEmpty() ? "ApproachSite".Translate(this.Label) : this.MainSitePartDef.approachOrderString.Formatted(this.Label);
			}
		}

		// Token: 0x17001274 RID: 4724
		// (get) Token: 0x06006DF4 RID: 28148 RVA: 0x00266A20 File Offset: 0x00264C20
		public string ApproachingReportString
		{
			get
			{
				return this.MainSitePartDef.approachingReportString.NullOrEmpty() ? "ApproachingSite".Translate(this.Label) : this.MainSitePartDef.approachingReportString.Formatted(this.Label);
			}
		}

		// Token: 0x17001275 RID: 4725
		// (get) Token: 0x06006DF5 RID: 28149 RVA: 0x00266A78 File Offset: 0x00264C78
		public float ActualThreatPoints
		{
			get
			{
				float num = 0f;
				for (int i = 0; i < this.parts.Count; i++)
				{
					num += this.parts[i].parms.threatPoints;
				}
				return num;
			}
		}

		// Token: 0x17001276 RID: 4726
		// (get) Token: 0x06006DF6 RID: 28150 RVA: 0x00266ABC File Offset: 0x00264CBC
		public bool IncreasesPopulation
		{
			get
			{
				if (base.HasMap)
				{
					return false;
				}
				for (int i = 0; i < this.parts.Count; i++)
				{
					if (this.parts[i].def.Worker.IncreasesPopulation(this.parts[i].parms))
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17001277 RID: 4727
		// (get) Token: 0x06006DF7 RID: 28151 RVA: 0x00266B1C File Offset: 0x00264D1C
		public bool BadEvenIfNoMap
		{
			get
			{
				for (int i = 0; i < this.parts.Count; i++)
				{
					if (this.parts[i].def.badEvenIfNoMap)
					{
						return true;
					}
				}
				return false;
			}
		}

		// Token: 0x17001278 RID: 4728
		// (get) Token: 0x06006DF8 RID: 28152 RVA: 0x00266B5A File Offset: 0x00264D5A
		public bool HasWorldObjectTimeout
		{
			get
			{
				return this.WorldObjectTimeoutTicksLeft != -1;
			}
		}

		// Token: 0x17001279 RID: 4729
		// (get) Token: 0x06006DF9 RID: 28153 RVA: 0x00266B68 File Offset: 0x00264D68
		public int WorldObjectTimeoutTicksLeft
		{
			get
			{
				List<Quest> questsListForReading = Find.QuestManager.QuestsListForReading;
				for (int i = 0; i < questsListForReading.Count; i++)
				{
					Quest quest = questsListForReading[i];
					if (quest.State == QuestState.Ongoing)
					{
						for (int j = 0; j < quest.PartsListForReading.Count; j++)
						{
							QuestPart_WorldObjectTimeout questPart_WorldObjectTimeout = quest.PartsListForReading[j] as QuestPart_WorldObjectTimeout;
							if (questPart_WorldObjectTimeout != null && questPart_WorldObjectTimeout.State == QuestPartState.Enabled && questPart_WorldObjectTimeout.worldObject == this)
							{
								return questPart_WorldObjectTimeout.TicksLeft;
							}
						}
					}
				}
				return -1;
			}
		}

		// Token: 0x06006DFA RID: 28154 RVA: 0x00266BEC File Offset: 0x00264DEC
		public override void Destroy()
		{
			base.Destroy();
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].def.Worker.PostDestroy(this.parts[i]);
			}
			for (int j = 0; j < this.parts.Count; j++)
			{
				this.parts[j].PostDestroy();
			}
		}

		// Token: 0x06006DFB RID: 28155 RVA: 0x00266C64 File Offset: 0x00264E64
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Values.Look<string>(ref this.customLabel, "customLabel", null, false);
			Scribe_Deep.Look<SiteCoreBackCompat>(ref this.coreBackCompat, "core", Array.Empty<object>());
			Scribe_Collections.Look<SitePart>(ref this.parts, "parts", LookMode.Deep, Array.Empty<object>());
			Scribe_Values.Look<bool>(ref this.startedCountdown, "startedCountdown", false, false);
			Scribe_Values.Look<bool>(ref this.anyEnemiesInitially, "anyEnemiesInitially", false, false);
			Scribe_Values.Look<bool>(ref this.allEnemiesDefeatedSignalSent, "allEnemiesDefeatedSignalSent", false, false);
			Scribe_Values.Look<bool>(ref this.factionMustRemainHostile, "factionMustRemainHostile", false, false);
			Scribe_Values.Look<float>(ref this.desiredThreatPoints, "desiredThreatPoints", 0f, false);
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				if (this.coreBackCompat != null && this.coreBackCompat.def != null)
				{
					this.parts.Insert(0, new SitePart(this, this.coreBackCompat.def, this.coreBackCompat.parms));
					this.coreBackCompat = null;
				}
				if (this.parts.RemoveAll((SitePart x) => x == null || x.def == null) != 0)
				{
					Log.Error("Some site parts were null after loading.", false);
				}
				for (int i = 0; i < this.parts.Count; i++)
				{
					this.parts[i].site = this;
				}
				BackCompatibility.PostExposeData(this);
			}
		}

		// Token: 0x06006DFC RID: 28156 RVA: 0x00266DC6 File Offset: 0x00264FC6
		public void AddPart(SitePart part)
		{
			this.parts.Add(part);
			part.def.Worker.Init(this, part);
		}

		// Token: 0x06006DFD RID: 28157 RVA: 0x00266DE8 File Offset: 0x00264FE8
		public override void Tick()
		{
			base.Tick();
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].SitePartTick();
			}
			for (int j = 0; j < this.parts.Count; j++)
			{
				this.parts[j].def.Worker.SitePartWorkerTick(this.parts[j]);
			}
			if (base.HasMap)
			{
				this.CheckStartForceExitAndRemoveMapCountdown();
				this.CheckAllEnemiesDefeated();
			}
		}

		// Token: 0x06006DFE RID: 28158 RVA: 0x00266E74 File Offset: 0x00265074
		public override void PostMapGenerate()
		{
			base.PostMapGenerate();
			Map map = base.Map;
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].def.Worker.PostMapGenerate(map);
			}
			this.anyEnemiesInitially = GenHostility.AnyHostileActiveThreatToPlayer(base.Map, false);
			this.allEnemiesDefeatedSignalSent = false;
		}

		// Token: 0x06006DFF RID: 28159 RVA: 0x00266EDC File Offset: 0x002650DC
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].def.Worker.PostDrawExtraSelectionOverlays(this.parts[i]);
			}
		}

		// Token: 0x06006E00 RID: 28160 RVA: 0x00266F2C File Offset: 0x0026512C
		public override void Notify_MyMapAboutToBeRemoved()
		{
			base.Notify_MyMapAboutToBeRemoved();
			for (int i = 0; i < this.parts.Count; i++)
			{
				this.parts[i].def.Worker.Notify_SiteMapAboutToBeRemoved(this.parts[i]);
			}
		}

		// Token: 0x06006E01 RID: 28161 RVA: 0x00266F7C File Offset: 0x0026517C
		public override bool ShouldRemoveMapNow(out bool alsoRemoveWorldObject)
		{
			if (!base.Map.mapPawns.AnyPawnBlockingMapRemoval)
			{
				bool flag;
				if (base.Map.listerThings.ThingsInGroup(ThingRequestGroup.ConditionCauser).Any<Thing>())
				{
					flag = !this.parts.Any((SitePart x) => x.def.Worker is SitePartWorker_ConditionCauser);
				}
				else
				{
					flag = true;
				}
				alsoRemoveWorldObject = flag;
				return true;
			}
			alsoRemoveWorldObject = false;
			return false;
		}

		// Token: 0x06006E02 RID: 28162 RVA: 0x00266FEC File Offset: 0x002651EC
		public override void GetChildHolders(List<IThingHolder> outChildren)
		{
			base.GetChildHolders(outChildren);
			for (int i = 0; i < this.parts.Count; i++)
			{
				outChildren.Add(this.parts[i]);
			}
		}

		// Token: 0x06006E03 RID: 28163 RVA: 0x00267028 File Offset: 0x00265228
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Caravan caravan)
		{
			foreach (FloatMenuOption floatMenuOption in this.<>n__1(caravan))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			if (!base.HasMap)
			{
				foreach (FloatMenuOption floatMenuOption2 in CaravanArrivalAction_VisitSite.GetFloatMenuOptions(caravan, this))
				{
					yield return floatMenuOption2;
				}
				enumerator = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x06006E04 RID: 28164 RVA: 0x0026703F File Offset: 0x0026523F
		public override IEnumerable<FloatMenuOption> GetTransportPodsFloatMenuOptions(IEnumerable<IThingHolder> pods, CompLaunchable representative)
		{
			foreach (FloatMenuOption floatMenuOption in this.<>n__2(pods, representative))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			foreach (FloatMenuOption floatMenuOption2 in TransportPodsArrivalAction_VisitSite.GetFloatMenuOptions(representative, pods, this))
			{
				yield return floatMenuOption2;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06006E05 RID: 28165 RVA: 0x0026705D File Offset: 0x0026525D
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in this.<>n__3())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			if (base.HasMap && Find.WorldSelector.SingleSelectedObject == this)
			{
				yield return SettleInExistingMapUtility.SettleCommand(base.Map, true);
			}
			yield break;
			yield break;
		}

		// Token: 0x06006E06 RID: 28166 RVA: 0x00267070 File Offset: 0x00265270
		private void CheckStartForceExitAndRemoveMapCountdown()
		{
			if (this.startedCountdown)
			{
				if (GenHostility.AnyHostileActiveThreatToPlayer(base.Map, false))
				{
					this.anyEnemiesInitially = true;
					this.startedCountdown = false;
					base.GetComponent<TimedForcedExit>().ResetForceExitAndRemoveMapCountdown();
				}
				return;
			}
			if (GenHostility.AnyHostileActiveThreatToPlayer(base.Map, false))
			{
				return;
			}
			this.startedCountdown = true;
			float num = 0f;
			for (int i = 0; i < this.parts.Count; i++)
			{
				num = Mathf.Max(num, this.parts[i].def.forceExitAndRemoveMapCountdownDurationDays);
			}
			int num2 = Mathf.RoundToInt(num * 60000f);
			Messages.Message(this.anyEnemiesInitially ? "MessageSiteCountdownBecauseNoMoreEnemies".Translate(TimedForcedExit.GetForceExitAndRemoveMapCountdownTimeLeftString(num2)) : "MessageSiteCountdownBecauseNoEnemiesInitially".Translate(TimedForcedExit.GetForceExitAndRemoveMapCountdownTimeLeftString(num2)), this, MessageTypeDefOf.PositiveEvent, true);
			base.GetComponent<TimedForcedExit>().StartForceExitAndRemoveMapCountdown(num2);
			TaleRecorder.RecordTale(TaleDefOf.CaravanAssaultSuccessful, new object[]
			{
				base.Map.mapPawns.FreeColonists.RandomElement<Pawn>()
			});
		}

		// Token: 0x06006E07 RID: 28167 RVA: 0x00267188 File Offset: 0x00265388
		private void CheckAllEnemiesDefeated()
		{
			if (this.allEnemiesDefeatedSignalSent || !base.HasMap || GenHostility.AnyHostileActiveThreatToPlayer(base.Map, true))
			{
				return;
			}
			QuestUtility.SendQuestTargetSignals(this.questTags, "AllEnemiesDefeated", this.Named("SUBJECT"));
			this.allEnemiesDefeatedSignalSent = true;
		}

		// Token: 0x06006E08 RID: 28168 RVA: 0x002671D8 File Offset: 0x002653D8
		public override bool AllMatchingObjectsOnScreenMatchesWith(WorldObject other)
		{
			Site site = other as Site;
			return site != null && site.MainSitePartDef == this.MainSitePartDef;
		}

		// Token: 0x06006E09 RID: 28169 RVA: 0x00267200 File Offset: 0x00265400
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			Site.tmpSitePartsLabels.Clear();
			for (int i = 0; i < this.parts.Count; i++)
			{
				if (!this.parts[i].hidden)
				{
					string postProcessedThreatLabel = this.parts[i].def.Worker.GetPostProcessedThreatLabel(this, this.parts[i]);
					if (!postProcessedThreatLabel.NullOrEmpty())
					{
						if (stringBuilder.Length != 0)
						{
							stringBuilder.AppendLine();
						}
						stringBuilder.Append(postProcessedThreatLabel.CapitalizeFirst());
					}
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x06006E0A RID: 28170 RVA: 0x002672A8 File Offset: 0x002654A8
		public override string GetDescription()
		{
			string text = this.MainSitePartDef.description;
			string description = base.GetDescription();
			if (!description.NullOrEmpty())
			{
				if (!text.NullOrEmpty())
				{
					text += "\n\n";
				}
				text += description;
			}
			return text;
		}

		// Token: 0x040043FD RID: 17405
		public string customLabel;

		// Token: 0x040043FE RID: 17406
		public List<SitePart> parts = new List<SitePart>();

		// Token: 0x040043FF RID: 17407
		public bool sitePartsKnown = true;

		// Token: 0x04004400 RID: 17408
		public bool factionMustRemainHostile;

		// Token: 0x04004401 RID: 17409
		public float desiredThreatPoints;

		// Token: 0x04004402 RID: 17410
		private SiteCoreBackCompat coreBackCompat;

		// Token: 0x04004403 RID: 17411
		private bool startedCountdown;

		// Token: 0x04004404 RID: 17412
		private bool anyEnemiesInitially;

		// Token: 0x04004405 RID: 17413
		private bool allEnemiesDefeatedSignalSent;

		// Token: 0x04004406 RID: 17414
		private Material cachedMat;

		// Token: 0x04004407 RID: 17415
		private static List<string> tmpSitePartsLabels = new List<string>();
	}
}
