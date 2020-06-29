using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using RimWorld.Planet;
using UnityEngine;
using Verse.AI;
using Verse.AI.Group;

namespace Verse
{
	
	public class Pawn : ThingWithComps, IStrippable, IBillGiver, IVerbOwner, ITrader, IAttackTarget, ILoadReferenceable, IAttackTargetSearcher, IThingHolder
	{
		
		// (get) Token: 0x060011B7 RID: 4535 RVA: 0x00063DE3 File Offset: 0x00061FE3
		// (set) Token: 0x060011B8 RID: 4536 RVA: 0x00063DEB File Offset: 0x00061FEB
		public Name Name
		{
			get
			{
				return this.nameInt;
			}
			set
			{
				this.nameInt = value;
			}
		}

		
		// (get) Token: 0x060011B9 RID: 4537 RVA: 0x00063DF4 File Offset: 0x00061FF4
		public RaceProperties RaceProps
		{
			get
			{
				return this.def.race;
			}
		}

		
		// (get) Token: 0x060011BA RID: 4538 RVA: 0x00063E01 File Offset: 0x00062001
		public Job CurJob
		{
			get
			{
				if (this.jobs == null)
				{
					return null;
				}
				return this.jobs.curJob;
			}
		}

		
		// (get) Token: 0x060011BB RID: 4539 RVA: 0x00063E18 File Offset: 0x00062018
		public JobDef CurJobDef
		{
			get
			{
				if (this.CurJob == null)
				{
					return null;
				}
				return this.CurJob.def;
			}
		}

		
		// (get) Token: 0x060011BC RID: 4540 RVA: 0x00063E2F File Offset: 0x0006202F
		public bool Downed
		{
			get
			{
				return this.health.Downed;
			}
		}

		
		// (get) Token: 0x060011BD RID: 4541 RVA: 0x00063E3C File Offset: 0x0006203C
		public bool Dead
		{
			get
			{
				return this.health.Dead;
			}
		}

		
		// (get) Token: 0x060011BE RID: 4542 RVA: 0x00063E49 File Offset: 0x00062049
		public string KindLabel
		{
			get
			{
				return GenLabel.BestKindLabel(this, false, false, false, -1);
			}
		}

		
		// (get) Token: 0x060011BF RID: 4543 RVA: 0x00063E55 File Offset: 0x00062055
		public bool InMentalState
		{
			get
			{
				return !this.Dead && this.mindState.mentalStateHandler.InMentalState;
			}
		}

		
		// (get) Token: 0x060011C0 RID: 4544 RVA: 0x00063E71 File Offset: 0x00062071
		public MentalState MentalState
		{
			get
			{
				if (this.Dead)
				{
					return null;
				}
				return this.mindState.mentalStateHandler.CurState;
			}
		}

		
		// (get) Token: 0x060011C1 RID: 4545 RVA: 0x00063E8D File Offset: 0x0006208D
		public MentalStateDef MentalStateDef
		{
			get
			{
				if (this.Dead)
				{
					return null;
				}
				return this.mindState.mentalStateHandler.CurStateDef;
			}
		}

		
		// (get) Token: 0x060011C2 RID: 4546 RVA: 0x00063EA9 File Offset: 0x000620A9
		public bool InAggroMentalState
		{
			get
			{
				return !this.Dead && this.mindState.mentalStateHandler.InMentalState && this.mindState.mentalStateHandler.CurStateDef.IsAggro;
			}
		}

		
		// (get) Token: 0x060011C3 RID: 4547 RVA: 0x00063EDE File Offset: 0x000620DE
		public bool Inspired
		{
			get
			{
				return !this.Dead && this.mindState.inspirationHandler.Inspired;
			}
		}

		
		// (get) Token: 0x060011C4 RID: 4548 RVA: 0x00063EFA File Offset: 0x000620FA
		public Inspiration Inspiration
		{
			get
			{
				if (this.Dead)
				{
					return null;
				}
				return this.mindState.inspirationHandler.CurState;
			}
		}

		
		// (get) Token: 0x060011C5 RID: 4549 RVA: 0x00063F16 File Offset: 0x00062116
		public InspirationDef InspirationDef
		{
			get
			{
				if (this.Dead)
				{
					return null;
				}
				return this.mindState.inspirationHandler.CurStateDef;
			}
		}

		
		// (get) Token: 0x060011C6 RID: 4550 RVA: 0x00063F32 File Offset: 0x00062132
		public override Vector3 DrawPos
		{
			get
			{
				return this.Drawer.DrawPos;
			}
		}

		
		// (get) Token: 0x060011C7 RID: 4551 RVA: 0x00063F3F File Offset: 0x0006213F
		public VerbTracker VerbTracker
		{
			get
			{
				return this.verbTracker;
			}
		}

		
		// (get) Token: 0x060011C8 RID: 4552 RVA: 0x00063F47 File Offset: 0x00062147
		public List<VerbProperties> VerbProperties
		{
			get
			{
				return this.def.Verbs;
			}
		}

		
		// (get) Token: 0x060011C9 RID: 4553 RVA: 0x00063F54 File Offset: 0x00062154
		public List<Tool> Tools
		{
			get
			{
				return this.def.tools;
			}
		}

		
		// (get) Token: 0x060011CA RID: 4554 RVA: 0x00063F61 File Offset: 0x00062161
		public bool IsColonist
		{
			get
			{
				return base.Faction != null && base.Faction.IsPlayer && this.RaceProps.Humanlike;
			}
		}

		
		// (get) Token: 0x060011CB RID: 4555 RVA: 0x00063F85 File Offset: 0x00062185
		public bool IsFreeColonist
		{
			get
			{
				return this.IsColonist && this.HostFaction == null;
			}
		}

		
		// (get) Token: 0x060011CC RID: 4556 RVA: 0x00063F9A File Offset: 0x0006219A
		public Faction HostFaction
		{
			get
			{
				if (this.guest == null)
				{
					return null;
				}
				return this.guest.HostFaction;
			}
		}

		
		// (get) Token: 0x060011CD RID: 4557 RVA: 0x00063FB1 File Offset: 0x000621B1
		public bool Drafted
		{
			get
			{
				return this.drafter != null && this.drafter.Drafted;
			}
		}

		
		// (get) Token: 0x060011CE RID: 4558 RVA: 0x00063FC8 File Offset: 0x000621C8
		public bool IsPrisoner
		{
			get
			{
				return this.guest != null && this.guest.IsPrisoner;
			}
		}

		
		// (get) Token: 0x060011CF RID: 4559 RVA: 0x00063FDF File Offset: 0x000621DF
		public bool IsPrisonerOfColony
		{
			get
			{
				return this.guest != null && this.guest.IsPrisoner && this.guest.HostFaction.IsPlayer;
			}
		}

		
		// (get) Token: 0x060011D0 RID: 4560 RVA: 0x00064008 File Offset: 0x00062208
		public bool IsColonistPlayerControlled
		{
			get
			{
				return base.Spawned && this.IsColonist && this.MentalStateDef == null && this.HostFaction == null;
			}
		}

		
		// (get) Token: 0x060011D1 RID: 4561 RVA: 0x0006402D File Offset: 0x0006222D
		public IEnumerable<IntVec3> IngredientStackCells
		{
			get
			{
				yield return this.InteractionCell;
				yield break;
			}
		}

		
		// (get) Token: 0x060011D2 RID: 4562 RVA: 0x0006403D File Offset: 0x0006223D
		public bool InContainerEnclosed
		{
			get
			{
				return base.ParentHolder.IsEnclosingContainer();
			}
		}

		
		// (get) Token: 0x060011D3 RID: 4563 RVA: 0x0006404A File Offset: 0x0006224A
		public Corpse Corpse
		{
			get
			{
				return base.ParentHolder as Corpse;
			}
		}

		
		// (get) Token: 0x060011D4 RID: 4564 RVA: 0x00064058 File Offset: 0x00062258
		public Pawn CarriedBy
		{
			get
			{
				if (base.ParentHolder == null)
				{
					return null;
				}
				Pawn_CarryTracker pawn_CarryTracker = base.ParentHolder as Pawn_CarryTracker;
				if (pawn_CarryTracker != null)
				{
					return pawn_CarryTracker.pawn;
				}
				return null;
			}
		}

		
		// (get) Token: 0x060011D5 RID: 4565 RVA: 0x00064088 File Offset: 0x00062288
		public override string LabelNoCount
		{
			get
			{
				if (this.Name == null)
				{
					return this.KindLabel;
				}
				if (this.story == null || this.story.TitleShortCap.NullOrEmpty())
				{
					return this.Name.ToStringShort;
				}
				return this.Name.ToStringShort + ", " + this.story.TitleShortCap;
			}
		}

		
		// (get) Token: 0x060011D6 RID: 4566 RVA: 0x000640EA File Offset: 0x000622EA
		public override string LabelShort
		{
			get
			{
				if (this.Name != null)
				{
					return this.Name.ToStringShort;
				}
				return this.LabelNoCount;
			}
		}

		
		// (get) Token: 0x060011D7 RID: 4567 RVA: 0x00064108 File Offset: 0x00062308
		public TaggedString LabelNoCountColored
		{
			get
			{
				if (this.Name == null)
				{
					return this.KindLabel;
				}
				if (this.story == null || this.story.TitleShortCap.NullOrEmpty())
				{
					return this.Name.ToStringShort.Colorize(ColoredText.NameColor);
				}
				return this.Name.ToStringShort.Colorize(ColoredText.NameColor) + ", " + this.story.TitleShortCap;
			}
		}

		
		// (get) Token: 0x060011D8 RID: 4568 RVA: 0x0006418D File Offset: 0x0006238D
		public TaggedString NameShortColored
		{
			get
			{
				if (this.Name != null)
				{
					return this.Name.ToStringShort.Colorize(ColoredText.NameColor);
				}
				return this.KindLabel;
			}
		}

		
		// (get) Token: 0x060011D9 RID: 4569 RVA: 0x000641BD File Offset: 0x000623BD
		public TaggedString NameFullColored
		{
			get
			{
				if (this.Name != null)
				{
					return this.Name.ToStringFull.Colorize(ColoredText.NameColor);
				}
				return this.KindLabel;
			}
		}

		
		// (get) Token: 0x060011DA RID: 4570 RVA: 0x000641ED File Offset: 0x000623ED
		public Pawn_DrawTracker Drawer
		{
			get
			{
				if (this.drawer == null)
				{
					this.drawer = new Pawn_DrawTracker(this);
				}
				return this.drawer;
			}
		}

		
		// (get) Token: 0x060011DB RID: 4571 RVA: 0x00064209 File Offset: 0x00062409
		public Faction FactionOrExtraHomeFaction
		{
			get
			{
				if (base.Faction != null && base.Faction.IsPlayer)
				{
					return this.GetExtraHomeFaction(null) ?? base.Faction;
				}
				return base.Faction;
			}
		}

		
		// (get) Token: 0x060011DC RID: 4572 RVA: 0x00064238 File Offset: 0x00062438
		public BillStack BillStack
		{
			get
			{
				return this.health.surgeryBills;
			}
		}

		
		// (get) Token: 0x060011DD RID: 4573 RVA: 0x00064248 File Offset: 0x00062448
		public override IntVec3 InteractionCell
		{
			get
			{
				Building_Bed building_Bed = this.CurrentBed();
				if (building_Bed != null)
				{
					IntVec3 position = base.Position;
					IntVec3 position2 = base.Position;
					IntVec3 position3 = base.Position;
					IntVec3 position4 = base.Position;
					if (building_Bed.Rotation.IsHorizontal)
					{
						position.z++;
						position2.z--;
						position3.x--;
						position4.x++;
					}
					else
					{
						position.x--;
						position2.x++;
						position3.z++;
						position4.z--;
					}
					if (position.Standable(base.Map))
					{
						if (position.GetThingList(base.Map).Find((Thing x) => x.def.IsBed) == null && position.GetDoor(base.Map) == null)
						{
							return position;
						}
					}
					if (position2.Standable(base.Map))
					{
						if (position2.GetThingList(base.Map).Find((Thing x) => x.def.IsBed) == null && position2.GetDoor(base.Map) == null)
						{
							return position2;
						}
					}
					if (position3.Standable(base.Map))
					{
						if (position3.GetThingList(base.Map).Find((Thing x) => x.def.IsBed) == null && position3.GetDoor(base.Map) == null)
						{
							return position3;
						}
					}
					if (position4.Standable(base.Map))
					{
						if (position4.GetThingList(base.Map).Find((Thing x) => x.def.IsBed) == null && position4.GetDoor(base.Map) == null)
						{
							return position4;
						}
					}
					if (position.Standable(base.Map))
					{
						if (position.GetThingList(base.Map).Find((Thing x) => x.def.IsBed) == null)
						{
							return position;
						}
					}
					if (position2.Standable(base.Map))
					{
						if (position2.GetThingList(base.Map).Find((Thing x) => x.def.IsBed) == null)
						{
							return position2;
						}
					}
					if (position3.Standable(base.Map))
					{
						if (position3.GetThingList(base.Map).Find((Thing x) => x.def.IsBed) == null)
						{
							return position3;
						}
					}
					if (position4.Standable(base.Map))
					{
						if (position4.GetThingList(base.Map).Find((Thing x) => x.def.IsBed) == null)
						{
							return position4;
						}
					}
					if (position.Standable(base.Map))
					{
						return position;
					}
					if (position2.Standable(base.Map))
					{
						return position2;
					}
					if (position3.Standable(base.Map))
					{
						return position3;
					}
					if (position4.Standable(base.Map))
					{
						return position4;
					}
				}
				return base.InteractionCell;
			}
		}

		
		// (get) Token: 0x060011DE RID: 4574 RVA: 0x00064589 File Offset: 0x00062789
		public TraderKindDef TraderKind
		{
			get
			{
				if (this.trader == null)
				{
					return null;
				}
				return this.trader.traderKind;
			}
		}

		
		// (get) Token: 0x060011DF RID: 4575 RVA: 0x000645A0 File Offset: 0x000627A0
		public IEnumerable<Thing> Goods
		{
			get
			{
				return this.trader.Goods;
			}
		}

		
		// (get) Token: 0x060011E0 RID: 4576 RVA: 0x000645AD File Offset: 0x000627AD
		public int RandomPriceFactorSeed
		{
			get
			{
				return this.trader.RandomPriceFactorSeed;
			}
		}

		
		// (get) Token: 0x060011E1 RID: 4577 RVA: 0x000645BA File Offset: 0x000627BA
		public string TraderName
		{
			get
			{
				return this.trader.TraderName;
			}
		}

		
		// (get) Token: 0x060011E2 RID: 4578 RVA: 0x000645C7 File Offset: 0x000627C7
		public bool CanTradeNow
		{
			get
			{
				return this.trader != null && this.trader.CanTradeNow;
			}
		}

		
		// (get) Token: 0x060011E3 RID: 4579 RVA: 0x0005AC15 File Offset: 0x00058E15
		public float TradePriceImprovementOffsetForPlayer
		{
			get
			{
				return 0f;
			}
		}

		
		// (get) Token: 0x060011E4 RID: 4580 RVA: 0x000645DE File Offset: 0x000627DE
		public float BodySize
		{
			get
			{
				return this.ageTracker.CurLifeStage.bodySizeFactor * this.RaceProps.baseBodySize;
			}
		}

		
		// (get) Token: 0x060011E5 RID: 4581 RVA: 0x000645FC File Offset: 0x000627FC
		public float HealthScale
		{
			get
			{
				return this.ageTracker.CurLifeStage.healthScaleFactor * this.RaceProps.baseHealthScale;
			}
		}

		
		// (get) Token: 0x060011E6 RID: 4582 RVA: 0x0006461A File Offset: 0x0006281A
		Thing IAttackTarget.Thing
		{
			get
			{
				return this;
			}
		}

		
		// (get) Token: 0x060011E7 RID: 4583 RVA: 0x0001BFCE File Offset: 0x0001A1CE
		public float TargetPriorityFactor
		{
			get
			{
				return 1f;
			}
		}

		
		// (get) Token: 0x060011E8 RID: 4584 RVA: 0x00064620 File Offset: 0x00062820
		public LocalTargetInfo TargetCurrentlyAimingAt
		{
			get
			{
				if (!base.Spawned)
				{
					return LocalTargetInfo.Invalid;
				}
				Stance curStance = this.stances.curStance;
				if (curStance is Stance_Warmup || curStance is Stance_Cooldown)
				{
					return ((Stance_Busy)curStance).focusTarg;
				}
				return LocalTargetInfo.Invalid;
			}
		}

		
		// (get) Token: 0x060011E9 RID: 4585 RVA: 0x0006461A File Offset: 0x0006281A
		Thing IAttackTargetSearcher.Thing
		{
			get
			{
				return this;
			}
		}

		
		// (get) Token: 0x060011EA RID: 4586 RVA: 0x00064668 File Offset: 0x00062868
		public LocalTargetInfo LastAttackedTarget
		{
			get
			{
				return this.mindState.lastAttackedTarget;
			}
		}

		
		// (get) Token: 0x060011EB RID: 4587 RVA: 0x00064675 File Offset: 0x00062875
		public int LastAttackTargetTick
		{
			get
			{
				return this.mindState.lastAttackTargetTick;
			}
		}

		
		// (get) Token: 0x060011EC RID: 4588 RVA: 0x00064684 File Offset: 0x00062884
		public Verb CurrentEffectiveVerb
		{
			get
			{
				Building_Turret building_Turret = this.MannedThing() as Building_Turret;
				if (building_Turret != null)
				{
					return building_Turret.AttackVerb;
				}
				return this.TryGetAttackVerb(null, !this.IsColonist);
			}
		}

		
		string IVerbOwner.UniqueVerbOwnerID()
		{
			return base.GetUniqueLoadID();
		}

		
		bool IVerbOwner.VerbsStillUsableBy(Pawn p)
		{
			return p == this;
		}

		
		// (get) Token: 0x060011EF RID: 4591 RVA: 0x0006461A File Offset: 0x0006281A
		Thing IVerbOwner.ConstantCaster
		{
			get
			{
				return this;
			}
		}

		
		// (get) Token: 0x060011F0 RID: 4592 RVA: 0x000646C5 File Offset: 0x000628C5
		ImplementOwnerTypeDef IVerbOwner.ImplementOwnerTypeDef
		{
			get
			{
				return ImplementOwnerTypeDefOf.Bodypart;
			}
		}

		
		public int GetRootTile()
		{
			return base.Tile;
		}

		
		public ThingOwner GetDirectlyHeldThings()
		{
			return null;
		}

		
		public void GetChildHolders(List<IThingHolder> outChildren)
		{
			ThingOwnerUtility.AppendThingHoldersFromThings(outChildren, this.GetDirectlyHeldThings());
			if (this.inventory != null)
			{
				outChildren.Add(this.inventory);
			}
			if (this.carryTracker != null)
			{
				outChildren.Add(this.carryTracker);
			}
			if (this.equipment != null)
			{
				outChildren.Add(this.equipment);
			}
			if (this.apparel != null)
			{
				outChildren.Add(this.apparel);
			}
		}

		
		public string GetKindLabelPlural(int count = -1)
		{
			return GenLabel.BestKindLabel(this, false, false, true, count);
		}

		
		public static void ResetStaticData()
		{
			Pawn.NotSurgeryReadyTrans = "NotSurgeryReady".Translate();
			Pawn.CannotReachTrans = "CannotReach".Translate();
		}

		
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<PawnKindDef>(ref this.kindDef, "kindDef");
			Scribe_Values.Look<Gender>(ref this.gender, "gender", Gender.Male, false);
			Scribe_Values.Look<int>(ref this.becameWorldPawnTickAbs, "becameWorldPawnTickAbs", -1, false);
			Scribe_Deep.Look<Name>(ref this.nameInt, "name", Array.Empty<object>());
			Scribe_Deep.Look<Pawn_MindState>(ref this.mindState, "mindState", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_JobTracker>(ref this.jobs, "jobs", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_StanceTracker>(ref this.stances, "stances", new object[]
			{
				this
			});
			Scribe_Deep.Look<VerbTracker>(ref this.verbTracker, "verbTracker", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_NativeVerbs>(ref this.natives, "natives", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_MeleeVerbs>(ref this.meleeVerbs, "meleeVerbs", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_RotationTracker>(ref this.rotationTracker, "rotationTracker", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_PathFollower>(ref this.pather, "pather", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_CarryTracker>(ref this.carryTracker, "carryTracker", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_ApparelTracker>(ref this.apparel, "apparel", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_StoryTracker>(ref this.story, "story", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_EquipmentTracker>(ref this.equipment, "equipment", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_DraftController>(ref this.drafter, "drafter", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_AgeTracker>(ref this.ageTracker, "ageTracker", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_HealthTracker>(ref this.health, "healthTracker", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_RecordsTracker>(ref this.records, "records", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_InventoryTracker>(ref this.inventory, "inventory", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_FilthTracker>(ref this.filth, "filth", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_NeedsTracker>(ref this.needs, "needs", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_GuestTracker>(ref this.guest, "guest", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_GuiltTracker>(ref this.guilt, "guilt", Array.Empty<object>());
			Scribe_Deep.Look<Pawn_RoyaltyTracker>(ref this.royalty, "royalty", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_RelationsTracker>(ref this.relations, "social", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_PsychicEntropyTracker>(ref this.psychicEntropy, "psychicEntropy", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_Ownership>(ref this.ownership, "ownership", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_InteractionsTracker>(ref this.interactions, "interactions", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_SkillTracker>(ref this.skills, "skills", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_AbilityTracker>(ref this.abilities, "abilities", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_WorkSettings>(ref this.workSettings, "workSettings", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_TraderTracker>(ref this.trader, "trader", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_OutfitTracker>(ref this.outfits, "outfits", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_DrugPolicyTracker>(ref this.drugs, "drugs", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_FoodRestrictionTracker>(ref this.foodRestriction, "foodRestriction", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_TimetableTracker>(ref this.timetable, "timetable", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_PlayerSettings>(ref this.playerSettings, "playerSettings", new object[]
			{
				this
			});
			Scribe_Deep.Look<Pawn_TrainingTracker>(ref this.training, "training", new object[]
			{
				this
			});
			BackCompatibility.PostExposeData(this);
		}

		
		public override string ToString()
		{
			if (this.story != null)
			{
				return this.LabelShort;
			}
			if (this.thingIDNumber > 0)
			{
				return base.ThingID;
			}
			if (this.kindDef != null)
			{
				return this.KindLabel + "_" + base.ThingID;
			}
			if (this.def != null)
			{
				return base.ThingID;
			}
			return base.GetType().ToString();
		}

		
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			if (this.Dead)
			{
				Log.Warning("Tried to spawn Dead Pawn " + this.ToStringSafe<Pawn>() + ". Replacing with corpse.", false);
				Corpse corpse = (Corpse)ThingMaker.MakeThing(this.RaceProps.corpseDef, null);
				corpse.InnerPawn = this;
				GenSpawn.Spawn(corpse, base.Position, map, WipeMode.Vanish);
				return;
			}
			if (this.def == null || this.kindDef == null)
			{
				Log.Warning("Tried to spawn pawn without def " + this.ToStringSafe<Pawn>() + ".", false);
				return;
			}
			base.SpawnSetup(map, respawningAfterLoad);
			if (Find.WorldPawns.Contains(this))
			{
				Find.WorldPawns.RemovePawn(this);
			}
			PawnComponentsUtility.AddComponentsForSpawn(this);
			if (!PawnUtility.InValidState(this))
			{
				Log.Error("Pawn " + this.ToStringSafe<Pawn>() + " spawned in invalid state. Destroying...", false);
				try
				{
					this.DeSpawn(DestroyMode.Vanish);
				}
				catch (Exception ex)
				{
					Log.Error(string.Concat(new object[]
					{
						"Tried to despawn ",
						this.ToStringSafe<Pawn>(),
						" because of the previous error but couldn't: ",
						ex
					}), false);
				}
				Find.WorldPawns.PassToWorld(this, PawnDiscardDecideMode.Discard);
				return;
			}
			this.Drawer.Notify_Spawned();
			this.rotationTracker.Notify_Spawned();
			if (!respawningAfterLoad)
			{
				this.pather.ResetToCurrentPosition();
			}
			base.Map.mapPawns.RegisterPawn(this);
			if (this.RaceProps.IsFlesh)
			{
				this.relations.everSeenByPlayer = true;
			}
			AddictionUtility.CheckDrugAddictionTeachOpportunity(this);
			if (this.needs != null && this.needs.mood != null && this.needs.mood.recentMemory != null)
			{
				this.needs.mood.recentMemory.Notify_Spawned(respawningAfterLoad);
			}
			if (this.equipment != null)
			{
				this.equipment.Notify_PawnSpawned();
			}
			if (!respawningAfterLoad)
			{
				this.records.AccumulateStoryEvent(StoryEventDefOf.Seen);
				Find.GameEnder.CheckOrUpdateGameOver();
				if (base.Faction == Faction.OfPlayer)
				{
					Find.StoryWatcher.statsRecord.UpdateGreatestPopulation();
					Find.World.StoryState.RecordPopulationIncrease();
				}
				PawnDiedOrDownedThoughtsUtility.RemoveDiedThoughts(this);
				if (this.IsQuestLodger())
				{
					for (int i = this.health.hediffSet.hediffs.Count - 1; i >= 0; i--)
					{
						if (this.health.hediffSet.hediffs[i].def.removeOnQuestLodgers)
						{
							this.health.RemoveHediff(this.health.hediffSet.hediffs[i]);
						}
					}
				}
			}
		}

		
		public override void PostMapInit()
		{
			base.PostMapInit();
			this.pather.TryResumePathingAfterLoading();
		}

		
		public override void DrawAt(Vector3 drawLoc, bool flip = false)
		{
			this.Drawer.DrawAt(drawLoc);
		}

		
		public override void DrawGUIOverlay()
		{
			this.Drawer.ui.DrawPawnGUIOverlay();
		}

		
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			if (this.IsColonistPlayerControlled)
			{
				if (this.pather.curPath != null)
				{
					this.pather.curPath.DrawPath(this);
				}
				this.jobs.DrawLinesBetweenTargets();
			}
		}

		
		public override void TickRare()
		{
			base.TickRare();
			if (!base.Suspended)
			{
				if (this.apparel != null)
				{
					this.apparel.ApparelTrackerTickRare();
				}
				this.inventory.InventoryTrackerTickRare();
			}
			if (this.training != null)
			{
				this.training.TrainingTrackerTickRare();
			}
			if (base.Spawned && this.RaceProps.IsFlesh)
			{
				GenTemperature.PushHeat(this, 0.3f * this.BodySize * 4.16666651f * (this.def.race.Humanlike ? 1f : 0.6f));
			}
		}

		
		public override void Tick()
		{
			if (DebugSettings.noAnimals && base.Spawned && this.RaceProps.Animal)
			{
				this.Destroy(DestroyMode.Vanish);
				return;
			}
			base.Tick();
			if (Find.TickManager.TicksGame % 250 == 0)
			{
				this.TickRare();
			}
			bool suspended = base.Suspended;
			if (!suspended)
			{
				if (base.Spawned)
				{
					this.pather.PatherTick();
				}
				if (base.Spawned)
				{
					this.stances.StanceTrackerTick();
					this.verbTracker.VerbsTick();
					this.natives.NativeVerbsTick();
				}
				if (base.Spawned)
				{
					this.jobs.JobTrackerTick();
				}
				if (base.Spawned)
				{
					this.Drawer.DrawTrackerTick();
					this.rotationTracker.RotationTrackerTick();
				}
				this.health.HealthTick();
				if (!this.Dead)
				{
					this.mindState.MindStateTick();
					this.carryTracker.CarryHandsTick();
				}
			}
			if (!this.Dead)
			{
				this.needs.NeedsTrackerTick();
			}
			if (!suspended)
			{
				if (this.equipment != null)
				{
					this.equipment.EquipmentTrackerTick();
				}
				if (this.apparel != null)
				{
					this.apparel.ApparelTrackerTick();
				}
				if (this.interactions != null && base.Spawned)
				{
					this.interactions.InteractionsTrackerTick();
				}
				if (this.caller != null)
				{
					this.caller.CallTrackerTick();
				}
				if (this.skills != null)
				{
					this.skills.SkillsTick();
				}
				if (this.abilities != null)
				{
					this.abilities.AbilitiesTick();
				}
				if (this.inventory != null)
				{
					this.inventory.InventoryTrackerTick();
				}
				if (this.drafter != null)
				{
					this.drafter.DraftControllerTick();
				}
				if (this.relations != null)
				{
					this.relations.RelationsTrackerTick();
				}
				if (ModsConfig.RoyaltyActive && this.psychicEntropy != null)
				{
					this.psychicEntropy.PsychicEntropyTrackerTick();
				}
				if (this.RaceProps.Humanlike)
				{
					this.guest.GuestTrackerTick();
				}
				if (this.royalty != null && ModsConfig.RoyaltyActive)
				{
					this.royalty.RoyaltyTrackerTick();
				}
				this.ageTracker.AgeTick();
				this.records.RecordsTick();
			}
		}

		
		public void TickMothballed(int interval)
		{
			if (!base.Suspended)
			{
				this.ageTracker.AgeTickMothballed(interval);
				this.records.RecordsTickMothballed(interval);
			}
		}

		
		public void Notify_Teleported(bool endCurrentJob = true, bool resetTweenedPos = true)
		{
			if (resetTweenedPos)
			{
				this.Drawer.tweener.ResetTweenedPosToRoot();
			}
			this.pather.Notify_Teleported_Int();
			if (endCurrentJob && this.jobs != null && this.jobs.curJob != null)
			{
				this.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
			}
		}

		
		public void Notify_PassedToWorld()
		{
			if (((base.Faction == null && this.RaceProps.Humanlike) || (base.Faction != null && base.Faction.IsPlayer) || base.Faction == Faction.OfAncients || base.Faction == Faction.OfAncientsHostile) && !this.Dead && Find.WorldPawns.GetSituation(this) == WorldPawnSituation.Free)
			{
				bool tryMedievalOrBetter = base.Faction != null && base.Faction.def.techLevel >= TechLevel.Medieval;
				Faction faction;
				if ( !this.GetExtraHomeFaction(null).IsPlayer)
				{
					if (base.Faction != this.GetExtraHomeFaction(null))
					{
						this.SetFaction(this.GetExtraHomeFaction(null), null);
					}
				}
				else if (Find.FactionManager.TryGetRandomNonColonyHumanlikeFaction(out faction, tryMedievalOrBetter, false, TechLevel.Undefined))
				{
					if (base.Faction != faction)
					{
						this.SetFaction(faction, null);
					}
				}
				else if (Find.FactionManager.TryGetRandomNonColonyHumanlikeFaction(out faction, tryMedievalOrBetter, true, TechLevel.Undefined))
				{
					if (base.Faction != faction)
					{
						this.SetFaction(faction, null);
					}
				}
				else if (base.Faction != null)
				{
					this.SetFaction(null, null);
				}
			}
			this.becameWorldPawnTickAbs = GenTicks.TicksAbs;
			if (!this.IsCaravanMember() && !PawnUtility.IsTravelingInTransportPodWorldObject(this))
			{
				this.ClearMind(false, false, true);
			}
			if (this.relations != null)
			{
				this.relations.Notify_PassedToWorld();
			}
		}

		
		public override void PreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
		{
			base.PreApplyDamage(ref dinfo, out absorbed);
			if (absorbed)
			{
				return;
			}
			this.health.PreApplyDamage(dinfo, out absorbed);
		}

		
		public override void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			base.PostApplyDamage(dinfo, totalDamageDealt);
			if (dinfo.Def.ExternalViolenceFor(this))
			{
				this.records.AddTo(RecordDefOf.DamageTaken, totalDamageDealt);
			}
			if (dinfo.Def.makesBlood && !dinfo.InstantPermanentInjury && totalDamageDealt > 0f && Rand.Chance(0.5f))
			{
				this.health.DropBloodFilth();
			}
			this.records.AccumulateStoryEvent(StoryEventDefOf.DamageTaken);
			this.health.PostApplyDamage(dinfo, totalDamageDealt);
			if (!this.Dead)
			{
				this.mindState.Notify_DamageTaken(dinfo);
			}
		}

		
		public override Thing SplitOff(int count)
		{
			if (count <= 0 || count >= this.stackCount)
			{
				return base.SplitOff(count);
			}
			throw new NotImplementedException("Split off on Pawns is not supported (unless we're taking a full stack).");
		}

		
		// (get) Token: 0x06001205 RID: 4613 RVA: 0x0006543D File Offset: 0x0006363D
		public int TicksPerMoveCardinal
		{
			get
			{
				return this.TicksPerMove(false);
			}
		}

		
		// (get) Token: 0x06001206 RID: 4614 RVA: 0x00065446 File Offset: 0x00063646
		public int TicksPerMoveDiagonal
		{
			get
			{
				return this.TicksPerMove(true);
			}
		}

		
		private int TicksPerMove(bool diagonal)
		{
			float num = this.GetStatValue(StatDefOf.MoveSpeed, true);
			if (RestraintsUtility.InRestraints(this))
			{
				num *= 0.35f;
			}
			if (this.carryTracker != null && this.carryTracker.CarriedThing != null && this.carryTracker.CarriedThing.def.category == ThingCategory.Pawn)
			{
				num *= 0.6f;
			}
			float num2 = num / 60f;
			float num3;
			if (num2 == 0f)
			{
				num3 = 450f;
			}
			else
			{
				num3 = 1f / num2;
				if (base.Spawned && !base.Map.roofGrid.Roofed(base.Position))
				{
					num3 /= base.Map.weatherManager.CurMoveSpeedMultiplier;
				}
				if (diagonal)
				{
					num3 *= 1.41421f;
				}
			}
			return Mathf.Clamp(Mathf.RoundToInt(num3), 1, 450);
		}

		
		public override void Kill(DamageInfo? dinfo, Hediff exactCulprit = null)
		{
			IntVec3 positionHeld = base.PositionHeld;
			Map map = base.Map;
			Map mapHeld = base.MapHeld;
			bool flag = base.Spawned;
			bool spawnedOrAnyParentSpawned = base.SpawnedOrAnyParentSpawned;
			bool wasWorldPawn = this.IsWorldPawn();
			Caravan caravan = this.GetCaravan();
			Building_Grave assignedGrave = null;
			if (this.ownership != null)
			{
				assignedGrave = this.ownership.AssignedGrave;
			}
			bool flag2 = this.InBed();
			float bedRotation = 0f;
			if (flag2)
			{
				bedRotation = this.CurrentBed().Rotation.AsAngle;
			}
			ThingOwner thingOwner = null;
			bool inContainerEnclosed = this.InContainerEnclosed;
			if (inContainerEnclosed)
			{
				thingOwner = this.holdingOwner;
				thingOwner.Remove(this);
			}
			bool flag3 = false;
			bool flag4 = false;
			if (Current.ProgramState == ProgramState.Playing && map != null)
			{
				flag3 = (map.designationManager.DesignationOn(this, DesignationDefOf.Hunt) != null);
				flag4 = (map.designationManager.DesignationOn(this, DesignationDefOf.Slaughter) != null);
			}
			bool flag5 = PawnUtility.ShouldSendNotificationAbout(this) && (!flag4 || dinfo == null || dinfo.Value.Def != DamageDefOf.ExecutionCut);
			float num = 0f;
			Thing attachment = this.GetAttachment(ThingDefOf.Fire);
			if (attachment != null)
			{
				num = ((Fire)attachment).CurrentSize();
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				Find.Storyteller.Notify_PawnEvent(this, AdaptationEvent.Died, null);
			}
			if (this.IsColonist)
			{
				Find.StoryWatcher.statsRecord.Notify_ColonistKilled();
			}
			if (flag && dinfo != null && dinfo.Value.Def.ExternalViolenceFor(this))
			{
				LifeStageUtility.PlayNearestLifestageSound(this, (LifeStageAge ls) => ls.soundDeath, 1f);
			}
			if (dinfo != null && dinfo.Value.Instigator != null)
			{
				Pawn pawn = dinfo.Value.Instigator as Pawn;
				if (pawn != null)
				{
					RecordsUtility.Notify_PawnKilled(this, pawn);
					if (this.IsColonist)
					{
						pawn.records.AccumulateStoryEvent(StoryEventDefOf.KilledPlayer);
					}
				}
			}
			TaleUtility.Notify_PawnDied(this, dinfo);
			if (flag)
			{
				Find.BattleLog.Add(new BattleLogEntry_StateTransition(this, this.RaceProps.DeathActionWorker.DeathRules, (dinfo != null) ? (dinfo.Value.Instigator as Pawn) : null, exactCulprit, (dinfo != null) ? dinfo.Value.HitPart : null));
			}
			this.health.surgeryBills.Clear();
			if (this.apparel != null)
			{
				this.apparel.Notify_PawnKilled(dinfo);
			}
			if (this.RaceProps.IsFlesh)
			{
				this.relations.Notify_PawnKilled(dinfo, map);
			}
			this.meleeVerbs.Notify_PawnKilled();
			for (int i = 0; i < this.health.hediffSet.hediffs.Count; i++)
			{
				this.health.hediffSet.hediffs[i].Notify_PawnKilled();
			}
			Pawn_CarryTracker pawn_CarryTracker = base.ParentHolder as Pawn_CarryTracker;
			Thing thing;
			if (pawn_CarryTracker != null && this.holdingOwner.TryDrop_NewTmp(this, pawn_CarryTracker.pawn.Position, pawn_CarryTracker.pawn.Map, ThingPlaceMode.Near, out thing, null, null, true))
			{
				map = pawn_CarryTracker.pawn.Map;
				flag = true;
			}
			PawnDiedOrDownedThoughtsUtility.RemoveLostThoughts(this);
			PawnDiedOrDownedThoughtsUtility.TryGiveThoughts(this, dinfo, PawnDiedOrDownedThoughtsKind.Died);
			this.health.SetDead();
			if (this.health.deflectionEffecter != null)
			{
				this.health.deflectionEffecter.Cleanup();
				this.health.deflectionEffecter = null;
			}
			if (this.health.woundedEffecter != null)
			{
				this.health.woundedEffecter.Cleanup();
				this.health.woundedEffecter = null;
			}
			if (caravan != null)
			{
				caravan.Notify_MemberDied(this);
			}
			Lord lord = this.GetLord();
			if (lord != null)
			{
				lord.Notify_PawnLost(this, PawnLostCondition.IncappedOrKilled, dinfo);
			}
			if (flag)
			{
				this.DropAndForbidEverything(false);
			}
			if (flag)
			{
				this.DeSpawn(DestroyMode.Vanish);
			}
			if (this.royalty != null)
			{
				this.royalty.Notify_PawnKilled();
			}
			Corpse corpse = null;
			if (!PawnGenerator.IsBeingGenerated(this))
			{
				if (inContainerEnclosed)
				{
					corpse = this.MakeCorpse(assignedGrave, flag2, bedRotation);
					if (!thingOwner.TryAdd(corpse, true))
					{
						corpse.Destroy(DestroyMode.Vanish);
						corpse = null;
					}
				}
				else if (spawnedOrAnyParentSpawned)
				{
					if (this.holdingOwner != null)
					{
						this.holdingOwner.Remove(this);
					}
					corpse = this.MakeCorpse(assignedGrave, flag2, bedRotation);
					if (GenPlace.TryPlaceThing(corpse, positionHeld, mapHeld, ThingPlaceMode.Direct, null, null, default(Rot4)))
					{
						corpse.Rotation = base.Rotation;
						if (HuntJobUtility.WasKilledByHunter(this, dinfo))
						{
							((Pawn)dinfo.Value.Instigator).Reserve(corpse, ((Pawn)dinfo.Value.Instigator).CurJob, 1, -1, null, true);
						}
						else if (!flag3 && !flag4)
						{
							corpse.SetForbiddenIfOutsideHomeArea();
						}
						if (num > 0f)
						{
							FireUtility.TryStartFireIn(corpse.Position, corpse.Map, num);
						}
					}
					else
					{
						corpse.Destroy(DestroyMode.Vanish);
						corpse = null;
					}
				}
				else if (caravan != null && caravan.Spawned)
				{
					corpse = this.MakeCorpse(assignedGrave, flag2, bedRotation);
					caravan.AddPawnOrItem(corpse, true);
				}
				else if (this.holdingOwner != null || this.IsWorldPawn())
				{
					Corpse.PostCorpseDestroy(this);
				}
				else
				{
					corpse = this.MakeCorpse(assignedGrave, flag2, bedRotation);
				}
			}
			if (corpse != null)
			{
				Hediff firstHediffOfDef = this.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.ToxicBuildup, false);
				Hediff firstHediffOfDef2 = this.health.hediffSet.GetFirstHediffOfDef(HediffDefOf.Scaria, false);
				CompRottable comp = corpse.GetComp<CompRottable>();
				if ((firstHediffOfDef != null && Rand.Value < firstHediffOfDef.Severity && comp != null) || (firstHediffOfDef2 != null && Rand.Chance(Find.Storyteller.difficulty.scariaRotChance)))
				{
					comp.RotImmediately();
				}
			}
			if (!base.Destroyed)
			{
				this.Destroy(DestroyMode.KillFinalize);
			}
			PawnComponentsUtility.RemoveComponentsOnKilled(this);
			this.health.hediffSet.DirtyCache();
			PortraitsCache.SetDirty(this);
			for (int j = this.health.hediffSet.hediffs.Count - 1; j >= 0; j--)
			{
				this.health.hediffSet.hediffs[j].Notify_PawnDied();
			}
			Faction factionOrExtraHomeFaction = this.FactionOrExtraHomeFaction;
			if (factionOrExtraHomeFaction != null)
			{
				factionOrExtraHomeFaction.Notify_MemberDied(this, dinfo, wasWorldPawn, mapHeld);
			}
			if (corpse != null)
			{
				if (this.RaceProps.DeathActionWorker != null && flag)
				{
					this.RaceProps.DeathActionWorker.PawnDied(corpse);
				}
				if (Find.Scenario != null)
				{
					Find.Scenario.Notify_PawnDied(corpse);
				}
			}
			if (base.Faction != null && base.Faction.IsPlayer)
			{
				BillUtility.Notify_ColonistUnavailable(this);
			}
			if (spawnedOrAnyParentSpawned)
			{
				GenHostility.Notify_PawnLostForTutor(this, mapHeld);
			}
			if (base.Faction != null && base.Faction.IsPlayer && Current.ProgramState == ProgramState.Playing)
			{
				Find.ColonistBar.MarkColonistsDirty();
			}
			Pawn_PsychicEntropyTracker pawn_PsychicEntropyTracker = this.psychicEntropy;
			if (pawn_PsychicEntropyTracker != null)
			{
				pawn_PsychicEntropyTracker.Notify_PawnDied();
			}
			if (flag5)
			{
				this.health.NotifyPlayerOfKilled(dinfo, exactCulprit, caravan);
			}
			Find.QuestManager.Notify_PawnKilled(this, dinfo);
		}

		
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			if (mode != DestroyMode.Vanish && mode != DestroyMode.KillFinalize)
			{
				Log.Error(string.Concat(new object[]
				{
					"Destroyed pawn ",
					this,
					" with unsupported mode ",
					mode,
					"."
				}), false);
			}
			base.Destroy(mode);
			Find.WorldPawns.Notify_PawnDestroyed(this);
			if (this.ownership != null)
			{
				Building_Grave assignedGrave = this.ownership.AssignedGrave;
				this.ownership.UnclaimAll();
				if (mode == DestroyMode.KillFinalize && assignedGrave != null)
				{
					assignedGrave.CompAssignableToPawn.TryAssignPawn(this);
				}
			}
			this.ClearMind(false, true, true);
			Lord lord = this.GetLord();
			if (lord != null)
			{
				PawnLostCondition cond = (mode == DestroyMode.KillFinalize) ? PawnLostCondition.IncappedOrKilled : PawnLostCondition.Vanished;
				lord.Notify_PawnLost(this, cond, null);
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				Find.GameEnder.CheckOrUpdateGameOver();
				Find.TaleManager.Notify_PawnDestroyed(this);
			}
			foreach (Pawn pawn in from p in PawnsFinder.AllMapsWorldAndTemporary_Alive
			where p.playerSettings != null && p.playerSettings.Master == this
			select p)
			{
				pawn.playerSettings.Master = null;
			}
			if (mode != DestroyMode.KillFinalize)
			{
				if (this.equipment != null)
				{
					this.equipment.DestroyAllEquipment(DestroyMode.Vanish);
				}
				this.inventory.DestroyAll(DestroyMode.Vanish);
				if (this.apparel != null)
				{
					this.apparel.DestroyAll(DestroyMode.Vanish);
				}
			}
			WorldPawns worldPawns = Find.WorldPawns;
			if (!worldPawns.IsBeingDiscarded(this) && !worldPawns.Contains(this))
			{
				worldPawns.PassToWorld(this, PawnDiscardDecideMode.Decide);
			}
		}

		
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			if (this.jobs != null && this.jobs.curJob != null)
			{
				this.jobs.StopAll(false, true);
			}
			base.DeSpawn(mode);
			if (this.pather != null)
			{
				this.pather.StopDead();
			}
			if (this.needs != null && this.needs.mood != null)
			{
				this.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
			}
			if (this.meleeVerbs != null)
			{
				this.meleeVerbs.Notify_PawnDespawned();
			}
			this.ClearAllReservations(false);
			if (map != null)
			{
				map.mapPawns.DeRegisterPawn(this);
			}
			PawnComponentsUtility.RemoveComponentsOnDespawned(this);
		}

		
		public override void Discard(bool silentlyRemoveReferences = false)
		{
			if (Find.WorldPawns.Contains(this))
			{
				Log.Warning("Tried to discard a world pawn " + this + ".", false);
				return;
			}
			base.Discard(silentlyRemoveReferences);
			if (this.relations != null)
			{
				this.relations.ClearAllRelations();
			}
			if (Current.ProgramState == ProgramState.Playing)
			{
				Find.PlayLog.Notify_PawnDiscarded(this, silentlyRemoveReferences);
				Find.BattleLog.Notify_PawnDiscarded(this, silentlyRemoveReferences);
				Find.TaleManager.Notify_PawnDiscarded(this, silentlyRemoveReferences);
				Find.QuestManager.Notify_PawnDiscarded(this);
			}
			foreach (Pawn pawn in PawnsFinder.AllMapsWorldAndTemporary_Alive)
			{
				if (pawn.needs != null && pawn.needs.mood != null)
				{
					pawn.needs.mood.thoughts.memories.Notify_PawnDiscarded(this);
				}
			}
			Corpse.PostCorpseDestroy(this);
		}

		
		public Corpse MakeCorpse(Building_Grave assignedGrave, bool inBed, float bedRotation)
		{
			if (this.holdingOwner != null)
			{
				Log.Warning("We can't make corpse because the pawn is in a ThingOwner. Remove him from the container first. This should have been already handled before calling this method. holder=" + base.ParentHolder, false);
				return null;
			}
			Corpse corpse = (Corpse)ThingMaker.MakeThing(this.RaceProps.corpseDef, null);
			corpse.InnerPawn = this;
			if (assignedGrave != null)
			{
				corpse.InnerPawn.ownership.ClaimGrave(assignedGrave);
			}
			if (inBed)
			{
				corpse.InnerPawn.Drawer.renderer.wiggler.SetToCustomRotation(bedRotation + 180f);
			}
			return corpse;
		}

		
		public void ExitMap(bool allowedToJoinOrCreateCaravan, Rot4 exitDir)
		{
			if (this.IsWorldPawn())
			{
				Log.Warning("Called ExitMap() on world pawn " + this, false);
				return;
			}
			if (allowedToJoinOrCreateCaravan && CaravanExitMapUtility.CanExitMapAndJoinOrCreateCaravanNow(this))
			{
				CaravanExitMapUtility.ExitMapAndJoinOrCreateCaravan(this, exitDir);
				return;
			}
			Lord lord = this.GetLord();
			if (lord != null)
			{
				lord.Notify_PawnLost(this, PawnLostCondition.ExitedMap, null);
			}
			if (this.carryTracker != null && this.carryTracker.CarriedThing != null)
			{
				Pawn pawn = this.carryTracker.CarriedThing as Pawn;
				if (pawn != null)
				{
					if (base.Faction != null && base.Faction != pawn.Faction)
					{
						base.Faction.kidnapped.Kidnap(pawn, this);
					}
					else
					{
						this.carryTracker.innerContainer.Remove(pawn);
						pawn.ExitMap(false, exitDir);
					}
				}
				else
				{
					this.carryTracker.CarriedThing.Destroy(DestroyMode.Vanish);
				}
				this.carryTracker.innerContainer.Clear();
			}
			bool flag = !this.IsCaravanMember() && !PawnUtility.IsTravelingInTransportPodWorldObject(this) && (!this.IsPrisoner || base.ParentHolder == null || base.ParentHolder is CompShuttle || (this.guest != null && this.guest.Released));
			if (base.Faction != null)
			{
				base.Faction.Notify_MemberExitedMap(this, flag);
			}
			if (this.ownership != null && flag)
			{
				this.ownership.UnclaimAll();
			}
			if (this.guest != null)
			{
				bool isPrisonerOfColony = this.IsPrisonerOfColony;
				if (flag)
				{
					this.guest.SetGuestStatus(null, false);
				}
				this.guest.Released = false;
				if (isPrisonerOfColony)
				{
					this.guest.interactionMode = PrisonerInteractionModeDefOf.NoInteraction;
				}
			}
			if (base.Spawned)
			{
				this.DeSpawn(DestroyMode.Vanish);
			}
			this.inventory.UnloadEverything = false;
			if (flag)
			{
				this.ClearMind(false, false, true);
			}
			if (this.relations != null)
			{
				this.relations.Notify_ExitedMap();
			}
			Find.WorldPawns.PassToWorld(this, PawnDiscardDecideMode.Decide);
			QuestUtility.SendQuestTargetSignals(this.questTags, "LeftMap", this.Named("SUBJECT"));
		}

		
		public override void PreTraded(TradeAction action, Pawn playerNegotiator, ITrader trader)
		{
			base.PreTraded(action, playerNegotiator, trader);
			if (base.SpawnedOrAnyParentSpawned)
			{
				this.DropAndForbidEverything(false);
			}
			if (this.ownership != null)
			{
				this.ownership.UnclaimAll();
			}
			if (action == TradeAction.PlayerSells)
			{
				Faction faction = this.GetExtraHomeFaction(null) ?? this.GetExtraHostFaction(null);
				if (faction != null && faction != Faction.OfPlayer)
				{
					faction.TrySetRelationKind(Faction.OfPlayer, FactionRelationKind.Hostile, true, "GoodwillChangedReason_SoldPawn".Translate(this), new GlobalTargetInfo?(this));
				}
			}
			if (this.guest != null)
			{
				this.guest.SetGuestStatus(null, false);
			}
			if (action == TradeAction.PlayerBuys)
			{
				if (this.needs.mood != null)
				{
					this.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.FreedFromSlavery, null);
				}
				this.SetFaction(Faction.OfPlayer, null);
			}
			else if (action == TradeAction.PlayerSells)
			{
				if (this.RaceProps.Humanlike)
				{
					TaleRecorder.RecordTale(TaleDefOf.SoldPrisoner, new object[]
					{
						playerNegotiator,
						this,
						trader
					});
				}
				if (base.Faction != null)
				{
					this.SetFaction(null, null);
				}
				if (this.RaceProps.IsFlesh)
				{
					this.relations.Notify_PawnSold(playerNegotiator);
				}
				if (this.RaceProps.Humanlike)
				{
					GenGuest.AddPrisonerSoldThoughts(this);
				}
			}
			this.ClearMind(false, false, true);
		}

		
		public void PreKidnapped(Pawn kidnapper)
		{
			Find.Storyteller.Notify_PawnEvent(this, AdaptationEvent.Kidnapped, null);
			if (this.IsColonist && kidnapper != null)
			{
				TaleRecorder.RecordTale(TaleDefOf.KidnappedColonist, new object[]
				{
					kidnapper,
					this
				});
			}
			if (this.ownership != null)
			{
				this.ownership.UnclaimAll();
			}
			if (this.guest != null)
			{
				this.guest.SetGuestStatus(null, false);
			}
			if (this.RaceProps.IsFlesh)
			{
				this.relations.Notify_PawnKidnapped();
			}
			this.ClearMind(false, false, true);
		}

		
		public override void SetFaction(Faction newFaction, Pawn recruiter = null)
		{
			if (newFaction == base.Faction)
			{
				Log.Warning("Used SetFaction to change " + this.ToStringSafe<Pawn>() + " to same faction " + newFaction.ToStringSafe<Faction>(), false);
				return;
			}
			Faction faction = base.Faction;
			if (this.guest != null)
			{
				this.guest.SetGuestStatus(null, false);
			}
			if (base.Spawned)
			{
				base.Map.mapPawns.DeRegisterPawn(this);
				base.Map.pawnDestinationReservationManager.ReleaseAllClaimedBy(this);
				base.Map.designationManager.RemoveAllDesignationsOn(this, false);
			}
			if ((newFaction == Faction.OfPlayer || base.Faction == Faction.OfPlayer) && Current.ProgramState == ProgramState.Playing)
			{
				Find.ColonistBar.MarkColonistsDirty();
			}
			Lord lord = this.GetLord();
			if (lord != null)
			{
				lord.Notify_PawnLost(this, PawnLostCondition.ChangedFaction, null);
			}
			if (PawnUtility.IsFactionLeader(this) && newFaction != PawnUtility.GetFactionLeaderFaction(this) && !this.HasExtraHomeFaction(PawnUtility.GetFactionLeaderFaction(this)))
			{
				base.Faction.Notify_LeaderLost();
			}
			if (newFaction == Faction.OfPlayer && this.RaceProps.Humanlike && !this.IsQuestLodger())
			{
				this.ChangeKind(newFaction.def.basicMemberKind);
			}
			base.SetFaction(newFaction, null);
			PawnComponentsUtility.AddAndRemoveDynamicComponents(this, false);
			if (base.Faction != null && base.Faction.IsPlayer)
			{
				if (this.workSettings != null)
				{
					this.workSettings.EnableAndInitialize();
				}
				Find.StoryWatcher.watcherPopAdaptation.Notify_PawnEvent(this, PopAdaptationEvent.GainedColonist);
			}
			if (this.Drafted)
			{
				this.drafter.Drafted = false;
			}
			ReachabilityUtility.ClearCacheFor(this);
			this.health.surgeryBills.Clear();
			if (base.Spawned)
			{
				base.Map.mapPawns.RegisterPawn(this);
			}
			this.GenerateNecessaryName();
			if (this.playerSettings != null)
			{
				this.playerSettings.ResetMedicalCare();
			}
			this.ClearMind(true, false, true);
			if (!this.Dead && this.needs.mood != null)
			{
				this.needs.mood.thoughts.situational.Notify_SituationalThoughtsDirty();
			}
			if (base.Spawned)
			{
				base.Map.attackTargetsCache.UpdateTarget(this);
			}
			Find.GameEnder.CheckOrUpdateGameOver();
			AddictionUtility.CheckDrugAddictionTeachOpportunity(this);
			if (this.needs != null)
			{
				this.needs.AddOrRemoveNeedsAsAppropriate();
			}
			if (this.playerSettings != null)
			{
				this.playerSettings.Notify_FactionChanged();
			}
			if (this.relations != null)
			{
				this.relations.Notify_ChangedFaction();
			}
			if (this.RaceProps.Animal && newFaction == Faction.OfPlayer)
			{
				this.training.SetWantedRecursive(TrainableDefOf.Tameness, true);
				this.training.Train(TrainableDefOf.Tameness, recruiter, true);
			}
			if (faction == Faction.OfPlayer)
			{
				BillUtility.Notify_ColonistUnavailable(this);
			}
			if (newFaction == Faction.OfPlayer)
			{
				Find.StoryWatcher.statsRecord.UpdateGreatestPopulation();
				Find.World.StoryState.RecordPopulationIncrease();
			}
		}

		
		public void ClearMind(bool ifLayingKeepLaying = false, bool clearInspiration = false, bool clearMentalState = true)
		{
			if (this.pather != null)
			{
				this.pather.StopDead();
			}
			if (this.mindState != null)
			{
				this.mindState.Reset(clearInspiration, clearMentalState);
			}
			if (this.jobs != null)
			{
				this.jobs.StopAll(ifLayingKeepLaying, true);
			}
			this.VerifyReservations();
		}

		
		public void ClearAllReservations(bool releaseDestinationsOnlyIfObsolete = true)
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				if (releaseDestinationsOnlyIfObsolete)
				{
					maps[i].pawnDestinationReservationManager.ReleaseAllObsoleteClaimedBy(this);
				}
				else
				{
					maps[i].pawnDestinationReservationManager.ReleaseAllClaimedBy(this);
				}
				maps[i].reservationManager.ReleaseAllClaimedBy(this);
				maps[i].physicalInteractionReservationManager.ReleaseAllClaimedBy(this);
				maps[i].attackTargetReservationManager.ReleaseAllClaimedBy(this);
			}
		}

		
		public void ClearReservationsForJob(Job job)
		{
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				maps[i].pawnDestinationReservationManager.ReleaseClaimedBy(this, job);
				maps[i].reservationManager.ReleaseClaimedBy(this, job);
				maps[i].physicalInteractionReservationManager.ReleaseClaimedBy(this, job);
				maps[i].attackTargetReservationManager.ReleaseClaimedBy(this, job);
			}
		}

		
		public void VerifyReservations()
		{
			if (this.jobs == null)
			{
				return;
			}
			if (this.CurJob != null || this.jobs.jobQueue.Count > 0 || this.jobs.startingNewJob)
			{
				return;
			}
			bool flag = false;
			List<Map> maps = Find.Maps;
			for (int i = 0; i < maps.Count; i++)
			{
				LocalTargetInfo obj = maps[i].reservationManager.FirstReservationFor(this);
				if (obj.IsValid)
				{
					Log.ErrorOnce(string.Format("Reservation manager failed to clean up properly; {0} still reserving {1}", this.ToStringSafe<Pawn>(), obj.ToStringSafe<LocalTargetInfo>()), 97771429 ^ this.thingIDNumber, false);
					flag = true;
				}
				LocalTargetInfo obj2 = maps[i].physicalInteractionReservationManager.FirstReservationFor(this);
				if (obj2.IsValid)
				{
					Log.ErrorOnce(string.Format("Physical interaction reservation manager failed to clean up properly; {0} still reserving {1}", this.ToStringSafe<Pawn>(), obj2.ToStringSafe<LocalTargetInfo>()), 19586765 ^ this.thingIDNumber, false);
					flag = true;
				}
				IAttackTarget attackTarget = maps[i].attackTargetReservationManager.FirstReservationFor(this);
				if (attackTarget != null)
				{
					Log.ErrorOnce(string.Format("Attack target reservation manager failed to clean up properly; {0} still reserving {1}", this.ToStringSafe<Pawn>(), attackTarget.ToStringSafe<IAttackTarget>()), 100495878 ^ this.thingIDNumber, false);
					flag = true;
				}
				IntVec3 obj3 = maps[i].pawnDestinationReservationManager.FirstObsoleteReservationFor(this);
				if (obj3.IsValid)
				{
					Job job = maps[i].pawnDestinationReservationManager.FirstObsoleteReservationJobFor(this);
					Log.ErrorOnce(string.Format("Pawn destination reservation manager failed to clean up properly; {0}/{1}/{2} still reserving {3}", new object[]
					{
						this.ToStringSafe<Pawn>(),
						job.ToStringSafe<Job>(),
						job.def.ToStringSafe<JobDef>(),
						obj3.ToStringSafe<IntVec3>()
					}), 1958674 ^ this.thingIDNumber, false);
					flag = true;
				}
			}
			if (flag)
			{
				this.ClearAllReservations(true);
			}
		}

		
		public void DropAndForbidEverything(bool keepInventoryAndEquipmentIfInBed = false)
		{
			if (this.kindDef.destroyGearOnDrop)
			{
				this.equipment.DestroyAllEquipment(DestroyMode.Vanish);
				this.apparel.DestroyAll(DestroyMode.Vanish);
			}
			if (this.InContainerEnclosed)
			{
				if (this.carryTracker != null && this.carryTracker.CarriedThing != null)
				{
					this.carryTracker.innerContainer.TryTransferToContainer(this.carryTracker.CarriedThing, this.holdingOwner, true);
				}
				if (this.equipment != null && this.equipment.Primary != null)
				{
					this.equipment.TryTransferEquipmentToContainer(this.equipment.Primary, this.holdingOwner);
				}
				if (this.inventory != null)
				{
					this.inventory.innerContainer.TryTransferAllToContainer(this.holdingOwner, true);
					return;
				}
			}
			else if (base.SpawnedOrAnyParentSpawned)
			{
				if (this.carryTracker != null && this.carryTracker.CarriedThing != null)
				{
					Thing thing;
					this.carryTracker.TryDropCarriedThing(base.PositionHeld, ThingPlaceMode.Near, out thing, null);
				}
				if (!keepInventoryAndEquipmentIfInBed || !this.InBed())
				{
					if (this.equipment != null)
					{
						this.equipment.DropAllEquipment(base.PositionHeld, true);
					}
					if (this.inventory != null && this.inventory.innerContainer.TotalStackCount > 0)
					{
						this.inventory.DropAllNearPawn(base.PositionHeld, true, false);
					}
				}
			}
		}

		
		public void GenerateNecessaryName()
		{
			if (base.Faction != Faction.OfPlayer || !this.RaceProps.Animal)
			{
				return;
			}
			if (this.Name == null)
			{
				this.Name = PawnBioAndNameGenerator.GeneratePawnName(this, NameStyle.Numeric, null);
			}
		}

		
		public Verb TryGetAttackVerb(Thing target, bool allowManualCastWeapons = false)
		{
			if (this.equipment != null && this.equipment.Primary != null && this.equipment.PrimaryEq.PrimaryVerb.Available() && (!this.equipment.PrimaryEq.PrimaryVerb.verbProps.onlyManualCast || (this.CurJob != null && this.CurJob.def != JobDefOf.Wait_Combat) || allowManualCastWeapons))
			{
				return this.equipment.PrimaryEq.PrimaryVerb;
			}
			return this.meleeVerbs.TryGetMeleeVerb(target);
		}

		
		public bool TryStartAttack(LocalTargetInfo targ)
		{
			if (this.stances.FullBodyBusy)
			{
				return false;
			}
			if (this.WorkTagIsDisabled(WorkTags.Violent))
			{
				return false;
			}
			bool allowManualCastWeapons = !this.IsColonist;
			Verb verb = this.TryGetAttackVerb(targ.Thing, allowManualCastWeapons);
			return verb != null && verb.TryStartCastOn(targ, false, true);
		}

		
		public override IEnumerable<Thing> ButcherProducts(Pawn butcher, float efficiency)
		{
			if (this.RaceProps.meatDef != null)
			{
				int num = GenMath.RoundRandom(this.GetStatValue(StatDefOf.MeatAmount, true) * efficiency);
				if (num > 0)
				{
					Thing thing = ThingMaker.MakeThing(this.RaceProps.meatDef, null);
					thing.stackCount = num;
					yield return thing;
				}
			}

			IEnumerator<Thing> enumerator = null;
			if (this.RaceProps.leatherDef != null)
			{
				int num2 = GenMath.RoundRandom(this.GetStatValue(StatDefOf.LeatherAmount, true) * efficiency);
				if (num2 > 0)
				{
					Thing thing3 = ThingMaker.MakeThing(this.RaceProps.leatherDef, null);
					thing3.stackCount = num2;
					yield return thing3;
				}
			}
			//if (!this.RaceProps.Humanlike)
			//{
			//	Pawn.c__DisplayClass203_0 c__DisplayClass203_ = new Pawn.c__DisplayClass203_0();
			//	c__DisplayClass203_.lifeStage = this.ageTracker.CurKindLifeStage;
			//	if (c__DisplayClass203_.lifeStage.butcherBodyPart != null && (this.gender == Gender.None || (this.gender == Gender.Male && c__DisplayClass203_.lifeStage.butcherBodyPart.allowMale) || (this.gender == Gender.Female && c__DisplayClass203_.lifeStage.butcherBodyPart.allowFemale)))
			//	{
			//		for (;;)
			//		{
			//			IEnumerable<BodyPartRecord> notMissingParts = this.health.hediffSet.GetNotMissingParts(BodyPartHeight.Undefined, BodyPartDepth.Undefined, null, null);
			//			Func<BodyPartRecord, bool> predicate;
			//			if ((predicate = c__DisplayClass203_.9__0) == null)
			//			{
			//				predicate = (c__DisplayClass203_.9__0 = ((BodyPartRecord x) => x.IsInGroup(c__DisplayClass203_.lifeStage.butcherBodyPart.bodyPartGroup)));
			//			}
			//			BodyPartRecord bodyPartRecord = notMissingParts.Where(predicate).FirstOrDefault<BodyPartRecord>();
			//			if (bodyPartRecord == null)
			//			{
			//				break;
			//			}
			//			this.health.AddHediff(HediffMaker.MakeHediff(HediffDefOf.MissingBodyPart, this, bodyPartRecord), null, null, null);
			//			Thing thing4;
			//			if (c__DisplayClass203_.lifeStage.butcherBodyPart.thing != null)
			//			{
			//				thing4 = ThingMaker.MakeThing(c__DisplayClass203_.lifeStage.butcherBodyPart.thing, null);
			//			}
			//			else
			//			{
			//				thing4 = ThingMaker.MakeThing(bodyPartRecord.def.spawnThingOnRemoved, null);
			//			}
			//			yield return thing4;
			//		}
			//	}
			//	c__DisplayClass203_ = null;
			//}
			//yield break;
			//yield break;
		}

		
		public string MainDesc(bool writeFaction)
		{
			bool flag = base.Faction == null || !base.Faction.IsPlayer;
			string text = (this.gender == Gender.None) ? "" : this.gender.GetLabel(this.AnimalOrWildMan());
			if (this.RaceProps.Animal || this.RaceProps.IsMechanoid)
			{
				string str = GenLabel.BestKindLabel(this, false, true, false, -1);
				if (this.Name != null)
				{
					text = text + " " + str;
				}
			}
			if (this.ageTracker != null)
			{
				if (text.Length > 0)
				{
					text += ", ";
				}
				text += "AgeIndicator".Translate(this.ageTracker.AgeNumberString);
			}
			if (!this.RaceProps.Animal && !this.RaceProps.IsMechanoid && flag)
			{
				if (text.Length > 0)
				{
					text += ", ";
				}
				text += GenLabel.BestKindLabel(this, false, true, false, -1);
			}
			if (writeFaction)
			{
				Pawn.tmpExtraFactions.Clear();
				QuestUtility.GetExtraFactionsFromQuestParts(this, Pawn.tmpExtraFactions, null);
				if (base.Faction != null && !base.Faction.def.hidden)
				{
					if (Pawn.tmpExtraFactions.Count == 0)
					{
						text = "PawnMainDescFactionedWrap".Translate(text, base.Faction.NameColored).Resolve();
					}
					else
					{
						text = "PawnMainDescUnderFactionedWrap".Translate(text, base.Faction.NameColored).Resolve();
					}
				}
				for (int i = 0; i < Pawn.tmpExtraFactions.Count; i++)
				{
					text += string.Format("\n{0}: {1}", Pawn.tmpExtraFactions[i].factionType.GetLabel().CapitalizeFirst(), Pawn.tmpExtraFactions[i].faction.NameColored.Resolve());
				}
				Pawn.tmpExtraFactions.Clear();
			}
			return text.CapitalizeFirst();
		}

		
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.AppendLine(this.MainDesc(true));
			Pawn_RoyaltyTracker pawn_RoyaltyTracker = this.royalty;
			RoyalTitle royalTitle = (pawn_RoyaltyTracker != null) ? pawn_RoyaltyTracker.MostSeniorTitle : null;
			if (royalTitle != null)
			{
				stringBuilder.AppendLine("PawnTitleDescWrap".Translate(royalTitle.def.GetLabelCapFor(this), royalTitle.faction.NameColored).Resolve());
			}
			string inspectString = base.GetInspectString();
			if (!inspectString.NullOrEmpty())
			{
				stringBuilder.AppendLine(inspectString);
			}
			if (this.TraderKind != null)
			{
				stringBuilder.AppendLine(this.TraderKind.LabelCap);
			}
			if (this.InMentalState)
			{
				stringBuilder.AppendLine(this.MentalState.InspectLine);
			}
			Pawn.states.Clear();
			if (this.stances != null && this.stances.stunner != null && this.stances.stunner.Stunned)
			{
				Pawn.states.AddDistinct("StunLower".Translate());
			}
			if (this.health != null && this.health.hediffSet != null)
			{
				List<Hediff> hediffs = this.health.hediffSet.hediffs;
				for (int i = 0; i < hediffs.Count; i++)
				{
					Hediff hediff = hediffs[i];
					if (!hediff.def.battleStateLabel.NullOrEmpty())
					{
						Pawn.states.AddDistinct(hediff.def.battleStateLabel);
					}
				}
			}
			if (Pawn.states.Count > 0)
			{
				Pawn.states.Sort();
				stringBuilder.AppendLine(string.Format("{0}: {1}", "State".Translate(), Pawn.states.ToCommaList(false).CapitalizeFirst()));
				Pawn.states.Clear();
			}
			if (this.Inspired)
			{
				stringBuilder.AppendLine(this.Inspiration.InspectLine);
			}
			if (this.equipment != null && this.equipment.Primary != null)
			{
				stringBuilder.AppendLine("Equipped".TranslateSimple() + ": " + ((this.equipment.Primary != null) ? this.equipment.Primary.Label : "EquippedNothing".TranslateSimple()).CapitalizeFirst());
			}
			if (this.carryTracker != null && this.carryTracker.CarriedThing != null)
			{
				stringBuilder.Append("Carrying".Translate() + ": ");
				stringBuilder.AppendLine(this.carryTracker.CarriedThing.LabelCap);
			}
			if ((base.Faction == Faction.OfPlayer || this.HostFaction == Faction.OfPlayer) && !this.InMentalState)
			{
				string text = null;
				Lord lord = this.GetLord();
				if (lord != null && lord.LordJob != null)
				{
					text = lord.LordJob.GetReport(this);
				}
				if (this.jobs.curJob != null)
				{
					try
					{
						string text2 = this.jobs.curDriver.GetReport().CapitalizeFirst();
						if (!text.NullOrEmpty())
						{
							text = text + ": " + text2;
						}
						else
						{
							text = text2;
						}
					}
					catch (Exception arg)
					{
						Log.Error("JobDriver.GetReport() exception: " + arg, false);
					}
				}
				if (!text.NullOrEmpty())
				{
					stringBuilder.AppendLine(text);
				}
			}
			if (this.jobs.curJob != null && this.jobs.jobQueue.Count > 0)
			{
				try
				{
					string text3 = this.jobs.jobQueue[0].job.GetReport(this).CapitalizeFirst();
					if (this.jobs.jobQueue.Count > 1)
					{
						text3 = string.Concat(new object[]
						{
							text3,
							" (+",
							this.jobs.jobQueue.Count - 1,
							")"
						});
					}
					stringBuilder.AppendLine("Queued".Translate() + ": " + text3);
				}
				catch (Exception arg2)
				{
					Log.Error("JobDriver.GetReport() exception: " + arg2, false);
				}
			}
			if (RestraintsUtility.ShouldShowRestraintsInfo(this))
			{
				stringBuilder.AppendLine("InRestraints".Translate());
			}
			return stringBuilder.ToString().TrimEndNewlines();
		}

		
		public override IEnumerable<Gizmo> GetGizmos()
		{
			IEnumerator<Gizmo> enumerator;
			if (this.IsColonistPlayerControlled)
			{
				enumerator = null;
				if (this.drafter != null)
				{
					foreach (Gizmo gizmo2 in this.drafter.GetGizmos())
					{
						yield return gizmo2;
					}
					enumerator = null;
				}
				foreach (Gizmo gizmo3 in PawnAttackGizmoUtility.GetAttackGizmos(this))
				{
					yield return gizmo3;
				}
				enumerator = null;
			}
			if (this.equipment != null)
			{
				foreach (Gizmo gizmo4 in this.equipment.GetGizmos())
				{
					yield return gizmo4;
				}
				enumerator = null;
			}
			if (this.psychicEntropy != null && this.psychicEntropy.NeedToShowGizmo())
			{
				yield return this.psychicEntropy.GetGizmo();
			}
			if (this.IsColonistPlayerControlled)
			{
				if (this.abilities != null)
				{
					foreach (Gizmo gizmo5 in this.abilities.GetGizmos())
					{
						yield return gizmo5;
					}
					enumerator = null;
				}
				if (this.playerSettings != null)
				{
					foreach (Gizmo gizmo6 in this.playerSettings.GetGizmos())
					{
						yield return gizmo6;
					}
					enumerator = null;
				}
			}
			if (this.apparel != null)
			{
				foreach (Gizmo gizmo7 in this.apparel.GetGizmos())
				{
					yield return gizmo7;
				}
				enumerator = null;
			}
			foreach (Gizmo gizmo8 in this.mindState.GetGizmos())
			{
				yield return gizmo8;
			}
			enumerator = null;
			if (this.royalty != null && this.IsColonistPlayerControlled)
			{
				if (this.royalty.HasAidPermit)
				{
					yield return this.royalty.RoyalAidGizmo();
				}
				foreach (RoyalTitle royalTitle in this.royalty.AllTitlesForReading)
				{
					if (royalTitle.def.permits != null)
					{
						Faction faction = royalTitle.faction;
						foreach (RoyalTitlePermitDef royalTitlePermitDef in royalTitle.def.permits)
						{
							IEnumerable<Gizmo> pawnGizmos = royalTitlePermitDef.Worker.GetPawnGizmos(this, faction);
							if (pawnGizmos != null)
							{
								foreach (Gizmo gizmo9 in pawnGizmos)
								{
									yield return gizmo9;
								}
								enumerator = null;
							}
						}
						List<RoyalTitlePermitDef>.Enumerator enumerator3 = default(List<RoyalTitlePermitDef>.Enumerator);
						faction = null;
					}
				}
				List<RoyalTitle>.Enumerator enumerator2 = default(List<RoyalTitle>.Enumerator);
			}
			foreach (Gizmo gizmo10 in QuestUtility.GetQuestRelatedGizmos(this))
			{
				yield return gizmo10;
			}
			enumerator = null;
			yield break;
			yield break;
		}

		
		public virtual IEnumerable<FloatMenuOption> GetExtraFloatMenuOptionsFor(IntVec3 sq)
		{
			yield break;
		}

		
		public override TipSignal GetTooltip()
		{
			string value = "";
			if (this.gender != Gender.None)
			{
				if (!this.LabelCap.EqualsIgnoreCase(this.KindLabel))
				{
					value = "PawnTooltipGenderAndKindLabel".Translate(this.GetGenderLabel(), this.KindLabel);
				}
				else
				{
					value = this.GetGenderLabel();
				}
			}
			else if (!this.LabelCap.EqualsIgnoreCase(this.KindLabel))
			{
				value = this.KindLabel;
			}
			string generalConditionLabel = HealthUtility.GetGeneralConditionLabel(this, false);
			bool flag = !string.IsNullOrEmpty(value);
			string text;
			if (this.equipment != null && this.equipment.Primary != null)
			{
				if (flag)
				{
					text = "PawnTooltipWithDescAndPrimaryEquip".Translate(this.LabelCap, value, this.equipment.Primary.LabelCap, generalConditionLabel);
				}
				else
				{
					text = "PawnTooltipWithPrimaryEquipNoDesc".Translate(this.LabelCap, value, generalConditionLabel);
				}
			}
			else if (flag)
			{
				text = "PawnTooltipWithDescNoPrimaryEquip".Translate(this.LabelCap, value, generalConditionLabel);
			}
			else
			{
				text = "PawnTooltipNoDescNoPrimaryEquip".Translate(this.LabelCap, generalConditionLabel);
			}
			return new TipSignal(text, this.thingIDNumber * 152317, TooltipPriority.Pawn);
		}

		
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{

			IEnumerator<StatDrawEntry> enumerator = null;
			yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "BodySize".Translate(), this.BodySize.ToString("F2"), "Stat_Race_BodySize_Desc".Translate(), 500, null, null, false);
			if (this.IsWildMan())
			{
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "Wildness".Translate(), 0.75f.ToStringPercent(), TrainableUtility.GetWildnessExplanation(this.def), 2050, null, null, false);
			}
			if (ModsConfig.RoyaltyActive && this.RaceProps.intelligence == Intelligence.Humanlike)
			{
				yield return new StatDrawEntry(StatCategoryDefOf.BasicsPawn, "MeditationFocuses".Translate(), MeditationUtility.FocusTypesAvailableForPawnString(this).CapitalizeFirst(), ("MeditationFocusesPawnDesc".Translate() + "\n\n" + MeditationUtility.FocusTypeAvailableExplanation(this)).Resolve(), 99995, null, null, false);
			}
			yield break;
			yield break;
		}

		
		public bool CurrentlyUsableForBills()
		{
			if (!this.InBed())
			{
				JobFailReason.Is(Pawn.NotSurgeryReadyTrans, null);
				return false;
			}
			if (!this.InteractionCell.IsValid)
			{
				JobFailReason.Is(Pawn.CannotReachTrans, null);
				return false;
			}
			return true;
		}

		
		public bool UsableForBillsAfterFueling()
		{
			return this.CurrentlyUsableForBills();
		}

		
		public bool AnythingToStrip()
		{
			if (this.equipment != null && this.equipment.HasAnything())
			{
				return true;
			}
			if (this.inventory != null && this.inventory.innerContainer.Count > 0)
			{
				return true;
			}
			if (this.apparel != null)
			{
				if (base.Destroyed)
				{
					if (this.apparel.AnyApparel)
					{
						return true;
					}
				}
				else if (this.apparel.AnyApparelUnlocked)
				{
					return true;
				}
			}
			return false;
		}

		
		public void Strip()
		{
			Caravan caravan = this.GetCaravan();
			if (caravan != null)
			{
				CaravanInventoryUtility.MoveAllInventoryToSomeoneElse(this, caravan.PawnsListForReading, null);
				if (this.apparel != null)
				{
					CaravanInventoryUtility.MoveAllApparelToSomeonesInventory(this, caravan.PawnsListForReading, base.Destroyed);
				}
				if (this.equipment != null)
				{
					CaravanInventoryUtility.MoveAllEquipmentToSomeonesInventory(this, caravan.PawnsListForReading);
				}
			}
			else
			{
				IntVec3 pos = (this.Corpse != null) ? this.Corpse.PositionHeld : base.PositionHeld;
				if (this.equipment != null)
				{
					this.equipment.DropAllEquipment(pos, false);
				}
				if (this.apparel != null)
				{
					this.apparel.DropAll(pos, false, base.Destroyed);
				}
				if (this.inventory != null)
				{
					this.inventory.DropAllNearPawn(pos, false, false);
				}
			}
			if (base.Faction != null)
			{
				base.Faction.Notify_MemberStripped(this, Faction.OfPlayer);
			}
		}

		
		// (get) Token: 0x06001224 RID: 4644 RVA: 0x000675DF File Offset: 0x000657DF
		public TradeCurrency TradeCurrency
		{
			get
			{
				return this.TraderKind.tradeCurrency;
			}
		}

		
		public IEnumerable<Thing> ColonyThingsWillingToBuy(Pawn playerNegotiator)
		{
			return this.trader.ColonyThingsWillingToBuy(playerNegotiator);
		}

		
		public void GiveSoldThingToTrader(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			this.trader.GiveSoldThingToTrader(toGive, countToGive, playerNegotiator);
		}

		
		public void GiveSoldThingToPlayer(Thing toGive, int countToGive, Pawn playerNegotiator)
		{
			this.trader.GiveSoldThingToPlayer(toGive, countToGive, playerNegotiator);
		}

		
		public void HearClamor(Thing source, ClamorDef type)
		{
			if (this.Dead || this.Downed)
			{
				return;
			}
			if (type == ClamorDefOf.Movement)
			{
				Pawn pawn = source as Pawn;
				if (pawn != null)
				{
					this.CheckForDisturbedSleep(pawn);
				}
				this.NotifyLordOfClamor(source, type);
			}
			if (type == ClamorDefOf.Harm && base.Faction != Faction.OfPlayer && !this.Awake() && base.Faction == source.Faction && this.HostFaction == null)
			{
				this.mindState.canSleepTick = Find.TickManager.TicksGame + 1000;
				if (this.CurJob != null)
				{
					this.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
				this.NotifyLordOfClamor(source, type);
			}
			if (type == ClamorDefOf.Construction && base.Faction != Faction.OfPlayer && !this.Awake() && base.Faction != source.Faction && this.HostFaction == null)
			{
				this.mindState.canSleepTick = Find.TickManager.TicksGame + 1000;
				if (this.CurJob != null)
				{
					this.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
				this.NotifyLordOfClamor(source, type);
			}
			if (type == ClamorDefOf.Ability && base.Faction != Faction.OfPlayer && base.Faction != source.Faction && this.HostFaction == null)
			{
				if (!this.Awake())
				{
					this.mindState.canSleepTick = Find.TickManager.TicksGame + 1000;
					if (this.CurJob != null)
					{
						this.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
					}
				}
				this.NotifyLordOfClamor(source, type);
			}
			if (type == ClamorDefOf.Impact)
			{
				this.mindState.canSleepTick = Find.TickManager.TicksGame + 1000;
				if (this.CurJob != null && !this.Awake())
				{
					this.jobs.EndCurrentJob(JobCondition.InterruptForced, true, true);
				}
				this.NotifyLordOfClamor(source, type);
			}
		}

		
		private void NotifyLordOfClamor(Thing source, ClamorDef type)
		{
			Lord lord = this.GetLord();
			if (lord != null)
			{
				lord.Notify_Clamor(source, type);
			}
		}

		
		public override void Notify_Explosion(Explosion explosion)
		{
			base.Notify_Explosion(explosion);
			this.mindState.Notify_Explosion(explosion);
		}

		
		private void CheckForDisturbedSleep(Pawn source)
		{
			if (this.needs.mood == null)
			{
				return;
			}
			if (this.Awake())
			{
				return;
			}
			if (base.Faction != Faction.OfPlayer)
			{
				return;
			}
			if (Find.TickManager.TicksGame < this.lastSleepDisturbedTick + 300)
			{
				return;
			}
			if (source != null)
			{
				if (LovePartnerRelationUtility.LovePartnerRelationExists(this, source))
				{
					return;
				}
				if (source.RaceProps.petness > 0f)
				{
					return;
				}
				if (source.relations != null)
				{
					if (source.relations.DirectRelations.Any((DirectPawnRelation dr) => dr.def == PawnRelationDefOf.Bond))
					{
						return;
					}
				}
			}
			this.lastSleepDisturbedTick = Find.TickManager.TicksGame;
			this.needs.mood.thoughts.memories.TryGainMemory(ThoughtDefOf.SleepDisturbed, null);
		}

		
		public float GetAcceptArrestChance(Pawn arrester)
		{
			float num = StatDefOf.ArrestSuccessChance.Worker.IsDisabledFor(arrester) ? StatDefOf.ArrestSuccessChance.valueIfMissing : arrester.GetStatValue(StatDefOf.ArrestSuccessChance, true);
			if (this.IsWildMan())
			{
				return num * 0.5f;
			}
			return num;
		}

		
		public bool CheckAcceptArrest(Pawn arrester)
		{
			if (this.health.Downed)
			{
				return true;
			}
			if (this.WorkTagIsDisabled(WorkTags.Violent))
			{
				return true;
			}
			Faction factionOrExtraHomeFaction = this.FactionOrExtraHomeFaction;
			if (factionOrExtraHomeFaction != null && factionOrExtraHomeFaction != arrester.factionInt)
			{
				factionOrExtraHomeFaction.Notify_MemberCaptured(this, arrester.Faction);
			}
			float acceptArrestChance = this.GetAcceptArrestChance(arrester);
			if (Rand.Value < acceptArrestChance)
			{
				return true;
			}
			Messages.Message("MessageRefusedArrest".Translate(this.LabelShort, this), this, MessageTypeDefOf.ThreatSmall, true);
			if (base.Faction == null || !arrester.HostileTo(this))
			{
				this.mindState.mentalStateHandler.TryStartMentalState(MentalStateDefOf.Berserk, null, false, false, null, false);
			}
			return false;
		}

		
		public bool ThreatDisabled(IAttackTargetSearcher disabledFor)
		{
			if (!base.Spawned)
			{
				return true;
			}
			if (!this.InMentalState && this.GetTraderCaravanRole() == TraderCaravanRole.Carrier && !(this.jobs.curDriver is JobDriver_AttackMelee))
			{
				return true;
			}
			if (this.mindState.duty != null && this.mindState.duty.def.threatDisabled)
			{
				return true;
			}
			if (!this.mindState.Active)
			{
				return true;
			}
			if (this.Downed)
			{
				if (disabledFor == null)
				{
					return true;
				}
				Pawn pawn = disabledFor.Thing as Pawn;
				if (pawn == null || pawn.mindState == null || pawn.mindState.duty == null || !pawn.mindState.duty.attackDownedIfStarving || !pawn.Starving())
				{
					return true;
				}
			}
			return this.IsInvisible();
		}

		
		public List<WorkTypeDef> GetDisabledWorkTypes(bool permanentOnly = false)
		{
			//Pawn.c__DisplayClass232_0 c__DisplayClass232_;
			//c__DisplayClass232_.4__this = this;
			//c__DisplayClass232_.permanentOnly = permanentOnly;
			//if (c__DisplayClass232_.permanentOnly)
			//{
			//	if (this.cachedDisabledWorkTypesPermanent == null)
			//	{
			//		this.cachedDisabledWorkTypesPermanent = new List<WorkTypeDef>();
			//	}
			//	this.<GetDisabledWorkTypes>g__FillList|232_0(this.cachedDisabledWorkTypesPermanent, ref c__DisplayClass232_);
			//	return this.cachedDisabledWorkTypesPermanent;
			//}
			//if (this.cachedDisabledWorkTypes == null)
			//{
			//	this.cachedDisabledWorkTypes = new List<WorkTypeDef>();
			//}
			//this.<GetDisabledWorkTypes>g__FillList|232_0(this.cachedDisabledWorkTypes, ref c__DisplayClass232_);
			return this.cachedDisabledWorkTypes;
		}

		
		public bool WorkTypeIsDisabled(WorkTypeDef w)
		{
			return this.GetDisabledWorkTypes(false).Contains(w);
		}

		
		public bool OneOfWorkTypesIsDisabled(List<WorkTypeDef> wts)
		{
			for (int i = 0; i < wts.Count; i++)
			{
				if (this.WorkTypeIsDisabled(wts[i]))
				{
					return true;
				}
			}
			return false;
		}

		
		public void Notify_DisabledWorkTypesChanged()
		{
			this.cachedDisabledWorkTypes = null;
			this.cachedDisabledWorkTypesPermanent = null;
			Pawn_WorkSettings pawn_WorkSettings = this.workSettings;
			if (pawn_WorkSettings == null)
			{
				return;
			}
			pawn_WorkSettings.Notify_DisabledWorkTypesChanged();
		}

		
		// (get) Token: 0x06001233 RID: 4659 RVA: 0x00067B98 File Offset: 0x00065D98
		public WorkTags CombinedDisabledWorkTags
		{
			get
			{
				WorkTags workTags = (this.story != null) ? this.story.DisabledWorkTagsBackstoryAndTraits : WorkTags.None;
				if (this.royalty != null)
				{
					foreach (RoyalTitle royalTitle in this.royalty.AllTitlesForReading)
					{
						if (royalTitle.conceited)
						{
							workTags |= royalTitle.def.disabledWorkTags;
						}
					}
				}
				if (this.health != null && this.health.hediffSet != null)
				{
					foreach (Hediff hediff in this.health.hediffSet.hediffs)
					{
						HediffStage curStage = hediff.CurStage;
						if (curStage != null)
						{
							workTags |= curStage.disabledWorkTags;
						}
					}
				}
				foreach (QuestPart_WorkDisabled questPart_WorkDisabled in QuestUtility.GetWorkDisabledQuestPart(this))
				{
					workTags |= questPart_WorkDisabled.disabledWorkTags;
				}
				return workTags;
			}
		}

		
		public bool WorkTagIsDisabled(WorkTags w)
		{
			return (this.CombinedDisabledWorkTags & w) > WorkTags.None;
		}

		
		public override bool PreventPlayerSellingThingsNearby(out string reason)
		{
			if (this.InAggroMentalState || (base.Faction.HostileTo(Faction.OfPlayer) && this.HostFaction == null && !this.Downed && !this.InMentalState))
			{
				reason = "Enemies".Translate();
				return true;
			}
			reason = null;
			return false;
		}

		
		public void ChangeKind(PawnKindDef newKindDef)
		{
			if (this.kindDef == newKindDef)
			{
				return;
			}
			this.kindDef = newKindDef;
			if (this.kindDef == PawnKindDefOf.WildMan)
			{
				this.mindState.WildManEverReachedOutside = false;
				ReachabilityUtility.ClearCacheFor(this);
			}
		}

		
		// (get) Token: 0x06001237 RID: 4663 RVA: 0x00067D6C File Offset: 0x00065F6C
		public bool HasPsylink
		{
			get
			{
				return this.psychicEntropy != null && this.psychicEntropy.Psylink != null;
			}
		}

		
		public PawnKindDef kindDef;

		
		private Name nameInt;

		
		public Gender gender;

		
		public Pawn_AgeTracker ageTracker;

		
		public Pawn_HealthTracker health;

		
		public Pawn_RecordsTracker records;

		
		public Pawn_InventoryTracker inventory;

		
		public Pawn_MeleeVerbs meleeVerbs;

		
		public VerbTracker verbTracker;

		
		public Pawn_CarryTracker carryTracker;

		
		public Pawn_NeedsTracker needs;

		
		public Pawn_MindState mindState;

		
		public Pawn_RotationTracker rotationTracker;

		
		public Pawn_PathFollower pather;

		
		public Pawn_Thinker thinker;

		
		public Pawn_JobTracker jobs;

		
		public Pawn_StanceTracker stances;

		
		public Pawn_NativeVerbs natives;

		
		public Pawn_FilthTracker filth;

		
		public Pawn_EquipmentTracker equipment;

		
		public Pawn_ApparelTracker apparel;

		
		public Pawn_Ownership ownership;

		
		public Pawn_SkillTracker skills;

		
		public Pawn_StoryTracker story;

		
		public Pawn_GuestTracker guest;

		
		public Pawn_GuiltTracker guilt;

		
		public Pawn_RoyaltyTracker royalty;

		
		public Pawn_AbilityTracker abilities;

		
		public Pawn_WorkSettings workSettings;

		
		public Pawn_TraderTracker trader;

		
		public Pawn_TrainingTracker training;

		
		public Pawn_CallTracker caller;

		
		public Pawn_RelationsTracker relations;

		
		public Pawn_PsychicEntropyTracker psychicEntropy;

		
		public Pawn_InteractionsTracker interactions;

		
		public Pawn_PlayerSettings playerSettings;

		
		public Pawn_OutfitTracker outfits;

		
		public Pawn_DrugPolicyTracker drugs;

		
		public Pawn_FoodRestrictionTracker foodRestriction;

		
		public Pawn_TimetableTracker timetable;

		
		public Pawn_DraftController drafter;

		
		private Pawn_DrawTracker drawer;

		
		public int becameWorldPawnTickAbs = -1;

		
		private const float HumanSizedHeatOutput = 0.3f;

		
		private const float AnimalHeatOutputFactor = 0.6f;

		
		private static string NotSurgeryReadyTrans;

		
		private static string CannotReachTrans;

		
		public const int MaxMoveTicks = 450;

		
		private static List<ExtraFaction> tmpExtraFactions = new List<ExtraFaction>();

		
		private static List<string> states = new List<string>();

		
		private int lastSleepDisturbedTick;

		
		private const int SleepDisturbanceMinInterval = 300;

		
		private List<WorkTypeDef> cachedDisabledWorkTypes;

		
		private List<WorkTypeDef> cachedDisabledWorkTypesPermanent;
	}
}
