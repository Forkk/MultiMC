using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Newtonsoft.Json;

namespace MultiMC.Mods
{
	[Serializable()]
	public class ModInfo
	{
		public ModInfo()
		{
			
		}

		public string ToJSON()
		{
			return JsonConvert.SerializeObject(this);
		}

		public static ModInfo FromJSON(string json)
		{
			try
			{
				return JsonConvert.DeserializeObject<ModInfo>(json);
			}
			catch (JsonReaderException)
			{
				return null;
			}
		}

		public string Name { get; set; }
		public string Description { get; set; }
		public string ModID { get; set; }
		public string HomePage { get; set; }
		public string ModVersion { get; set; }
		public string MCVersion { get; set; }
		public string MMCVersion { get; set; }

		public List<InstallRule> InstallRules { get; set; }
	}

	[Serializable()]
	public class InstallRule
	{
		public List<string> Files { get; set; }
		public bool InstallToJar { get; set; }
		//public List<object> ExtractTo { get; set; }
	}
}
