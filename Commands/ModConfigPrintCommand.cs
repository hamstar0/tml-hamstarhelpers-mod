using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET.Reflection;
using HamstarHelpers.Services.Configs;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;


namespace HamstarHelpers.Commands {
	/// @private
	public class ModConfigPrintCommand : ModCommand {
		/// @private
		public override CommandType Type => CommandType.Chat | CommandType.Console;
		/// @private
		public override string Command => "mh-configprint";
		/// @private
		public override string Usage => "/" + this.Command + " MyModConfigName";
		/// @private
		public override string Description => "Outputs a given ModConfig (stacked, if applicable) to chat and log."
			+ "\n   Parameters: <ModConfig class name>";



		////////////////

		/// @private
		public override void Action( CommandCaller caller, string input, string[] args ) {
			if( args.Length != 1 ) {
				caller.Reply( "Invalid arguments supplied.", Color.Red );
				return;
			}

			string className = args[0];
			IEnumerable<Type> configTypes = ReflectionHelpers.GetAllAvailableSubTypesFromMods( typeof(ModConfig) );
			Type classType = System.Type.GetType( className );
			Type configType = null;

			foreach( Type subConfigType in configTypes ) {
				if( subConfigType.IsAbstract ) { continue; }
				if( !subConfigType.Name.Equals(className) ) { continue; }

				configType = subConfigType;
				break;
			}

			if( configType == null ) {
				caller.Reply( "Could not config class "+className, Color.Yellow );
				return;
			}

			ModConfig config = ModConfigStack.GetMergedConfigsForType( configType );
			string configJson = JsonConvert.SerializeObject( config );

			Main.NewText( configJson );
			LogHelpers.Log( configJson );
		}
	}
}
