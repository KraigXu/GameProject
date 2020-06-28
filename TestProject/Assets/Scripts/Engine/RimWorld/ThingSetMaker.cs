using System;
using System.Collections.Generic;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000912 RID: 2322
	public abstract class ThingSetMaker
	{
		// Token: 0x06003720 RID: 14112 RVA: 0x00128F20 File Offset: 0x00127120
		public List<Thing> Generate()
		{
			return this.Generate(default(ThingSetMakerParams));
		}

		// Token: 0x06003721 RID: 14113 RVA: 0x00128F3C File Offset: 0x0012713C
		public List<Thing> Generate(ThingSetMakerParams parms)
		{
			List<Thing> list = new List<Thing>();
			ThingSetMaker.thingsBeingGeneratedNow.Add(list);
			try
			{
				ThingSetMakerParams parms2 = this.ApplyFixedParams(parms);
				this.Generate(parms2, list);
				this.PostProcess(list);
			}
			catch (Exception arg)
			{
				Log.Error("Exception while generating thing set: " + arg, false);
				for (int i = list.Count - 1; i >= 0; i--)
				{
					list[i].Destroy(DestroyMode.Vanish);
					list.RemoveAt(i);
				}
			}
			finally
			{
				ThingSetMaker.thingsBeingGeneratedNow.Remove(list);
			}
			return list;
		}

		// Token: 0x06003722 RID: 14114 RVA: 0x00128FD8 File Offset: 0x001271D8
		public bool CanGenerate(ThingSetMakerParams parms)
		{
			ThingSetMakerParams parms2 = this.ApplyFixedParams(parms);
			return this.CanGenerateSub(parms2);
		}

		// Token: 0x06003723 RID: 14115 RVA: 0x0001028D File Offset: 0x0000E48D
		protected virtual bool CanGenerateSub(ThingSetMakerParams parms)
		{
			return true;
		}

		// Token: 0x06003724 RID: 14116
		protected abstract void Generate(ThingSetMakerParams parms, List<Thing> outThings);

		// Token: 0x06003725 RID: 14117 RVA: 0x00128FF4 File Offset: 0x001271F4
		public IEnumerable<ThingDef> AllGeneratableThingsDebug()
		{
			return this.AllGeneratableThingsDebug(default(ThingSetMakerParams));
		}

		// Token: 0x06003726 RID: 14118 RVA: 0x00129010 File Offset: 0x00127210
		public IEnumerable<ThingDef> AllGeneratableThingsDebug(ThingSetMakerParams parms)
		{
			if (!this.CanGenerate(parms))
			{
				yield break;
			}
			ThingSetMakerParams parms2 = this.ApplyFixedParams(parms);
			foreach (ThingDef thingDef in this.AllGeneratableThingsDebugSub(parms2).Distinct<ThingDef>())
			{
				yield return thingDef;
			}
			IEnumerator<ThingDef> enumerator = null;
			yield break;
			yield break;
		}

		// Token: 0x06003727 RID: 14119 RVA: 0x0001BFCE File Offset: 0x0001A1CE
		public virtual float ExtraSelectionWeightFactor(ThingSetMakerParams parms)
		{
			return 1f;
		}

		// Token: 0x06003728 RID: 14120
		protected abstract IEnumerable<ThingDef> AllGeneratableThingsDebugSub(ThingSetMakerParams parms);

		// Token: 0x06003729 RID: 14121 RVA: 0x00129028 File Offset: 0x00127228
		private void PostProcess(List<Thing> things)
		{
			if (things.RemoveAll((Thing x) => x == null) != 0)
			{
				Log.Error(base.GetType() + " generated null things.", false);
			}
			this.ChangeDeadPawnsToTheirCorpses(things);
			for (int i = things.Count - 1; i >= 0; i--)
			{
				if (things[i].Destroyed)
				{
					Log.Error(base.GetType() + " generated destroyed thing " + things[i].ToStringSafe<Thing>(), false);
					things.RemoveAt(i);
				}
				else if (things[i].stackCount <= 0)
				{
					Log.Error(string.Concat(new object[]
					{
						base.GetType(),
						" generated ",
						things[i].ToStringSafe<Thing>(),
						" with stackCount=",
						things[i].stackCount
					}), false);
					things.RemoveAt(i);
				}
			}
			this.Minify(things);
		}

		// Token: 0x0600372A RID: 14122 RVA: 0x00129138 File Offset: 0x00127338
		private void Minify(List<Thing> things)
		{
			for (int i = 0; i < things.Count; i++)
			{
				if (things[i].def.Minifiable)
				{
					int stackCount = things[i].stackCount;
					things[i].stackCount = 1;
					MinifiedThing minifiedThing = things[i].MakeMinified();
					minifiedThing.stackCount = stackCount;
					things[i] = minifiedThing;
				}
			}
		}

		// Token: 0x0600372B RID: 14123 RVA: 0x001291A0 File Offset: 0x001273A0
		private void ChangeDeadPawnsToTheirCorpses(List<Thing> things)
		{
			for (int i = 0; i < things.Count; i++)
			{
				if (things[i].ParentHolder is Corpse)
				{
					things[i] = (Corpse)things[i].ParentHolder;
				}
			}
		}

		// Token: 0x0600372C RID: 14124 RVA: 0x001291EC File Offset: 0x001273EC
		private ThingSetMakerParams ApplyFixedParams(ThingSetMakerParams parms)
		{
			ThingSetMakerParams result = this.fixedParams;
			Gen.ReplaceNullFields<ThingSetMakerParams>(ref result, parms);
			return result;
		}

		// Token: 0x0600372D RID: 14125 RVA: 0x00129209 File Offset: 0x00127409
		public virtual void ResolveReferences()
		{
			if (this.fixedParams.filter != null)
			{
				this.fixedParams.filter.ResolveReferences();
			}
		}

		// Token: 0x0400204B RID: 8267
		public ThingSetMakerParams fixedParams;

		// Token: 0x0400204C RID: 8268
		public static List<List<Thing>> thingsBeingGeneratedNow = new List<List<Thing>>();
	}
}
