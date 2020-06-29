using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Verse;
using Verse.AI;
using Verse.Sound;

namespace RimWorld
{
	
	public class Targeter
	{
		
		
		public bool IsTargeting
		{
			get
			{
				return this.targetingSource != null || this.action != null;
			}
		}

		
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

		
		public void OrderPawnForceTarget(ITargetingSource targetingSource)
		{
			LocalTargetInfo target = this.CurrentTargetUnderMouse(true);
			if (!target.IsValid)
			{
				return;
			}
			targetingSource.OrderForceTarget(target);
		}

		
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

		
		private Verb GetTargetingVerb(Pawn pawn)
		{
			return pawn.equipment.AllEquipmentVerbs.FirstOrDefault((Verb x) => x.verbProps == this.targetingSource.GetVerb.verbProps);
		}

		
		public ITargetingSource targetingSource;

		
		public ITargetingSource targetingSourceParent;

		
		public List<Pawn> targetingSourceAdditionalPawns;

		
		private Action<LocalTargetInfo> action;

		
		private Pawn caster;

		
		private TargetingParameters targetParams;

		
		private Action actionWhenFinished;

		
		private Texture2D mouseAttachment;

		
		private bool needsStopTargetingCall;
	}
}
