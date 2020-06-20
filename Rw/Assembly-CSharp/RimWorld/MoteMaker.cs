using System;
using System.Collections.Generic;
using UnityEngine;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CA9 RID: 3241
	public static class MoteMaker
	{
		// Token: 0x06004E5C RID: 20060 RVA: 0x001A56D4 File Offset: 0x001A38D4
		public static Mote ThrowMetaIcon(IntVec3 cell, Map map, ThingDef moteDef)
		{
			if (!cell.ShouldSpawnMotesAt(map) || map.moteCounter.Saturated)
			{
				return null;
			}
			MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(moteDef, null);
			moteThrown.Scale = 0.7f;
			moteThrown.rotationRate = Rand.Range(-3f, 3f);
			moteThrown.exactPosition = cell.ToVector3Shifted();
			moteThrown.exactPosition += new Vector3(0.35f, 0f, 0.35f);
			moteThrown.exactPosition += new Vector3(Rand.Value, 0f, Rand.Value) * 0.1f;
			moteThrown.SetVelocity((float)Rand.Range(30, 60), 0.42f);
			GenSpawn.Spawn(moteThrown, cell, map, WipeMode.Vanish);
			return moteThrown;
		}

		// Token: 0x06004E5D RID: 20061 RVA: 0x001A57A5 File Offset: 0x001A39A5
		public static void MakeStaticMote(IntVec3 cell, Map map, ThingDef moteDef, float scale = 1f)
		{
			MoteMaker.MakeStaticMote(cell.ToVector3Shifted(), map, moteDef, scale);
		}

		// Token: 0x06004E5E RID: 20062 RVA: 0x001A57B8 File Offset: 0x001A39B8
		public static Mote MakeStaticMote(Vector3 loc, Map map, ThingDef moteDef, float scale = 1f)
		{
			if (!loc.ShouldSpawnMotesAt(map) || map.moteCounter.Saturated)
			{
				return null;
			}
			Mote mote = (Mote)ThingMaker.MakeThing(moteDef, null);
			mote.exactPosition = loc;
			mote.Scale = scale;
			GenSpawn.Spawn(mote, loc.ToIntVec3(), map, WipeMode.Vanish);
			return mote;
		}

		// Token: 0x06004E5F RID: 20063 RVA: 0x001A5806 File Offset: 0x001A3A06
		public static void ThrowText(Vector3 loc, Map map, string text, float timeBeforeStartFadeout = -1f)
		{
			MoteMaker.ThrowText(loc, map, text, Color.white, timeBeforeStartFadeout);
		}

		// Token: 0x06004E60 RID: 20064 RVA: 0x001A5818 File Offset: 0x001A3A18
		public static void ThrowText(Vector3 loc, Map map, string text, Color color, float timeBeforeStartFadeout = -1f)
		{
			IntVec3 intVec = loc.ToIntVec3();
			if (!intVec.InBounds(map))
			{
				return;
			}
			MoteText moteText = (MoteText)ThingMaker.MakeThing(ThingDefOf.Mote_Text, null);
			moteText.exactPosition = loc;
			moteText.SetVelocity((float)Rand.Range(5, 35), Rand.Range(0.42f, 0.45f));
			moteText.text = text;
			moteText.textColor = color;
			if (timeBeforeStartFadeout >= 0f)
			{
				moteText.overrideTimeBeforeStartFadeout = timeBeforeStartFadeout;
			}
			GenSpawn.Spawn(moteText, intVec, map, WipeMode.Vanish);
		}

		// Token: 0x06004E61 RID: 20065 RVA: 0x001A5898 File Offset: 0x001A3A98
		public static void ThrowMetaPuffs(CellRect rect, Map map)
		{
			if (!Find.TickManager.Paused)
			{
				for (int i = rect.minX; i <= rect.maxX; i++)
				{
					for (int j = rect.minZ; j <= rect.maxZ; j++)
					{
						MoteMaker.ThrowMetaPuffs(new TargetInfo(new IntVec3(i, 0, j), map, false));
					}
				}
			}
		}

		// Token: 0x06004E62 RID: 20066 RVA: 0x001A58F4 File Offset: 0x001A3AF4
		public static void ThrowMetaPuffs(TargetInfo targ)
		{
			Vector3 a = targ.HasThing ? targ.Thing.TrueCenter() : targ.Cell.ToVector3Shifted();
			int num = Rand.RangeInclusive(4, 6);
			for (int i = 0; i < num; i++)
			{
				MoteMaker.ThrowMetaPuff(a + new Vector3(Rand.Range(-0.5f, 0.5f), 0f, Rand.Range(-0.5f, 0.5f)), targ.Map);
			}
		}

		// Token: 0x06004E63 RID: 20067 RVA: 0x001A5978 File Offset: 0x001A3B78
		public static void ThrowMetaPuff(Vector3 loc, Map map)
		{
			if (!loc.ShouldSpawnMotesAt(map))
			{
				return;
			}
			MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(ThingDefOf.Mote_MetaPuff, null);
			moteThrown.Scale = 1.9f;
			moteThrown.rotationRate = (float)Rand.Range(-60, 60);
			moteThrown.exactPosition = loc;
			moteThrown.SetVelocity((float)Rand.Range(0, 360), Rand.Range(0.6f, 0.78f));
			GenSpawn.Spawn(moteThrown, loc.ToIntVec3(), map, WipeMode.Vanish);
		}

		// Token: 0x06004E64 RID: 20068 RVA: 0x001A59F0 File Offset: 0x001A3BF0
		private static MoteThrown NewBaseAirPuff()
		{
			MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(ThingDefOf.Mote_AirPuff, null);
			moteThrown.Scale = 1.5f;
			moteThrown.rotationRate = (float)Rand.RangeInclusive(-240, 240);
			return moteThrown;
		}

		// Token: 0x06004E65 RID: 20069 RVA: 0x001A5A24 File Offset: 0x001A3C24
		public static void ThrowAirPuffUp(Vector3 loc, Map map)
		{
			if (!loc.ToIntVec3().ShouldSpawnMotesAt(map) || map.moteCounter.SaturatedLowPriority)
			{
				return;
			}
			MoteThrown moteThrown = MoteMaker.NewBaseAirPuff();
			moteThrown.exactPosition = loc;
			moteThrown.exactPosition += new Vector3(Rand.Range(-0.02f, 0.02f), 0f, Rand.Range(-0.02f, 0.02f));
			moteThrown.SetVelocity((float)Rand.Range(-45, 45), Rand.Range(1.2f, 1.5f));
			GenSpawn.Spawn(moteThrown, loc.ToIntVec3(), map, WipeMode.Vanish);
		}

		// Token: 0x06004E66 RID: 20070 RVA: 0x001A5AC0 File Offset: 0x001A3CC0
		public static void ThrowBreathPuff(Vector3 loc, Map map, float throwAngle, Vector3 inheritVelocity)
		{
			if (!loc.ToIntVec3().ShouldSpawnMotesAt(map) || map.moteCounter.SaturatedLowPriority)
			{
				return;
			}
			MoteThrown moteThrown = MoteMaker.NewBaseAirPuff();
			moteThrown.exactPosition = loc;
			moteThrown.exactPosition += new Vector3(Rand.Range(-0.005f, 0.005f), 0f, Rand.Range(-0.005f, 0.005f));
			moteThrown.SetVelocity(throwAngle + (float)Rand.Range(-10, 10), Rand.Range(0.1f, 0.8f));
			moteThrown.Velocity += inheritVelocity * 0.5f;
			moteThrown.Scale = Rand.Range(0.6f, 0.7f);
			GenSpawn.Spawn(moteThrown, loc.ToIntVec3(), map, WipeMode.Vanish);
		}

		// Token: 0x06004E67 RID: 20071 RVA: 0x001A5B8E File Offset: 0x001A3D8E
		public static void ThrowDustPuff(IntVec3 cell, Map map, float scale)
		{
			MoteMaker.ThrowDustPuff(cell.ToVector3() + new Vector3(Rand.Value, 0f, Rand.Value), map, scale);
		}

		// Token: 0x06004E68 RID: 20072 RVA: 0x001A5BB8 File Offset: 0x001A3DB8
		public static void ThrowDustPuff(Vector3 loc, Map map, float scale)
		{
			if (!loc.ShouldSpawnMotesAt(map) || map.moteCounter.SaturatedLowPriority)
			{
				return;
			}
			MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(ThingDefOf.Mote_DustPuff, null);
			moteThrown.Scale = 1.9f * scale;
			moteThrown.rotationRate = (float)Rand.Range(-60, 60);
			moteThrown.exactPosition = loc;
			moteThrown.SetVelocity((float)Rand.Range(0, 360), Rand.Range(0.6f, 0.75f));
			GenSpawn.Spawn(moteThrown, loc.ToIntVec3(), map, WipeMode.Vanish);
		}

		// Token: 0x06004E69 RID: 20073 RVA: 0x001A5C40 File Offset: 0x001A3E40
		public static void ThrowDustPuffThick(Vector3 loc, Map map, float scale, Color color)
		{
			if (!loc.ShouldSpawnMotesAt(map) || map.moteCounter.SaturatedLowPriority)
			{
				return;
			}
			MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(ThingDefOf.Mote_DustPuffThick, null);
			moteThrown.Scale = scale;
			moteThrown.rotationRate = (float)Rand.Range(-60, 60);
			moteThrown.exactPosition = loc;
			moteThrown.instanceColor = color;
			moteThrown.SetVelocity((float)Rand.Range(0, 360), Rand.Range(0.6f, 0.75f));
			GenSpawn.Spawn(moteThrown, loc.ToIntVec3(), map, WipeMode.Vanish);
		}

		// Token: 0x06004E6A RID: 20074 RVA: 0x001A5CC8 File Offset: 0x001A3EC8
		public static void ThrowTornadoDustPuff(Vector3 loc, Map map, float scale, Color color)
		{
			if (!loc.ShouldSpawnMotesAt(map) || map.moteCounter.SaturatedLowPriority)
			{
				return;
			}
			MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(ThingDefOf.Mote_TornadoDustPuff, null);
			moteThrown.Scale = 1.9f * scale;
			moteThrown.rotationRate = (float)Rand.Range(-60, 60);
			moteThrown.exactPosition = loc;
			moteThrown.instanceColor = color;
			moteThrown.SetVelocity((float)Rand.Range(0, 360), Rand.Range(0.6f, 0.75f));
			GenSpawn.Spawn(moteThrown, loc.ToIntVec3(), map, WipeMode.Vanish);
		}

		// Token: 0x06004E6B RID: 20075 RVA: 0x001A5D58 File Offset: 0x001A3F58
		public static void ThrowSmoke(Vector3 loc, Map map, float size)
		{
			if (!loc.ShouldSpawnMotesAt(map) || map.moteCounter.SaturatedLowPriority)
			{
				return;
			}
			MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(ThingDefOf.Mote_Smoke, null);
			moteThrown.Scale = Rand.Range(1.5f, 2.5f) * size;
			moteThrown.rotationRate = Rand.Range(-30f, 30f);
			moteThrown.exactPosition = loc;
			moteThrown.SetVelocity((float)Rand.Range(30, 40), Rand.Range(0.5f, 0.7f));
			GenSpawn.Spawn(moteThrown, loc.ToIntVec3(), map, WipeMode.Vanish);
		}

		// Token: 0x06004E6C RID: 20076 RVA: 0x001A5DEC File Offset: 0x001A3FEC
		public static void ThrowFireGlow(IntVec3 c, Map map, float size)
		{
			Vector3 vector = c.ToVector3Shifted();
			if (!vector.ShouldSpawnMotesAt(map) || map.moteCounter.SaturatedLowPriority)
			{
				return;
			}
			vector += size * new Vector3(Rand.Value - 0.5f, 0f, Rand.Value - 0.5f);
			if (!vector.InBounds(map))
			{
				return;
			}
			MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(ThingDefOf.Mote_FireGlow, null);
			moteThrown.Scale = Rand.Range(4f, 6f) * size;
			moteThrown.rotationRate = Rand.Range(-3f, 3f);
			moteThrown.exactPosition = vector;
			moteThrown.SetVelocity((float)Rand.Range(0, 360), 0.12f);
			GenSpawn.Spawn(moteThrown, vector.ToIntVec3(), map, WipeMode.Vanish);
		}

		// Token: 0x06004E6D RID: 20077 RVA: 0x001A5EB8 File Offset: 0x001A40B8
		public static void ThrowHeatGlow(IntVec3 c, Map map, float size)
		{
			Vector3 vector = c.ToVector3Shifted();
			if (!vector.ShouldSpawnMotesAt(map) || map.moteCounter.SaturatedLowPriority)
			{
				return;
			}
			vector += size * new Vector3(Rand.Value - 0.5f, 0f, Rand.Value - 0.5f);
			if (!vector.InBounds(map))
			{
				return;
			}
			MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(ThingDefOf.Mote_HeatGlow, null);
			moteThrown.Scale = Rand.Range(4f, 6f) * size;
			moteThrown.rotationRate = Rand.Range(-3f, 3f);
			moteThrown.exactPosition = vector;
			moteThrown.SetVelocity((float)Rand.Range(0, 360), 0.12f);
			GenSpawn.Spawn(moteThrown, vector.ToIntVec3(), map, WipeMode.Vanish);
		}

		// Token: 0x06004E6E RID: 20078 RVA: 0x001A5F84 File Offset: 0x001A4184
		public static void ThrowMicroSparks(Vector3 loc, Map map)
		{
			if (!loc.ShouldSpawnMotesAt(map) || map.moteCounter.SaturatedLowPriority)
			{
				return;
			}
			MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(ThingDefOf.Mote_MicroSparks, null);
			moteThrown.Scale = Rand.Range(0.8f, 1.2f);
			moteThrown.rotationRate = Rand.Range(-12f, 12f);
			moteThrown.exactPosition = loc;
			moteThrown.exactPosition -= new Vector3(0.5f, 0f, 0.5f);
			moteThrown.exactPosition += new Vector3(Rand.Value, 0f, Rand.Value);
			moteThrown.SetVelocity((float)Rand.Range(35, 45), 1.2f);
			GenSpawn.Spawn(moteThrown, loc.ToIntVec3(), map, WipeMode.Vanish);
		}

		// Token: 0x06004E6F RID: 20079 RVA: 0x001A6058 File Offset: 0x001A4258
		public static void ThrowLightningGlow(Vector3 loc, Map map, float size)
		{
			if (!loc.ShouldSpawnMotesAt(map) || map.moteCounter.SaturatedLowPriority)
			{
				return;
			}
			MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(ThingDefOf.Mote_LightningGlow, null);
			moteThrown.Scale = Rand.Range(4f, 6f) * size;
			moteThrown.rotationRate = Rand.Range(-3f, 3f);
			moteThrown.exactPosition = loc + size * new Vector3(Rand.Value - 0.5f, 0f, Rand.Value - 0.5f);
			moteThrown.SetVelocity((float)Rand.Range(0, 360), 1.2f);
			GenSpawn.Spawn(moteThrown, loc.ToIntVec3(), map, WipeMode.Vanish);
		}

		// Token: 0x06004E70 RID: 20080 RVA: 0x001A6110 File Offset: 0x001A4310
		public static void PlaceFootprint(Vector3 loc, Map map, float rot)
		{
			if (!loc.ShouldSpawnMotesAt(map) || map.moteCounter.SaturatedLowPriority)
			{
				return;
			}
			MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(ThingDefOf.Mote_Footprint, null);
			moteThrown.Scale = 0.5f;
			moteThrown.exactRotation = rot;
			moteThrown.exactPosition = loc;
			GenSpawn.Spawn(moteThrown, loc.ToIntVec3(), map, WipeMode.Vanish);
		}

		// Token: 0x06004E71 RID: 20081 RVA: 0x001A616B File Offset: 0x001A436B
		public static void ThrowHorseshoe(Pawn thrower, IntVec3 targetCell)
		{
			MoteMaker.ThrowObjectAt(thrower, targetCell, ThingDefOf.Mote_Horseshoe);
		}

		// Token: 0x06004E72 RID: 20082 RVA: 0x001A6179 File Offset: 0x001A4379
		public static void ThrowStone(Pawn thrower, IntVec3 targetCell)
		{
			MoteMaker.ThrowObjectAt(thrower, targetCell, ThingDefOf.Mote_Stone);
		}

		// Token: 0x06004E73 RID: 20083 RVA: 0x001A6188 File Offset: 0x001A4388
		private static void ThrowObjectAt(Pawn thrower, IntVec3 targetCell, ThingDef mote)
		{
			if (!thrower.Position.ShouldSpawnMotesAt(thrower.Map) || thrower.Map.moteCounter.Saturated)
			{
				return;
			}
			float num = Rand.Range(3.8f, 5.6f);
			Vector3 vector = targetCell.ToVector3Shifted() + Vector3Utility.RandomHorizontalOffset((1f - (float)thrower.skills.GetSkill(SkillDefOf.Shooting).Level / 20f) * 1.8f);
			vector.y = thrower.DrawPos.y;
			MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(mote, null);
			moteThrown.Scale = 1f;
			moteThrown.rotationRate = (float)Rand.Range(-300, 300);
			moteThrown.exactPosition = thrower.DrawPos;
			moteThrown.SetVelocity((vector - moteThrown.exactPosition).AngleFlat(), num);
			moteThrown.airTimeLeft = (float)Mathf.RoundToInt((moteThrown.exactPosition - vector).MagnitudeHorizontal() / num);
			GenSpawn.Spawn(moteThrown, thrower.Position, thrower.Map, WipeMode.Vanish);
		}

		// Token: 0x06004E74 RID: 20084 RVA: 0x001A629C File Offset: 0x001A449C
		public static Mote MakeStunOverlay(Thing stunnedThing)
		{
			Mote mote = (Mote)ThingMaker.MakeThing(ThingDefOf.Mote_Stun, null);
			mote.Attach(stunnedThing);
			GenSpawn.Spawn(mote, stunnedThing.Position, stunnedThing.Map, WipeMode.Vanish);
			return mote;
		}

		// Token: 0x06004E75 RID: 20085 RVA: 0x001A62D0 File Offset: 0x001A44D0
		public static MoteDualAttached MakeInteractionOverlay(ThingDef moteDef, TargetInfo A, TargetInfo B)
		{
			MoteDualAttached moteDualAttached = (MoteDualAttached)ThingMaker.MakeThing(moteDef, null);
			moteDualAttached.Scale = 0.5f;
			moteDualAttached.Attach(A, B);
			GenSpawn.Spawn(moteDualAttached, A.Cell, A.Map ?? B.Map, WipeMode.Vanish);
			return moteDualAttached;
		}

		// Token: 0x06004E76 RID: 20086 RVA: 0x001A6320 File Offset: 0x001A4520
		public static Mote MakeAttachedOverlay(Thing thing, ThingDef moteDef, Vector3 offset, float scale = 1f, float solidTimeOverride = -1f)
		{
			Mote mote = (Mote)ThingMaker.MakeThing(moteDef, null);
			mote.Attach(thing);
			mote.Scale = scale;
			mote.exactPosition = thing.DrawPos + offset;
			mote.solidTimeOverride = solidTimeOverride;
			GenSpawn.Spawn(mote, thing.Position, thing.MapHeld, WipeMode.Vanish);
			return mote;
		}

		// Token: 0x06004E77 RID: 20087 RVA: 0x001A637C File Offset: 0x001A457C
		public static void MakeColonistActionOverlay(Pawn pawn, ThingDef moteDef)
		{
			MoteThrownAttached moteThrownAttached = (MoteThrownAttached)ThingMaker.MakeThing(moteDef, null);
			moteThrownAttached.Attach(pawn);
			moteThrownAttached.exactPosition = pawn.DrawPos;
			moteThrownAttached.Scale = 1.5f;
			moteThrownAttached.SetVelocity(Rand.Range(20f, 25f), 0.4f);
			GenSpawn.Spawn(moteThrownAttached, pawn.Position, pawn.Map, WipeMode.Vanish);
		}

		// Token: 0x06004E78 RID: 20088 RVA: 0x001A63E8 File Offset: 0x001A45E8
		private static MoteBubble ExistingMoteBubbleOn(Pawn pawn)
		{
			if (!pawn.Spawned)
			{
				return null;
			}
			for (int i = 0; i < 4; i++)
			{
				if ((pawn.Position + MoteMaker.UpRightPattern[i]).InBounds(pawn.Map))
				{
					List<Thing> thingList = pawn.Position.GetThingList(pawn.Map);
					for (int j = 0; j < thingList.Count; j++)
					{
						MoteBubble moteBubble = thingList[j] as MoteBubble;
						if (moteBubble != null && moteBubble.link1.Linked && moteBubble.link1.Target.HasThing && moteBubble.link1.Target == pawn)
						{
							return moteBubble;
						}
					}
				}
			}
			return null;
		}

		// Token: 0x06004E79 RID: 20089 RVA: 0x001A64A8 File Offset: 0x001A46A8
		public static MoteBubble MakeMoodThoughtBubble(Pawn pawn, Thought thought)
		{
			if (Current.ProgramState != ProgramState.Playing)
			{
				return null;
			}
			if (!pawn.Spawned)
			{
				return null;
			}
			float num = thought.MoodOffset();
			if (num == 0f)
			{
				return null;
			}
			MoteBubble moteBubble = MoteMaker.ExistingMoteBubbleOn(pawn);
			if (moteBubble != null)
			{
				if (moteBubble.def == ThingDefOf.Mote_Speech)
				{
					return null;
				}
				if (moteBubble.def == ThingDefOf.Mote_ThoughtBad || moteBubble.def == ThingDefOf.Mote_ThoughtGood)
				{
					moteBubble.Destroy(DestroyMode.Vanish);
				}
			}
			MoteBubble moteBubble2 = (MoteBubble)ThingMaker.MakeThing((num > 0f) ? ThingDefOf.Mote_ThoughtGood : ThingDefOf.Mote_ThoughtBad, null);
			moteBubble2.SetupMoteBubble(thought.Icon, null);
			moteBubble2.Attach(pawn);
			GenSpawn.Spawn(moteBubble2, pawn.Position, pawn.Map, WipeMode.Vanish);
			return moteBubble2;
		}

		// Token: 0x06004E7A RID: 20090 RVA: 0x001A6560 File Offset: 0x001A4760
		public static MoteBubble MakeThoughtBubble(Pawn pawn, string iconPath, bool maintain = false)
		{
			MoteBubble moteBubble = MoteMaker.ExistingMoteBubbleOn(pawn);
			if (moteBubble != null)
			{
				moteBubble.Destroy(DestroyMode.Vanish);
			}
			MoteBubble moteBubble2 = (MoteBubble)ThingMaker.MakeThing(maintain ? ThingDefOf.Mote_ForceJobMaintained : ThingDefOf.Mote_ForceJob, null);
			moteBubble2.SetupMoteBubble(ContentFinder<Texture2D>.Get(iconPath, true), null);
			moteBubble2.Attach(pawn);
			GenSpawn.Spawn(moteBubble2, pawn.Position, pawn.Map, WipeMode.Vanish);
			return moteBubble2;
		}

		// Token: 0x06004E7B RID: 20091 RVA: 0x001A65C8 File Offset: 0x001A47C8
		public static MoteBubble MakeInteractionBubble(Pawn initiator, Pawn recipient, ThingDef interactionMote, Texture2D symbol)
		{
			MoteBubble moteBubble = MoteMaker.ExistingMoteBubbleOn(initiator);
			if (moteBubble != null)
			{
				if (moteBubble.def == ThingDefOf.Mote_Speech)
				{
					moteBubble.Destroy(DestroyMode.Vanish);
				}
				if (moteBubble.def == ThingDefOf.Mote_ThoughtBad || moteBubble.def == ThingDefOf.Mote_ThoughtGood)
				{
					moteBubble.Destroy(DestroyMode.Vanish);
				}
			}
			MoteBubble moteBubble2 = (MoteBubble)ThingMaker.MakeThing(interactionMote, null);
			moteBubble2.SetupMoteBubble(symbol, recipient);
			moteBubble2.Attach(initiator);
			GenSpawn.Spawn(moteBubble2, initiator.Position, initiator.Map, WipeMode.Vanish);
			return moteBubble2;
		}

		// Token: 0x06004E7C RID: 20092 RVA: 0x001A6648 File Offset: 0x001A4848
		public static MoteBubble MakeSpeechBubble(Pawn initiator, Texture2D symbol)
		{
			MoteBubble moteBubble = MoteMaker.ExistingMoteBubbleOn(initiator);
			if (moteBubble != null)
			{
				if (moteBubble.def == ThingDefOf.Mote_Speech)
				{
					moteBubble.Destroy(DestroyMode.Vanish);
				}
				if (moteBubble.def == ThingDefOf.Mote_ThoughtBad || moteBubble.def == ThingDefOf.Mote_ThoughtGood)
				{
					moteBubble.Destroy(DestroyMode.Vanish);
				}
			}
			MoteBubble moteBubble2 = (MoteBubble)ThingMaker.MakeThing(ThingDefOf.Mote_Speech, null);
			moteBubble2.SetupMoteBubble(symbol, null);
			moteBubble2.Attach(initiator);
			GenSpawn.Spawn(moteBubble2, initiator.Position, initiator.Map, WipeMode.Vanish);
			return moteBubble2;
		}

		// Token: 0x06004E7D RID: 20093 RVA: 0x001A66CC File Offset: 0x001A48CC
		public static void ThrowExplosionCell(IntVec3 cell, Map map, ThingDef moteDef, Color color)
		{
			if (!cell.ShouldSpawnMotesAt(map))
			{
				return;
			}
			Mote mote = (Mote)ThingMaker.MakeThing(moteDef, null);
			mote.exactRotation = (float)(90 * Rand.RangeInclusive(0, 3));
			mote.exactPosition = cell.ToVector3Shifted();
			mote.instanceColor = color;
			GenSpawn.Spawn(mote, cell, map, WipeMode.Vanish);
			if (Rand.Value < 0.7f)
			{
				MoteMaker.ThrowDustPuff(cell, map, 1.2f);
			}
		}

		// Token: 0x06004E7E RID: 20094 RVA: 0x001A6738 File Offset: 0x001A4938
		public static void ThrowExplosionInteriorMote(Vector3 loc, Map map, ThingDef moteDef)
		{
			if (!loc.ShouldSpawnMotesAt(map) || map.moteCounter.SaturatedLowPriority)
			{
				return;
			}
			MoteThrown moteThrown = (MoteThrown)ThingMaker.MakeThing(moteDef, null);
			moteThrown.Scale = Rand.Range(3f, 4.5f);
			moteThrown.rotationRate = Rand.Range(-30f, 30f);
			moteThrown.exactPosition = loc;
			moteThrown.SetVelocity((float)Rand.Range(0, 360), Rand.Range(0.48f, 0.72f));
			GenSpawn.Spawn(moteThrown, loc.ToIntVec3(), map, WipeMode.Vanish);
		}

		// Token: 0x06004E7F RID: 20095 RVA: 0x001A67C8 File Offset: 0x001A49C8
		public static void MakeWaterSplash(Vector3 loc, Map map, float size, float velocity)
		{
			if (!loc.ShouldSpawnMotesAt(map) || map.moteCounter.SaturatedLowPriority)
			{
				return;
			}
			MoteSplash moteSplash = (MoteSplash)ThingMaker.MakeThing(ThingDefOf.Mote_WaterSplash, null);
			moteSplash.Initialize(loc, size, velocity);
			GenSpawn.Spawn(moteSplash, loc.ToIntVec3(), map, WipeMode.Vanish);
		}

		// Token: 0x06004E80 RID: 20096 RVA: 0x001A6808 File Offset: 0x001A4A08
		public static void MakeBombardmentMote(IntVec3 cell, Map map)
		{
			Mote mote = (Mote)ThingMaker.MakeThing(ThingDefOf.Mote_Bombardment, null);
			mote.exactPosition = cell.ToVector3Shifted();
			mote.Scale = (float)Mathf.Max(23, 25) * 6f;
			mote.rotationRate = 1.2f;
			GenSpawn.Spawn(mote, cell, map, WipeMode.Vanish);
		}

		// Token: 0x06004E81 RID: 20097 RVA: 0x001A685C File Offset: 0x001A4A5C
		public static void MakePowerBeamMote(IntVec3 cell, Map map)
		{
			Mote mote = (Mote)ThingMaker.MakeThing(ThingDefOf.Mote_PowerBeam, null);
			mote.exactPosition = cell.ToVector3Shifted();
			mote.Scale = 90f;
			mote.rotationRate = 1.2f;
			GenSpawn.Spawn(mote, cell, map, WipeMode.Vanish);
		}

		// Token: 0x06004E82 RID: 20098 RVA: 0x001A689A File Offset: 0x001A4A9A
		public static void PlaceTempRoof(IntVec3 cell, Map map)
		{
			if (!cell.ShouldSpawnMotesAt(map))
			{
				return;
			}
			Mote mote = (Mote)ThingMaker.MakeThing(ThingDefOf.Mote_TempRoof, null);
			mote.exactPosition = cell.ToVector3Shifted();
			GenSpawn.Spawn(mote, cell, map, WipeMode.Vanish);
		}

		// Token: 0x06004E83 RID: 20099 RVA: 0x001A68CC File Offset: 0x001A4ACC
		public static Mote MakeConnectingLine(Vector3 start, Vector3 end, ThingDef moteType, Map map, float width = 1f)
		{
			Vector3 vector = end - start;
			float x = vector.MagnitudeHorizontal();
			Mote mote = MoteMaker.MakeStaticMote(start + vector * 0.5f, map, moteType, 1f);
			if (mote != null)
			{
				mote.exactScale = new Vector3(x, 1f, width);
				mote.exactRotation = Mathf.Atan2(-vector.z, vector.x) * 57.29578f;
			}
			return mote;
		}

		// Token: 0x04002C0D RID: 11277
		private static IntVec3[] UpRightPattern = new IntVec3[]
		{
			new IntVec3(0, 0, 0),
			new IntVec3(1, 0, 0),
			new IntVec3(0, 0, 1),
			new IntVec3(1, 0, 1)
		};
	}
}
