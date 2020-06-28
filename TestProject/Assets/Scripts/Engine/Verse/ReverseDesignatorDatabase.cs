using System;
using System.Collections.Generic;
using RimWorld;

namespace Verse
{
	// Token: 0x02000331 RID: 817
	public class ReverseDesignatorDatabase
	{
		// Token: 0x170004F3 RID: 1267
		// (get) Token: 0x060017FF RID: 6143 RVA: 0x00088CCE File Offset: 0x00086ECE
		public List<Designator> AllDesignators
		{
			get
			{
				if (this.desList == null)
				{
					this.InitDesignators();
				}
				return this.desList;
			}
		}

		// Token: 0x06001800 RID: 6144 RVA: 0x00088CE4 File Offset: 0x00086EE4
		public void Reinit()
		{
			this.desList = null;
		}

		// Token: 0x06001801 RID: 6145 RVA: 0x00088CF0 File Offset: 0x00086EF0
		public T Get<T>() where T : Designator
		{
			if (this.desList == null)
			{
				this.InitDesignators();
			}
			for (int i = 0; i < this.desList.Count; i++)
			{
				T t = this.desList[i] as T;
				if (t != null)
				{
					return t;
				}
			}
			return default(T);
		}

		// Token: 0x06001802 RID: 6146 RVA: 0x00088D4C File Offset: 0x00086F4C
		private void InitDesignators()
		{
			this.desList = new List<Designator>();
			this.desList.Add(new Designator_Cancel());
			this.desList.Add(new Designator_Claim());
			this.desList.Add(new Designator_Deconstruct());
			this.desList.Add(new Designator_Uninstall());
			this.desList.Add(new Designator_Haul());
			this.desList.Add(new Designator_Hunt());
			this.desList.Add(new Designator_Slaughter());
			this.desList.Add(new Designator_Tame());
			this.desList.Add(new Designator_PlantsCut());
			this.desList.Add(new Designator_PlantsHarvest());
			this.desList.Add(new Designator_PlantsHarvestWood());
			this.desList.Add(new Designator_Mine());
			this.desList.Add(new Designator_Strip());
			this.desList.Add(new Designator_Open());
			this.desList.Add(new Designator_SmoothSurface());
			this.desList.RemoveAll((Designator des) => !Current.Game.Rules.DesignatorAllowed(des));
		}

		// Token: 0x04000F05 RID: 3845
		private List<Designator> desList;
	}
}
