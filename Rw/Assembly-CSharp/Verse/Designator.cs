using System;
using System.Collections.Generic;
using RimWorld;
using UnityEngine;
using Verse.Sound;

namespace Verse
{
	// Token: 0x0200032E RID: 814
	public abstract class Designator : Command
	{
		// Token: 0x170004E7 RID: 1255
		// (get) Token: 0x060017CE RID: 6094 RVA: 0x000885D0 File Offset: 0x000867D0
		public Map Map
		{
			get
			{
				return Find.CurrentMap;
			}
		}

		// Token: 0x170004E8 RID: 1256
		// (get) Token: 0x060017CF RID: 6095 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual int DraggableDimensions
		{
			get
			{
				return 0;
			}
		}

		// Token: 0x170004E9 RID: 1257
		// (get) Token: 0x060017D0 RID: 6096 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool DragDrawMeasurements
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170004EA RID: 1258
		// (get) Token: 0x060017D1 RID: 6097 RVA: 0x00010306 File Offset: 0x0000E506
		protected override bool DoTooltip
		{
			get
			{
				return false;
			}
		}

		// Token: 0x170004EB RID: 1259
		// (get) Token: 0x060017D2 RID: 6098 RVA: 0x00019EA1 File Offset: 0x000180A1
		protected virtual DesignationDef Designation
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170004EC RID: 1260
		// (get) Token: 0x060017D3 RID: 6099 RVA: 0x0005AC15 File Offset: 0x00058E15
		public virtual float PanelReadoutTitleExtraRightMargin
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x170004ED RID: 1261
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

		// Token: 0x170004EE RID: 1262
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

		// Token: 0x170004EF RID: 1263
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

		// Token: 0x170004F0 RID: 1264
		// (get) Token: 0x060017D7 RID: 6103 RVA: 0x00088665 File Offset: 0x00086865
		public override IEnumerable<FloatMenuOption> RightClickFloatMenuOptions
		{
			get
			{
				foreach (FloatMenuOption floatMenuOption in this.<>n__0())
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

		// Token: 0x060017D8 RID: 6104 RVA: 0x00088675 File Offset: 0x00086875
		public Designator()
		{
			this.activateSound = SoundDefOf.Tick_Tiny;
			this.designateAllLabel = "DesignateAll".Translate();
		}

		// Token: 0x060017D9 RID: 6105 RVA: 0x000886A8 File Offset: 0x000868A8
		protected bool CheckCanInteract()
		{
			return !TutorSystem.TutorialMode || TutorSystem.AllowAction(this.TutorTagSelect);
		}

		// Token: 0x060017DA RID: 6106 RVA: 0x000886C6 File Offset: 0x000868C6
		public override void ProcessInput(Event ev)
		{
			if (!this.CheckCanInteract())
			{
				return;
			}
			base.ProcessInput(ev);
			Find.DesignatorManager.Select(this);
		}

		// Token: 0x060017DB RID: 6107 RVA: 0x000886E3 File Offset: 0x000868E3
		public virtual AcceptanceReport CanDesignateThing(Thing t)
		{
			return AcceptanceReport.WasRejected;
		}

		// Token: 0x060017DC RID: 6108 RVA: 0x000255BF File Offset: 0x000237BF
		public virtual void DesignateThing(Thing t)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060017DD RID: 6109
		public abstract AcceptanceReport CanDesignateCell(IntVec3 loc);

		// Token: 0x060017DE RID: 6110 RVA: 0x000886EC File Offset: 0x000868EC
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

		// Token: 0x060017DF RID: 6111 RVA: 0x000255BF File Offset: 0x000237BF
		public virtual void DesignateSingleCell(IntVec3 c)
		{
			throw new NotImplementedException();
		}

		// Token: 0x060017E0 RID: 6112 RVA: 0x00010306 File Offset: 0x0000E506
		public virtual bool ShowWarningForCell(IntVec3 c)
		{
			return false;
		}

		// Token: 0x060017E1 RID: 6113 RVA: 0x00088794 File Offset: 0x00086994
		public void Finalize(bool somethingSucceeded)
		{
			if (somethingSucceeded)
			{
				this.FinalizeDesignationSucceeded();
				return;
			}
			this.FinalizeDesignationFailed();
		}

		// Token: 0x060017E2 RID: 6114 RVA: 0x000887A6 File Offset: 0x000869A6
		protected virtual void FinalizeDesignationSucceeded()
		{
			if (this.soundSucceeded != null)
			{
				this.soundSucceeded.PlayOneShotOnCamera(null);
			}
		}

		// Token: 0x060017E3 RID: 6115 RVA: 0x000887BC File Offset: 0x000869BC
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

		// Token: 0x060017E4 RID: 6116 RVA: 0x00088808 File Offset: 0x00086A08
		public virtual string LabelCapReverseDesignating(Thing t)
		{
			return this.LabelCap;
		}

		// Token: 0x060017E5 RID: 6117 RVA: 0x00088810 File Offset: 0x00086A10
		public virtual string DescReverseDesignating(Thing t)
		{
			return this.Desc;
		}

		// Token: 0x060017E6 RID: 6118 RVA: 0x00088818 File Offset: 0x00086A18
		public virtual Texture2D IconReverseDesignating(Thing t, out float angle, out Vector2 offset)
		{
			angle = this.iconAngle;
			offset = this.iconOffset;
			return this.icon;
		}

		// Token: 0x060017E7 RID: 6119 RVA: 0x0001028D File Offset: 0x0000E48D
		protected virtual bool RemoveAllDesignationsAffects(LocalTargetInfo target)
		{
			return true;
		}

		// Token: 0x060017E8 RID: 6120 RVA: 0x00088834 File Offset: 0x00086A34
		public virtual void DrawMouseAttachments()
		{
			if (this.useMouseIcon)
			{
				GenUI.DrawMouseAttachment(this.icon, "", this.iconAngle, this.iconOffset, null, false, default(Color));
			}
		}

		// Token: 0x060017E9 RID: 6121 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void DrawPanelReadout(ref float curY, float width)
		{
		}

		// Token: 0x060017EA RID: 6122 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void DoExtraGuiControls(float leftX, float bottomY)
		{
		}

		// Token: 0x060017EB RID: 6123 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void SelectedUpdate()
		{
		}

		// Token: 0x060017EC RID: 6124 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void SelectedProcessInput(Event ev)
		{
		}

		// Token: 0x060017ED RID: 6125 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void Rotate(RotationDirection rotDir)
		{
		}

		// Token: 0x060017EE RID: 6126 RVA: 0x0001028D File Offset: 0x0000E48D
		public virtual bool CanRemainSelected()
		{
			return true;
		}

		// Token: 0x060017EF RID: 6127 RVA: 0x00002681 File Offset: 0x00000881
		public virtual void Selected()
		{
		}

		// Token: 0x060017F0 RID: 6128 RVA: 0x00088878 File Offset: 0x00086A78
		public virtual void RenderHighlight(List<IntVec3> dragCells)
		{
			DesignatorUtility.RenderHighlightOverSelectableThings(this, dragCells);
		}

		// Token: 0x04000EF4 RID: 3828
		protected bool useMouseIcon;

		// Token: 0x04000EF5 RID: 3829
		public bool isOrder;

		// Token: 0x04000EF6 RID: 3830
		public SoundDef soundDragSustain;

		// Token: 0x04000EF7 RID: 3831
		public SoundDef soundDragChanged;

		// Token: 0x04000EF8 RID: 3832
		protected SoundDef soundSucceeded;

		// Token: 0x04000EF9 RID: 3833
		protected SoundDef soundFailed = SoundDefOf.Designate_Failed;

		// Token: 0x04000EFA RID: 3834
		protected bool hasDesignateAllFloatMenuOption;

		// Token: 0x04000EFB RID: 3835
		protected string designateAllLabel;

		// Token: 0x04000EFC RID: 3836
		private string cachedTutorTagSelect;

		// Token: 0x04000EFD RID: 3837
		private string cachedTutorTagDesignate;

		// Token: 0x04000EFE RID: 3838
		protected string cachedHighlightTag;
	}
}
