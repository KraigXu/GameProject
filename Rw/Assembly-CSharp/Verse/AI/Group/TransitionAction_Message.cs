using System;
using System.Linq;
using RimWorld;

namespace Verse.AI.Group
{
	// Token: 0x020005E1 RID: 1505
	public class TransitionAction_Message : TransitionAction
	{
		// Token: 0x060029DA RID: 10714 RVA: 0x000F58D4 File Offset: 0x000F3AD4
		public TransitionAction_Message(string message, string repeatAvoiderTag = null, float repeatAvoiderSeconds = 1f) : this(message, MessageTypeDefOf.NeutralEvent, repeatAvoiderTag, repeatAvoiderSeconds)
		{
		}

		// Token: 0x060029DB RID: 10715 RVA: 0x000F58E4 File Offset: 0x000F3AE4
		public TransitionAction_Message(string message, MessageTypeDef messageType, string repeatAvoiderTag = null, float repeatAvoiderSeconds = 1f)
		{
			this.lookTarget = TargetInfo.Invalid;
			base..ctor();
			this.message = message;
			this.type = messageType;
			this.repeatAvoiderTag = repeatAvoiderTag;
			this.repeatAvoiderSeconds = repeatAvoiderSeconds;
		}

		// Token: 0x060029DC RID: 10716 RVA: 0x000F5914 File Offset: 0x000F3B14
		public TransitionAction_Message(string message, MessageTypeDef messageType, TargetInfo lookTarget, string repeatAvoiderTag = null, float repeatAvoiderSeconds = 1f)
		{
			this.lookTarget = TargetInfo.Invalid;
			base..ctor();
			this.message = message;
			this.type = messageType;
			this.lookTarget = lookTarget;
			this.repeatAvoiderTag = repeatAvoiderTag;
			this.repeatAvoiderSeconds = repeatAvoiderSeconds;
		}

		// Token: 0x060029DD RID: 10717 RVA: 0x000F594C File Offset: 0x000F3B4C
		public TransitionAction_Message(string message, MessageTypeDef messageType, Func<TargetInfo> lookTargetGetter, string repeatAvoiderTag = null, float repeatAvoiderSeconds = 1f)
		{
			this.lookTarget = TargetInfo.Invalid;
			base..ctor();
			this.message = message;
			this.type = messageType;
			this.lookTargetGetter = lookTargetGetter;
			this.repeatAvoiderTag = repeatAvoiderTag;
			this.repeatAvoiderSeconds = repeatAvoiderSeconds;
		}

		// Token: 0x060029DE RID: 10718 RVA: 0x000F5984 File Offset: 0x000F3B84
		public override void DoAction(Transition trans)
		{
			if (!this.repeatAvoiderTag.NullOrEmpty() && !MessagesRepeatAvoider.MessageShowAllowed(this.repeatAvoiderTag, this.repeatAvoiderSeconds))
			{
				return;
			}
			TargetInfo target = (this.lookTargetGetter != null) ? this.lookTargetGetter() : this.lookTarget;
			if (!target.IsValid)
			{
				target = trans.target.lord.ownedPawns.FirstOrDefault<Pawn>();
			}
			Messages.Message(this.message, target, this.type, true);
		}

		// Token: 0x04001910 RID: 6416
		public string message;

		// Token: 0x04001911 RID: 6417
		public MessageTypeDef type;

		// Token: 0x04001912 RID: 6418
		public TargetInfo lookTarget;

		// Token: 0x04001913 RID: 6419
		public Func<TargetInfo> lookTargetGetter;

		// Token: 0x04001914 RID: 6420
		public string repeatAvoiderTag;

		// Token: 0x04001915 RID: 6421
		public float repeatAvoiderSeconds;
	}
}
