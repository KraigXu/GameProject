using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace World
{
    public static class Current
    {
		private static Game _game;

		private static Root _root;

		public static Root Root
		{
			get
			{
				return Current._root;
			}
		}

		public static Game Game
		{
			get
			{
				return Current._game;
			}
			set
			{
				Current._game = value;
			}
		}


	}
}
