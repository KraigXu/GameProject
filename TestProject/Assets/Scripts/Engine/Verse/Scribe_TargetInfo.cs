using System;
using RimWorld.Planet;

namespace Verse
{
	// Token: 0x020002D8 RID: 728
	public static class Scribe_TargetInfo
	{
		// Token: 0x06001467 RID: 5223 RVA: 0x000783DE File Offset: 0x000765DE
		public static void Look(ref LocalTargetInfo value, string label)
		{
			Scribe_TargetInfo.Look(ref value, false, label, LocalTargetInfo.Invalid);
		}

		// Token: 0x06001468 RID: 5224 RVA: 0x000783ED File Offset: 0x000765ED
		public static void Look(ref LocalTargetInfo value, bool saveDestroyedThings, string label)
		{
			Scribe_TargetInfo.Look(ref value, saveDestroyedThings, label, LocalTargetInfo.Invalid);
		}

		// Token: 0x06001469 RID: 5225 RVA: 0x000783FC File Offset: 0x000765FC
		public static void Look(ref LocalTargetInfo value, string label, LocalTargetInfo defaultValue)
		{
			Scribe_TargetInfo.Look(ref value, false, label, defaultValue);
		}

		// Token: 0x0600146A RID: 5226 RVA: 0x00078408 File Offset: 0x00076608
		public static void Look(ref LocalTargetInfo value, bool saveDestroyedThings, string label, LocalTargetInfo defaultValue)
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				if (!value.Equals(defaultValue))
				{
					if (value.Thing != null && Scribe_References.CheckSaveReferenceToDestroyedThing(value.Thing, label, saveDestroyedThings))
					{
						return;
					}
					Scribe.saver.WriteElement(label, value.ToString());
					return;
				}
			}
			else
			{
				if (Scribe.mode == LoadSaveMode.LoadingVars)
				{
					value = ScribeExtractor.LocalTargetInfoFromNode(Scribe.loader.curXmlParent[label], label, defaultValue);
					return;
				}
				if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
				{
					value = ScribeExtractor.ResolveLocalTargetInfo(value, label);
				}
			}
		}

		// Token: 0x0600146B RID: 5227 RVA: 0x00078496 File Offset: 0x00076696
		public static void Look(ref TargetInfo value, string label)
		{
			Scribe_TargetInfo.Look(ref value, false, label, TargetInfo.Invalid);
		}

		// Token: 0x0600146C RID: 5228 RVA: 0x000784A5 File Offset: 0x000766A5
		public static void Look(ref TargetInfo value, bool saveDestroyedThings, string label)
		{
			Scribe_TargetInfo.Look(ref value, saveDestroyedThings, label, TargetInfo.Invalid);
		}

		// Token: 0x0600146D RID: 5229 RVA: 0x000784B4 File Offset: 0x000766B4
		public static void Look(ref TargetInfo value, string label, TargetInfo defaultValue)
		{
			Scribe_TargetInfo.Look(ref value, false, label, defaultValue);
		}

		// Token: 0x0600146E RID: 5230 RVA: 0x000784C0 File Offset: 0x000766C0
		public static void Look(ref TargetInfo value, bool saveDestroyedThings, string label, TargetInfo defaultValue)
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				if (!value.Equals(defaultValue))
				{
					if (value.Thing != null && Scribe_References.CheckSaveReferenceToDestroyedThing(value.Thing, label, saveDestroyedThings))
					{
						return;
					}
					if (!value.HasThing && value.Cell.IsValid && (value.Map == null || !Find.Maps.Contains(value.Map)))
					{
						Scribe.saver.WriteElement(label, "null");
						return;
					}
					Scribe.saver.WriteElement(label, value.ToString());
					return;
				}
			}
			else
			{
				if (Scribe.mode == LoadSaveMode.LoadingVars)
				{
					value = ScribeExtractor.TargetInfoFromNode(Scribe.loader.curXmlParent[label], label, defaultValue);
					return;
				}
				if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
				{
					value = ScribeExtractor.ResolveTargetInfo(value, label);
				}
			}
		}

		// Token: 0x0600146F RID: 5231 RVA: 0x00078594 File Offset: 0x00076794
		public static void Look(ref GlobalTargetInfo value, string label)
		{
			Scribe_TargetInfo.Look(ref value, false, label, GlobalTargetInfo.Invalid);
		}

		// Token: 0x06001470 RID: 5232 RVA: 0x000785A3 File Offset: 0x000767A3
		public static void Look(ref GlobalTargetInfo value, bool saveDestroyedThings, string label)
		{
			Scribe_TargetInfo.Look(ref value, saveDestroyedThings, label, GlobalTargetInfo.Invalid);
		}

		// Token: 0x06001471 RID: 5233 RVA: 0x000785B2 File Offset: 0x000767B2
		public static void Look(ref GlobalTargetInfo value, string label, GlobalTargetInfo defaultValue)
		{
			Scribe_TargetInfo.Look(ref value, false, label, defaultValue);
		}

		// Token: 0x06001472 RID: 5234 RVA: 0x000785C0 File Offset: 0x000767C0
		public static void Look(ref GlobalTargetInfo value, bool saveDestroyedThings, string label, GlobalTargetInfo defaultValue)
		{
			if (Scribe.mode == LoadSaveMode.Saving)
			{
				if (!value.Equals(defaultValue))
				{
					if (value.Thing != null && Scribe_References.CheckSaveReferenceToDestroyedThing(value.Thing, label, saveDestroyedThings))
					{
						return;
					}
					if (value.WorldObject != null && Scribe_References.CheckSaveReferenceToDestroyedWorldObject(value.WorldObject, label, saveDestroyedThings))
					{
						return;
					}
					if (!value.HasThing && !value.HasWorldObject && value.Cell.IsValid && (value.Map == null || !Find.Maps.Contains(value.Map)))
					{
						Scribe.saver.WriteElement(label, "null");
						return;
					}
					Scribe.saver.WriteElement(label, value.ToString());
					return;
				}
			}
			else
			{
				if (Scribe.mode == LoadSaveMode.LoadingVars)
				{
					value = ScribeExtractor.GlobalTargetInfoFromNode(Scribe.loader.curXmlParent[label], label, defaultValue);
					return;
				}
				if (Scribe.mode == LoadSaveMode.ResolvingCrossRefs)
				{
					value = ScribeExtractor.ResolveGlobalTargetInfo(value, label);
				}
			}
		}
	}
}
