using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Newtonsoft.Json;
using Terraria;
using Terraria.ModLoader;
using Terraria.ModLoader.Config;
using HamstarHelpers.Libraries.Debug;
using HamstarHelpers.Libraries.DotNET.Reflection;


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
			IEnumerable<Type> configTypes = ReflectionLibraries.GetAllAvailableSubTypesFromMods( typeof(ModConfig) );
			Type classType = System.Type.GetType( className );
			Type configType = null;

			foreach( Type subConfigType in configTypes ) {
				if( subConfigType.IsAbstract ) { continue; }
				if( !subConfigType.Name.Equals(className) ) { continue; }

				configType = subConfigType;
				break;
			}

			if( configType == null ) {
				caller.Reply( "Could not get config class "+className, Color.Yellow );
				return;
			}

			ModConfig configSingleton;
			bool success = ReflectionLibraries.RunMethod<ModConfig>( //ModContent.GetInstance<T>();
				classType: typeof( ModContent ),
				instance: null,
				methodName: "GetInstance",
				args: new object[] { },
				generics: new Type[] { configType },
				out configSingleton
			);
			if( configSingleton == null ) {
				caller.Reply( "Could not get config instance of class "+className, Color.Yellow );
				return;
			}

			string configJson = JsonConvert.SerializeObject( configSingleton );

			Main.NewText( configJson );
			LogLibraries.Log( configJson );
		}
	}
}
