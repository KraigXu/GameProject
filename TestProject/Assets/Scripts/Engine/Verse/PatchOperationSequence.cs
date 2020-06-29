﻿using System;
using System.Collections.Generic;
using System.Xml;

namespace Verse
{
	
	public class PatchOperationSequence : PatchOperation
	{
		
		protected override bool ApplyWorker(XmlDocument xml)
		{
			foreach (PatchOperation patchOperation in this.operations)
			{
				if (!patchOperation.Apply(xml))
				{
					this.lastFailedOperation = patchOperation;
					return false;
				}
			}
			return true;
		}

		
		public override void Complete(string modIdentifier)
		{
			base.Complete(modIdentifier);
			this.lastFailedOperation = null;
		}

		
		public override string ToString()
		{
			int num = (this.operations != null) ? this.operations.Count : 0;
			string text = string.Format("{0}(count={1}", base.ToString(), num);
			if (this.lastFailedOperation != null)
			{
				text = text + ", lastFailedOperation=" + this.lastFailedOperation;
			}
			return text + ")";
		}

		
		private List<PatchOperation> operations;

		
		private PatchOperation lastFailedOperation;
	}
}
