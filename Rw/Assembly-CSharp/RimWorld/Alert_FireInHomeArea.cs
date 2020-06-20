using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DD9 RID: 3545
	public class Alert_FireInHomeArea : Alert_Critical
	{
		// Token: 0x06005611 RID: 22033 RVA: 0x001C899D File Offset: 0x001C6B9D
		public Alert_FireInHomeArea()
		{
			this.defaultLabel = "FireInHomeArea".Translate();
			this.defaultExplanation = "FireInHomeAreaDesc".Translate();
		}

		// Token: 0x17000F5C RID: 3932
		// (get) Token: 0x06005612 RID: 22034 RVA: 0x001C89D0 File Offset: 0x001C6BD0
		private Fire FireInHomeArea
		{
			get
			{
				List<Map> maps = Find.Maps;
				for (int i = 0; i < maps.Count; i++)
				{
					List<Thing> list = maps[i].listerThings.ThingsOfDef(ThingDefOf.Fire);
					for (int j = 0; j < list.Count; j++)
					{
						Thing thing = list[j];
						if (maps[i].areaManager.Home[thing.Position] && !thing.Position.Fogged(thing.Map))
						{
							return (Fire)thing;
						}
					}
				}
				return null;
			}
		}

		// Token: 0x06005613 RID: 22035 RVA: 0x001C8A62 File Offset: 0x001C6C62
		public override AlertReport GetReport()
		{
			return this.FireInHomeArea;
		}
	}
}
