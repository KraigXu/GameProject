using System;
using System.Linq;
using Verse;

namespace RimWorld
{
	// Token: 0x02000C36 RID: 3126
	public static class TaleFactory
	{
		// Token: 0x06004A84 RID: 19076 RVA: 0x001930C8 File Offset: 0x001912C8
		public static Tale MakeRawTale(TaleDef def, params object[] args)
		{
			Tale result;
			try
			{
				Tale tale = (Tale)Activator.CreateInstance(def.taleClass, args);
				tale.def = def;
				tale.id = Find.UniqueIDsManager.GetNextTaleID();
				tale.date = Find.TickManager.TicksAbs;
				result = tale;
			}
			catch (Exception arg)
			{
				Exception arg2;
				Log.Error(string.Format("Failed to create tale object {0} with parameters {1}: {2}", def, (from arg in args
				select arg.ToStringSafe<object>()).ToCommaList(false), arg2), false);
				result = null;
			}
			return result;
		}

		// Token: 0x06004A85 RID: 19077 RVA: 0x00193164 File Offset: 0x00191364
		public static Tale MakeRandomTestTale(TaleDef def = null)
		{
			if (def == null)
			{
				def = (from d in DefDatabase<TaleDef>.AllDefs
				where d.usableForArt
				select d).RandomElement<TaleDef>();
			}
			Tale tale = TaleFactory.MakeRawTale(def, Array.Empty<object>());
			tale.GenerateTestData();
			return tale;
		}
	}
}
