using HamstarHelpers.Components.Errors;
using HamstarHelpers.Helpers.DebugHelpers;
using HamstarHelpers.Helpers.DotNetHelpers;
using System;
using System.Collections.Generic;
using Terraria.ModLoader;


namespace HamstarHelpers.Helpers.TmlHelpers.ModHelpers {
	public class ModIdentityHelpers {
		private static IDictionary<Mod, string> ModIds = new Dictionary<Mod, string>();



		////////////////

		public static string GetModUniqueName( Mod mod ) {
			if( ModIdentityHelpers.ModIds.ContainsKey( mod ) ) {
				return ModIdentityHelpers.ModIds[mod];
			}
			ModIdentityHelpers.ModIds[mod] = mod.Name + ":" + mod.Version;
			return ModIdentityHelpers.ModIds[mod];
		}


		public static IDictionary<Mod, Version> FindDependencyModMajorVersionMismatches( Mod mod ) {
			Services.Tml.BuildPropertiesEditor buildEditor = Services.Tml.BuildPropertiesEditor.GetBuildPropertiesForModFile( mod.File );
			IDictionary<string, Version> modRefs = buildEditor.ModReferences;
			var badModDeps = new Dictionary<Mod, Version>();

			foreach( var kv in modRefs ) {
				Mod depMod = ModLoader.GetMod( kv.Key );
				if( depMod == null ) { continue; }

				if( depMod.Version.Major != kv.Value.Major ) {
					badModDeps[depMod] = kv.Value;
				}
			}

			return badModDeps;
		}

		////////////////

		public static string FormatBadDependencyModList( Mod mod ) {
			IDictionary<Mod, Version> badDepMods = ModIdentityHelpers.FindDependencyModMajorVersionMismatches( mod );

			if( badDepMods.Count != 0 ) {
				IEnumerable<string> badDepModsList = badDepMods.SafeSelect(
					kv => kv.Key.DisplayName + " (needs " + kv.Value.ToString() + ", is " + kv.Key.Version.ToString() + ")"
				);
				return mod.DisplayName + " (" + mod.Name + ") is out of date with its dependency mod(s): " + string.Join( ", \n", badDepModsList );
			}
			return null;
		}


		////////////////

		public static bool IsProperlyPresented( string modName ) {

		}

		public static bool IsProperlyPresented( string modName, string author, string description, string homepage ) {
			if( description.Contains("Learn how to mod with tModLoader by exploring the source code for this mod.") ) { return false; }
			if( description.Contains("Modify this file with a description of your mod.") ) { return false; }
			if( description.Length < 16 ) { return false; }
			if( homepage.Length < 16 ) { return false; }

			homepage = homepage.ToLower();

			if( homepage.Contains( "//discord.gg" ) ) { return false; }

			if( homepage.Contains("//bit.ly") ) { return false; }
			if( homepage.Contains("//bitly.com") ) { return false; }
			if( homepage.Contains("//goo.gl") ) { return false; }
			if( homepage.Contains("//tinyurl.com") ) { return false; }
			if( homepage.Contains("//is.gd") ) { return false; }
			if( homepage.Contains("//cli.gs") ) { return false; }
			if( homepage.Contains("//pic.gd") ) { return false; }
			if( homepage.Contains("//dwarfurl.com") ) { return false; }
			if( homepage.Contains("//ow.ly") ) { return false; }
			if( homepage.Contains("//yfrog.com") ) { return false; }
			if( homepage.Contains("//migre.me") ) { return false; }
			if( homepage.Contains("//ff.im") ) { return false; }
			if( homepage.Contains("//tiny.cc") ) { return false; }
			if( homepage.Contains("//url4.eu") ) { return false; }
			if( homepage.Contains("//tr.im") ) { return false; }
			if( homepage.Contains("//twit.ac") ) { return false; }
			if( homepage.Contains("//su.pr") ) { return false; }
			if( homepage.Contains("//twurl.nl") ) { return false; }
			if( homepage.Contains("//snipurl.com") ) { return false; }
			if( homepage.Contains("//budurl.com") ) { return false; }
			if( homepage.Contains("//short.to") ) { return false; }
			if( homepage.Contains("//ping.fm") ) { return false; }
			if( homepage.Contains("//digg.com") ) { return false; }
			if( homepage.Contains("//post.ly") ) { return false; }
			if( homepage.Contains("//just.as") ) { return false; }
			if( homepage.Contains("//redd.it") ) { return false; }
			if( homepage.Contains("//to.ly") ) { return false; }	// Much much more remain...

			return true;
		}
	}
}
