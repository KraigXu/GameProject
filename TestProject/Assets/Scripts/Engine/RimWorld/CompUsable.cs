using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Verse;
using Verse.AI;

namespace RimWorld
{
	
	public class CompUsable : ThingComp
	{
		
		// (get) Token: 0x06005403 RID: 21507 RVA: 0x001C0E19 File Offset: 0x001BF019
		public CompProperties_Usable Props
		{
			get
			{
				return (CompProperties_Usable)this.props;
			}
		}

		
		protected virtual string FloatMenuOptionLabel(Pawn pawn)
		{
			return this.Props.useLabel;
		}

		
		public override IEnumerable<FloatMenuOption> CompFloatMenuOptions(Pawn myPawn)
		{
			string text;
			if (!this.CanBeUsedBy(myPawn, out text))
			{
				yield return new FloatMenuOption(this.FloatMenuOptionLabel(myPawn) + ((text != null) ? (" (" + text + ")") : ""), null, MenuOptionPriority.Default, null, null, 0f, null, null);
			}
			else if (!myPawn.CanReach(this.parent, PathEndMode.Touch, Danger.Deadly, false, TraverseMode.ByPawn))
			{
				yield return new FloatMenuOption(this.FloatMenuOptionLabel(myPawn) + " (" + "NoPath".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null);
			}
			else if (!myPawn.CanReserve(this.parent, 1, -1, null, false))
			{
				yield return new FloatMenuOption(this.FloatMenuOptionLabel(myPawn) + " (" + "Reserved".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null);
			}
			else if (!myPawn.health.capacities.CapableOf(PawnCapacityDefOf.Manipulation))
			{
				yield return new FloatMenuOption(this.FloatMenuOptionLabel(myPawn) + " (" + "Incapable".Translate() + ")", null, MenuOptionPriority.Default, null, null, 0f, null, null);
			}
			else
			{
				FloatMenuOption floatMenuOption = new FloatMenuOption(this.FloatMenuOptionLabel(myPawn), delegate
				{
					if (myPawn.CanReserveAndReach(this.parent, PathEndMode.Touch, Danger.Deadly, 1, -1, null, false))
					{
						using (IEnumerator<CompUseEffect> enumerator = this.parent.GetComps<CompUseEffect>().GetEnumerator())
						{
							while (enumerator.MoveNext())
							{
								if (enumerator.Current.SelectedUseOption(myPawn))
								{
									return;
								}
							}
						}
						this.TryStartUseJob(myPawn, LocalTargetInfo.Invalid);
					}
				}, MenuOptionPriority.Default, null, null, 0f, null, null);
				yield return floatMenuOption;
			}
			yield break;
		}

		
		public virtual void TryStartUseJob(Pawn pawn, LocalTargetInfo extraTarget)
		{
			//CompUsable.c__DisplayClass4_0 c__DisplayClass4_ = new CompUsable.c__DisplayClass4_0();
			//c__DisplayClass4_.extraTarget = extraTarget;
			//c__DisplayClass4_.4__this = this;
			//c__DisplayClass4_.pawn = pawn;
			//if (!c__DisplayClass4_.pawn.CanReserveAndReach(this.parent, PathEndMode.Touch, Danger.Deadly, 1, -1, null, false))
			//{
			//	return;
			//}
			//string text;
			//if (!this.CanBeUsedBy(c__DisplayClass4_.pawn, out text))
			//{
			//	return;
			//}
			//StringBuilder stringBuilder = new StringBuilder();
			//foreach (CompUseEffect compUseEffect in this.parent.GetComps<CompUseEffect>())
			//{
			//	TaggedString taggedString = compUseEffect.ConfirmMessage(c__DisplayClass4_.pawn);
			//	if (!taggedString.NullOrEmpty())
			//	{
			//		if (stringBuilder.Length != 0)
			//		{
			//			stringBuilder.AppendLine();
			//		}
			//		stringBuilder.AppendTagged(taggedString);
			//	}
			//}
			//string str = stringBuilder.ToString();
			//if (str.NullOrEmpty())
			//{
			//	c__DisplayClass4_.<TryStartUseJob>g__StartJob|0();
			//	return;
			//}
			//Find.WindowStack.Add(Dialog_MessageBox.CreateConfirmation(str, delegate
			//{
			//	Job job = c__DisplayClass4_.extraTarget.IsValid ? JobMaker.MakeJob(c__DisplayClass4_.4__this.Props.useJob, c__DisplayClass4_.4__this.parent, c__DisplayClass4_.extraTarget) : JobMaker.MakeJob(c__DisplayClass4_.4__this.Props.useJob, c__DisplayClass4_.4__this.parent);
			//	c__DisplayClass4_.pawn.jobs.TryTakeOrderedJob(job, JobTag.Misc);
			//}, false, null));
		}

		
		public void UsedBy(Pawn p)
		{
			string text;
			if (!this.CanBeUsedBy(p, out text))
			{
				return;
			}
			foreach (CompUseEffect compUseEffect in from x in this.parent.GetComps<CompUseEffect>()
			orderby x.OrderPriority descending
			select x)
			{
				try
				{
					compUseEffect.DoEffect(p);
				}
				catch (Exception arg)
				{
					Log.Error("Error in CompUseEffect: " + arg, false);
				}
			}
		}

		
		private bool CanBeUsedBy(Pawn p, out string failReason)
		{
			List<ThingComp> allComps = this.parent.AllComps;
			for (int i = 0; i < allComps.Count; i++)
			{
				CompUseEffect compUseEffect = allComps[i] as CompUseEffect;
				if (compUseEffect != null && !compUseEffect.CanBeUsedBy(p, out failReason))
				{
					return false;
				}
			}
			failReason = null;
			return true;
		}
	}
}
