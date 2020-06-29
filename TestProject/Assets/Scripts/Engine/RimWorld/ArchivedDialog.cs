using System;
using UnityEngine;
using Verse;

namespace RimWorld
{
	
	public class ArchivedDialog : IArchivable, IExposable, ILoadReferenceable
	{
		
		
		Texture IArchivable.ArchivedIcon
		{
			get
			{
				return null;
			}
		}

		
		
		Color IArchivable.ArchivedIconColor
		{
			get
			{
				return Color.white;
			}
		}

		
		
		string IArchivable.ArchivedLabel
		{
			get
			{
				return this.text.Flatten();
			}
		}

		
		
		string IArchivable.ArchivedTooltip
		{
			get
			{
				return this.text;
			}
		}

		
		
		int IArchivable.CreatedTicksGame
		{
			get
			{
				return this.createdTick;
			}
		}

		
		
		bool IArchivable.CanCullArchivedNow
		{
			get
			{
				return true;
			}
		}

		
		
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
