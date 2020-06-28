using System;
using System.Collections.Generic;
using System.Text;
using RimWorld;
using UnityEngine;

namespace Verse
{
	// Token: 0x02000317 RID: 791
	public class ThingWithComps : Thing
	{
		// Token: 0x170004BB RID: 1211
		// (get) Token: 0x060016EC RID: 5868 RVA: 0x0008412F File Offset: 0x0008232F
		public List<ThingComp> AllComps
		{
			get
			{
				if (this.comps == null)
				{
					return ThingWithComps.EmptyCompsList;
				}
				return this.comps;
			}
		}

		// Token: 0x170004BC RID: 1212
		// (get) Token: 0x060016ED RID: 5869 RVA: 0x00084148 File Offset: 0x00082348
		// (set) Token: 0x060016EE RID: 5870 RVA: 0x00084174 File Offset: 0x00082374
		public override Color DrawColor
		{
			get
			{
				CompColorable comp = this.GetComp<CompColorable>();
				if (comp != null && comp.Active)
				{
					return comp.Color;
				}
				return base.DrawColor;
			}
			set
			{
				this.SetColor(value, true);
			}
		}

		// Token: 0x170004BD RID: 1213
		// (get) Token: 0x060016EF RID: 5871 RVA: 0x00084180 File Offset: 0x00082380
		public override string LabelNoCount
		{
			get
			{
				string text = base.LabelNoCount;
				if (this.comps != null)
				{
					int i = 0;
					int count = this.comps.Count;
					while (i < count)
					{
						text = this.comps[i].TransformLabel(text);
						i++;
					}
				}
				return text;
			}
		}

		// Token: 0x170004BE RID: 1214
		// (get) Token: 0x060016F0 RID: 5872 RVA: 0x000841C8 File Offset: 0x000823C8
		public override string DescriptionFlavor
		{
			get
			{
				StringBuilder stringBuilder = new StringBuilder();
				stringBuilder.Append(base.DescriptionFlavor);
				if (this.comps != null)
				{
					for (int i = 0; i < this.comps.Count; i++)
					{
						string descriptionPart = this.comps[i].GetDescriptionPart();
						if (!descriptionPart.NullOrEmpty())
						{
							if (stringBuilder.Length > 0)
							{
								stringBuilder.AppendLine();
								stringBuilder.AppendLine();
							}
							stringBuilder.Append(descriptionPart);
						}
					}
				}
				return stringBuilder.ToString();
			}
		}

		// Token: 0x060016F1 RID: 5873 RVA: 0x00084248 File Offset: 0x00082448
		public override void PostMake()
		{
			base.PostMake();
			this.InitializeComps();
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PostPostMake();
				}
			}
		}

		// Token: 0x060016F2 RID: 5874 RVA: 0x00084290 File Offset: 0x00082490
		public T GetComp<T>() where T : ThingComp
		{
			if (this.comps != null)
			{
				int i = 0;
				int count = this.comps.Count;
				while (i < count)
				{
					T t = this.comps[i] as T;
					if (t != null)
					{
						return t;
					}
					i++;
				}
			}
			return default(T);
		}

		// Token: 0x060016F3 RID: 5875 RVA: 0x000842E7 File Offset: 0x000824E7
		public IEnumerable<T> GetComps<T>() where T : ThingComp
		{
			if (this.comps != null)
			{
				int num;
				for (int i = 0; i < this.comps.Count; i = num + 1)
				{
					T t = this.comps[i] as T;
					if (t != null)
					{
						yield return t;
					}
					num = i;
				}
			}
			yield break;
		}

		// Token: 0x060016F4 RID: 5876 RVA: 0x000842F8 File Offset: 0x000824F8
		public ThingComp GetCompByDef(CompProperties def)
		{
			if (this.comps != null)
			{
				int i = 0;
				int count = this.comps.Count;
				while (i < count)
				{
					if (this.comps[i].props == def)
					{
						return this.comps[i];
					}
					i++;
				}
			}
			return null;
		}

		// Token: 0x060016F5 RID: 5877 RVA: 0x00084348 File Offset: 0x00082548
		public void InitializeComps()
		{
			if (this.def.comps.Any<CompProperties>())
			{
				this.comps = new List<ThingComp>();
				for (int i = 0; i < this.def.comps.Count; i++)
				{
					ThingComp thingComp = null;
					try
					{
						thingComp = (ThingComp)Activator.CreateInstance(this.def.comps[i].compClass);
						thingComp.parent = this;
						this.comps.Add(thingComp);
						thingComp.Initialize(this.def.comps[i]);
					}
					catch (Exception arg)
					{
						Log.Error("Could not instantiate or initialize a ThingComp: " + arg, false);
						this.comps.Remove(thingComp);
					}
				}
			}
		}

		// Token: 0x060016F6 RID: 5878 RVA: 0x00084414 File Offset: 0x00082614
		public override string GetCustomLabelNoCount(bool includeHp = true)
		{
			string text = base.GetCustomLabelNoCount(includeHp);
			if (this.comps != null)
			{
				int i = 0;
				int count = this.comps.Count;
				while (i < count)
				{
					text = this.comps[i].TransformLabel(text);
					i++;
				}
			}
			return text;
		}

		// Token: 0x060016F7 RID: 5879 RVA: 0x00084460 File Offset: 0x00082660
		public override void ExposeData()
		{
			base.ExposeData();
			if (Scribe.mode == LoadSaveMode.LoadingVars)
			{
				this.InitializeComps();
			}
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PostExposeData();
				}
			}
		}

		// Token: 0x060016F8 RID: 5880 RVA: 0x000844B0 File Offset: 0x000826B0
		public void BroadcastCompSignal(string signal)
		{
			this.ReceiveCompSignal(signal);
			if (this.comps != null)
			{
				int i = 0;
				int count = this.comps.Count;
				while (i < count)
				{
					this.comps[i].ReceiveCompSignal(signal);
					i++;
				}
			}
		}

		// Token: 0x060016F9 RID: 5881 RVA: 0x00002681 File Offset: 0x00000881
		protected virtual void ReceiveCompSignal(string signal)
		{
		}

		// Token: 0x060016FA RID: 5882 RVA: 0x000844F8 File Offset: 0x000826F8
		public override void SpawnSetup(Map map, bool respawningAfterLoad)
		{
			base.SpawnSetup(map, respawningAfterLoad);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PostSpawnSetup(respawningAfterLoad);
				}
			}
		}

		// Token: 0x060016FB RID: 5883 RVA: 0x00084540 File Offset: 0x00082740
		public override void DeSpawn(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			base.DeSpawn(mode);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PostDeSpawn(map);
				}
			}
		}

		// Token: 0x060016FC RID: 5884 RVA: 0x0008458C File Offset: 0x0008278C
		public override void Destroy(DestroyMode mode = DestroyMode.Vanish)
		{
			Map map = base.Map;
			base.Destroy(mode);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PostDestroy(mode, map);
				}
			}
		}

		// Token: 0x060016FD RID: 5885 RVA: 0x000845D8 File Offset: 0x000827D8
		public override void Tick()
		{
			if (this.comps != null)
			{
				int i = 0;
				int count = this.comps.Count;
				while (i < count)
				{
					this.comps[i].CompTick();
					i++;
				}
			}
		}

		// Token: 0x060016FE RID: 5886 RVA: 0x00084618 File Offset: 0x00082818
		public override void TickRare()
		{
			if (this.comps != null)
			{
				int i = 0;
				int count = this.comps.Count;
				while (i < count)
				{
					this.comps[i].CompTickRare();
					i++;
				}
			}
		}

		// Token: 0x060016FF RID: 5887 RVA: 0x00084658 File Offset: 0x00082858
		public override void PreApplyDamage(ref DamageInfo dinfo, out bool absorbed)
		{
			base.PreApplyDamage(ref dinfo, out absorbed);
			if (absorbed)
			{
				return;
			}
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PostPreApplyDamage(dinfo, out absorbed);
					if (absorbed)
					{
						return;
					}
				}
			}
		}

		// Token: 0x06001700 RID: 5888 RVA: 0x000846B0 File Offset: 0x000828B0
		public override void PostApplyDamage(DamageInfo dinfo, float totalDamageDealt)
		{
			base.PostApplyDamage(dinfo, totalDamageDealt);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PostPostApplyDamage(dinfo, totalDamageDealt);
				}
			}
		}

		// Token: 0x06001701 RID: 5889 RVA: 0x000846F6 File Offset: 0x000828F6
		public override void Draw()
		{
			base.Draw();
			this.Comps_PostDraw();
		}

		// Token: 0x06001702 RID: 5890 RVA: 0x00084704 File Offset: 0x00082904
		protected void Comps_PostDraw()
		{
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PostDraw();
				}
			}
		}

		// Token: 0x06001703 RID: 5891 RVA: 0x00084740 File Offset: 0x00082940
		public override void DrawExtraSelectionOverlays()
		{
			base.DrawExtraSelectionOverlays();
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PostDrawExtraSelectionOverlays();
				}
			}
		}

		// Token: 0x06001704 RID: 5892 RVA: 0x00084784 File Offset: 0x00082984
		public override void Print(SectionLayer layer)
		{
			base.Print(layer);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PostPrintOnto(layer);
				}
			}
		}

		// Token: 0x06001705 RID: 5893 RVA: 0x000847C8 File Offset: 0x000829C8
		public virtual void PrintForPowerGrid(SectionLayer layer)
		{
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].CompPrintForPowerGrid(layer);
				}
			}
		}

		// Token: 0x06001706 RID: 5894 RVA: 0x00084805 File Offset: 0x00082A05
		public override IEnumerable<Gizmo> GetGizmos()
		{
			if (this.comps != null)
			{
				int num;
				for (int i = 0; i < this.comps.Count; i = num + 1)
				{
					foreach (Gizmo gizmo in this.comps[i].CompGetGizmosExtra())
					{
						yield return gizmo;
					}
					IEnumerator<Gizmo> enumerator = null;
					num = i;
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x06001707 RID: 5895 RVA: 0x00084818 File Offset: 0x00082A18
		public override bool TryAbsorbStack(Thing other, bool respectStackLimit)
		{
			if (!this.CanStackWith(other))
			{
				return false;
			}
			int count = ThingUtility.TryAbsorbStackNumToTake(this, other, respectStackLimit);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PreAbsorbStack(other, count);
				}
			}
			return base.TryAbsorbStack(other, respectStackLimit);
		}

		// Token: 0x06001708 RID: 5896 RVA: 0x00084874 File Offset: 0x00082A74
		public override Thing SplitOff(int count)
		{
			Thing thing = base.SplitOff(count);
			if (thing != null && this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PostSplitOff(thing);
				}
			}
			return thing;
		}

		// Token: 0x06001709 RID: 5897 RVA: 0x000848C0 File Offset: 0x00082AC0
		public override bool CanStackWith(Thing other)
		{
			if (!base.CanStackWith(other))
			{
				return false;
			}
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					if (!this.comps[i].AllowStackWith(other))
					{
						return false;
					}
				}
			}
			return true;
		}

		// Token: 0x0600170A RID: 5898 RVA: 0x00084910 File Offset: 0x00082B10
		public override string GetInspectString()
		{
			StringBuilder stringBuilder = new StringBuilder();
			stringBuilder.Append(base.GetInspectString());
			string text = this.InspectStringPartsFromComps();
			if (!text.NullOrEmpty())
			{
				if (stringBuilder.Length > 0)
				{
					stringBuilder.AppendLine();
				}
				stringBuilder.Append(text);
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600170B RID: 5899 RVA: 0x00084960 File Offset: 0x00082B60
		protected string InspectStringPartsFromComps()
		{
			if (this.comps == null)
			{
				return null;
			}
			StringBuilder stringBuilder = new StringBuilder();
			for (int i = 0; i < this.comps.Count; i++)
			{
				string text = this.comps[i].CompInspectStringExtra();
				if (!text.NullOrEmpty())
				{
					if (Prefs.DevMode && char.IsWhiteSpace(text[text.Length - 1]))
					{
						Log.ErrorOnce(this.comps[i].GetType() + " CompInspectStringExtra ended with whitespace: " + text, 25612, false);
						text = text.TrimEndNewlines();
					}
					if (stringBuilder.Length != 0)
					{
						stringBuilder.AppendLine();
					}
					stringBuilder.Append(text);
				}
			}
			return stringBuilder.ToString();
		}

		// Token: 0x0600170C RID: 5900 RVA: 0x00084A18 File Offset: 0x00082C18
		public override void DrawGUIOverlay()
		{
			base.DrawGUIOverlay();
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].DrawGUIOverlay();
				}
			}
		}

		// Token: 0x0600170D RID: 5901 RVA: 0x00084A5A File Offset: 0x00082C5A
		public override IEnumerable<FloatMenuOption> GetFloatMenuOptions(Pawn selPawn)
		{
			foreach (FloatMenuOption floatMenuOption in this.<>n__0(selPawn))
			{
				yield return floatMenuOption;
			}
			IEnumerator<FloatMenuOption> enumerator = null;
			if (this.comps != null)
			{
				int num;
				for (int i = 0; i < this.comps.Count; i = num + 1)
				{
					foreach (FloatMenuOption floatMenuOption2 in this.comps[i].CompFloatMenuOptions(selPawn))
					{
						yield return floatMenuOption2;
					}
					enumerator = null;
					num = i;
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x0600170E RID: 5902 RVA: 0x00084A74 File Offset: 0x00082C74
		public override void PreTraded(TradeAction action, Pawn playerNegotiator, ITrader trader)
		{
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PrePreTraded(action, playerNegotiator, trader);
				}
			}
			base.PreTraded(action, playerNegotiator, trader);
		}

		// Token: 0x0600170F RID: 5903 RVA: 0x00084ABC File Offset: 0x00082CBC
		public override void PostGeneratedForTrader(TraderKindDef trader, int forTile, Faction forFaction)
		{
			base.PostGeneratedForTrader(trader, forTile, forFaction);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PostPostGeneratedForTrader(trader, forTile, forFaction);
				}
			}
		}

		// Token: 0x06001710 RID: 5904 RVA: 0x00084B04 File Offset: 0x00082D04
		protected override void PrePostIngested(Pawn ingester)
		{
			base.PrePostIngested(ingester);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PrePostIngested(ingester);
				}
			}
		}

		// Token: 0x06001711 RID: 5905 RVA: 0x00084B48 File Offset: 0x00082D48
		protected override void PostIngested(Pawn ingester)
		{
			base.PostIngested(ingester);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].PostIngested(ingester);
				}
			}
		}

		// Token: 0x06001712 RID: 5906 RVA: 0x00084B8C File Offset: 0x00082D8C
		public override void Notify_SignalReceived(Signal signal)
		{
			base.Notify_SignalReceived(signal);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].Notify_SignalReceived(signal);
				}
			}
		}

		// Token: 0x06001713 RID: 5907 RVA: 0x00084BD0 File Offset: 0x00082DD0
		public override void Notify_LordDestroyed()
		{
			base.Notify_LordDestroyed();
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].Notify_LordDestroyed();
				}
			}
		}

		// Token: 0x06001714 RID: 5908 RVA: 0x00084C14 File Offset: 0x00082E14
		public override void Notify_Equipped(Pawn pawn)
		{
			base.Notify_Equipped(pawn);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].Notify_Equipped(pawn);
				}
			}
		}

		// Token: 0x06001715 RID: 5909 RVA: 0x00084C58 File Offset: 0x00082E58
		public override void Notify_UsedWeapon(Pawn pawn)
		{
			base.Notify_UsedWeapon(pawn);
			if (this.comps != null)
			{
				for (int i = 0; i < this.comps.Count; i++)
				{
					this.comps[i].Notify_UsedWeapon(pawn);
				}
			}
		}

		// Token: 0x06001716 RID: 5910 RVA: 0x00084C9C File Offset: 0x00082E9C
		public override IEnumerable<StatDrawEntry> SpecialDisplayStats()
		{
			foreach (StatDrawEntry statDrawEntry in this.<>n__1())
			{
				yield return statDrawEntry;
			}
			IEnumerator<StatDrawEntry> enumerator = null;
			if (this.comps != null)
			{
				int num;
				for (int i = 0; i < this.comps.Count; i = num + 1)
				{
					IEnumerable<StatDrawEntry> enumerable = this.comps[i].SpecialDisplayStats();
					if (enumerable != null)
					{
						foreach (StatDrawEntry statDrawEntry2 in enumerable)
						{
							yield return statDrawEntry2;
						}
						enumerator = null;
					}
					num = i;
				}
			}
			yield break;
			yield break;
		}

		// Token: 0x06001717 RID: 5911 RVA: 0x00084CAC File Offset: 0x00082EAC
		public override void Notify_Explosion(Explosion explosion)
		{
			base.Notify_Explosion(explosion);
			CompWakeUpDormant comp = this.GetComp<CompWakeUpDormant>();
			if (comp != null && (explosion.Position - base.Position).LengthHorizontal <= explosion.radius)
			{
				comp.Activate(true, false);
			}
		}

		// Token: 0x04000E92 RID: 3730
		private List<ThingComp> comps;

		// Token: 0x04000E93 RID: 3731
		private static readonly List<ThingComp> EmptyCompsList = new List<ThingComp>();
	}
}
