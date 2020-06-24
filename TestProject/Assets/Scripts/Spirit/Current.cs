using RimWorld.Planet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Spirit
{
    public static class Current
    {
		private static Game _game;

		private static Root _root;

		private static ProgramState _programState;

		private static World _creatingWorldInt;

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

		public static ProgramState ProgramState
		{
			get
			{
				return Current._programState;
			}
			set
			{
				Current._programState = value;
			}
		}
		public static World CreatingWorld
		{
			get
			{
				return Current._creatingWorldInt;
			}
			set
			{
				Current._creatingWorldInt = value;
			}
		}

	}
}
