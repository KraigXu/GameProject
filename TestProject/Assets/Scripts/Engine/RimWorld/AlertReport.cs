using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	// Token: 0x02000DD3 RID: 3539
	public struct AlertReport
	{
		// Token: 0x17000F53 RID: 3923
		// (get) Token: 0x060055ED RID: 21997 RVA: 0x001C8148 File Offset: 0x001C6348
		public bool AnyCulpritValid
		{
			get
			{
				if (!this.culpritsThings.NullOrEmpty<Thing>() || !this.culpritsPawns.NullOrEmpty<Pawn>() || !this.culpritsCaravans.NullOrEmpty<Caravan>())
				{
					return true;
				}
				if (this.culpritTarget != null && this.culpritTarget.Value.IsValid)
				{
					return true;
				}
				if (this.culpritsTargets != null)
				{
					for (int i = 0; i < this.culpritsTargets.Count; i++)
					{
						if (this.culpritsTargets[i].IsValid)
						{
							return true;
						}
					}
				}
				return false;
			}
		}

		// Token: 0x17000F54 RID: 3924
		// (get) Token: 0x060055EE RID: 21998 RVA: 0x001C81D9 File Offset: 0x001C63D9
		public IEnumerable<GlobalTargetInfo> AllCulprits
		{
			get
			{
				if (this.culpritsThings != null)
				{
					int num;
					for (int i = 0; i < this.culpritsThings.Count; i = num + 1)
					{
						yield return this.culpritsThings[i];
						num = i;
					}
				}
				if (this.culpritsPawns != null)
				{
					int num;
					for (int i = 0; i < this.culpritsPawns.Count; i = num + 1)
					{
						yield return this.culpritsPawns[i];
						num = i;
					}
				}
				if (this.culpritsCaravans != null)
				{
					int num;
					for (int i = 0; i < this.culpritsCaravans.Count; i = num + 1)
					{
						yield return this.culpritsCaravans[i];
						num = i;
					}
				}
				if (this.culpritTarget != null)
				{
					yield return this.culpritTarget.Value;
				}
				if (this.culpritsTargets != null)
				{
					int num;
					for (int i = 0; i < this.culpritsTargets.Count; i = num + 1)
					{
						yield return this.culpritsTargets[i];
						num = i;
					}
				}
				yield break;
			}
		}

		// Token: 0x060055EF RID: 21999 RVA: 0x001C81F0 File Offset: 0x001C63F0
		public static AlertReport CulpritIs(GlobalTargetInfo culp)
		{
			AlertReport result = default(AlertReport);
			result.active = culp.IsValid;
			if (culp.IsValid)
			{
				result.culpritTarget = new GlobalTargetInfo?(culp);
			}
			return result;
		}

		// Token: 0x060055F0 RID: 22000 RVA: 0x001C822C File Offset: 0x001C642C
		public static AlertReport CulpritsAre(List<Thing> culprits)
		{
			AlertReport result = default(AlertReport);
			result.culpritsThings = culprits;
			result.active = result.AnyCulpritValid;
			return result;
		}

		// Token: 0x060055F1 RID: 22001 RVA: 0x001C8258 File Offset: 0x001C6458
		public static AlertReport CulpritsAre(List<Pawn> culprits)
		{
			AlertReport result = default(AlertReport);
			result.culpritsPawns = culprits;
			result.active = result.AnyCulpritValid;
			return result;
		}

		// Token: 0x060055F2 RID: 22002 RVA: 0x001C8284 File Offset: 0x001C6484
		public static AlertReport CulpritsAre(List<Caravan> culprits)
		{
			AlertReport result = default(AlertReport);
			result.culpritsCaravans = culprits;
			result.active = result.AnyCulpritValid;
			return result;
		}

		// Token: 0x060055F3 RID: 22003 RVA: 0x001C82B0 File Offset: 0x001C64B0
		public static AlertReport CulpritsAre(List<GlobalTargetInfo> culprits)
		{
			AlertReport result = default(AlertReport);
			result.culpritsTargets = culprits;
			result.active = result.AnyCulpritValid;
			return result;
		}

		// Token: 0x060055F4 RID: 22004 RVA: 0x001C82DC File Offset: 0x001C64DC
		public static implicit operator AlertReport(bool b)
		{
			return new AlertReport
			{
				active = b
			};
		}

		// Token: 0x060055F5 RID: 22005 RVA: 0x001C82FA File Offset: 0x001C64FA
		public static implicit operator AlertReport(Thing culprit)
		{
			return AlertReport.CulpritIs(culprit);
		}

		// Token: 0x060055F6 RID: 22006 RVA: 0x001C8307 File Offset: 0x001C6507
		public static implicit operator AlertReport(WorldObject culprit)
		{
			return AlertReport.CulpritIs(culprit);
		}

		// Token: 0x060055F7 RID: 22007 RVA: 0x001C8314 File Offset: 0x001C6514
		public static implicit operator AlertReport(GlobalTargetInfo culprit)
		{
			return AlertReport.CulpritIs(culprit);
		}

		// Token: 0x17000F55 RID: 3925
		// (get) Token: 0x060055F8 RID: 22008 RVA: 0x001C831C File Offset: 0x001C651C
		public static AlertReport Active
		{
			get
			{
				return new AlertReport
				{
					active = true
				};
			}
		}

		// Token: 0x17000F56 RID: 3926
		// (get) Token: 0x060055F9 RID: 22009 RVA: 0x001C833C File Offset: 0x001C653C
		public static AlertReport Inactive
		{
			get
			{
				return new AlertReport
				{
					active = false
				};
			}
		}

		// Token: 0x04002F05 RID: 12037
		public bool active;

		// Token: 0x04002F06 RID: 12038
		public List<Thing> culpritsThings;

		// Token: 0x04002F07 RID: 12039
		public List<Pawn> culpritsPawns;

		// Token: 0x04002F08 RID: 12040
		public List<Caravan> culpritsCaravans;

		// Token: 0x04002F09 RID: 12041
		public List<GlobalTargetInfo> culpritsTargets;

		// Token: 0x04002F0A RID: 12042
		public GlobalTargetInfo? culpritTarget;
	}
}
