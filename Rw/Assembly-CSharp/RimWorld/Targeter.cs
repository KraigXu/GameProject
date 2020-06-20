using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	// Token: 0x02000EBA RID: 3770
	public class Targeter
	{
		// Token: 0x170010A1 RID: 4257
		// (get) Token: 0x06005C1F RID: 23583 RVA: 0x001FCEFE File Offset: 0x001FB0FE
		public bool IsTargeting
		{
			get
			{
				return this.targetingSource != null || this.action != null;
			}
		}

		// Token: 0x06005C20 RID: 23584 RVA: 0x001FCF14 File Offset: 0x001FB114
		public void BeginTargeting(ITargetingSource source, ITargetingSource parent = null)
		{
			if (source.Targetable)
			{
				this.targetingSource = source;
				this.targetingSourceAdditionalPawns = new List<Pawn>();
			}
			else
			{
				Job job = JobMaker.MakeJob(JobDefOf.UseVerbOnThing);
				job.verbToUse = this.targetingSource.GetVerb;
				source.CasterPawn.jobs.StartJob(job, JobCondition.None, null, false, true, null, null, false, false);
			}
			this.action = null;
			this.caster = null;
			this.targetParams = null;
			this.actionWhenFinished = null;
			this.mouseAttachment = null;
			this.targetingSourceParent = parent;
			this.needsStopTargetingCall = false;
		}

		// Token: 0x06005C21 RID: 23585 RVA: 0x001FCFAC File Offset: 0x001FB1AC
		public void BeginTargeting(TargetingParameters targetParams, Action<LocalTargetInfo> action, Pawn caster = null, Action actionWhenFinished = null, Texture2D mouseAttachment = null)
		{
			this.targetingSource = null;
			this.targetingSourceParent = null;
			this.targetingSourceAdditionalPawns = null;
			this.action = action;
			this.targetParams = targetParams;
			this.caster = caster;
			this.actionWhenFinished = actionWhenFinished;
			this.mouseAttachment = mouseAttachment;
			this.needsStopTargetingCall = false;
		}

		// Token: 0x06005C22 RID: 23586 RVA: 0x001FCFFC File Offset: 0x001FB1FC
		public void BeginTargeting(TargetingParameters targetParams, ITargetingSource ability, Action<LocalTargetInfo> action, Action actionWhenFinished = null, Texture2D mouseAttachment = null)
		{
			this.targetingSource = null;
			this.targetingSourceParent = null;
			this.targetingSourceAdditionalPawns = null;
			this.action = action;
			this.actionWhenFinished = actionWhenFinished;
			this.caster = null;
			this.targetParams = targetParams;
			this.mouseAttachment = mouseAttachment;
			this.targetingSource = ability;
			this.needsStopTargetingCall = false;
		}

		// Token: 0x06005C23 RID: 23587 RVA: 0x001FD051 File Offset: 0x001FB251
		public void StopTargeting()
		{
			if (this.actionWhenFinished != null)
			{
				Action action = this.actionWhenFinished;
				this.actionWhenFinished = null;
				action();
			}
			this.targetingSource = null;
			this.action = null;
			this.targetParams = null;
		}

		// Token: 0x06005C24 RID: 23588 RVA: 0x001FD084 File Offset: 0x001FB284
		public void ProcessInputEvents()
		{
			this.ConfirmStillValid();
			if (this.IsTargeting)
			{
				if (Event.current.type == EventType.MouseDown && Event.current.button == 0)
				{
					LocalTargetInfo localTargetInfo = this.CurrentTargetUnderMouse(false);
					this.needsStopTargetingCall = true;
					if (this.targetingSource != null)
					{
						if (!this.targetingSource.ValidateTarget(localTargetInfo))
						{
							Event.current.Use();
							return;
						}
						this.OrderVerbForceTarget();
					}
					if (this.action != null && localTargetInfo.IsValid)
					{
						this.action(localTargetInfo);
					}
					SoundDefOf.Tick_High.PlayOneShotOnCamera(null);
					if (this.targetingSource != null)
					{
						if (this.targetingSource.DestinationSelector != null)
						{
							this.BeginTargeting(this.targetingSource.DestinationSelector, this.targetingSource);
						}
						else if (this.targetingSource.MultiSelect && Event.current.shift)
						{
							this.BeginTargeting(this.targetingSource, null);
						}
						else if (this.targetingSourceParent != null && this.targetingSourceParent.MultiSelect && Event.current.shift)
						{
							this.BeginTargeting(this.targetingSourceParent, null);
						}
					}
					if (this.needsStopTargetingCall)
					{
						this.StopTargeting();
					}
					Event.current.Use();
				}
				if ((Event.current.type == EventType.MouseDown && Event.current.button == 1) || KeyBindingDefOf.Cancel.KeyDownEvent)
				{
					SoundDefOf.CancelMode.PlayOneShotOnCamera(null);
					this.StopTargeting();
					Event.current.Use();
				}
			}
		}

		// Token: 0x06005C25 RID: 23589 RVA: 0x001FD1FC File Offset: 0x001FB3FC
		public void TargeterOnGUI()
		{
			if (this.targetingSource != null)
			{
				LocalTargetInfo target = this.CurrentTargetUnderMouse(true);
				this.targetingSource.OnGUI(target);
			}
			if (this.action != null)
			{
				GenUI.DrawMouseAttachment(this.mouseAttachment ?? TexCommand.Attack);
			}
		}

		// Token: 0x06005C26 RID: 23590 RVA: 0x001FD244 File Offset: 0x001FB444
		public void TargeterUpdate()
		{
			if (this.targetingSource != null)
			{
				this.targetingSource.DrawHighlight(this.CurrentTargetUnderMouse(true));
			}
			if (this.action != null)
			{
				LocalTargetInfo targ = this.CurrentTargetUnderMouse(false);
				if (targ.IsValid)
				{
					GenDraw.DrawTargetHighlight(targ);
				}
			}
		}

		// Token: 0x06005C27 RID: 23591 RVA: 0x001FD28C File Offset: 0x001FB48C
		public bool IsPawnTargeting(Pawn p)
		{
			if (this.caster == p)
			{
				return true;
			}
			if (this.targetingSource != null && this.targetingSource.CasterIsPawn)
			{
				if (this.targetingSource.CasterPawn == p)
				{
					return true;
				}
				for (int i = 0; i < this.targetingSourceAdditionalPawns.Count; i++)
				{
					if (this.targetingSourceAdditionalPawns[i] == p)
					{
						return true;
					}
				}
			}
			return false;
		}

		// Token: 0x06005C28 RID: 23592 RVA: 0x001FD2F4 File Offset: 0x001FB4F4
		private void ConfirmStillValid()
		{
			if (this.caster != null && (this.caster.Map != Find.CurrentMap || this.caster.Destroyed || !Find.Selector.IsSelected(this.caster)))
			{
				this.StopTargeting();
			}
			if (this.targetingSource != null)
			{
				Selector selector = Find.Selector;
				if (this.targetingSource.Caster.Map != Find.CurrentMap || this.targetingSource.Caster.Destroyed || !selector.IsSelected(this.targetingSource.Caster))
				{
					this.StopTargeting();
					return;
				}
				for (int i = 0; i < this.targetingSourceAdditionalPawns.Count; i++)
				{
					if (this.targetingSourceAdditionalPawns[i].Destroyed || !selector.IsSelected(this.targetingSourceAdditionalPawns[i]))
					{
						this.StopTargeting();
						return;
					}
				}
			}
		}

		// Token: 0x06005C29 RID: 23593 RVA: 0x001FD3D8 File Offset: 0x001FB5D8
		private void OrderVerbForceTarget()
		{
			if (this.targetingSource.CasterIsPawn)
			{
				this.OrderPawnForceTarget(this.targetingSource);
				for (int i = 0; i < this.targetingSourceAdditionalPawns.Count; i++)
				{
					Verb targetingVerb = this.GetTargetingVerb(this.targetingSourceAdditionalPawns[i]);
					if (targetingVerb != null)
					{
						this.OrderPawnForceTarget(targetingVerb);
					}
				}
				return;
			}
			int numSelected = Find.Selector.NumSelected;
			List<object> selectedObjects = Find.Selector.SelectedObjects;
			for (int j = 0; j < numSelected; j++)
			{
				Building_Turret building_Turret = selectedObjects[j] as Building_Turret;
				if (building_Turret != null && building_Turret.Map == Find.CurrentMap)
				{
					LocalTargetInfo targ = this.CurrentTargetUnderMouse(true);
					building_Turret.OrderAttack(targ);
				}
			}
		}

		// Token: 0x06005C2A RID: 23594 RVA: 0x001FD48C File Offset: 0x001FB68C
		public void OrderPawnForceTarget(ITargetingSource targetingSource)
		{
			LocalTargetInfo target = this.CurrentTargetUnderMouse(true);
			if (!target.IsValid)
			{
				return;
			}
			targetingSource.OrderForceTarget(target);
		}

		// Token: 0x06005C2B RID: 23595 RVA: 0x001FD4B4 File Offset: 0x001FB6B4
		private LocalTargetInfo CurrentTargetUnderMouse(bool mustBeHittableNowIfNotMelee)
		{
			if (!this.IsTargeting)
			{
				return LocalTargetInfo.Invalid;
			}
			TargetingParameters targetingParameters = (this.targetingSource != null) ? this.targetingSource.targetParams : this.targetParams;
			LocalTargetInfo localTargetInfo = GenUI.TargetsAtMouse_NewTemp(targetingParameters, false, this.targetingSource).FirstOrFallback(LocalTargetInfo.Invalid);
			if (localTargetInfo.IsValid && this.targetingSource != null)
			{
				if (mustBeHittableNowIfNotMelee && !(localTargetInfo.Thing is Pawn) && !this.targetingSource.IsMeleeAttack)
				{
					if (this.targetingSourceAdditionalPawns != null && this.targetingSourceAdditionalPawns.Any<Pawn>())
					{
						bool flag = false;
						for (int i = 0; i < this.targetingSourceAdditionalPawns.Count; i++)
						{
							Verb targetingVerb = this.GetTargetingVerb(this.targetingSourceAdditionalPawns[i]);
							if (targetingVerb != null && targetingVerb.CanHitTarget(localTargetInfo))
							{
								flag = true;
								break;
							}
						}
						if (!flag)
						{
							localTargetInfo = LocalTargetInfo.Invalid;
						}
					}
					else if (!this.targetingSource.CanHitTarget(localTargetInfo))
					{
						localTargetInfo = LocalTargetInfo.Invalid;
					}
				}
				if (localTargetInfo == this.targetingSource.Caster && !targetingParameters.canTargetSelf)
				{
					localTargetInfo = LocalTargetInfo.Invalid;
				}
			}
			return localTargetInfo;
		}

		// Token: 0x06005C2C RID: 23596 RVA: 0x001FD5D4 File Offset: 0x001FB7D4
		private Verb GetTargetingVerb(Pawn pawn)
		{
			return pawn.equipment.AllEquipmentVerbs.FirstOrDefault((Verb x) => x.verbProps == this.targetingSource.GetVerb.verbProps);
		}

		// Token: 0x0400323F RID: 12863
		public ITargetingSource targetingSource;

		// Token: 0x04003240 RID: 12864
		public ITargetingSource targetingSourceParent;

		// Token: 0x04003241 RID: 12865
		public List<Pawn> targetingSourceAdditionalPawns;

		// Token: 0x04003242 RID: 12866
		private Action<LocalTargetInfo> action;

		// Token: 0x04003243 RID: 12867
		private Pawn caster;

		// Token: 0x04003244 RID: 12868
		private TargetingParameters targetParams;

		// Token: 0x04003245 RID: 12869
		private Action actionWhenFinished;

		// Token: 0x04003246 RID: 12870
		private Texture2D mouseAttachment;

		// Token: 0x04003247 RID: 12871
		private bool needsStopTargetingCall;
	}
}
