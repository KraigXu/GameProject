using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000BB9 RID: 3001
	[StaticConstructorOnStartup]
	public static class HostilityResponseModeUtility
	{
		// Token: 0x060046E6 RID: 18150 RVA: 0x0017FD3F File Offset: 0x0017DF3F
		public static Texture2D GetIcon(this HostilityResponseMode response)
		{
			switch (response)
			{
			case HostilityResponseMode.Ignore:
				return HostilityResponseModeUtility.IgnoreIcon;
			case HostilityResponseMode.Attack:
				return HostilityResponseModeUtility.AttackIcon;
			case HostilityResponseMode.Flee:
				return HostilityResponseModeUtility.FleeIcon;
			default:
				return BaseContent.BadTex;
			}
		}

		// Token: 0x060046E7 RID: 18151 RVA: 0x0017FD6C File Offset: 0x0017DF6C
		public static HostilityResponseMode GetNextResponse(Pawn pawn)
		{
			switch (pawn.playerSettings.hostilityResponse)
			{
			case HostilityResponseMode.Ignore:
				if (pawn.WorkTagIsDisabled(WorkTags.Violent))
				{
					return HostilityResponseMode.Flee;
				}
				return HostilityResponseMode.Attack;
			case HostilityResponseMode.Attack:
				return HostilityResponseMode.Flee;
			case HostilityResponseMode.Flee:
				return HostilityResponseMode.Ignore;
			default:
				return HostilityResponseMode.Ignore;
			}
		}

		// Token: 0x060046E8 RID: 18152 RVA: 0x0017FDAB File Offset: 0x0017DFAB
		public static string GetLabel(this HostilityResponseMode response)
		{
			return ("HostilityResponseMode_" + response).Translate();
		}

		// Token: 0x060046E9 RID: 18153 RVA: 0x0017FDC8 File Offset: 0x0017DFC8
		public static void DrawResponseButton(Rect rect, Pawn pawn, bool paintable)
		{
			Widgets.Dropdown<Pawn, HostilityResponseMode>(rect, pawn, HostilityResponseModeUtility.IconColor, new Func<Pawn, HostilityResponseMode>(HostilityResponseModeUtility.DrawResponseButton_GetResponse), new Func<Pawn, IEnumerable<Widgets.DropdownMenuElement<HostilityResponseMode>>>(HostilityResponseModeUtility.DrawResponseButton_GenerateMenu), null, pawn.playerSettings.hostilityResponse.GetIcon(), null, null, delegate
			{
				PlayerKnowledgeDatabase.KnowledgeDemonstrated(ConceptDefOf.HostilityResponse, KnowledgeAmount.SpecificInteraction);
			}, paintable);
			UIHighlighter.HighlightOpportunity(rect, "HostilityResponse");
			if (Mouse.IsOver(rect))
			{
				TooltipHandler.TipRegion(rect, "HostilityReponseTip".Translate() + "\n\n" + "HostilityResponseCurrentMode".Translate() + ": " + pawn.playerSettings.hostilityResponse.GetLabel());
			}
		}

		// Token: 0x060046EA RID: 18154 RVA: 0x0017FE8C File Offset: 0x0017E08C
		private static HostilityResponseMode DrawResponseButton_GetResponse(Pawn pawn)
		{
			return pawn.playerSettings.hostilityResponse;
		}

		// Token: 0x060046EB RID: 18155 RVA: 0x0017FE99 File Offset: 0x0017E099
		private static IEnumerable<Widgets.DropdownMenuElement<HostilityResponseMode>> DrawResponseButton_GenerateMenu(Pawn p)
		{
			using (IEnumerator enumerator = Enum.GetValues(typeof(HostilityResponseMode)).GetEnumerator())
			{
				while (enumerator.MoveNext())
				{
					HostilityResponseMode response = (HostilityResponseMode)enumerator.Current;
					if (response != HostilityResponseMode.Attack || !p.WorkTagIsDisabled(WorkTags.Violent))
					{
						yield return new Widgets.DropdownMenuElement<HostilityResponseMode>
						{
							option = new FloatMenuOption(response.GetLabel(), delegate
							{
								p.playerSettings.hostilityResponse = response;
							}, response.GetIcon(), Color.white, MenuOptionPriority.Default, null, null, 0f, null, null),
							payload = response
						};
					}
				}
			}
			IEnumerator enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x040028AF RID: 10415
		private static readonly Texture2D IgnoreIcon = ContentFinder<Texture2D>.Get("UI/Icons/HostilityResponse/Ignore", true);

		// Token: 0x040028B0 RID: 10416
		private static readonly Texture2D AttackIcon = ContentFinder<Texture2D>.Get("UI/Icons/HostilityResponse/Attack", true);

		// Token: 0x040028B1 RID: 10417
		private static readonly Texture2D FleeIcon = ContentFinder<Texture2D>.Get("UI/Icons/HostilityResponse/Flee", true);

		// Token: 0x040028B2 RID: 10418
		private static readonly Color IconColor = new Color(0.84f, 0.84f, 0.84f);
	}
}
