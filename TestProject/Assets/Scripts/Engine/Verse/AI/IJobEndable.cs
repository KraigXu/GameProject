using System;

namespace Verse.AI
{
	// Token: 0x02000529 RID: 1321
	public interface IJobEndable
	{
		// Token: 0x060025DF RID: 9695
		Pawn GetActor();

		// Token: 0x060025E0 RID: 9696
		void AddEndCondition(Func<JobCondition> newEndCondition);
	}
}
