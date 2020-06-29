using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public abstract class MainTabWindow_PawnTable : MainTabWindow
	{
		
		// (get) Token: 0x06005C7C RID: 23676 RVA: 0x001FF25B File Offset: 0x001FD45B
		protected virtual float ExtraBottomSpace
		{
			get
			{
				return 53f;
			}
		}

		
		// (get) Token: 0x06005C7D RID: 23677 RVA: 0x0005AC15 File Offset: 0x00058E15
		protected virtual float ExtraTopSpace
		{
			get
			{
				return 0f;
			}
		}

		
		// (get) Token: 0x06005C7E RID: 23678
		protected abstract PawnTableDef PawnTableDef { get; }

		
		// (get) Token: 0x06005C7F RID: 23679 RVA: 0x001FF262 File Offset: 0x001FD462
		protected override float Margin
		{
			get
			{
				return 6f;
			}
		}

		
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

		
		// (get) Token: 0x06005C81 RID: 23681 RVA: 0x001FF2D4 File Offset: 0x001FD4D4
		protected virtual IEnumerable<Pawn> Pawns
		{
			get
			{
				return Find.CurrentMap.mapPawns.FreeColonists;
			}
		}

		
		public override void PostOpen()
		{
			if (this.table == null)
			{
				this.table = this.CreateTable();
			}
			this.SetDirty();
		}

		
		public override void DoWindowContents(Rect rect)
		{
			base.DoWindowContents(rect);
			this.table.PawnTableOnGUI(new Vector2(rect.x, rect.y + this.ExtraTopSpace));
		}

		
		public void Notify_PawnsChanged()
		{
			this.SetDirty();
		}

		
		public override void Notify_ResolutionChanged()
		{
			this.table = this.CreateTable();
			base.Notify_ResolutionChanged();
		}

		
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

		
		protected void SetDirty()
		{
			this.table.SetDirty();
			this.SetInitialSizeAndPosition();
		}

		
		private PawnTable table;
	}
}
