using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CF5 RID: 3317
	public class CompBladelinkWeapon : ThingComp
	{
		// Token: 0x0600509E RID: 20638 RVA: 0x001B184C File Offset: 0x001AFA4C
		public override void Notify_Equipped(Pawn pawn)
		{
			if (!ModLister.RoyaltyInstalled)
			{
				Log.ErrorOnce("Persona weapons are a Royalty-specific game system. If you want to use this code please check ModLister.RoyaltyInstalled before calling it. See rules on the Ludeon forum for more info.", 988331, false);
				return;
			}
			if (pawn.IsColonistPlayerControlled && this.bondedPawn == null)
			{
				Find.LetterStack.ReceiveLetter("LetterBladelinkWeaponBondedLabel".Translate(pawn.Named("PAWN"), this.parent.Named("WEAPON")), "LetterBladelinkWeaponBonded".Translate(pawn.Named("PAWN"), this.parent.Named("WEAPON")).Resolve(), LetterDefOf.PositiveEvent, null);
			}
			this.bonded = true;
			this.bondedPawnLabel = pawn.Name.ToStringFull;
			this.bondedPawn = pawn;
		}

		// Token: 0x0600509F RID: 20639 RVA: 0x001B1908 File Offset: 0x001AFB08
		public override string CompInspectStringExtra()
		{
			if (this.bondedPawn == null)
			{
				return "NotBonded".Translate();
			}
			return "BondedWith".Translate(this.bondedPawnLabel.ApplyTag(TagType.Name, null)).Resolve();
		}

		// Token: 0x060050A0 RID: 20640 RVA: 0x001B1954 File Offset: 0x001AFB54
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<bool>(ref this.bonded, "bonded", false, false);
			Scribe_Values.Look<string>(ref this.bondedPawnLabel, "bondedPawnLabel", null, false);
			Scribe_References.Look<Pawn>(ref this.bondedPawn, "bondedPawn", true);
			if (Scribe.mode == LoadSaveMode.PostLoadInit && (string.IsNullOrEmpty(this.bondedPawnLabel) || !this.bonded) && this.bondedPawn != null)
			{
				this.bondedPawnLabel = this.bondedPawn.Name.ToStringFull;
				this.bonded = true;
			}
		}

		// Token: 0x060050A1 RID: 20641 RVA: 0x001B19DE File Offset: 0x001AFBDE
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			foreach (Faction faction in Find.FactionManager.AllFactions)
			{
				RoyalTitleDef minTitleToUse = ThingRequiringRoyalPermissionUtility.GetMinTitleToUse(this.parent.def, faction, 0);
				if (minTitleToUse != null)
				{
					yield return new StatDrawEntry(StatCategoryDefOf.BasicsNonPawnImportant, "Stat_Thing_MinimumRoyalTitle_Name".Translate(faction.Named("FACTION")).Resolve(), minTitleToUse.GetLabelCapForBothGenders(), "Stat_Thing_Weapon_MinimumRoyalTitle_Desc".Translate(faction.Named("FACTION")).Resolve(), 2100, null, null, false);
				}
			}
			IEnumerator<Faction> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x060050A2 RID: 20642 RVA: 0x00002681 File Offset: 0x00000881
		[Obsolete("Will be removed in the future")]
		public override void Notify_UsedWeapon(Pawn pawn)
		{
		}

		// Token: 0x04002CCA RID: 11466
		private bool bonded;

		// Token: 0x04002CCB RID: 11467
		private string bondedPawnLabel;

		// Token: 0x04002CCC RID: 11468
		public Pawn bondedPawn;
	}
}
