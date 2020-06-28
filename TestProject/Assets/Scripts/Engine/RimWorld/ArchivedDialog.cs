using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000926 RID: 2342
	public class ArchivedDialog : IArchivable, IExposable, ILoadReferenceable
	{
		// Token: 0x170009F0 RID: 2544
		// (get) Token: 0x060037A2 RID: 14242 RVA: 0x00019EA1 File Offset: 0x000180A1
		Texture IArchivable.ArchivedIcon
		{
			get
			{
				return null;
			}
		}

		// Token: 0x170009F1 RID: 2545
		// (get) Token: 0x060037A3 RID: 14243 RVA: 0x00017A00 File Offset: 0x00015C00
		Color IArchivable.ArchivedIconColor
		{
			get
			{
				return Color.white;
			}
		}

		// Token: 0x170009F2 RID: 2546
		// (get) Token: 0x060037A4 RID: 14244 RVA: 0x0012AD59 File Offset: 0x00128F59
		string IArchivable.ArchivedLabel
		{
			get
			{
				return this.text.Flatten();
			}
		}

		// Token: 0x170009F3 RID: 2547
		// (get) Token: 0x060037A5 RID: 14245 RVA: 0x0012AD66 File Offset: 0x00128F66
		string IArchivable.ArchivedTooltip
		{
			get
			{
				return this.text;
			}
		}

		// Token: 0x170009F4 RID: 2548
		// (get) Token: 0x060037A6 RID: 14246 RVA: 0x0012AD6E File Offset: 0x00128F6E
		int IArchivable.CreatedTicksGame
		{
			get
			{
				return this.createdTick;
			}
		}

		// Token: 0x170009F5 RID: 2549
		// (get) Token: 0x060037A7 RID: 14247 RVA: 0x0001028D File Offset: 0x0000E48D
		bool IArchivable.CanCullArchivedNow
		{
			get
			{
				return true;
			}
		}

		// Token: 0x170009F6 RID: 2550
		// (get) Token: 0x060037A8 RID: 14248 RVA: 0x00019EA1 File Offset: 0x000180A1
		LookTargets IArchivable.LookTargets
		{
			get
			{
				return null;
			}
		}

		// Token: 0x060037A9 RID: 14249 RVA: 0x0000F2A9 File Offset: 0x0000D4A9
		public ArchivedDialog()
		{
		}

		// Token: 0x060037AA RID: 14250 RVA: 0x0012AD78 File Offset: 0x00128F78
		public ArchivedDialog(string text, string title = null, Faction relatedFaction = null)
		{
			this.text = text;
			this.title = title;
			this.relatedFaction = relatedFaction;
			this.createdTick = GenTicks.TicksGame;
			if (Find.UniqueIDsManager != null)
			{
				this.ID = Find.UniqueIDsManager.GetNextArchivedDialogID();
				return;
			}
			this.ID = Rand.Int;
		}

		// Token: 0x060037AB RID: 14251 RVA: 0x0012ADD0 File Offset: 0x00128FD0
		void IArchivable.OpenArchived()
		{
			DiaNode diaNode = new DiaNode(this.text);
			DiaOption diaOption = new DiaOption("OK".Translate());
			diaOption.resolveTree = true;
			diaNode.options.Add(diaOption);
			Find.WindowStack.Add(new Dialog_NodeTreeWithFactionInfo(diaNode, this.relatedFaction, false, false, this.title));
		}

		// Token: 0x060037AC RID: 14252 RVA: 0x0012AE34 File Offset: 0x00129034
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ID, "ID", 0, false);
			Scribe_Values.Look<string>(ref this.text, "text", null, false);
			Scribe_Values.Look<string>(ref this.title, "title", null, false);
			Scribe_References.Look<Faction>(ref this.relatedFaction, "relatedFaction", false);
			Scribe_Values.Look<int>(ref this.createdTick, "createdTick", 0, false);
		}

		// Token: 0x060037AD RID: 14253 RVA: 0x0012AE9A File Offset: 0x0012909A
		public string GetUniqueLoadID()
		{
			return "ArchivedDialog_" + this.ID;
		}

		// Token: 0x040020F3 RID: 8435
		public int ID;

		// Token: 0x040020F4 RID: 8436
		public string text;

		// Token: 0x040020F5 RID: 8437
		public string title;

		// Token: 0x040020F6 RID: 8438
		public Faction relatedFaction;

		// Token: 0x040020F7 RID: 8439
		public int createdTick;
	}
}
