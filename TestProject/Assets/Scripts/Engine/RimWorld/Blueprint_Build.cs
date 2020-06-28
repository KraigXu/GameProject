using System;
using System.Collections.Generic;
using System.Text;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C4B RID: 3147
	public class Blueprint_Build : Blueprint
	{
		// Token: 0x17000D31 RID: 3377
		// (get) Token: 0x06004B18 RID: 19224 RVA: 0x001956E4 File Offset: 0x001938E4
		public override string Label
		{
			get
			{
				string label = this.def.entityDefToBuild.label;
				if (this.stuffToUse != null)
				{
					return "ThingMadeOfStuffLabel".Translate(this.stuffToUse.LabelAsStuff, label) + "BlueprintLabelExtra".Translate();
				}
				return label + "BlueprintLabelExtra".Translate();
			}
		}

		// Token: 0x17000D32 RID: 3378
		// (get) Token: 0x06004B19 RID: 19225 RVA: 0x00195754 File Offset: 0x00193954
		protected override float WorkTotal
		{
			get
			{
				return this.def.entityDefToBuild.GetStatValueAbstract(StatDefOf.WorkToBuild, this.stuffToUse);
			}
		}

		// Token: 0x06004B1A RID: 19226 RVA: 0x00195771 File Offset: 0x00193971
		public override void ExposeData()
		{
			base.ExposeData();
			Scribe_Defs.Look<ThingDef>(ref this.stuffToUse, "stuffToUse");
		}

		// Token: 0x06004B1B RID: 19227 RVA: 0x00195789 File Offset: 0x00193989
		public override ThingDef EntityToBuildStuff()
		{
			return this.stuffToUse;
		}

		// Token: 0x06004B1C RID: 19228 RVA: 0x00195791 File Offset: 0x00193991
		public override List<ThingDefCountClass> MaterialsNeeded()
		{
			return this.def.entityDefToBuild.CostListAdjusted(this.stuffToUse, true);
		}

		// Token: 0x06004B1D RID: 19229 RVA: 0x001957AA File Offset: 0x001939AA
		protected override Thing MakeSolidThing()
		{
			return ThingMaker.MakeThing(this.def.entityDefToBuild.frameDef, this.stuffToUse);
		}

		// Token: 0x06004B1E RID: 19230 RVA: 0x001957C7 File Offset: 0x001939C7
		public override IEnumerable<Gizmo> GetGizmos()
		{
			foreach (Gizmo gizmo in this.<>n__0())
			{
				yield return gizmo;
			}
			IEnumerator<Gizmo> enumerator = null;
			Command command = BuildCopyCommandUtility.BuildCopyCommand(this.def.entityDefToBuild, this.stuffToUse);
			if (command != null)
			{
				yield return command;
			}
			if (base.Faction == Faction.OfPlayer)
			{
				foreach (Command command2 in BuildFacilityCommandUtility.BuildFacilityCommands(this.def.entityDefToBuild))
				{
					yield return command2;
				}
				IEnumerator<Command> enumerator2 = null;
			}
			yield break;
			yield break;
		}

		// Token: 0x06004B1F RID: 19231 RVA: 0x001957D8 File Offset: 0x001939D8
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			if (stringBuilder.Length > 0)
			{
				stringBuilder.AppendLine();
			}
			stringBuilder.AppendLine("ContainedResources".Translate() + ":");
			bool flag = true;
			foreach (ThingDefCountClass thingDefCountClass in this.MaterialsNeeded())
			{
				if (!flag)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.Append(thingDefCountClass.thingDef.LabelCap + ": 0 / " + thingDefCountClass.count);
				flag = false;
			}
			return stringBuilder.ToString().Trim();
		}

		// Token: 0x04002A7F RID: 10879
		public ThingDef stuffToUse;
	}
}
