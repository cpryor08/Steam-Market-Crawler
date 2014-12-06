using System;
using System.Collections.Generic;
using System.IO;
namespace SteamMarketCrawler
{
	public sealed class IniFile
	{
		private class IniValueStructure
		{
			public string Variable;
			public string Value;
		}
		private class IniSectionStructure
		{
			public Dictionary<string, IniFile.IniValueStructure> Variables;
			public string SectionName;
		}
		public string path;
		private Dictionary<string, IniFile.IniSectionStructure> Sections = new Dictionary<string, IniFile.IniSectionStructure>();
		public IniFile(string INIPath)
		{
			this.path = INIPath;
			if (File.Exists(this.path))
			{
				this.Read();
			}
		}
		public void Read()
		{
			string[] array = File.ReadAllLines(this.path);
			string text = "";
			string[] array2 = array;
			for (int i = 0; i < array2.Length; i++)
			{
				string text2 = array2[i];
				if (text2.Length > 0)
				{
					if (text2[0] == '[' && text2[text2.Length - 1] == ']')
					{
						text = text2;
						IniFile.IniSectionStructure iniSectionStructure = new IniFile.IniSectionStructure();
						iniSectionStructure.SectionName = text;
						iniSectionStructure.Variables = new Dictionary<string, IniFile.IniValueStructure>();
						this.Sections.Add(text, iniSectionStructure);
					}
					else
					{
						IniFile.IniValueStructure iniValueStructure = new IniFile.IniValueStructure();
						iniValueStructure.Variable = text2.Split(new char[]
						{
							'='
						})[0];
						iniValueStructure.Value = text2.Split(new char[]
						{
							'='
						})[1];
						IniFile.IniSectionStructure iniSectionStructure = null;
						this.Sections.TryGetValue(text, out iniSectionStructure);
						if (iniSectionStructure != null)
						{
							if (!iniSectionStructure.Variables.ContainsKey(iniValueStructure.Variable))
							{
								iniSectionStructure.Variables.Add(iniValueStructure.Variable, iniValueStructure);
							}
						}
					}
				}
			}
		}
		public void Close()
		{
			this.Sections.Clear();
		}
		public void Save()
		{
			string text = "";
			foreach (IniFile.IniSectionStructure current in this.Sections.Values)
			{
				text = text + current.SectionName + "\r\n";
				foreach (IniFile.IniValueStructure current2 in current.Variables.Values)
				{
					string text2 = text;
					text = string.Concat(new string[]
					{
						text2,
						current2.Variable,
						"=",
						current2.Value,
						"\r\n"
					});
				}
			}
			if (File.Exists(this.path))
			{
				File.Delete(this.path);
				File.Create(this.path).Close();
				File.WriteAllText(this.path, text);
			}
			else
			{
				File.Create(this.path).Close();
				File.WriteAllText(this.path, text);
			}
		}
		private void IniWriteValue(string ssection, string Key, string Value)
		{
			string text = "[" + ssection + "]";
			IniFile.IniSectionStructure iniSectionStructure = null;
			this.Sections.TryGetValue(text, out iniSectionStructure);
			if (iniSectionStructure != null)
			{
				IniFile.IniValueStructure iniValueStructure = null;
				iniSectionStructure.Variables.TryGetValue(Key, out iniValueStructure);
				if (iniValueStructure != null)
				{
					if (iniValueStructure.Variable == Key)
					{
						iniValueStructure.Value = Value;
					}
				}
				else
				{
					iniSectionStructure.Variables.Add(Key, new IniFile.IniValueStructure
					{
						Value = Value,
						Variable = Key
					});
				}
			}
			else
			{
				iniSectionStructure = new IniFile.IniSectionStructure
				{
					SectionName = text,
					Variables = new Dictionary<string, IniFile.IniValueStructure>()
				};
				this.Sections.Add(text, iniSectionStructure);
				IniFile.IniValueStructure iniValueStructure = null;
				iniSectionStructure.Variables.TryGetValue(Key, out iniValueStructure);
				if (iniValueStructure != null)
				{
					if (iniValueStructure.Variable == Key)
					{
						iniValueStructure.Value = Value;
					}
				}
				else
				{
					iniSectionStructure.Variables.Add(Key, new IniFile.IniValueStructure
					{
						Value = Value,
						Variable = Key
					});
				}
			}
		}
		public byte ReadByte(string Section, string Key)
		{
			string key = "[" + Section + "]";
			IniFile.IniSectionStructure iniSectionStructure = null;
			this.Sections.TryGetValue(key, out iniSectionStructure);
			byte result;
			if (iniSectionStructure != null)
			{
				IniFile.IniValueStructure iniValueStructure = null;
				iniSectionStructure.Variables.TryGetValue(Key, out iniValueStructure);
				if (iniValueStructure != null)
				{
					result = byte.Parse(iniValueStructure.Value);
					return result;
				}
			}
			result = 0;
			return result;
		}
		public sbyte ReadSbyte(string Section, string Key)
		{
			string key = "[" + Section + "]";
			IniFile.IniSectionStructure iniSectionStructure = null;
			this.Sections.TryGetValue(key, out iniSectionStructure);
			sbyte result;
			if (iniSectionStructure != null)
			{
				IniFile.IniValueStructure iniValueStructure = null;
				iniSectionStructure.Variables.TryGetValue(Key, out iniValueStructure);
				if (iniValueStructure != null)
				{
					result = sbyte.Parse(iniValueStructure.Value);
					return result;
				}
			}
			result = 0;
			return result;
		}
		public short ReadInt16(string Section, string Key)
		{
			string key = "[" + Section + "]";
			IniFile.IniSectionStructure iniSectionStructure = null;
			this.Sections.TryGetValue(key, out iniSectionStructure);
			short result;
			if (iniSectionStructure != null)
			{
				IniFile.IniValueStructure iniValueStructure = null;
				iniSectionStructure.Variables.TryGetValue(Key, out iniValueStructure);
				if (iniValueStructure != null)
				{
					result = short.Parse(iniValueStructure.Value);
					return result;
				}
			}
			result = 0;
			return result;
		}
		public int ReadInt32(string Section, string Key)
		{
			string key = "[" + Section + "]";
			IniFile.IniSectionStructure iniSectionStructure = null;
			this.Sections.TryGetValue(key, out iniSectionStructure);
			int result;
			if (iniSectionStructure != null)
			{
				IniFile.IniValueStructure iniValueStructure = null;
				iniSectionStructure.Variables.TryGetValue(Key, out iniValueStructure);
				if (iniValueStructure != null)
				{
					result = int.Parse(iniValueStructure.Value);
					return result;
				}
			}
			result = 0;
			return result;
		}
		public long ReadInt64(string Section, string Key)
		{
			string key = "[" + Section + "]";
			IniFile.IniSectionStructure iniSectionStructure = null;
			this.Sections.TryGetValue(key, out iniSectionStructure);
			long result;
			if (iniSectionStructure != null)
			{
				IniFile.IniValueStructure iniValueStructure = null;
				iniSectionStructure.Variables.TryGetValue(Key, out iniValueStructure);
				if (iniValueStructure != null)
				{
					result = long.Parse(iniValueStructure.Value);
					return result;
				}
			}
			result = 0L;
			return result;
		}
		public ushort ReadUInt16(string Section, string Key)
		{
			string key = "[" + Section + "]";
			IniFile.IniSectionStructure iniSectionStructure = null;
			this.Sections.TryGetValue(key, out iniSectionStructure);
			ushort result;
			if (iniSectionStructure != null)
			{
				IniFile.IniValueStructure iniValueStructure = null;
				iniSectionStructure.Variables.TryGetValue(Key, out iniValueStructure);
				if (iniValueStructure != null)
				{
					result = ushort.Parse(iniValueStructure.Value);
					return result;
				}
			}
			result = 0;
			return result;
		}
		public uint ReadUInt32(string Section, string Key)
		{
			string key = "[" + Section + "]";
			IniFile.IniSectionStructure iniSectionStructure = null;
			this.Sections.TryGetValue(key, out iniSectionStructure);
			uint result;
			if (iniSectionStructure != null)
			{
				IniFile.IniValueStructure iniValueStructure = null;
				iniSectionStructure.Variables.TryGetValue(Key, out iniValueStructure);
				if (iniValueStructure != null)
				{
					result = uint.Parse(iniValueStructure.Value);
					return result;
				}
			}
			result = 0u;
			return result;
		}
		public ulong ReadUInt64(string Section, string Key)
		{
			string key = "[" + Section + "]";
			IniFile.IniSectionStructure iniSectionStructure = null;
			this.Sections.TryGetValue(key, out iniSectionStructure);
			ulong result;
			if (iniSectionStructure != null)
			{
				IniFile.IniValueStructure iniValueStructure = null;
				iniSectionStructure.Variables.TryGetValue(Key, out iniValueStructure);
				if (iniValueStructure != null)
				{
					result = ulong.Parse(iniValueStructure.Value);
					return result;
				}
			}
			result = 0uL;
			return result;
		}
		public double ReadDouble(string Section, string Key)
		{
			string key = "[" + Section + "]";
			IniFile.IniSectionStructure iniSectionStructure = null;
			this.Sections.TryGetValue(key, out iniSectionStructure);
			double result;
			if (iniSectionStructure != null)
			{
				IniFile.IniValueStructure iniValueStructure = null;
				iniSectionStructure.Variables.TryGetValue(Key, out iniValueStructure);
				if (iniValueStructure != null)
				{
					result = double.Parse(iniValueStructure.Value);
					return result;
				}
			}
			result = 0.0;
			return result;
		}
		public float ReadFloat(string Section, string Key)
		{
			string key = "[" + Section + "]";
			IniFile.IniSectionStructure iniSectionStructure = null;
			this.Sections.TryGetValue(key, out iniSectionStructure);
			float result;
			if (iniSectionStructure != null)
			{
				IniFile.IniValueStructure iniValueStructure = null;
				iniSectionStructure.Variables.TryGetValue(Key, out iniValueStructure);
				if (iniValueStructure != null)
				{
					result = float.Parse(iniValueStructure.Value);
					return result;
				}
			}
			result = 0f;
			return result;
		}
		public string ReadString(string Section, string Key)
		{
			string key = "[" + Section + "]";
			IniFile.IniSectionStructure iniSectionStructure = null;
			this.Sections.TryGetValue(key, out iniSectionStructure);
			string result;
			if (iniSectionStructure != null)
			{
				IniFile.IniValueStructure iniValueStructure = null;
				iniSectionStructure.Variables.TryGetValue(Key, out iniValueStructure);
				if (iniValueStructure != null)
				{
					result = iniValueStructure.Value;
					return result;
				}
			}
			result = "";
			return result;
		}
		public bool ReadBoolean(string Section, string Key)
		{
			string key = "[" + Section + "]";
			IniFile.IniSectionStructure iniSectionStructure = null;
			this.Sections.TryGetValue(key, out iniSectionStructure);
			bool result;
			if (iniSectionStructure != null)
			{
				IniFile.IniValueStructure iniValueStructure = null;
				iniSectionStructure.Variables.TryGetValue(Key, out iniValueStructure);
				if (iniValueStructure != null)
				{
					result = (byte.Parse(iniValueStructure.Value) == 1);
					return result;
				}
			}
			result = false;
			return result;
		}
		public void WriteString(string Section, string Key, string Value)
		{
			this.IniWriteValue(Section, Key, Value);
		}
		public void WriteInteger(string Section, string Key, byte Value)
		{
			this.IniWriteValue(Section, Key, Value.ToString());
		}
		public void WriteInteger(string Section, string Key, ulong Value)
		{
			this.IniWriteValue(Section, Key, Value.ToString());
		}
		public void WriteInteger(string Section, string Key, double Value)
		{
			this.IniWriteValue(Section, Key, Value.ToString());
		}
		public void WriteInteger(string Section, string Key, long Value)
		{
			this.IniWriteValue(Section, Key, Value.ToString());
		}
		public void WriteInteger(string Section, string Key, float Value)
		{
			this.IniWriteValue(Section, Key, Value.ToString());
		}
		public void WriteBoolean(string Section, string Key, bool Value)
		{
			this.IniWriteValue(Section, Key, (Value ? 1 : 0).ToString());
		}
	}
}
