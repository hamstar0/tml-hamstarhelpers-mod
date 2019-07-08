using HamstarHelpers.Components.Config;
using HamstarHelpers.Components.Protocols.Packet;
using HamstarHelpers.Helpers.Debug;
using HamstarHelpers.Helpers.DotNET;
using HamstarHelpers.Services.Messages;
using System;
using System.IO;
using Terraria;
using Terraria.ModLoader;


namespace HamstarHelpers {
	/// @private
	partial class ModHelpersMod : Mod {
		public static ModHelpersMod Instance;



		////////////////

		private static void UnhandledLogger( object sender, UnhandledExceptionEventArgs e ) {
			LogHelpers.Log( "UNHANDLED crash? " + e.IsTerminating
				+ " \nSender: " + sender.ToString()
				+ " \nMessage: " + e.ExceptionObject.ToString() );
		}



		////////////////

		internal JsonConfig<HamstarHelpersConfigData> ConfigJson;
		public HamstarHelpersConfigData Config => ConfigJson.Data;

		////

		private int LastSeenCPScreenWidth = -1;
		private int LastSeenCPScreenHeight = -1;


		private bool HasUnhandledExceptionLogger = false;



		////////////////

		public ModHelpersMod() {
			ModHelpersMod.Instance = this;

			this.HasSetupContent = false;
			this.HasAddedRecipeGroups = false;
			this.HasAddedRecipes = false;

			this.InitializeModules();
			this.PostInitializeInternal();
		}


		public override void Load() {
			//ErrorLogger.Log( "Loading Mod Helpers. Ensure you have .NET Framework v4.6+ installed, if you're having problems." );
			if( Environment.Version < new Version( 4, 0, 30319, 42000 ) ) {
				SystemHelpers.OpenUrl( "https://dotnet.microsoft.com/download/dotnet-framework-runtime" );
				throw new FileNotFoundException( "Mod Helpers "+this.Version+" requires .NET Framework v4.6+ to work." );
			}

			this.LoadFull();

			InboxMessages.SetMessage( "ModHelpers:ControlPanelTags",
				"Mod tag lists have now been added to the Control Panel. Mod tags can be modified in the Mod Info menu page via. the main menu.",
				false
			);
		}

		////

		public override void Unload() {
			this.UnloadFull();

			ModHelpersMod.Instance = null;
		}


		////////////////

		public override void PostSetupContent() {
			this.PostSetupContentFull();

			this.HasSetupContent = true;
			this.CheckAndProcessLoadFinish();
		}

		////////////////

		public override void AddRecipes() {
			this.AddRecipesFull();
		}

		public override void AddRecipeGroups() {
			this.AddRecipeGroupsFull();

			this.HasAddedRecipeGroups = true;
			this.CheckAndProcessLoadFinish();
		}

		public override void PostAddRecipes() {
			this.PostAddRecipesFull();

			this.HasAddedRecipes = true;
			this.CheckAndProcessLoadFinish();
		}


		////////////////

		private void CheckAndProcessLoadFinish() {
			if( !this.HasSetupContent ) { return; }
			if( !this.HasAddedRecipeGroups ) { return; }
			if( !this.HasAddedRecipes ) { return; }

			this.PostLoadFull();
/*DataDumper.SetDumpSource( "DEBUG", () => {
	var data = Services.DataStore.DataStore.GetAll();
	string str = "";

	foreach( var kv in data ) {
		string key = kv.Key as string;
		if( key == null ) { continue; }

		if( key[key.Length-1] != 'A' ) {
			continue;
		}

		string keyB = key.Substring( 0, key.Length - 1 ) + "B";
		if( !data.ContainsKey(keyB) ) { continue; }

		double valA = (double)kv.Value;
		double valB = (double)data[keyB];
		if( valA != valB ) {
			str += key + " " + valA + " vs " + valB+",\n";
		}
	}

	return str;
} );*/
		}


		////////////////

		public override void PreSaveAndQuit() {
			this.Promises.PreSaveAndExit();
		}


		////////////////

		public override void HandlePacket( BinaryReader reader, int playerWho ) {
//Services.DataStore.DataStore.Add( DebugHelpers.GetCurrentContext()+"_A", 1 );
			try {
				int protocolCode = reader.ReadInt32();
				
				if( Main.netMode == 1 ) {
					PacketProtocol.HandlePacketOnClient( protocolCode, reader, playerWho );
				} else if( Main.netMode == 2 ) {
					PacketProtocol.HandlePacketOnServer( protocolCode, reader, playerWho );
				}
			} catch( Exception e ) {
				LogHelpers.Alert( e.ToString() );
			}
//Services.DataStore.DataStore.Add( DebugHelpers.GetCurrentContext()+"_B", 1 );
		}


		////////////////

		//public override void UpdateMusic( ref int music ) { //, ref MusicPriority priority
		//	this.MusicHelpers.UpdateMusic();
		//}
	}
}
