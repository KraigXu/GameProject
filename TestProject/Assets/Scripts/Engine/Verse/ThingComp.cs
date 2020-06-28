using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000324 RID: 804
	public abstract class ThingComp
	{
		// Token: 0x170004DB RID: 1243
		// (get) Token: 0x0600176E RID: 5998 RVA: 0x00085B7D File Offset: 0x00083D7D
		public IThingHolder ParentHolder
		{
			get
			{
				return this.parent.ParentHolder;
			}
		}

		// Token: 0x0600176F RID: 5999 RVA: 0x00085B8A File Offset: 0x00083D8A
		public virtual void Initialize(CompProperties props)
		{
			this.props = props;
		}

		// Token: 0x06001770 RID: 6000 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void ReceiveCompSignal(string signal)
		{
		}

		// Token: 0x06001771 RID: 6001 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void PostExposeData()
		{
		}

		// Token: 0x06001772 RID: 6002 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void PostSpawnSetup(bool respawningAfterLoad)
		{
		}

		// Token: 0x06001773 RID: 6003 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void PostDeSpawn(Map map)
		{
		}

		// Token: 0x06001774 RID: 6004 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void PostDestroy(DestroyMode mode, Map previousMap)
		{
		}

		// Token: 0x06001775 RID: 6005 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void PostPostMake()
		{
		}

		// Token: 0x06001776 RID: 6006 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void CompTick()
		{
		}

		// Token: 0x06001777 RID: 6007 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void CompTickRare()
		{
		}

		// Token: 0x06001778 RID: 6008 RVA: 0x0008197E File Offset: 0x0007FB7E
		public virtual void PostPreApplyDamage(DamageInfo dinfo, out bool absorbed)
		{
			absorbed = false;
		}

		// Token: 0x06001779 RID: 6009 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void PostPostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
		}

		// Token: 0x0600177A RID: 6010 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void PostDraw()
		{
		}

		// Token: 0x0600177B RID: 6011 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void PostDrawExtraSelectionOverlays()
		{
		}

		// Token: 0x0600177C RID: 6012 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void PostPrintOnto(SectionLayer layer)
		{
		}

		// Token: 0x0600177D RID: 6013 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void CompPrintForPowerGrid(SectionLayer layer)
		{
		}

		// Token: 0x0600177E RID: 6014 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void PreAbsorbStack(Thing otherStack, int count)
		{
		}

		// Token: 0x0600177F RID: 6015 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void PostSplitOff(Thing piece)
		{
		}

		// Token: 0x06001780 RID: 6016 RVA: 0x0002D90A File Offset: 0x0002BB0A
		public virtual string TransformLabel(string label)
		{
			return label;
		}

		// Token: 0x06001781 RID: 6017 RVA: 0x00085B93 File Offset: 0x00083D93
		public virtual IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			yield break;
		}

		// Token: 0x06001782 RID: 6018 RVA: 0x0001028D File Offset: 0x0000E48D
		public virtual bool AllowStackWith(Thing other)
		{
			return true;
		}

		// Token: 0x06001783 RID: 6019 RVA: 0x00019EA1 File Offset: 0x000180A1
		public virtual string CompInspectStringExtra()
		{
			return null;
		}

		// Token: 0x06001784 RID: 6020 RVA: 0x00019EA1 File Offset: 0x000180A1
		public virtual string GetDescriptionPart()
		{
			return null;
		}

		// Token: 0x06001785 RID: 6021 RVA: 0x00085B9C File Offset: 0x00083D9C
		public virtual IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn selPawn)
		{
			yield break;
		}

		// Token: 0x06001786 RID: 6022 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void PrePreTraded(TradeAction action, Pawn playerNegotiator, ITrader trader)
		{
		}

		// Token: 0x06001787 RID: 6023 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void PrePostIngested(Pawn ingester)
		{
		}

		// Token: 0x06001788 RID: 6024 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void PostIngested(Pawn ingester)
		{
		}

		// Token: 0x06001789 RID: 6025 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void PostPostGeneratedForTrader(TraderKindDef trader, int forTile, Faction forFaction)
		{
		}

		// Token: 0x0600178A RID: 6026 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void Notify_SignalReceived(Signal signal)
		{
		}

		// Token: 0x0600178B RID: 6027 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void Notify_LordDestroyed()
		{
		}

		// Token: 0x0600178C RID: 6028 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void DrawGUIOverlay()
		{
		}

		// Token: 0x0600178D RID: 6029 RVA: 0x00019EA1 File Offset: 0x000180A1
		public virtual IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			return null;
		}

		// Token: 0x0600178E RID: 6030 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void Notify_Equipped(Pawn pawn)
		{
		}

		// Token: 0x0600178F RID: 6031 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void Notify_UsedWeapon(Pawn pawn)
		{
		}

		// Token: 0x06001790 RID: 6032 RVA: 0x00085BA8 File Offset: 0x00083DA8
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

		// Token: 0x04000EAA RID: 3754
		public ThingWithComps parent;

		// Token: 0x04000EAB RID: 3755
		public CompProperties props;
	}
}
