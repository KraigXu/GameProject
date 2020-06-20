using System;

namespace Verse
{
	// Token: 0x02000408 RID: 1032
	public static class DataSerializeUtility
	{
		// Token: 0x06001E73 RID: 7795 RVA: 0x000BE1A4 File Offset: 0x000BC3A4
		public static byte[] SerializeByte(int elements, Func<int, byte> reader)
		{
			byte[] array = new byte[elements];
			for (int i = 0; i < elements; i++)
			{
				array[i] = reader(i);
			}
			return array;
		}

		// Token: 0x06001E74 RID: 7796 RVA: 0x0006461A File Offset: 0x0006281A
		public static byte[] SerializeByte(byte[] data)
		{
			return data;
		}

		// Token: 0x06001E75 RID: 7797 RVA: 0x0006461A File Offset: 0x0006281A
		public static byte[] DeserializeByte(byte[] data)
		{
			return data;
		}

		// Token: 0x06001E76 RID: 7798 RVA: 0x000BE1D0 File Offset: 0x000BC3D0
		public static void LoadByte(byte[] arr, int elements, Action<int, byte> writer)
		{
			if (arr == null || arr.Length == 0)
			{
				return;
			}
			for (int i = 0; i < elements; i++)
			{
				writer(i, arr[i]);
			}
		}

		// Token: 0x06001E77 RID: 7799 RVA: 0x000BE1FC File Offset: 0x000BC3FC
		public static byte[] SerializeUshort(int elements, Func<int, ushort> reader)
		{
			byte[] array = new byte[elements * 2];
			for (int i = 0; i < elements; i++)
			{
				ushort num = reader(i);
				array[i * 2] = (byte)(num & 255);
				array[i * 2 + 1] = (byte)(num >> 8 & 255);
			}
			return array;
		}

		// Token: 0x06001E78 RID: 7800 RVA: 0x000BE248 File Offset: 0x000BC448
		public static byte[] SerializeUshort(ushort[] data)
		{
			return DataSerializeUtility.SerializeUshort(data.Length, (int i) => data[i]);
		}

		// Token: 0x06001E79 RID: 7801 RVA: 0x000BE27C File Offset: 0x000BC47C
		public static ushort[] DeserializeUshort(byte[] data)
		{
			ushort[] result = new ushort[data.Length / 2];
			DataSerializeUtility.LoadUshort(data, result.Length, delegate(int i, ushort dat)
			{
				result[i] = dat;
			});
			return result;
		}

		// Token: 0x06001E7A RID: 7802 RVA: 0x000BE2C0 File Offset: 0x000BC4C0
		public static void LoadUshort(byte[] arr, int elements, Action<int, ushort> writer)
		{
			if (arr == null || arr.Length == 0)
			{
				return;
			}
			for (int i = 0; i < elements; i++)
			{
				writer(i, (ushort)((int)arr[i * 2] | (int)arr[i * 2 + 1] << 8));
			}
		}

		// Token: 0x06001E7B RID: 7803 RVA: 0x000BE2F8 File Offset: 0x000BC4F8
		public static byte[] SerializeInt(int elements, Func<int, int> reader)
		{
			byte[] array = new byte[elements * 4];
			for (int i = 0; i < elements; i++)
			{
				int num = reader(i);
				array[i * 4] = (byte)(num & 255);
				array[i * 4 + 1] = (byte)(num >> 8 & 255);
				array[i * 4 + 2] = (byte)(num >> 16 & 255);
				array[i * 4 + 3] = (byte)(num >> 24 & 255);
			}
			return array;
		}

		// Token: 0x06001E7C RID: 7804 RVA: 0x000BE368 File Offset: 0x000BC568
		public static byte[] SerializeInt(int[] data)
		{
			return DataSerializeUtility.SerializeInt(data.Length, (int i) => data[i]);
		}

		// Token: 0x06001E7D RID: 7805 RVA: 0x000BE39C File Offset: 0x000BC59C
		public static int[] DeserializeInt(byte[] data)
		{
			int[] result = new int[data.Length / 4];
			DataSerializeUtility.LoadInt(data, result.Length, delegate(int i, int dat)
			{
				result[i] = dat;
			});
			return result;
		}

		// Token: 0x06001E7E RID: 7806 RVA: 0x000BE3E0 File Offset: 0x000BC5E0
		public static void LoadInt(byte[] arr, int elements, Action<int, int> writer)
		{
			if (arr == null || arr.Length == 0)
			{
				return;
			}
			for (int i = 0; i < elements; i++)
			{
				writer(i, (int)arr[i * 4] | (int)arr[i * 4 + 1] << 8 | (int)arr[i * 4 + 2] << 16 | (int)arr[i * 4 + 3] << 24);
			}
		}
	}
}
