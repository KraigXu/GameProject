using System;

namespace Verse.AI
{
	// Token: 0x020005AA RID: 1450
	public struct ThinkResult : IEquatable<ThinkResult>
	{
		// Token: 0x170007B5 RID: 1973
		// (get) Token: 0x060028A8 RID: 10408 RVA: 0x000EF668 File Offset: 0x000ED868
		public Job Job
		{
			get
			{
				return this.jobInt;
			}
		}

		// Token: 0x170007B6 RID: 1974
		// (get) Token: 0x060028A9 RID: 10409 RVA: 0x000EF670 File Offset: 0x000ED870
		public ThinkNode SourceNode
		{
			get
			{
				return this.sourceNodeInt;
			}
		}

		// Token: 0x170007B7 RID: 1975
		// (get) Token: 0x060028AA RID: 10410 RVA: 0x000EF678 File Offset: 0x000ED878
		public JobTag? Tag
		{
			get
			{
				return this.tag;
			}
		}

		// Token: 0x170007B8 RID: 1976
		// (get) Token: 0x060028AB RID: 10411 RVA: 0x000EF680 File Offset: 0x000ED880
		public bool FromQueue
		{
			get
			{
				return this.fromQueue;
			}
		}

		// Token: 0x170007B9 RID: 1977
		// (get) Token: 0x060028AC RID: 10412 RVA: 0x000EF688 File Offset: 0x000ED888
		public bool IsValid
		{
			get
			{
				return this.Job != null;
			}
		}

		// Token: 0x170007BA RID: 1978
		// (get) Token: 0x060028AD RID: 10413 RVA: 0x000EF694 File Offset: 0x000ED894
		public static ThinkResult NoJob
		{
			get
			{
				return new ThinkResult(null, null, null, false);
			}
		}

		// Token: 0x060028AE RID: 10414 RVA: 0x000EF6B2 File Offset: 0x000ED8B2
		public ThinkResult(Job job, ThinkNode sourceNode, JobTag? tag = null, bool fromQueue = false)
		{
			this.jobInt = job;
			this.sourceNodeInt = sourceNode;
			this.tag = tag;
			this.fromQueue = fromQueue;
		}

		// Token: 0x060028AF RID: 10415 RVA: 0x000EF6D4 File Offset: 0x000ED8D4
		public override string ToString()
		{
			string text = (this.Job != null) ? this.Job.ToString() : "null";
			string text2 = (this.SourceNode != null) ? this.SourceNode.ToString() : "null";
			return string.Concat(new string[]
			{
				"(job=",
				text,
				" sourceNode=",
				text2,
				")"
			});
		}

		// Token: 0x060028B0 RID: 10416 RVA: 0x000EF742 File Offset: 0x000ED942
		public override int GetHashCode()
		{
			return Gen.HashCombineStruct<bool>(Gen.HashCombine<JobTag?>(Gen.HashCombine<ThinkNode>(Gen.HashCombine<Job>(0, this.jobInt), this.sourceNodeInt), this.tag), this.fromQueue);
		}

		// Token: 0x060028B1 RID: 10417 RVA: 0x000EF771 File Offset: 0x000ED971
		public override bool Equals(object obj)
		{
			return obj is ThinkResult && this.Equals((ThinkResult)obj);
		}

		// Token: 0x060028B2 RID: 10418 RVA: 0x000EF78C File Offset: 0x000ED98C
		public bool Equals(ThinkResult other)
		{
			if (this.jobInt == other.jobInt && this.sourceNodeInt == other.sourceNodeInt)
			{
				JobTag? jobTag = this.tag;
				JobTag? jobTag2 = other.tag;
				if (jobTag.GetValueOrDefault() == jobTag2.GetValueOrDefault() & jobTag != null == (jobTag2 != null))
				{
					return this.fromQueue == other.fromQueue;
				}
			}
			return false;
		}

		// Token: 0x060028B3 RID: 10419 RVA: 0x000EF7F6 File Offset: 0x000ED9F6
		public static bool operator ==(ThinkResult lhs, ThinkResult rhs)
		{
			return lhs.Equals(rhs);
		}

		// Token: 0x060028B4 RID: 10420 RVA: 0x000EF800 File Offset: 0x000EDA00
		public static bool operator !=(ThinkResult lhs, ThinkResult rhs)
		{
			return !(lhs == rhs);
		}

		// Token: 0x04001864 RID: 6244
		private Job jobInt;

		// Token: 0x04001865 RID: 6245
		private ThinkNode sourceNodeInt;

		// Token: 0x04001866 RID: 6246
		private JobTag? tag;

		// Token: 0x04001867 RID: 6247
		private bool fromQueue;
	}
}
