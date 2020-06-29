using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class ArchivedDialog : IArchivable, IExposable, ILoadReferenceable
	{
		
		// (get) Token: 0x060037A2 RID: 14242 RVA: 0x00019EA1 File Offset: 0x000180A1
		Texture IArchivable.ArchivedIcon
		{
			get
			{
				return null;
			}
		}

		
		// (get) Token: 0x060037A3 RID: 14243 RVA: 0x00017A00 File Offset: 0x00015C00
		Color IArchivable.ArchivedIconColor
		{
			get
			{
				return Color.white;
			}
		}

		
		// (get) Token: 0x060037A4 RID: 14244 RVA: 0x0012AD59 File Offset: 0x00128F59
		string IArchivable.ArchivedLabel
		{
			get
			{
				return this.text.Flatten();
			}
		}

		
		// (get) Token: 0x060037A5 RID: 14245 RVA: 0x0012AD66 File Offset: 0x00128F66
		string IArchivable.ArchivedTooltip
		{
			get
			{
				return this.text;
			}
		}

		
		// (get) Token: 0x060037A6 RID: 14246 RVA: 0x0012AD6E File Offset: 0x00128F6E
		int IArchivable.CreatedTicksGame
		{
			get
			{
				return this.createdTick;
			}
		}

		
		// (get) Token: 0x060037A7 RID: 14247 RVA: 0x0001028D File Offset: 0x0000E48D
		bool IArchivable.CanCullArchivedNow
		{
			get
			{
				return true;
			}
		}

		
		// (get) Token: 0x060037A8 RID: 14248 RVA: 0x00019EA1 File Offset: 0x000180A1
		LookTargets IArchivable.LookTargets
		{
			get
			{
				return null;
			}
		}

		
		public ArchivedDialog()
		{
		}

		
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

		
		void IArchivable.OpenArchived()
		{
			DiaNode diaNode = new DiaNode(this.text);
			DiaOption diaOption = new DiaOption("OK".Translate());
			diaOption.resolveTree = true;
			diaNode.options.Add(diaOption);
			Find.WindowStack.Add(new Dialog_NodeTreeWithFactionInfo(diaNode, this.relatedFaction, false, false, this.title));
		}

		
		public void ExposeData()
		{
			Scribe_Values.Look<int>(ref this.ID, "ID", 0, false);
			Scribe_Values.Look<string>(ref this.text, "text", null, false);
			Scribe_Values.Look<string>(ref this.title, "title", null, false);
			Scribe_References.Look<Faction>(ref this.relatedFaction, "relatedFaction", false);
			Scribe_Values.Look<int>(ref this.createdTick, "createdTick", 0, false);
		}

		
		public string GetUniqueLoadID()
		{
			return "ArchivedDialog_" + this.ID;
		}

		
		public int ID;

		
		public string text;

		
		public string title;

		
		public Faction relatedFaction;

		
		public int createdTick;
	}
}
