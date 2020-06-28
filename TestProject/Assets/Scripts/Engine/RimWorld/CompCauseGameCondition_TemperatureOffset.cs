using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	// Token: 0x02000CE2 RID: 3298
	public class CompCauseGameCondition_TemperatureOffset : CompCauseGameCondition
	{
		// Token: 0x17000E10 RID: 3600
		// (get) Token: 0x06005006 RID: 20486 RVA: 0x001AFA4F File Offset: 0x001ADC4F
		public new CompProperties_CausesGameCondition_ClimateAdjuster Props
		{
			get
			{
				return (CompProperties_CausesGameCondition_ClimateAdjuster)this.props;
			}
		}

		// Token: 0x06005007 RID: 20487 RVA: 0x001AFA5C File Offset: 0x001ADC5C
		public override void Initialize(CompProperties props)
		{
			base.Initialize(props);
			this.temperatureOffset = this.Props.temperatureOffsetRange.min;
		}

		// Token: 0x06005008 RID: 20488 RVA: 0x001AFA7B File Offset: 0x001ADC7B
		public override void PostExposeData()
		{
			base.PostExposeData();
			Scribe_Values.Look<float>(ref this.temperatureOffset, "temperatureOffset", 0f, false);
		}

		// Token: 0x06005009 RID: 20489 RVA: 0x001AFA99 File Offset: 0x001ADC99
		private string GetFloatStringWithSign(float val)
		{
			if (val < 0f)
			{
				return val.ToString("0");
			}
			return "+" + val.ToString("0");
		}

		// Token: 0x0600500A RID: 20490 RVA: 0x001AFAC6 File Offset: 0x001ADCC6
		public void SetTemperatureOffset(float offset)
		{
			this.temperatureOffset = this.Props.temperatureOffsetRange.ClampToRange(offset);
			base.ReSetupAllConditions();
		}

		// Token: 0x0600500B RID: 20491 RVA: 0x001AFAE5 File Offset: 0x001ADCE5
		public override IEnumerable<Gizmo> CompGetGizmosExtra()
		{
			if (!Prefs.DevMode)
			{
				yield break;
			}
			Command_Action command_Action = new Command_Action();
			command_Action.defaultLabel = "-10";
			Command_Action command_Action2 = command_Action;
			command_Action2.action = (Action)Delegate.Combine(command_Action2.action, new Action(delegate
			{
				this.SetTemperatureOffset(this.temperatureOffset - 10f);
			}));
			command_Action.hotKey = KeyBindingDefOf.Misc1;
			yield return command_Action;
			Command_Action command_Action3 = new Command_Action();
			command_Action3.defaultLabel = "-1";
			Command_Action command_Action4 = command_Action3;
			command_Action4.action = (Action)Delegate.Combine(command_Action4.action, new Action(delegate
			{
				this.SetTemperatureOffset(this.temperatureOffset - 1f);
			}));
			command_Action3.hotKey = KeyBindingDefOf.Misc2;
			yield return command_Action3;
			Command_Action command_Action5 = new Command_Action();
			command_Action5.defaultLabel = "+1";
			Command_Action command_Action6 = command_Action5;
			command_Action6.action = (Action)Delegate.Combine(command_Action6.action, new Action(delegate
			{
				this.SetTemperatureOffset(this.temperatureOffset + 1f);
			}));
			command_Action5.hotKey = KeyBindingDefOf.Misc3;
			yield return command_Action5;
			Command_Action command_Action7 = new Command_Action();
			command_Action7.defaultLabel = "+10";
			Command_Action command_Action8 = command_Action7;
			command_Action8.action = (Action)Delegate.Combine(command_Action8.action, new Action(delegate
			{
				this.SetTemperatureOffset(this.temperatureOffset + 10f);
			}));
			command_Action7.hotKey = KeyBindingDefOf.Misc4;
			yield return command_Action7;
			yield break;
		}

		// Token: 0x0600500C RID: 20492 RVA: 0x001AFAF8 File Offset: 0x001ADCF8
		public override string CompInspectStringExtra()
		{
			string text = base.CompInspectStringExtra();
			if (!text.NullOrEmpty())
			{
				text += "\n";
			}
			return text + ("Temperature".Translate() + ": " + this.GetFloatStringWithSign(this.temperatureOffset));
		}

		// Token: 0x0600500D RID: 20493 RVA: 0x001AFB52 File Offset: 0x001ADD52
		protected override void SetupCondition(GameCondition condition, Map map)
		{
			base.SetupCondition(condition, map);
			((GameCondition_TemperatureOffset)condition).tempOffset = this.temperatureOffset;
		}

		// Token: 0x0600500E RID: 20494 RVA: 0x001AFB6D File Offset: 0x001ADD6D
		public override void RandomizeSettings()
		{
			this.temperatureOffset = (Rand.Bool ? this.Props.temperatureOffsetRange.min : this.Props.temperatureOffsetRange.max);
		}

		// Token: 0x04002CB6 RID: 11446
		public float temperatureOffset;
	}
}
