using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	
	public abstract class ThingComp
	{
		
		// (get) Token: 0x0600176E RID: 5998 RVA: 0x00085B7D File Offset: 0x00083D7D
		public IThingHolder ParentHolder
		{
			get
			{
				return this.parent.ParentHolder;
			}
		}

		
		public virtual void Initialize(CompProperties props)
		{
			this.props = props;
		}

		
		public virtual void ReceiveCompSignal(string signal)
		{
		}

		
		public virtual void PostExposeData()
		{
		}

		
		public virtual void PostSpawnSetup(bool respawningAfterLoad)
		{
		}

		
		public virtual void PostDeSpawn(Map map)
		{
		}

		
		public virtual void PostDestroy(DestroyMode mode, Map previousMap)
		{
		}

		
		public virtual void PostPostMake()
		{
		}

		
		public virtual void CompTick()
		{
		}

		
		public virtual void CompTickRare()
		{
		}

		
		public virtual void PostPreApplyDamage(DamageInfo dinfo, out bool absorbed)
		{
			absorbed = false;
		}

		
		public virtual void PostPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
		}

		
		public virtual void PostDraw()
		{
		}

		
		public virtual void PostDrawExtraSelectionOverlays()
		{
		}

		
		public virtual void PostPrintOnto(SectionLayer layer)
		{
		}

		
		public virtual void CompPrintForPowerGrid(SectionLayer layer)
		{
		}

		
		public virtual void PreAbsorbStack(Thing otherStack, int count)
		{
		}

		
		public virtual void PostSplitOff(Thing piece)
		{
		}

		
		public virtual string TransformLabel(string label)
		{
			return label;
		}

		
		public virtual IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			yield break;
		}

		
		public virtual bool AllowStackWith(Thing other)
		{
			return true;
		}

		
		public virtual string CompInspectStringExtra()
		{
			return null;
		}

		
		public virtual string GetDescriptionPart()
		{
			return null;
		}

		
		public virtual IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
		{
			yield break;
		}

		
		public virtual void PrePreTraded(TradeAction action, Pawn playerNegotiator, ITrader trader)
		{
		}

		
		public virtual void PrePostIngested(Pawn ingester)
		{
		}

		
		public virtual void PostIngested(Pawn ingester)
		{
		}

		
		public virtual void PostPostGeneratedForTrader(TraderKindDef trader, int forTile, Faction forFaction)
		{
		}

		
		public virtual void Notify_SignalReceived(Signal signal)
		{
		}

		
		public virtual void Notify_LordDestroyed()
		{
		}

		
		public virtual void DrawGUIOverlay()
		{
		}

		
		public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			return null;
		}

		
		public virtual void Notify_Equipped(Pawn pawn)
		{
		}

		
		public virtual void Notify_UsedWeapon(Pawn pawn)
		{
		}

		
		public override string ToString()
		{
			return string.Concat(new object[]
			{
				base.GetType().Name,
				"(parent=",
				this.parent,
				" at=",
				(this.parent != null) ? this.parent.Position : IntVec3.Invalid,
				")"
			});
		}

		
		public ThingWithComps parent;

		
		public CompProperties props;
	}
}
