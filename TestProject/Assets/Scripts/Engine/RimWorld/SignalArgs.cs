using System;
using System.Collections.Generic;
using Verse;

namespace RimWorld
{
	
	public struct SignalArgs
	{
		
		
		public int Count
		{
			get
			{
				return this.count;
			}
		}

		
		
		public IEnumerable<NamedArgument> Args
		{
			get
			{
				if (this.count == 0)
				{
					yield break;
				}
				if (this.args != null)
				{
					int num;
					for (int i = 0; i < this.args.Length; i = num + 1)
					{
						yield return this.args[i];
						num = i;
					}
				}
				else
				{
					yield return this.arg1;
					if (this.count >= 2)
					{
						yield return this.arg2;
					}
					if (this.count >= 3)
					{
						yield return this.arg3;
					}
					if (this.count >= 4)
					{
						yield return this.arg4;
					}
				}
				yield break;
			}
		}

		
		public SignalArgs(SignalArgs args)
		{
			this.count = args.count;
			this.arg1 = args.arg1;
			this.arg2 = args.arg2;
			this.arg3 = args.arg3;
			this.arg4 = args.arg4;
			this.args = args.args;
		}

		
		public SignalArgs(NamedArgument arg1)
		{
			this.count = 1;
			this.arg1 = arg1;
			this.arg2 = default(NamedArgument);
			this.arg3 = default(NamedArgument);
			this.arg4 = default(NamedArgument);
			this.args = null;
		}

		
		public SignalArgs(NamedArgument arg1, NamedArgument arg2)
		{
			this.count = 2;
			this.arg1 = arg1;
			this.arg2 = arg2;
			this.arg3 = default(NamedArgument);
			this.arg4 = default(NamedArgument);
			this.args = null;
		}

		
		public SignalArgs(NamedArgument arg1, NamedArgument arg2, NamedArgument arg3)
		{
			this.count = 3;
			this.arg1 = arg1;
			this.arg2 = arg2;
			this.arg3 = arg3;
			this.arg4 = default(NamedArgument);
			this.args = null;
		}

		
		public SignalArgs(NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4)
		{
			this.count = 4;
			this.arg1 = arg1;
			this.arg2 = arg2;
			this.arg3 = arg3;
			this.arg4 = arg4;
			this.args = null;
		}

		
		public SignalArgs(params NamedArgument[] args)
		{
			this.count = args.Length;
			if (args.Length > 4)
			{
				this.arg1 = default(NamedArgument);
				this.arg2 = default(NamedArgument);
				this.arg3 = default(NamedArgument);
				this.arg4 = default(NamedArgument);
				this.args = new NamedArgument[args.Length];
				for (int i = 0; i < args.Length; i++)
				{
					this.args[i] = args[i];
				}
				return;
			}
			if (args.Length == 1)
			{
				this.arg1 = args[0];
				this.arg2 = default(NamedArgument);
				this.arg3 = default(NamedArgument);
				this.arg4 = default(NamedArgument);
			}
			else if (args.Length == 2)
			{
				this.arg1 = args[0];
				this.arg2 = args[1];
				this.arg3 = default(NamedArgument);
				this.arg4 = default(NamedArgument);
			}
			else if (args.Length == 3)
			{
				this.arg1 = args[0];
				this.arg2 = args[1];
				this.arg3 = args[2];
				this.arg4 = default(NamedArgument);
			}
			else if (args.Length == 4)
			{
				this.arg1 = args[0];
				this.arg2 = args[1];
				this.arg3 = args[2];
				this.arg4 = args[3];
			}
			else
			{
				this.arg1 = default(NamedArgument);
				this.arg2 = default(NamedArgument);
				this.arg3 = default(NamedArgument);
				this.arg4 = default(NamedArgument);
			}
			this.args = null;
		}

		
		public bool TryGetArg(int index, out NamedArgument arg)
		{
			if (index < 0 || index >= this.count)
			{
				arg = default(NamedArgument);
				return false;
			}
			if (this.args != null)
			{
				arg = this.args[index];
			}
			else if (index == 0)
			{
				arg = this.arg1;
			}
			else if (index == 1)
			{
				arg = this.arg2;
			}
			else if (index == 2)
			{
				arg = this.arg3;
			}
			else
			{
				arg = this.arg4;
			}
			return true;
		}

		
		public bool TryGetArg(string name, out NamedArgument arg)
		{
			if (this.count == 0)
			{
				arg = default(NamedArgument);
				return false;
			}
			if (this.args != null)
			{
				for (int i = 0; i < this.args.Length; i++)
				{
					if (this.args[i].label == name)
					{
						arg = this.args[i];
						return true;
					}
				}
			}
			else
			{
				if (this.count >= 1 && this.arg1.label == name)
				{
					arg = this.arg1;
					return true;
				}
				if (this.count >= 2 && this.arg2.label == name)
				{
					arg = this.arg2;
					return true;
				}
				if (this.count >= 3 && this.arg3.label == name)
				{
					arg = this.arg3;
					return true;
				}
				if (this.count >= 4 && this.arg4.label == name)
				{
					arg = this.arg4;
					return true;
				}
			}
			arg = default(NamedArgument);
			return false;
		}

		
		public bool TryGetArg<T>(string name, out T arg)
		{
			NamedArgument namedArgument;
			if (!this.TryGetArg(name, out namedArgument) || !(namedArgument.arg is T))
			{
				arg = default(T);
				return false;
			}
			arg = (T)((object)namedArgument.arg);
			return true;
		}

		
		public NamedArgument GetArg(int index)
		{
			NamedArgument result;
			if (this.TryGetArg(index, out result))
			{
				return result;
			}
			throw new ArgumentOutOfRangeException("index");
		}

		
		public NamedArgument GetArg(string name)
		{
			NamedArgument result;
			if (this.TryGetArg(name, out result))
			{
				return result;
			}
			throw new ArgumentException("Could not find arg named " + name);
		}

		
		public T GetArg<T>(string name)
		{
			T result;
			if (this.TryGetArg<T>(name, out result))
			{
				return result;
			}
			throw new ArgumentException("Could not find arg named " + name + " of type " + typeof(T).Name);
		}

		
		public TaggedString GetFormattedText(TaggedString text)
		{
			if (this.count == 0)
			{
				return text.Formatted(Array.Empty<NamedArgument>());
			}
			if (this.args != null)
			{
				return text.Formatted(this.args);
			}
			if (this.count == 1)
			{
				return text.Formatted(this.arg1);
			}
			if (this.count == 2)
			{
				return text.Formatted(this.arg1, this.arg2);
			}
			if (this.count == 3)
			{
				return text.Formatted(this.arg1, this.arg2, this.arg3);
			}
			return text.Formatted(this.arg1, this.arg2, this.arg3, this.arg4);
		}

		
		public TaggedString GetTranslatedText(string textKey)
		{
			if (this.count == 0)
			{
				return textKey.Translate();
			}
			if (this.args != null)
			{
				return textKey.Translate(this.args);
			}
			if (this.count == 1)
			{
				return textKey.Translate(this.arg1);
			}
			if (this.count == 2)
			{
				return textKey.Translate(this.arg1, this.arg2);
			}
			if (this.count == 3)
			{
				return textKey.Translate(this.arg1, this.arg2, this.arg3);
			}
			return textKey.Translate(this.arg1, this.arg2, this.arg3, this.arg4);
		}

		
		public void Add(NamedArgument arg)
		{
			if (this.args != null)
			{
				NamedArgument[] array = new NamedArgument[this.args.Length + 1];
				for (int i = 0; i < this.args.Length; i++)
				{
					array[i] = this.args[i];
				}
				array[array.Length - 1] = arg;
				this.args = array;
				this.count = this.args.Length;
				return;
			}
			if (this.count == 0)
			{
				this.arg1 = arg;
			}
			else if (this.count == 1)
			{
				this.arg2 = arg;
			}
			else if (this.count == 2)
			{
				this.arg3 = arg;
			}
			else if (this.count == 3)
			{
				this.arg4 = arg;
			}
			else
			{
				this.args = new NamedArgument[5];
				this.args[0] = this.arg1;
				this.args[1] = this.arg2;
				this.args[2] = this.arg3;
				this.args[3] = this.arg4;
				this.args[4] = arg;
				this.arg1 = default(NamedArgument);
				this.arg2 = default(NamedArgument);
				this.arg3 = default(NamedArgument);
				this.arg4 = default(NamedArgument);
			}
			this.count++;
		}

		
		public void Add(NamedArgument arg1, NamedArgument arg2)
		{
			this.Add(arg1);
			this.Add(arg2);
		}

		
		public void Add(NamedArgument arg1, NamedArgument arg2, NamedArgument arg3)
		{
			this.Add(arg1);
			this.Add(arg2);
			this.Add(arg3);
		}

		
		public void Add(NamedArgument arg1, NamedArgument arg2, NamedArgument arg3, NamedArgument arg4)
		{
			this.Add(arg1);
			this.Add(arg2);
			this.Add(arg3);
			this.Add(arg4);
		}

		
		public void Add(params NamedArgument[] args)
		{
			for (int i = 0; i < args.Length; i++)
			{
				this.Add(args[i]);
			}
		}

		
		public void Add(SignalArgs args)
		{
			if (args.count == 0)
			{
				return;
			}
			if (args.args != null)
			{
				for (int i = 0; i < args.args.Length; i++)
				{
					this.Add(args.args[i]);
				}
				return;
			}
			if (args.count >= 1)
			{
				this.Add(args.arg1);
			}
			if (args.count >= 2)
			{
				this.Add(args.arg2);
			}
			if (args.count >= 3)
			{
				this.Add(args.arg3);
			}
			if (args.count >= 4)
			{
				this.Add(args.arg4);
			}
		}

		
		private int count;

		
		private NamedArgument arg1;

		
		private NamedArgument arg2;

		
		private NamedArgument arg3;

		
		private NamedArgument arg4;

		
		private NamedArgument[] args;
	}
}
