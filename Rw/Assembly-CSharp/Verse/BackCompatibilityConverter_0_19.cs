using System;
using System.Xml;
using RimWorld;

namespace Verse
{
	// Token: 0x020003F5 RID: 1013
	public class BackCompatibilityConverter_0_19 : BackCompatibilityConverter
	{
		// Token: 0x06001E26 RID: 7718 RVA: 0x000BB814 File Offset: 0x000B9A14
		public override bool AppliesToVersion(int majorVer, int minorVer)
		{
			return majorVer == 0 && minorVer <= 19;
		}

		// Token: 0x06001E27 RID: 7719 RVA: 0x00019EA1 File Offset: 0x000180A1
		public override string BackCompatibleDefName(Type defType, string defName, bool forDefInjections = false, XmlNode node = null)
		{
			return null;
		}

		// Token: 0x06001E28 RID: 7720 RVA: 0x00019EA1 File Offset: 0x000180A1
		public override Type GetBackCompatibleType(Type baseType, string providedClassName, XmlNode node)
		{
			return null;
		}

		// Token: 0x06001E29 RID: 7721 RVA: 0x000BB824 File Offset: 0x000B9A24
		public override void PostExposeData(object obj)
		{
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				Game game = obj as Game;
				if (game != null && game.foodRestrictionDatabase == null)
				{
					game.foodRestrictionDatabase = new FoodRestrictionDatabase();
				}
			}
			if (Scribe.mode == LoadSaveMode.PostLoadInit)
			{
				Pawn pawn = obj as Pawn;
				if (pawn != null && pawn.foodRestriction == null && pawn.RaceProps.Humanlike && ((pawn.Faction != null && pawn.Faction.IsPlayer) || (pawn.HostFaction != null && pawn.HostFaction.IsPlayer)))
				{
					pawn.foodRestriction = new Pawn_FoodRestrictionTracker(pawn);
				}
			}
		}
	}
}
