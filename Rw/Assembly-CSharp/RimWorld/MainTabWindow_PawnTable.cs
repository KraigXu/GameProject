using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000EC7 RID: 3783
	public abstract class MainTabWindow_PawnTable : MainTabWindow
	{
		// Token: 0x170010BB RID: 4283
		// (get) Token: 0x06005C7C RID: 23676 RVA: 0x001FF25B File Offset: 0x001FD45B
		protected virtual float ExtraBottomSpace
		{
			get
			{
				return 53f;
			}
		}

		// Token: 0x170010BC RID: 4284
		// (get) Token: 0x06005C7D RID: 23677 RVA: 0x0005AC15 File Offset: 0x00058E15
		protected virtual float ExtraTopSpace
		{
			get
			{
				return 0f;
			}
		}

		// Token: 0x170010BD RID: 4285
		// (get) Token: 0x06005C7E RID: 23678
		protected abstract PawnTableDef PawnTableDef { get; }

		// Token: 0x170010BE RID: 4286
		// (get) Token: 0x06005C7F RID: 23679 RVA: 0x001FF262 File Offset: 0x001FD462
		protected override float Margin
		{
			get
			{
				return 6f;
			}
		}

		// Token: 0x170010BF RID: 4287
		// (get) Token: 0x06005C80 RID: 23680 RVA: 0x001FF26C File Offset: 0x001FD46C
		public override Vector2 RequestedTabSize
		{
			get
			{
				if (this.table == null)
				{
					return Vector2.zero;
				}
				return new Vector2(this.table.Size.x + this.Margin * 2f, this.table.Size.y + this.ExtraBottomSpace + this.ExtraTopSpace + this.Margin * 2f);
			}
		}

		// Token: 0x170010C0 RID: 4288
		// (get) Token: 0x06005C81 RID: 23681 RVA: 0x001FF2D4 File Offset: 0x001FD4D4
		protected virtual IEnumerable<Pawn> Pawns
		{
			get
			{
				return Find.CurrentMap.mapPawns.FreeColonists;
			}
		}

		// Token: 0x06005C82 RID: 23682 RVA: 0x001FF2E5 File Offset: 0x001FD4E5
		public override void PostOpen()
		{
			if (this.table == null)
			{
				this.table = this.CreateTable();
			}
			this.SetDirty();
		}

		// Token: 0x06005C83 RID: 23683 RVA: 0x001FF301 File Offset: 0x001FD501
		public override void DoWindowContents(Rect rect)
		{
			base.DoWindowContents(rect);
			this.table.PawnTableOnGUI(new Vector2(rect.x, rect.y + this.ExtraTopSpace));
		}

		// Token: 0x06005C84 RID: 23684 RVA: 0x001FF32F File Offset: 0x001FD52F
		public void Notify_PawnsChanged()
		{
			this.SetDirty();
		}

		// Token: 0x06005C85 RID: 23685 RVA: 0x001FF337 File Offset: 0x001FD537
		public override void Notify_ResolutionChanged()
		{
			this.table = this.CreateTable();
			base.Notify_ResolutionChanged();
		}

		// Token: 0x06005C86 RID: 23686 RVA: 0x001FF34C File Offset: 0x001FD54C
		private PawnTable CreateTable()
		{
			return (PawnTable)Activator.CreateInstance(this.PawnTableDef.workerClass, new object[]
			{
				this.PawnTableDef,
				new Func<IEnumerable<Pawn>>(() => this.Pawns),
				UI.screenWidth - (int)(this.Margin * 2f),
				(int)((float)(UI.screenHeight - 35) - this.ExtraBottomSpace - this.ExtraTopSpace - this.Margin * 2f)
			});
		}

		// Token: 0x06005C87 RID: 23687 RVA: 0x001FF3D4 File Offset: 0x001FD5D4
		protected void SetDirty()
		{
			this.table.SetDirty();
			this.SetInitialSizeAndPosition();
		}

		// Token: 0x0400326A RID: 12906
		private PawnTable table;
	}
}
