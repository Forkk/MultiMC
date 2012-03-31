using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MultiMC
{
	/// <summary>
	/// File that maps build numbers from minecraft/bin/version to their respective 
	/// minecraft versions
	/// </summary>
	public class MCVersionMap : ConfigFile
	{
		public static MCVersionMap VersionMap;

		static MCVersionMap()
		{
			VersionMap = new MCVersionMap();
			VersionMap.Load(Properties.Resources.MCVersionFile);
		}

		MCVersionMap()
			: base()
		{

		}

		/// <summary>
		/// Gets the build number for the specified version
		/// </summary>
		/// <param name="v">The minecraft version</param>
		/// <returns>The build number for the given version or null 
		/// if the version wasn't found.</returns>
		public new string this[string v]
		{
			get { return this[v, null]; }
		}
	}
}
