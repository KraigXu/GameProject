using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	
	public abstract class Designator : Command
	{
		
		// (get) Token: 0x060017CE RID: 6094 RVA: 0x000885D0 File Offset: 0x000867D0
		public Map Map
		{
			get
			{
				return Find.CurrentMap;
			}
		}

		
		// (get) Token: 0x060017CF RID: 6095 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual int DraggableDimensions
		{
			get
			{
				return 0;
			}
		}

		
		// (get) Token: 0x060017D0 RID: 6096 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool DragDrawMeasurements
		{
			get
			{
				return false;
			}
		}

		
		// (get) Token: 0x060017D1 RID: 6097 RVA: 0x00010306 File Offset: 0x0000E506
		protected override bool DoTooltip
		{
			get
			{
				return false;
			}
		}

		
		// (get) Token: 0x060017D2 RID: 6098 RVA: 0x00019EA1 File Offset: 0x000180A1
		protected virtual DesignationDef Designation
		{
			get
			{
				return null;
			}
		}

		
		// (get) Token: 0x060017D3 RID: 6099 RVA: 0x0005AC15 File Offset: 0x00058E15
		public virtual float PanelReadoutTitleExtraRightMargin
		{
			get
			{
				return 0f;
			}
		}

		
		// (get) Token: 0x060017D4 RID: 6100 RVA: 0x000885D7 File Offset: 0x000867D7
		public override string TutorTagSelect
		{
			get
			{
				if (this.tutorTag == null)
				{
					return null;
				}
				if (this.cachedTutorTagSelect == null)
				{
					this.cachedTutorTagSelect = "SelectDesignator-" + this.tutorTag;
				}
				return this.cachedTutorTagSelect;
			}
		}

		
		// (get) Token: 0x060017D5 RID: 6101 RVA: 0x00088607 File Offset: 0x00086807
		public string TutorTagDesignate
		{
			get
			{
				if (this.tutorTag == null)
				{
					return null;
				}
				if (this.cachedTutorTagDesignate == null)
				{
					this.cachedTutorTagDesignate = "Designate-" + this.tutorTag;
				}
				return this.cachedTutorTagDesignate;
			}
		}

		
		// (get) Token: 0x060017D6 RID: 6102 RVA: 0x00088637 File Offset: 0x00086837
		public override string HighlightTag
		{
			get
			{
				if (this.cachedHighlightTag == null && this.tutorTag != null)
				{
					this.cachedHighlightTag = "Designator-" + this.tutorTag;
				}
				return this.cachedHighlightTag;
			}
		}

		
		// (get) Token: 0x060017D7 RID: 6103 RVA: 0x00088665 File Offset: 0x00086865
		public override IEnumerable<FloatMenuOption> RightClickFloatMenuOptions
		{
			get
			{
				foreach (FloatMenuOption floatMenuOption in this.n__0())
				{
					yield return floatMenuOption;
				}
				IEnumerator<FloatMenuOption> enumerator = null;
				if (this.hasDesignateAllFloatMenuOption)
				{
					int num = 0;
					List<Thing> things = this.Map.listerThings.AllThings;
					for (int i = 0; i < things.Count; i++)
					{
						Thing t = things[i];
						if (!t.Fogged() && this.CanDesignateThing(t).Accepted)
						{
							num++;
						}
					}
					if (num > 0)
					{
						yield return new FloatMenuOption(this.designateAllLabel + " (" + "CountToDesignate".Translate(num) + ")", delegate
						{
							for (int k = 0; k < things.Count; k++)
							{
								Thing t2 = things[k];
								if (!t2.Fogged() && this.CanDesignateThing(t2).Accepted)
								{
									this.DesignateThing(things[k]);
								}
							}
						}, MenuOptionPriority.Default, null, null, 0f, null, null);
					}
					else
					{
						yield return new FloatMenuOption(this.designateAllLabel + " (" + "NoneLower".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null);
					}
				}
				DesignationDef designation = this.Designation;
				if (this.Designation != null)
				{
					int num2 = 0;
					List<Designation> designations = this.Map.designationManager.allDesignations;
					for (int j = 0; j < designations.Count; j++)
					{
						if (designations[j].def == designation && this.RemoveAllDesignationsAffects(designations[j].target))
						{
							num2++;
						}
					}
					if (num2 > 0)
					{
						yield return new FloatMenuOption("RemoveAllDesignations".Translate() + " (" + num2 + ")", delegate
						{
							for (int k = designations.Count - 1; k >= 0; k--)
							{
								if (designations[k].def == designation && this.RemoveAllDesignationsAffects(designations[k].target))
								{
									this.Map.designationManager.RemoveDesignation(designations[k]);
								}
							}
						}, MenuOptionPriority.Default, null, null, 0f, null, null);
					}
					else
					{
						yield return new FloatMenuOption("RemoveAllDesignations".Translate() + " (" + "NoneLower".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null);
					}
				}
				yield break;
				yield break;
			}
		}

		
		public Designator()
		{
			this.activateSound = SoundDefOf.Tick_Tiny;
			this.designateAllLabel = "DesignateAll".Translate();
		}

		
		protected bool CheckCanInteract()
		{
			return !TutorSystem.TutorialMode || TutorSystem.AllowAction(this.TutorTagSelect);
		}

		
		public override void ProcessInput(Event ev)
		{
			if (!this.CheckCanInteract())
			{
				return;
			}
			base.ProcessInput(ev);
			Find.DesignatorManager.Select(this);
		}

		
		public virtual AcceptanceReport CanDesignateThing(Thing t)
		{
			return AcceptanceReport.WasRejected;
		}

		
		public virtual void DesignateThing(Thing t)
		{
			throw new NotImplementedException();
		}

		
		public abstract AcceptanceReport CanDesignateCell(IntVec3 loc);

		
		public virtual void DesignateMultiCell(IEnumerable<IntVec3> cells)
		{
			if (TutorSystem.TutorialMode && !TutorSystem.AllowAction(new EventPack(this.TutorTagDesignate, cells)))
			{
				return;
			}
			bool somethingSucceeded = false;
			bool flag = false;
			foreach (IntVec3 intVec in cells)
			{
				if (this.CanDesignateCell(intVec).Accepted)
				{
					this.DesignateSingleCell(intVec);
					somethingSucceeded = true;
					if (!flag)
					{
						flag = this.ShowWarningForCell(intVec);
					}
				}
			}
			this.Finalize(somethingSucceeded);
			if (TutorSystem.TutorialMode)
			{
				TutorSystem.Notify_Event(new EventPack(this.TutorTagDesignate, cells));
			}
		}

		
		public virtual void DesignateSingleCell(IntVec3 c)
		{
			throw new NotImplementedException();
		}

		
		public virtual bool ShowWarningForCell(IntVec3 c)
		{
			return false;
		}

		
		public void Finalize(bool somethingSucceeded)
		{
			if (somethingSucceeded)
			{
				this.FinalizeDesignationSucceeded();
				return;
			}
			this.FinalizeDesignationFailed();
		}

		
		protected virtual void FinalizeDesignationSucceeded()
		{
			if (this.soundSucceeded != null)
			{
				this.soundSucceeded.PlayOneShotOnCamera(null);
			}
		}

		
		protected virtual void FinalizeDesignationFailed()
		{
			if (this.soundFailed != null)
			{
				this.soundFailed.PlayOneShotOnCamera(null);
			}
			if (Find.DesignatorManager.Dragger.FailureReason != null)
			{
				Messages.Message(Find.DesignatorManager.Dragger.FailureReason, MessageTypeDefOf.RejectInput, false);
			}
		}

		
		public virtual string LabelCapReverseDesignating(Thing t)
		{
			return this.LabelCap;
		}

		
		public virtual string DescReverseDesignating(Thing t)
		{
			return this.Desc;
		}

		
		public virtual Texture2D IconReverseDesignating(Thing t, out float angle, out Vector2 offset)
		{
			angle = this.iconAngle;
			offset = this.iconOffset;
			return this.icon;
		}

		
		protected virtual bool RemoveAllDesignationsAffects(LocalTargetInfo target)
		{
			return true;
		}

		
		public virtual void DrawMouseAttachments()
		{
			if (this.useMouseIcon)
			{
				GenUI.DrawMouseAttachment(this.icon, "", this.iconAngle, this.iconOffset, null, false, default(Color));
			}
		}

		
		public virtual void DrawPanelReadout(ref float curY, float width)
		{
		}

		
		public virtual void DoExtraGuiControls(float leftX, float bottomY)
		{
		}

		
		public virtual void SelectedUpdate()
		{
		}

		
		public virtual void SelectedProcessInput(Event ev)
		{
		}

		
		public virtual void Rotate(RotationDirection rotDir)
		{
		}

		
		public virtual bool CanRemainSelected()
		{
			return true;
		}

		
		public virtual void Selected()
		{
		}

		
		public virtual void RenderHighlight(List<IntVec3> dragCells)
		{
			DesignatorUtility.RenderHighlightOverSelectableThings(this, dragCells);
		}

		
		protected bool useMouseIcon;

		
		public bool isOrder;

		
		public SoundDef soundDragSustain;

		
		public SoundDef soundDragChanged;

		
		protected SoundDef soundSucceeded;

		
		protected SoundDef soundFailed = SoundDefOf.Designate_Failed;

		
		protected bool hasDesignateAllFloatMenuOption;

		
		protected string designateAllLabel;

		
		private string cachedTutorTagSelect;

		
		private string cachedTutorTagDesignate;

		
		protected string cachedHighlightTag;
	}
}
