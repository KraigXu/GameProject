using System;
using System.Collections.Generic;
using RimWorld.Planet;
using Verse;

namespace RimWorld
{
	
	public struct AlertReport
	{
		
		
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

		
		public static AlertReport CulpritsAre(List<Thing> culprits)
		{
			AlertReport result = default(AlertReport);
			result.culpritsThings = culprits;
			result.active = result.AnyCulpritValid;
			return result;
		}

		
		public static AlertReport CulpritsAre(List<Pawn> culprits)
		{
			AlertReport result = default(AlertReport);
			result.culpritsPawns = culprits;
			result.active = result.AnyCulpritValid;
			return result;
		}

		
		public static AlertReport CulpritsAre(List<Caravan> culprits)
		{
			AlertReport result = default(AlertReport);
			result.culpritsCaravans = culprits;
			result.active = result.AnyCulpritValid;
			return result;
		}

		
		public static AlertReport CulpritsAre(List<GlobalTargetInfo> culprits)
		{
			AlertReport result = default(AlertReport);
			result.culpritsTargets = culprits;
			result.active = result.AnyCulpritValid;
			return result;
		}

		
		public static implicit operator AlertReport(bool b)
		{
			return new AlertReport
			{
				active = b
			};
		}

		
		public static implicit operator AlertReport(Thing culprit)
		{
			return AlertReport.CulpritIs(culprit);
		}

		
		public static implicit operator AlertReport(WorldObject culprit)
		{
			return AlertReport.CulpritIs(culprit);
		}

		
		public static implicit operator AlertReport(GlobalTargetInfo culprit)
		{
			return AlertReport.CulpritIs(culprit);
		}

		
		
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

		
		public bool active;

		
		public List<Thing> culpritsThings;

		
		public List<Pawn> culpritsPawns;

		
		public List<Caravan> culpritsCaravans;

		
		public List<GlobalTargetInfo> culpritsTargets;

		
		public GlobalTargetInfo? culpritTarget;
	}
}
