using System;

namespace Verse.AI
{
	
	public struct ThinkResult : IEquatable<ThinkResult>
	{
		
		
		public Job Job
		{
			get
			{
				return this.jobInt;
			}
		}

		
		
		public ThinkNode SourceNode
		{
			get
			{
				return this.sourceNodeInt;
			}
		}

		
		
		public JobTag? Tag
		{
			get
			{
				return this.tag;
			}
		}

		
		
		public bool FromQueue
		{
			get
			{
				return this.fromQueue;
			}
		}

		
		
		public bool IsValid
		{
			get
			{
				return this.Job != null;
			}
		}

		
		
		public static ThinkResult NoJob
		{
			get
			{
				return new ThinkResult(null, null, null, false);
			}
		}

		
		public ThinkResult(Job job, ThinkNode sourceNode, JobTag? tag = null, bool fromQueue = false)
		{
			this.jobInt = job;
			this.sourceNodeInt = sourceNode;
			this.tag = tag;
			this.fromQueue = fromQueue;
		}

		
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

		
		public override int GetHashCode()
		{
			return Gen.HashCombineStruct<bool>(Gen.HashCombine<JobTag?>(Gen.HashCombine<ThinkNode>(Gen.HashCombine<Job>(0, this.jobInt), this.sourceNodeInt), this.tag), this.fromQueue);
		}

		
		public override bool Equals(object obj)
		{
			return obj is ThinkResult && this.Equals((ThinkResult)obj);
		}

		
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

		
		public static bool operator ==(ThinkResult lhs, ThinkResult rhs)
		{
			return lhs.Equals(rhs);
		}

		
		public static bool operator !=(ThinkResult lhs, ThinkResult rhs)
		{
			return !(lhs == rhs);
		}

		
		private Job jobInt;

		
		private ThinkNode sourceNodeInt;

		
		private JobTag? tag;

		
		private bool fromQueue;
	}
}
